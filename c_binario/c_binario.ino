// Controlador Binario
// R -> enviar estado del pulsador
// + -> prender led y enviar estado del led
// - -> apagar led y enviar estado del led

#define LED_PIN 2
#define BUTTON_PIN 3

void setup() {
  Serial.begin(115200);
  pinMode(LED_PIN, OUTPUT);
  pinMode(BUTTON_PIN, INPUT);
}

void protocoloBinario() {
  if (Serial.available()) {
    byte myBuffer[1];
    Serial.readBytes(myBuffer, 1);
    byte data = myBuffer[0];

    switch (data) {
      case 0x52: // read
        Serial.write(digitalRead(BUTTON_PIN));
        Serial.write(0x5A);
        break;
      case 0x2B: // + >> prender led
        digitalWrite(LED_PIN, 1);
        Serial.write(0x50);
        Serial.write(0x5A);
        break;
      case 0x2D: // - >> apagar led
        digitalWrite(LED_PIN, 0);
        Serial.write(0x41);
        Serial.write(0x5A);
        break;
    }
  }
}

void loop() {
  protocoloBinario();
}
