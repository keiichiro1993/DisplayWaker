#define SENSOR 8

void setup() 
{
  pinMode(SENSOR, INPUT);
  Serial.begin(500000);
}

bool previousStatus = false;
void loop() 
{
  bool currentStatus = digitalRead(SENSOR);
  if(currentStatus != previousStatus)
  {
    if(currentStatus)
    {
      Serial.println("High");
    }
    else
    {
      Serial.println("Low");
    }
    previousStatus = currentStatus;
  }
}
