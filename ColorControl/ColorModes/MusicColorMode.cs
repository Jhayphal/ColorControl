using NAudio.Dsp;
using NAudio.Wave;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ColorControl.ColorModes
{
	class MusicColorMode : ColorMode
	{
		private WasapiLoopbackCapture capture;
		private SampleAggregator aggregator;

		public MusicColorMode() : base()
		{
			Name = "Music colors";

			aggregator = new SampleAggregator(8192)
			{
				PerformFFT = true
			};
			//aggregator.FftCalculated += Aggregator_FftCalculated;

			capture = new WasapiLoopbackCapture();
			capture.DataAvailable += Capture_DataAvailable;
			capture.RecordingStopped += Capture_RecordingStopped;
			capture.StartRecording();
		}

		private void Capture_RecordingStopped(object sender, StoppedEventArgs e)
		{
			capture.Dispose();
		}

		float maxA, maxF;

		private void Aggregator_FftCalculated(object sender, FftEventArgs e)
		{
			Func<Complex, float> selectorF = x => x.X > 0 ? x.X : -x.X;
			Func<Complex, float> selectorA = x => x.Y > 0 ? x.Y : -x.Y;

			var modaA = e.Result
				.Where(x => selectorA(x) > 0f)
				.GroupBy(selectorA)
				.Select(x => new { x.Key, Count = e.Result.Count(y => selectorA(y) == x.Key) })
				.OrderByDescending(x => x.Count)
				.FirstOrDefault()?
				.Key ?? 0f;

			var currentMaxA = e.Result.Max(selectorA);

			if (currentMaxA > maxA)
				maxA = currentMaxA;

			var saturation = modaA / maxA;

			var modaF = e.Result
				.Where(x => selectorF(x) > 0f)
				.GroupBy(selectorF)
				.Select(x => new { x.Key, Count = e.Result.Count(y => selectorF(y) == x.Key) })
				.OrderByDescending(x => x.Count)
				.FirstOrDefault()?
				.Key ?? 0f;

			var currentMaxF = e.Result.Max(selectorF);

			if (currentMaxF > maxF)
				maxF = currentMaxF;

			const float ToValue = 15f;
			var hue = ToValue - (ToValue * (modaF / maxF));

			CurrentColor = CircleColorMode.HSLToRGB((int)hue, 1f, 0.5f);
		}

		short maxVal = 0;
		ConcurrentQueue<int> samples = new ConcurrentQueue<int>();

		private void Capture_DataAvailable(object sender, WaveInEventArgs e)
		{
			byte[] buffer = e.Buffer;

			for (int index = 0; index < e.BytesRecorded; index += 2)
			{
				short sample = (short)((buffer[index + 1] << 8) |
										buffer[index + 0]);
				samples.Enqueue(sample);

				float sample32 = sample / 32768f;

				aggregator.Add(sample32);

				if (sample > maxVal)
					maxVal = sample;				
			}
		}

		public override Task UpdateAsync(string address, bool force = false)
		{
			const float ToValue = 90;

			double hue = 0;

			if (samples.Count > 0)
			{
				var values = new List<int>();

				while (samples.TryDequeue(out int sample))
					values.Add(sample);

				hue = ToValue * (values.Average(x => x > 0 ? x : -x) / maxVal);
			}

			CurrentColor = CircleColorMode.HSLToRGB((int)hue, 1f, 0.5f);

			return base.UpdateAsync(address, force);
		}

		public override void Dispose()
		{
			capture.StopRecording();

			base.Dispose();
		}
	}

	public class SampleAggregator
	{
		// volume
		public event EventHandler<MaxSampleEventArgs> MaximumCalculated;
		private float maxValue;
		private float minValue;
		public int NotificationCount { get; set; }
		int count;

		// FFT
		public event EventHandler<FftEventArgs> FftCalculated;
		public bool PerformFFT { get; set; }
		private Complex[] fftBuffer;
		private FftEventArgs fftArgs;
		private int fftPos;
		private int fftLength;
		private int m;

		public SampleAggregator(int fftLength = 1024)
		{
			if (!IsPowerOfTwo(fftLength))
			{
				throw new ArgumentException("FFT Length must be a power of two");
			}
			this.m = (int)Math.Log(fftLength, 2.0);
			this.fftLength = fftLength;
			this.fftBuffer = new Complex[fftLength];
			this.fftArgs = new FftEventArgs(fftBuffer);
		}

		bool IsPowerOfTwo(int x)
		{
			return (x & (x - 1)) == 0;
		}


		public void Reset()
		{
			count = 0;
			maxValue = minValue = 0;
		}

		public void Add(float value)
		{
			if (PerformFFT && FftCalculated != null)
			{
				fftBuffer[fftPos].X = (float)(value * FastFourierTransform.HammingWindow(fftPos, fftBuffer.Length));
				fftBuffer[fftPos].Y = 0;
				fftPos++;
				if (fftPos >= fftBuffer.Length)
				{
					fftPos = 0;
					// 1024 = 2^10
					FastFourierTransform.FFT(true, m, fftBuffer);
					FftCalculated(this, fftArgs);
				}
			}

			maxValue = Math.Max(maxValue, value);
			minValue = Math.Min(minValue, value);
			count++;
			if (count >= NotificationCount && NotificationCount > 0)
			{
				if (MaximumCalculated != null)
				{
					MaximumCalculated(this, new MaxSampleEventArgs(minValue, maxValue));
				}
				Reset();
			}
		}
	}

	public class MaxSampleEventArgs : EventArgs
	{
		[DebuggerStepThrough]
		public MaxSampleEventArgs(float minValue, float maxValue)
		{
			this.MaxSample = maxValue;
			this.MinSample = minValue;
		}
		public float MaxSample { get; private set; }
		public float MinSample { get; private set; }
	}

	public class FftEventArgs : EventArgs
	{
		[DebuggerStepThrough]
		public FftEventArgs(Complex[] result)
		{
			this.Result = result;
		}
		public Complex[] Result { get; private set; }
	}
}
