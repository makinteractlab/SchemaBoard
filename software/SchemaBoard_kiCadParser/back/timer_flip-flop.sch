EESchema Schematic File Version 4
LIBS:555 timer_flip-flop-cache
EELAYER 29 0
EELAYER END
$Descr A4 11693 8268
encoding utf-8
Sheet 1 1
Title ""
Date ""
Rev ""
Comp ""
Comment1 ""
Comment2 ""
Comment3 ""
Comment4 ""
$EndDescr
$Comp
L 555-timer_flip-flop-rescue:+3.3V-SchemaBoard_Library #PWR0101
U 1 1 5D4D6C05
P 1400 1200
F 0 "#PWR0101" H 1400 1050 50  0001 C CNN
F 1 "+3.3V" H 1488 1237 50  0000 L CNN
F 2 "" H 1400 1200 50  0001 C CNN
F 3 "" H 1400 1200 50  0001 C CNN
	1    1400 1200
	1    0    0    -1  
$EndComp
$Comp
L 555-timer_flip-flop-rescue:LM555-SchemaBoard_Library U1
U 1 1 5D4D75EC
P 4800 4400
F 0 "U1" H 4800 3933 50  0000 C CNN
F 1 "LM555" H 4800 4024 50  0000 C CNN
F 2 "" H 4800 4400 50  0001 C CNN
F 3 "http://www.ti.com/lit/ds/symlink/lm555.pdf" H 4800 4400 50  0001 C CNN
	1    4800 4400
	-1   0    0    1   
$EndComp
$Comp
L 555-timer_flip-flop-rescue:GND-SchemaBoard_Library #GND0101
U 1 1 5D4DA759
P 7200 1200
F 0 "#GND0101" H 7200 950 50  0001 C CNN
F 1 "GND" H 7288 1163 50  0000 L CNN
F 2 "" H 7200 1200 50  0001 C CNN
F 3 "" H 7200 1200 50  0001 C CNN
	1    7200 1200
	-1   0    0    1   
$EndComp
$Comp
L 555-timer_flip-flop-rescue:Resistor-SchemaBoard_Library R8
U 1 1 5D4DC2A4
P 5450 6200
F 0 "R8" V 5655 6200 50  0000 C CNN
F 1 "Resistor" V 5564 6200 50  0000 C CNN
F 2 "" H 5450 6200 50  0001 C CNN
F 3 "~" H 5450 6200 50  0001 C CNN
	1    5450 6200
	0    -1   -1   0   
$EndComp
$Comp
L 555-timer_flip-flop-rescue:Resistor-SchemaBoard_Library R6
U 1 1 5D4DD8CD
P 2850 2700
F 0 "R6" V 3055 2700 50  0000 C CNN
F 1 "Resistor" V 2964 2700 50  0000 C CNN
F 2 "" H 2850 2700 50  0001 C CNN
F 3 "~" H 2850 2700 50  0001 C CNN
	1    2850 2700
	0    -1   -1   0   
$EndComp
$Comp
L 555-timer_flip-flop-rescue:Resistor-SchemaBoard_Library R7
U 1 1 5D4DD38D
P 2250 4450
F 0 "R7" V 2455 4450 50  0000 C CNN
F 1 "Resistor" V 2364 4450 50  0000 C CNN
F 2 "" H 2250 4450 50  0001 C CNN
F 3 "~" H 2250 4450 50  0001 C CNN
	1    2250 4450
	0    -1   -1   0   
$EndComp
$Comp
L 555-timer_flip-flop-rescue:Resistor-SchemaBoard_Library R5
U 1 1 5D4DCCCE
P 2000 6500
F 0 "R5" V 2205 6500 50  0000 C CNN
F 1 "Resistor" V 2114 6500 50  0000 C CNN
F 2 "" H 2000 6500 50  0001 C CNN
F 3 "~" H 2000 6500 50  0001 C CNN
	1    2000 6500
	0    -1   -1   0   
$EndComp
$Comp
L 555-timer_flip-flop-rescue:LED-SchemaBoard_Library LED10
U 1 1 5D4E318A
P 3150 4450
F 0 "LED10" H 3143 4195 50  0000 C CNN
F 1 "LED" H 3143 4286 50  0000 C CNN
F 2 "" H 3150 4450 50  0001 C CNN
F 3 "~" H 3150 4450 50  0001 C CNN
	1    3150 4450
	-1   0    0    1   
$EndComp
$Comp
L 555-timer_flip-flop-rescue:SwitchPush-SchemaBoard_Library SW4
U 1 1 5D4E42D5
P 4800 2700
F 0 "SW4" H 4800 2985 50  0000 C CNN
F 1 "SwitchPush" H 4800 2894 50  0000 C CNN
F 2 "" H 4800 2900 50  0001 C CNN
F 3 "~" H 4800 2900 50  0001 C CNN
	1    4800 2700
	1    0    0    -1  
$EndComp
$Comp
L 555-timer_flip-flop-rescue:SwitchPush-SchemaBoard_Library SW3
U 1 1 5D4E4B63
P 2450 5550
F 0 "SW3" H 2450 5835 50  0000 C CNN
F 1 "SwitchPush" H 2450 5744 50  0000 C CNN
F 2 "" H 2450 5750 50  0001 C CNN
F 3 "~" H 2450 5750 50  0001 C CNN
	1    2450 5550
	1    0    0    -1  
$EndComp
$Comp
L 555-timer_flip-flop-rescue:LED-SchemaBoard_Library LED9
U 1 1 5D4E5243
P 4400 5600
F 0 "LED9" V 4439 5482 50  0000 R CNN
F 1 "LED" V 4348 5482 50  0000 R CNN
F 2 "" H 4400 5600 50  0001 C CNN
F 3 "~" H 4400 5600 50  0001 C CNN
	1    4400 5600
	0    -1   -1   0   
$EndComp
$Comp
L 555-timer_flip-flop-rescue:C-SchemaBoard_Library C2
U 1 1 5D4E5DD3
P 6100 5000
F 0 "C2" V 5848 5000 50  0000 C CNN
F 1 "C" V 5939 5000 50  0000 C CNN
F 2 "" H 6138 4850 50  0001 C CNN
F 3 "~" H 6100 5000 50  0001 C CNN
	1    6100 5000
	0    1    1    0   
$EndComp
Wire Wire Line
	1400 6500 1750 6500
Wire Wire Line
	3550 6500 3550 5550
Wire Wire Line
	3550 4600 4300 4600
Wire Wire Line
	2250 6500 3550 6500
Wire Wire Line
	3300 4450 3950 4450
Wire Wire Line
	2500 4450 3000 4450
Wire Wire Line
	2000 4450 1400 4450
Wire Wire Line
	1400 1200 1400 2700
Connection ~ 1400 4450
Wire Wire Line
	1400 4450 1400 6500
Wire Wire Line
	2650 5550 3550 5550
Connection ~ 3550 5550
Wire Wire Line
	3550 5550 3550 4600
Wire Wire Line
	5700 6200 7200 6200
Wire Wire Line
	5300 4450 7200 4450
Connection ~ 7200 4450
Wire Wire Line
	7200 4450 7200 5000
Wire Wire Line
	5300 4600 5950 4600
Wire Wire Line
	5950 4600 5950 5000
Wire Wire Line
	6250 5000 7200 5000
Connection ~ 7200 5000
Wire Wire Line
	7200 5000 7200 6200
Wire Wire Line
	3950 4450 3950 5450
Wire Wire Line
	3950 5450 4400 5450
Connection ~ 3950 4450
Wire Wire Line
	3950 4450 4300 4450
Wire Wire Line
	4400 5750 4400 6200
Wire Wire Line
	4400 6200 5200 6200
Wire Wire Line
	1800 5550 2250 5550
Wire Wire Line
	2600 2700 1400 2700
Connection ~ 1400 2700
Wire Wire Line
	1400 2700 1400 3250
Wire Wire Line
	3100 2700 3750 2700
Wire Wire Line
	5000 2700 7200 2700
Wire Wire Line
	7200 1200 7200 1650
Connection ~ 7200 2700
Wire Wire Line
	7200 2700 7200 4450
Wire Wire Line
	1400 3250 5300 3250
Wire Wire Line
	5300 3250 5300 4150
Connection ~ 1400 3250
Wire Wire Line
	1400 3250 1400 4450
Wire Wire Line
	3750 2700 3750 4300
Wire Wire Line
	3750 4300 4300 4300
Connection ~ 3750 2700
Wire Wire Line
	3750 2700 4600 2700
Wire Wire Line
	4300 4150 1800 4150
Wire Wire Line
	1800 1650 1800 4150
Connection ~ 1800 4150
Wire Wire Line
	1800 4150 1800 5550
Wire Wire Line
	1800 1650 7200 1650
Connection ~ 7200 1650
Wire Wire Line
	7200 1650 7200 2700
$EndSCHEMATC