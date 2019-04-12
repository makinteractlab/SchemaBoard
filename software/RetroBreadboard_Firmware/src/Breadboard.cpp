#include "Breadboard.hpp"

Breadboard::Breadboard (uint8_t latchPin, uint8_t clockPin, uint8_t dataPin):
            latch(latchPin), clock(clockPin), data(dataPin)
{
    pinMode (latch, OUTPUT);
    pinMode (clock, OUTPUT);
    pinMode (data, OUTPUT);
}


void Breadboard::resetAll()
{
    setAllOff();
    update();
}

void Breadboard::setAllOff ()
{
    ledsLeft=0;
    ledsRight=0;
}

void Breadboard::setOn (uint8_t led)
{
    if (led <1 || led > LEDS) return;
    led--; // base 0

    uint16_t mask = ON << led % ROWS;
    if (led < ROWS) ledsLeft |= mask;
    else ledsRight |= mask;
}
    
void Breadboard::setOff (uint8_t led)
{
    if (led <1 || led > LEDS) return;
    led--; // base 0

    uint16_t mask = 0xFFFF ^ ( ON << led % ROWS);
    if (led < ROWS) ledsLeft &= mask;
    else ledsRight &= mask;
}
    
void Breadboard::toggle (uint8_t led)
{
    if (led <1 || led > LEDS) return;
    led--; // base 0

    uint16_t mask = ON << led % ROWS;
    if (led < ROWS) ledsLeft ^= mask;
    else ledsRight ^= mask;
}

void Breadboard::set (uint16_t left,uint16_t right)
{
    ledsLeft= left;
    ledsRight= right;
}

void Breadboard::update()
{
    Serial.println("==========");
    // Serial.println(ledsLeft, BIN);
    // Serial.println(ledsRight, BIN);
    digitalWrite (latch, LOW);
    byte selection= ledsRight >> LEDS_PER_CHIP;
    Serial.println (selection, BIN);
    shiftOut (data, clock, MSBFIRST, selection); 
    digitalWrite (latch, HIGH);


    digitalWrite (latch, LOW);
    selection= ledsRight;
    Serial.println (selection, BIN);
    shiftOut (data, clock, MSBFIRST, selection); 
    digitalWrite (latch, HIGH);

    digitalWrite (latch, LOW);
    selection= ledsLeft >> LEDS_PER_CHIP;
    Serial.println (selection, BIN);
    shiftOut (data, clock, MSBFIRST, selection); 
    digitalWrite (latch, HIGH);

    digitalWrite (latch, LOW);
    selection= ledsLeft;
    Serial.println (selection, BIN);
    shiftOut (data, clock, MSBFIRST, selection); 
    digitalWrite (latch, HIGH);
}