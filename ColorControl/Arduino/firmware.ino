#include <ESP8266WiFi.h> 
#include <ESP8266WebServer.h>

#define R 0
#define G 1
#define B 2

const char* ssid = "****";
const char* password = "****";

ESP8266WebServer server(80);   

void handleColor()
{
  // RGB-лента с общей "землей" - значит логика инвертирована
  // 255 - значит 0 и наоборот
  int red = 255;
  int green = 255;
  int blue = 255;

  if (server.hasArg("R"))
    red = 255 - server.arg("R").toInt();

  if (server.hasArg("G"))
    green = 255 - server.arg("G").toInt();

  if (server.hasArg("B"))
    blue = 255 - server.arg("B").toInt();

  analogWrite(R, red);
  analogWrite(G, green);
  analogWrite(B, blue);

  server.send(200, "text/plain", "");
}

void handleControl()
{
  server.send(200, "text/html", "<!DOCTYPE html><html><head><meta charset=\"utf-8\"><title>Управление цветом</title><link rel=\"icon\" sizes=\"192x192\" href=\"https://htmlcolorcodes.com/assets/images/icons/icon-192x192.png\"><style>@import url('https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0-alpha.6/css/bootstrap.min.css');html,body{height:100%}b{margin-right:10px;font-size:18px}input[type=\"color\"]{width:50%}.container{height:100%;display:flex;justify-content:center;align-items:center}</style></head><body><div class=\"container\"> <b>Укажите цвет:</b> <input type=\"color\" id=\"picker\"/></div> <script>picker.oninput=function(){\"use strict\";let rgb=/^#?([a-f\\d]{2})([a-f\\d]{2})([a-f\\d]{2})$/i.exec(this.value);let r=parseInt(rgb[1],16);let g=parseInt(rgb[2],16);let b=parseInt(rgb[3],16);let query=`/color?R=${r}&G=${g}&B=${b}`;let request=new XMLHttpRequest();request.open(\"GET\",query,true);request.send();}</script> </body></html>");
}

void setup()
{
  WiFi.begin(ssid, password);

  while (WiFi.status() != WL_CONNECTED)
    delay(1000);

  server.on("/color", handleColor);
  server.on("/control", handleControl);

  server.begin();

  pinMode(R, OUTPUT);
  pinMode(G, OUTPUT);
  pinMode(B, OUTPUT);
}

void loop() 
{
  server.handleClient();
}