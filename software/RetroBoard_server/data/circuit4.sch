EESchema Schematic File Version 4
LIBS:circuit4-cache
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
L SchemaBoard_Library:8pinChip U1-8
U 1 1 5D4536D3
P 6250 4900
F 0 "U1-8" H 6250 5367 50  0000 C CNN
F 1 "8pinChip" H 6250 5276 50  0000 C CNN
F 2 "" H 6250 4900 50  0001 C CNN
F 3 "http://www.ti.com/lit/ds/symlink/lm555.pdf" H 6250 4900 50  0001 C CNN
	1    6250 4900
	1    0    0    -1  
$EndComp
$Comp
L SchemaBoard_Library:LED LED1
U 1 1 5D4552BA
P 3000 3850
F 0 "LED1" H 2993 4066 50  0000 C CNN
F 1 "LED" H 2993 3975 50  0000 C CNN
F 2 "" H 3000 3850 50  0001 C CNN
F 3 "~" H 3000 3850 50  0001 C CNN
	1    3000 3850
	1    0    0    -1  
$EndComp
$Comp
L SchemaBoard_Library:Battery_Cell BT1
U 1 1 5D455990
P 2950 5750
F 0 "BT1" V 2695 5800 50  0000 C CNN
F 1 "Battery_Cell" V 2786 5800 50  0000 C CNN
F 2 "" V 2950 5810 50  0001 C CNN
F 3 "~" V 2950 5810 50  0001 C CNN
	1    2950 5750
	0    1    1    0   
$EndComp
$Comp
L SchemaBoard_Library:CP CP1
U 1 1 5D45475C
P 3000 1300
F 0 "CP1" V 2745 1300 50  0000 C CNN
F 1 "CP" V 2836 1300 50  0000 C CNN
F 2 "" H 3038 1150 50  0001 C CNN
F 3 "~" H 3000 1300 50  0001 C CNN
	1    3000 1300
	0    1    1    0   
$EndComp
Wire Wire Line
	5150 5000 5750 5000
Wire Wire Line
	2850 5750 1150 5750
Wire Wire Line
	1150 1300 2850 1300
Wire Wire Line
	3150 1300 5150 1300
Wire Wire Line
	1150 3850 2850 3850
Wire Wire Line
	3150 3850 5150 3850
$Comp
L SchemaBoard_Library:R_PHOTO LDR2
U 1 1 5D4244B8
P 1150 2400
F 0 "LDR2" H 1220 2446 50  0000 L CNN
F 1 "R_PHOTO" H 1220 2355 50  0000 L CNN
F 2 "" V 1200 2150 50  0001 L CNN
F 3 "~" H 1150 2350 50  0001 C CNN
	1    1150 2400
	1    0    0    -1  
$EndComp
$Comp
L SchemaBoard_Library:Resistor R1
U 1 1 5D42507A
P 5150 2300
F 0 "R1" H 5218 2346 50  0000 L CNN
F 1 "Resistor" H 5218 2255 50  0000 L CNN
F 2 "" H 5150 2300 50  0001 C CNN
F 3 "~" H 5150 2300 50  0001 C CNN
	1    5150 2300
	1    0    0    -1  
$EndComp
Wire Wire Line
	5150 1300 5150 2050
Wire Wire Line
	1150 1300 1150 2250
Wire Wire Line
	7600 5000 6750 5000
Wire Wire Line
	3150 5750 7600 5750
Connection ~ 1150 3850
Wire Wire Line
	1150 2550 1150 3850
Wire Wire Line
	5150 2550 5150 3850
Wire Wire Line
	1150 3850 1150 5750
Wire Wire Line
	5150 3850 5150 5000
Wire Wire Line
	7600 5000 7600 5750
Connection ~ 5150 3850
$EndSCHEMATC
