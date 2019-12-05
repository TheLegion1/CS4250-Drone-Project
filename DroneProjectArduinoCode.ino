//status led
int r_pin = 0;
int g_pin = 1;
int b_pin = 13;

float prevLX = 0;
float prevRX = 0;
float prevLY = 0;
float prevRY = 0;
float prevLeftTrigger = 0;
float prevRightTrigger = 0;
//serial boolean
bool newData = false;
int motor1Status, motor2Status, motor3Status = 0;
//motor control pins

//motor 1 (left)
int r_ena = 10; //blue wire
int r_speed = 255;
int r_pin1 = 4;
int r_pin2 = 5;
//motor 2 (right)
int l_ena = 12; //yellow wire
int l_speed = 255;
int l_pin1 = 8;
int l_pin2 = 9;
//motor 3 (back)
int b_ena = 11; //green wire
int b_speed = 255;
int b_pin1 = 6;
int b_pin2 = 7;

bool runMotor = true;
void setup() {
  // put your setup code here, to run once:
  pinMode(r_pin1, OUTPUT);
  pinMode(r_pin2, OUTPUT);
  pinMode(l_pin1, OUTPUT);
  pinMode(l_pin2, OUTPUT);
  pinMode(b_pin1, OUTPUT);
  pinMode(b_pin2, OUTPUT);
  pinMode(r_pin, OUTPUT);
  pinMode(g_pin, OUTPUT);
  pinMode(b_pin, OUTPUT);
  Serial.begin(9600);
}



void loop() {
  //checks to make sure speed is not above or below the thresholds
  if(r_speed > 255){
    r_speed = 255;
    }
  if(l_speed > 255){
    l_speed = 255;
    }
  if(b_speed > 255){
    b_speed = 255;
    }
  if(r_speed < 0){
    r_speed = 0;
    }
  if(l_speed < 0){
    l_speed = 0;
    }
  if(b_speed < 0){
    b_speed = 0;
    }

  //write initial speeds set to full power
  analogWrite(l_ena, l_speed);
  analogWrite(r_ena, r_speed);
  analogWrite(b_ena, b_speed);

   
  // put your main code here, to run repeatedly:
  //Serial.println("DRIVING MOTOR FORWARD");
 // comm();
  readSerial();
  delay(10);
  
  
}
//start of functions 

void Move(float rightJoy, float LeftJoy, float rightTrigger, float leftTrigger) {
  //driving forwards and backards
  ChangeStatus(3);
if(rightTrigger > 0){
  Serial.println("Right trigger triggered");
  DriveMotor(1,false);
  DriveMotor(2,true);
  DriveMotor(3, true);
  }else if(leftTrigger > 0){
      Serial.println("Left trigger triggered");
      DriveMotor(1,true);
      DriveMotor(2,false);
      DriveMotor(3,false);
    }
if(LeftJoy > 0){
     DriveMotor(1,false);
     DriveMotor(2,false);
 } else if(LeftJoy < 0){
      DriveMotor(1,true);
      DriveMotor(2,true);
} else if(LeftJoy == 0 && leftTrigger ==0 && rightTrigger == 0){
  StopMotor(1);
  StopMotor(2);
  }

if(rightJoy > 0){
  if(motor3Status != 1){
     DriveMotor(3, false);
    }
} else if(rightJoy < 0){
  if(motor3Status != -1){
     DriveMotor(3, true);
    }
} else if(rightJoy == 0 && leftTrigger == 0 && rightTrigger == 0){
  StopMotor(3);
  }

}

void readSerial(){

  byte data[41];
  String startIndicator = "<SOF>";
  String endIndicator = "<EOF>";
   //ChangeStatus(0);

  if(Serial.available() > 30){
      Serial.println("INSIDE FUNCTION, HAS DATA");
   
    ChangeStatus(2);
    for(int i = 0; i < 40; i++){
      byte inByte = Serial.read();
      data[i] = inByte;
      }
      data[41] = 0;
      String result = ((char*)data);
      Serial.println();
      int startIndex = result.indexOf(startIndicator);
      int endIndex = result.indexOf(endIndicator);
      if(startIndex == -1 || endIndex == -1){
        byte data2[40];
        for(int i = 0; i < 40; i++){
          byte inByte = Serial.read();
          data2[i] = inByte;
          }
          result += ((char*)data2);
          startIndex = result.indexOf(startIndicator);
          endIndex = result.indexOf(endIndicator);
        }
      String sIn = result.substring(startIndex, endIndex);
    float leftJoyY = 10;
    Serial.println("LEFT JOY: " + (String)leftJoyY);
    float leftJoyX = 10;
    float rightJoyX = 10;
    float rightJoyY = 10;
      //old code
    //Left Joystick X
    int sIndex = sIn.indexOf('=');
    int eIndex = sIn.indexOf(',');
    String temp = sIn.substring((sIndex+1),eIndex);
    leftJoyX = temp.toFloat();
    //Left Joystick Y
    sIndex = sIn.indexOf('=', eIndex);
    eIndex = sIn.indexOf('}');
    temp = sIn.substring((sIndex+1),eIndex);
    leftJoyY = temp.toFloat();
    //Right Joystick X
    sIndex = sIn.indexOf('=',eIndex);
    eIndex = sIn.indexOf(',',sIndex);
    temp = sIn.substring((sIndex+1),eIndex);
    rightJoyX = temp.toFloat();
    //Right Joystick Y
    sIndex = sIn.indexOf('=',eIndex);
    eIndex = sIn.indexOf('}',sIndex);
    temp = sIn.substring((sIndex+1),eIndex);
    rightJoyY = temp.toFloat();
    //Left Trigger
    sIndex = sIn.indexOf('|',eIndex);
    eIndex = sIn.indexOf('|',sIndex+1);
    temp = sIn.substring((sIndex+1),eIndex);
    float leftTrigger = temp.toFloat();
    //Right Trigger
    sIndex = sIn.indexOf('|',eIndex);
    eIndex = sIn.indexOf('|',sIndex+2);
    temp = sIn.substring((sIndex+1),eIndex);
    float rightTrigger = temp.toFloat();
    //Buttons
    sIndex = sIn.indexOf(eIndex);
    eIndex = sIn.indexOf('<',sIndex);
    temp = sIn.substring((sIndex+1),eIndex);
    String buttonsBS = temp;
    Serial.println("LEFT JOY: " + (String)leftJoyY);
    Serial.println("Left Trigger: " + (String)leftTrigger);
       Serial.println("Right Trigger: " + (String)rightTrigger);
    newData = false;
    if(leftJoyY != prevLY || rightJoyY != prevRY || leftTrigger != prevLeftTrigger || rightTrigger != prevRightTrigger){
         prevLY = leftJoyY;
         prevRY = rightJoyY;
         prevLeftTrigger = leftTrigger;
         prevRightTrigger = rightTrigger;
         Serial.println("MOVING WHEELS");
         Move(rightJoyY ,leftJoyY, rightTrigger, leftTrigger);

      } else{
        Serial.println("SAME VALUES, SKIPPING");
        }
  
        
        
      
     
   
    } else{
      ChangeStatus(4);
   //   Serial.println("not running serial code");
      }
  
  }

void ChangeStatus(int statusNum){
  switch(statusNum){
    case 0: //no serial data
    digitalWrite(r_pin, HIGH);
    digitalWrite(g_pin, LOW);
    digitalWrite(b_pin, LOW);
    break;
    case 1:
    digitalWrite(r_pin, LOW);
    digitalWrite(g_pin, HIGH);
    digitalWrite(b_pin, LOW);
    break;
    case 2:
    digitalWrite(r_pin, LOW);
    digitalWrite(g_pin, LOW);
    digitalWrite(b_pin, HIGH);
    break;
    case 3:
    digitalWrite(r_pin, LOW);
    digitalWrite(g_pin, HIGH);
    digitalWrite(b_pin, HIGH);
    break;
    case 4:
    analogWrite(r_pin, 128);
    analogWrite(g_pin, 255);
    digitalWrite(b_pin, LOW);
    break;
    }
  }

 void DriveMotor(int motor, bool dir){
  bool changeSpeed = true;
  switch(motor){
    case 1:
    if(dir){
      //backwards
      ChangeStatus(2);
      motor1Status = -1;
      digitalWrite(l_pin1, LOW);
      digitalWrite(l_pin2, HIGH);
      }else{
        //forwards
        motor1Status = 1;
        ChangeStatus(2);
        digitalWrite(l_pin1, HIGH);
      digitalWrite(l_pin2, LOW);
        }
        break;
    case 2:
    if(dir){
      ChangeStatus(3);
      motor2Status = -1;
      digitalWrite(r_pin1, HIGH);
      digitalWrite(r_pin2, LOW);
      }else{
        ChangeStatus(3);
        motor2Status = -1;
        digitalWrite(r_pin1, LOW);
      digitalWrite(r_pin2, HIGH);
        }
        break;
    case 3:
    if(dir){
      //left
      motor3Status = -1;
      ChangeStatus(4);
      digitalWrite(b_pin1, HIGH);
      digitalWrite(b_pin2, LOW);
      }else{
        //right
        ChangeStatus(4);
        motor3Status = 1;
        digitalWrite(b_pin1, LOW);
      digitalWrite(b_pin2, HIGH);
        }
        break;
    }
  }
 void SetSpeed(int motor, int newSpeed){
  switch(motor){
    case 1: //left
    if(newSpeed < 256 && newSpeed >= 0){
      l_speed = newSpeed;
      analogWrite(l_ena, l_speed);
      }
    break;  
    case 2: //right
    if(newSpeed < 256 && newSpeed >= 0){
      r_speed = newSpeed;
      analogWrite(r_ena, r_speed);
      }
    break;
    case 3: //back
    if(newSpeed < 256 && newSpeed >= 0){
      b_speed = newSpeed;
      analogWrite(b_ena, b_speed);
      }
    break;
    }
  }

 void StopMotor(int motor){
  //ChangeStatus(1);
  switch(motor){
    case 1: //left
    motor1Status = 0;
    digitalWrite(l_pin1, LOW);
    digitalWrite(l_pin2, LOW);
    break;  
    case 2: //right
    motor2Status = 0;
    digitalWrite(r_pin1, LOW);
    digitalWrite(r_pin2, LOW);
    break;
    case 3: //back
    ChangeStatus(4);
    motor3Status = 0;
    digitalWrite(b_pin1, LOW);
    digitalWrite(b_pin2, LOW);
    break;
    }
  }
