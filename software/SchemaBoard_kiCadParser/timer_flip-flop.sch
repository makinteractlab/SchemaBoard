EESchema Schematic File Version 4
LIBS:timer_flip-flop-cache
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
L schemaboard:+9V #PWR0200
U 1 1 5D4D6C05
P 1500 2000
F 0 "#PWR0200" H 1500 1850 50  0001 C CNN
F 1 "+9V" H 1588 2037 50  0000 L CNN
F 2 "" H 1500 2000 50  0001 C CNN
F 3 "" H 1500 2000 50  0001 C CNN
	1    1500 2000
	1    0    0    -1  
$EndComp
$Comp
L schemaboard:GND #GND0100
U 1 1 5D4DA759
P 7300 1950
F 0 "#GND0100" H 7300 1700 50  0001 C CNN
F 1 "GND" H 7388 1913 50  0000 L CNN
F 2 "" H 7300 1950 50  0001 C CNN
F 3 "" H 7300 1950 50  0001 C CNN
	1    7300 1950
	-1   0    0    1   
$EndComp
$Comp
L schemaboard:Resistor R8
U 1 1 5D4DC2A4
P 5650 5900
F 0 "R8" V 5855 5900 50  0000 C CNN
F 1 "220" V 5764 5900 50  0000 C CNN
F 2 "" H 5650 5900 50  0001 C CNN
F 3 "~" H 5650 5900 50  0001 C CNN
	1    5650 5900
	0    1    1    0   
$EndComp
$Comp
L schemaboard:Resistor R7
U 1 1 5D4DD38D
P 2350 4300
F 0 "R7" V 2555 4300 50  0000 C CNN
F 1 "220" V 2464 4300 50  0000 C CNN
F 2 "" H 2350 4300 50  0001 C CNN
F 3 "~" H 2350 4300 50  0001 C CNN
	1    2350 4300
	0    1    1    0   
$EndComp
$Comp
L schemaboard:Resistor R5
U 1 1 5D4DCCCE
P 2350 5900
F 0 "R5" V 2555 5900 50  0000 C CNN
F 1 "10K" V 2464 5900 50  0000 C CNN
F 2 "" H 2350 5900 50  0001 C CNN
F 3 "~" H 2350 5900 50  0001 C CNN
	1    2350 5900
	0    1    1    0   
$EndComp
$Comp
L schemaboard:LED LED10
U 1 1 5D4E318A
P 3250 4300
F 0 "LED10" H 3243 4045 50  0000 C CNN
F 1 "LED" H 3243 4136 50  0000 C CNN
F 2 "" H 3250 4300 50  0001 C CNN
F 3 "~" H 3250 4300 50  0001 C CNN
	1    3250 4300
	-1   0    0    1   
$EndComp
$Comp
L schemaboard:SwitchPush SW4
U 1 1 5D4E42D5
P 5300 3300
F 0 "SW4" H 5300 3585 50  0000 C CNN
F 1 "button" H 5300 3494 50  0000 C CNN
F 2 "" H 5300 3500 50  0001 C CNN
F 3 "~" H 5300 3500 50  0001 C CNN
	1    5300 3300
	1    0    0    -1  
$EndComp
$Comp
L schemaboard:SwitchPush SW3
U 1 1 5D4E4B63
P 2800 5150
F 0 "SW3" H 2800 5435 50  0000 C CNN
F 1 "button" H 2800 5344 50  0000 C CNN
F 2 "" H 2800 5350 50  0001 C CNN
F 3 "~" H 2800 5350 50  0001 C CNN
	1    2800 5150
	1    0    0    -1  
$EndComp
$Comp
L schemaboard:LED LED9
U 1 1 5D4E5243
P 4300 5150
F 0 "LED9" V 4339 5032 50  0000 R CNN
F 1 "LED" V 4248 5032 50  0000 R CNN
F 2 "" H 4300 5150 50  0001 C CNN
F 3 "~" H 4300 5150 50  0001 C CNN
	1    4300 5150
	0    -1   -1   0   
$EndComp
$Comp
L schemaboard:C C2
U 1 1 5D4E5DD3
P 6700 5150
F 0 "C2" V 6448 5150 50  0000 C CNN
F 1 "0.01μF" V 6539 5150 50  0000 C CNN
F 2 "" H 6738 5000 50  0001 C CNN
F 3 "~" H 6700 5150 50  0001 C CNN
	1    6700 5150
	0    1    1    0   
$EndComp
Wire Wire Line
	2600 4300 3100 4300
Wire Wire Line
	2100 4300 1500 4300
Connection ~ 1500 4300
Wire Wire Line
	1500 4300 1500 5900
Wire Wire Line
	6850 5150 7300 5150
Wire Wire Line
	6050 5150 6550 5150
$Comp
L schemaboard:Resistor R6
U 1 1 5D4DD8CD
P 2800 3300
F 0 "R6" V 3005 3300 50  0000 C CNN
F 1 "10K" V 2914 3300 50  0000 C CNN
F 2 "" H 2800 3300 50  0001 C CNN
F 3 "~" H 2800 3300 50  0001 C CNN
	1    2800 3300
	0    1    1    0   
$EndComp
Wire Wire Line
	6050 4450 6050 5150
Wire Wire Line
	3650 4450 3650 5150
Wire Wire Line
	1900 5150 2600 5150
Wire Wire Line
	3000 5150 3650 5150
Wire Wire Line
	2600 5900 3650 5900
Wire Wire Line
	1500 5900 2100 5900
Wire Wire Line
	4300 3300 4300 4150
Wire Wire Line
	6050 2400 6050 4000
Connection ~ 3650 5150
Wire Wire Line
	3650 5150 3650 5900
Wire Wire Line
	2550 3300 1500 3300
Connection ~ 1500 3300
Wire Wire Line
	1500 3300 1500 4300
Wire Wire Line
	3050 3300 4300 3300
Connection ~ 7300 3300
Wire Wire Line
	4300 5900 5400 5900
Wire Wire Line
	5900 5900 7300 5900
Wire Wire Line
	7300 2650 7300 3300
Wire Wire Line
	1900 2650 7300 2650
Wire Wire Line
	1500 2400 1500 3300
Wire Wire Line
	1500 2400 6050 2400
Wire Wire Line
	1900 2650 1900 4000
Wire Wire Line
	1900 4000 1900 5150
Connection ~ 1900 4000
Wire Wire Line
	3400 4300 4300 4300
Wire Wire Line
	4300 4300 4300 5000
Connection ~ 4300 4300
Wire Wire Line
	4300 5300 4300 5900
$Comp
L schemaboard:LM555 U1-8
U 1 1 5D4D75EC
P 5300 4250
F 0 "U1-8" H 5300 3783 50  0000 C CNN
F 1 "LM555 Timer" H 5300 3874 50  0000 C CNN
F 2 "" H 5300 4250 50  0001 C CNN
F 3 "http://www.ti.com/lit/ds/symlink/lm555.pdf" H 5300 4250 50  0001 C CNN
	1    5300 4250
	-1   0    0    1   
$EndComp
Wire Wire Line
	7300 3300 7300 4300
Wire Wire Line
	5800 4000 6050 4000
Wire Wire Line
	5800 4300 7300 4300
Connection ~ 7300 4300
Wire Wire Line
	7300 4300 7300 5150
Wire Wire Line
	5800 4450 6050 4450
Wire Wire Line
	1900 4000 4800 4000
Wire Wire Line
	4300 4150 4800 4150
Wire Wire Line
	4300 4300 4800 4300
Wire Wire Line
	3650 4450 4800 4450
Connection ~ 7300 5150
Wire Wire Line
	7300 5150 7300 5900
Wire Wire Line
	5500 3300 7300 3300
Wire Wire Line
	5100 3300 4300 3300
Connection ~ 4300 3300
Wire Wire Line
	1500 2000 1500 2400
Connection ~ 1500 2400
Wire Wire Line
	7300 1950 7300 2650
Connection ~ 7300 2650
$EndSCHEMATC
