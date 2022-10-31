// Timer1 Interrupt
void setup() {
  Serial.begin(115200);
  delay(100);  
  pinMode(2, INPUT_PULLUP);
  pinMode(3, OUTPUT); 
  pinMode(5, OUTPUT); 
  pinMode(6, OUTPUT); 
  pinMode(11, OUTPUT); 
  analogWrite(3,128);
  analogWrite(5,1);
  analogWrite(6,128);
  analogWrite(11,128);
  attachInterrupt(digitalPinToInterrupt(2),LimitsensingR,RISING);  
  Timer_initial();
}

unsigned int pulse_value=0;

void LimitsensingR()
{
  pulse_value++;
}

void Timer_initial()
{
  // Timer0
  /*TCCR0A=0x00;
  TCCR0B=0x07;
  TIFR0=0x00;
  TCNT0=0x00;
  //GTCCR=0x80;  */
   
  // Timer1: a 16bit counter
  // Page 170, 
  TCCR1A=0x40;
  // Page 173, f_io/1024, f_io=16MHz
  TCCR1B=0x05;
  // Page 175, 
  TCCR1C=0x80;
  // TC1 Interrupt Flag Register, Page 185
  // Set OCFA
  TIFR1=0x02;
  // Clear counter value
  TCNT1=0x0000;

  // Timer/Counter 1 Interrupt Mask Register, Page 184
  // Set OCIEA
  TIMSK1=0x02;
  // Global Interrupt Enable, Page 27
  SREG=0x80;
  //GTCCR=0x00;

  OCR1AH = 0x3D; 
  OCR1AL = 0x09; 
}

unsigned int frequency;

//Others:TIMER1_CAPT_vect,TIMER1_COMPB_vect, TIMER1_COMPA_vect,TIMER1_OVF_vect
ISR(TIMER1_COMPA_vect) 
{
  frequency=pulse_value;
  pulse_value=0;
  TCNT1=0x0000;  
  Serial.println(frequency);  
}

void loop() {
}
