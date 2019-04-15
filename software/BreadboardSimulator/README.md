POST REQUEST
http://127.0.0.1:8081/set

COMMANDS

// RESET - ALL OFF
{"cmd": "reset"}

// SINGLE PIN ON [1-32]
{"cmd": "on", "data": 1}

// SINGLE PIN OFF [1-32]
{"cmd": "off", "data": 1}

// SINGLE PIN TOGGLE [1-32]
{"cmd": "toggle", "data": 1}

// SINGLE PIN BLINK [1-32]
{"cmd": "blink", "data": 1}

// SET ALL PINS to either be ON/OFF
{"cmd": "set", "left": 0, "right":7}

// SET ALL PINS to either be ON/OFF or to BLINK/NO_BLINK
{"cmd": "set", "left": 0, "right":7, "leftBlink":65535, "rightBlink":0}