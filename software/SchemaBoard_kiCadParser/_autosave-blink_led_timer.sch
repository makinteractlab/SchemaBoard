EESchema Schematic File Version 4
LIBS:blink_led_timer-cache
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
L blink_led_timer-rescue:+3.3V-SchemaBoard_Library #PWR0200
U 1 1 5D5FB361
P 1550 2100
F 0 "#PWR0200" H 1550 1950 50  0001 C CNN
F 1 "+3.3V" H 1638 2137 50  0000 L CNN
F 2 "" H 1550 2100 50  0001 C CNN
F 3 "" H 1550 2100 50  0001 C CNN
	1    1550 2100
	1    0    0    -1  
$EndComp
$Comp
L blink_led_timer-rescue:C-SchemaBoard_Library C4
U 1 1 5D5FBA56
P 5150 5250
F 0 "C4" V 4898 5250 50  0000 C CNN
F 1 "0.01" V 4989 5250 50  0000 C CNN
F 2 "" H 5188 5100 50  0001 C CNN
F 3 "~" H 5150 5250 50  0001 C CNN
	1    5150 5250
	0    1    1    0   
$EndComp
$Comp
L blink_led_timer-rescue:LED-SchemaBoard_Library LED6
U 1 1 5D5FCF34
P 2350 5700
F 0 "LED6" V 2389 5582 50  0000 R CNN
F 1 "LED" V 2298 5582 50  0000 R CNN
F 2 "" H 2350 5700 50  0001 C CNN
F 3 "~" H 2350 5700 50  0001 C CNN
	1    2350 5700
	0    -1   -1   0   
$EndComp
$Comp
L blink_led_timer-rescue:CP-SchemaBoard_Library CP4
U 1 1 5D5FE6BF
P 6250 4800
F 0 "CP4" V 6505 4800 50  0000 C CNN
F 1 "10" V 6414 4800 50  0000 C CNN
F 2 "" H 6288 4650 50  0001 C CNN
F 3 "~" H 6250 4800 50  0001 C CNN
	1    6250 4800
	0    -1   -1   0   
$EndComp
$Comp
L blink_led_timer-rescue:Resistor-SchemaBoard_Library R7
U 1 1 5D5FFA2F
P 4000 6250
F 0 "R7" V 3795 6250 50  0000 C CNN
F 1 "220" V 3886 6250 50  0000 C CNN
F 2 "" H 4000 6250 50  0001 C CNN
F 3 "~" H 4000 6250 50  0001 C CNN
	1    4000 6250
	0    1    1    0   
$EndComp
Wire Wire Line
	2350 6250 2350 5850
Wire Wire Line
	2350 5550 2350 4150
Wire Wire Line
	2350 4150 2750 4150
Wire Wire Line
	2750 4300 1550 4300
Wire Wire Line
	2750 4000 2350 4000
Wire Wire Line
	6400 4800 7100 4800
$Comp
L blink_led_timer-rescue:LM555-SchemaBoard_Library U3
U 1 1 5D5FD651
P 3250 4100
F 0 "U3" H 3250 3633 50  0000 C CNN
F 1 "LM555" H 3250 3724 50  0000 C CNN
F 2 "" H 3250 4100 50  0001 C CNN
F 3 "http://www.ti.com/lit/ds/symlink/lm555.pdf" H 3250 4100 50  0001 C CNN
	1    3250 4100
	-1   0    0    1   
$EndComp
$Comp
L blink_led_timer-rescue:GND-SchemaBoard_Library #GND0100
U 1 1 5D5FC987
P 7100 2050
F 0 "#GND0100" H 7100 1800 50  0001 C CNN
F 1 "GND" H 7022 2013 50  0000 R CNN
F 2 "" H 7100 2050 50  0001 C CNN
F 3 "" H 7100 2050 50  0001 C CNN
	1    7100 2050
	-1   0    0    1   
$EndComp
$Comp
L blink_led_timer-rescue:Resistor-SchemaBoard_Library R1
U 1 1 5D60081A
P 5350 3500
F 0 "R1" V 5145 3500 50  0000 C CNN
F 1 "5.1K" V 5236 3500 50  0000 C CNN
F 2 "" H 5350 3500 50  0001 C CNN
F 3 "~" H 5350 3500 50  0001 C CNN
	1    5350 3500
	0    1    1    0   
$EndComp
$Comp
L blink_led_timer-rescue:Resistor-SchemaBoard_Library R2
U 1 1 5D600158
P 5350 4400
F 0 "R2" V 5145 4400 50  0000 C CNN
F 1 "4.7K" V 5236 4400 50  0000 C CNN
F 2 "" H 5350 4400 50  0001 C CNN
F 3 "~" H 5350 4400 50  0001 C CNN
	1    5350 4400
	0    1    1    0   
$EndComp
Wire Wire Line
	1550 2100 1550 2500
Wire Wire Line
	2750 3850 2050 3850
Wire Wire Line
	2050 3850 2050 2950
Wire Wire Line
	2050 2950 7100 2950
Connection ~ 7100 2950
Wire Wire Line
	7100 2950 7100 2050
Wire Wire Line
	3750 4150 4050 4150
Wire Wire Line
	6150 4000 3750 4000
Wire Wire Line
	5600 3500 6150 3500
Wire Wire Line
	6150 3500 6150 4000
Connection ~ 6150 4000
Wire Wire Line
	3750 3850 3750 3500
Wire Wire Line
	3750 3500 4600 3500
Wire Wire Line
	4450 4400 4450 4800
Wire Wire Line
	4450 4800 6100 4800
Wire Wire Line
	4050 4150 4050 3100
Wire Wire Line
	4050 3100 2350 3100
Wire Wire Line
	2350 3100 2350 4000
Wire Wire Line
	4600 3500 4600 2500
Wire Wire Line
	4600 2500 1550 2500
Connection ~ 4600 3500
Wire Wire Line
	4600 3500 5100 3500
Connection ~ 1550 2500
Wire Wire Line
	1550 2500 1550 4300
Wire Wire Line
	2350 6250 3750 6250
Wire Wire Line
	5000 5250 3750 5250
Wire Wire Line
	3750 4300 3750 5250
Wire Wire Line
	8550 5200 8550 5250
Connection ~ 7100 4800
Wire Wire Line
	7100 2950 7100 4800
Wire Wire Line
	4050 4400 4450 4400
Wire Wire Line
	7100 4800 7100 5250
Wire Wire Line
	5300 5250 7100 5250
Connection ~ 7100 5250
Wire Wire Line
	7100 5250 7100 6250
Connection ~ 4450 4400
Wire Wire Line
	4450 4400 5100 4400
Wire Wire Line
	4050 4150 4050 4400
Wire Wire Line
	6150 4000 6150 4400
Wire Wire Line
	5600 4400 6150 4400
Connection ~ 4050 4150
Wire Wire Line
	4250 6250 7100 6250
$EndSCHEMATC
