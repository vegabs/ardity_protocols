// Controlador ASCII
// read -> enviar estado del pulsador
// on -> prender led y enviar estado del led
// off -> apagar led y enviar estado del led

#define LED_PIN 2
#define BUTTON_PIN 3

void setup() {
  Serial.begin(115200);
  pinMode(LED_PIN, OUTPUT);
  pinMode(BUTTON_PIN, INPUT);
}

void protocoloASCII() {
  
  if (Serial.available() > 0) {
    String data = Serial.readStringUntil('\n');

    if (data == "read") {
      Serial.println(digitalRead(BUTTON_PIN));

    } else if (data == "on") {
      digitalWrite(LED_PIN, 1);
      Serial.println("prendido");

    } else if (data == "off") {
      digitalWrite(LED_PIN, 0);
      Serial.println("apagado");

    }
  }
}

void loop() {
  protocoloASCII();
}
