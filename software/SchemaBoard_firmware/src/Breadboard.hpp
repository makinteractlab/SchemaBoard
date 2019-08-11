#ifndef __BREADBOARD__H__
#define __BREADBOARD__H__

#include "Arduino.h"
#include "constants.hpp"


class Breadboard
{
    public:
    Breadboard (uint8_t latchPin, uint8_t clockPin, uint8_t dataPin);
    void resetAll();

    void set (uint16_t left,uint16_t right);
    void setAllOff ();
    void setOn (uint8_t led);
    void setOff (uint8_t led);
    void toggle (uint8_t led);

    void update();

    private:
    uint8_t latch, clock, data;
    uint16_t ledsLeft, ledsRight;
};

#endif