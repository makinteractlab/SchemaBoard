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
    blinkOn= false;
    ledsLeft=0;
    ledsRight=0;
    ledsLeftBlink=0;
    ledsRightBlink=0;
}

void Breadboard::setOn (uint8_t led)
{
    if (led <1 || led > LEDS) return;
    led--; // base 0

    uint16_t mask = ON << led % ROWS;
    uint16_t offMask = 0xFFFF ^ ( ON << led % ROWS);

    if (led < ROWS) 
    {
        ledsLeft |= mask;
        ledsLeftBlink &= offMask;
    } else {
        ledsRight |= mask;
        ledsRightBlink &= offMask;
    }
}
    
void Breadboard::setOff (uint8_t led)
{
    if (led <1 || led > LEDS) return;
    led--; // base 0

    uint16_t mask = 0xFFFF ^ ( ON << led % ROWS);
    if (led < ROWS) 
    {
        ledsLeft &= mask;
        ledsLeftBlink &= mask;
    } else {
        ledsRight &= mask;
        ledsRightBlink &= mask;
    }
}
    
void Breadboard::toggle (uint8_t led)
{
    if (led <1 || led > LEDS) return;
    led--; // base 0

    uint16_t mask = ON << led % ROWS;
    uint16_t offMask = 0xFFFF ^ ( ON << led % ROWS);

    if (led < ROWS) 
    {
        ledsLeft ^= mask;
        ledsLeftBlink &= offMask;
    } else {
        ledsRight ^= mask;
        ledsRightBlink &= offMask;
    }
}

void Breadboard::blink (uint8_t led)
{
    if (led <1 || led > LEDS) return;
    led--; // base 0

    uint16_t mask = ON << led % ROWS;

    if (led < ROWS) ledsLeftBlink |= mask;
    else ledsRightBlink |= mask;
}

void Breadboard::set (uint16_t left,uint16_t right)
{
    ledsLeft= left;
    ledsRight= right;
    ledsLeftBlink&= (0xFFFF ^ ledsLeft);
    ledsRightBlink&= (0xFFFF ^ ledsRight);
}

void Breadboard::set (uint16_t left, uint16_t right, uint16_t leftBlink ,uint16_t rightBlink)
{
    ledsLeft= left;
    ledsRight= right;
    ledsLeftBlink= leftBlink;
    ledsRightBlink= rightBlink;
}

void Breadboard::blinkUpdate()
{
    if (ledsLeftBlink == 0 && ledsRightBlink == 0) return;
    if (blinkOn)
    {
        ledsLeft |= ledsLeftBlink;
        ledsRight |= ledsRightBlink;
    }else{
        ledsLeft &= (0xFFFF ^ ledsLeftBlink);
        ledsRight &= (0xFFFF ^ ledsRightBlink);
    }
    update();
    blinkOn= !blinkOn;
}   

void Breadboard::update()
{
    digitalWrite (latch, LOW);
    byte selection= ledsRight >> LEDS_PER_CHIP;
    shiftOut (data, clock, MSBFIRST, selection); 

    selection= ledsRight;
    shiftOut (data, clock, MSBFIRST, selection); 

    selection= ledsLeft >> LEDS_PER_CHIP;
    shiftOut (data, clock, MSBFIRST, selection); 

    selection= ledsLeft;
    shiftOut (data, clock, MSBFIRST, selection); 
    digitalWrite (latch, HIGH);
}