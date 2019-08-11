EESchema Schematic File Version 4
LIBS:timer_RC-cache
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
L timer_RC-rescue:Resistor-SchemaBoard_Library-555-timer_RC-rescue R4
U 1 1 5D4DD38D
P 4750 2900
F 0 "R4" H 4682 2854 50  0000 R CNN
F 1 "Resistor" H 4682 2945 50  0000 R CNN
F 2 "" H 4750 2900 50  0001 C CNN
F 3 "~" H 4750 2900 50  0001 C CNN
	1    4750 2900
	-1   0    0    1   
$EndComp
$Comp
L timer_RC-rescue:Resistor-SchemaBoard_Library-555-timer_RC-rescue R5
U 1 1 5D4DCCCE
P 6000 2900
F 0 "R5" H 5932 2854 50  0000 R CNN
F 1 "Resistor" H 5932 2945 50  0000 R CNN
F 2 "" H 6000 2900 50  0001 C CNN
F 3 "~" H 6000 2900 50  0001 C CNN
	1    6000 2900
	-1   0    0    1   
$EndComp
$Comp
L timer_RC-rescue:Resistor-SchemaBoard_Library-555-timer_RC-rescue R3
U 1 1 5D500A29
P 3900 6750
F 0 "R3" V 3695 6750 50  0000 C CNN
F 1 "Resistor" V 3786 6750 50  0000 C CNN
F 2 "" H 3900 6750 50  0001 C CNN
F 3 "~" H 3900 6750 50  0001 C CNN
	1    3900 6750
	0    1    1    0   
$EndComp
$Comp
L timer_RC-rescue:C-SchemaBoard_Library-555-timer_RC-rescue C9
U 1 1 5D501BEA
P 4750 4900
F 0 "C9" V 4498 4900 50  0000 C CNN
F 1 "C" V 4589 4900 50  0000 C CNN
F 2 "" H 4788 4750 50  0001 C CNN
F 3 "~" H 4750 4900 50  0001 C CNN
	1    4750 4900
	0    1    1    0   
$EndComp
$Comp
L timer_RC-rescue:CP-SchemaBoard_Library-555-timer_RC-rescue CP7
U 1 1 5D503BC1
P 4150 5500
F 0 "CP7" V 3895 5500 50  0000 C CNN
F 1 "CP" V 3986 5500 50  0000 C CNN
F 2 "" H 4188 5350 50  0001 C CNN
F 3 "~" H 4150 5500 50  0001 C CNN
	1    4150 5500
	0    1    1    0   
$EndComp
$Comp
L timer_RC-rescue:CP-SchemaBoard_Library-555-timer_RC-rescue CP6
U 1 1 5D5048EF
P 2500 6750
F 0 "CP6" V 2755 6750 50  0000 C CNN
F 1 "CP" V 2664 6750 50  0000 C CNN
F 2 "" H 2538 6600 50  0001 C CNN
F 3 "~" H 2500 6750 50  0001 C CNN
	1    2500 6750
	0    -1   -1   0   
$EndComp
$Comp
L timer_RC-rescue:Speaker-SchemaBoard_Library-555-timer_RC-rescue SP8
U 1 1 5D5051E9
P 5300 6950
F 0 "SP8" V 5217 7130 50  0000 L CNN
F 1 "Speaker" V 5308 7130 50  0000 L CNN
F 2 "" H 5300 6750 50  0001 C CNN
F 3 "~" H 5290 6900 50  0001 C CNN
	1    5300 6950
	0    1    1    0   
$EndComp
Wire Wire Line
	3300 1800 5600 1800
Wire Wire Line
	6000 1800 6750 1800
Wire Wire Line
	6750 1800 6750 2250
Wire Wire Line
	6750 5500 4300 5500
Wire Wire Line
	3850 4400 4300 4400
Wire Wire Line
	4300 4400 4300 4900
Wire Wire Line
	4300 4900 4600 4900
Wire Wire Line
	4900 4900 6150 4900
Wire Wire Line
	3300 1800 3300 1450
Wire Wire Line
	5300 6750 6150 6750
Wire Wire Line
	4150 6750 5200 6750
Wire Wire Line
	2650 6750 3650 6750
Wire Wire Line
	2850 4250 1950 4250
Wire Wire Line
	1950 6750 2350 6750
Wire Wire Line
	3400 5500 3400 5000
Wire Wire Line
	3400 5000 2450 5000
Wire Wire Line
	2450 5000 2450 4400
Wire Wire Line
	2450 4400 2850 4400
Connection ~ 3400 5500
Wire Wire Line
	3400 5500 4000 5500
Wire Wire Line
	3850 4250 4200 4250
Wire Wire Line
	4850 4250 4850 4550
Wire Wire Line
	6150 4550 6150 4900
Connection ~ 6150 4900
Wire Wire Line
	2850 4100 2300 4100
Wire Wire Line
	2300 4100 2300 3250
Wire Wire Line
	2300 3250 4200 3250
Wire Wire Line
	4200 3250 4200 4250
Connection ~ 4200 4250
Wire Wire Line
	4200 4250 4850 4250
Connection ~ 4850 4250
Wire Wire Line
	6000 3150 6000 4250
Wire Wire Line
	4850 4250 6000 4250
Wire Wire Line
	3850 4100 4750 4100
Wire Wire Line
	5450 4100 5450 2650
Wire Wire Line
	5450 2650 6000 2650
Wire Wire Line
	4750 3150 4750 4100
Connection ~ 4750 4100
Wire Wire Line
	4750 4100 5450 4100
Wire Wire Line
	4750 2650 4750 2250
Wire Wire Line
	4750 2250 6750 2250
Connection ~ 6750 2250
Wire Wire Line
	6750 2250 6750 5500
Wire Wire Line
	3850 3950 4450 3950
Wire Wire Line
	4450 3950 4450 2250
Wire Wire Line
	4450 2250 4750 2250
Connection ~ 4750 2250
Connection ~ 6150 6750
Wire Wire Line
	6150 6750 6150 7300
Wire Wire Line
	6150 4900 6150 6750
$Comp
L timer_RC-rescue:C-SchemaBoard_Library-555-timer_RC-rescue C10
U 1 1 5D5010FC
P 5650 4550
F 0 "C10" V 5398 4550 50  0000 C CNN
F 1 "C" V 5489 4550 50  0000 C CNN
F 2 "" H 5688 4400 50  0001 C CNN
F 3 "~" H 5650 4550 50  0001 C CNN
	1    5650 4550
	0    1    1    0   
$EndComp
Wire Wire Line
	4850 4550 5500 4550
Wire Wire Line
	5800 4550 6150 4550
Wire Wire Line
	2150 1800 2150 1450
Wire Wire Line
	1500 1800 2150 1800
Wire Wire Line
	6150 7300 1500 7300
Wire Wire Line
	2950 5500 3400 5500
$Comp
L timer_RC-rescue:Resistor-SchemaBoard_Library-555-timer_RC-rescue R2
U 1 1 5D4DC2A4
P 2700 5500
F 0 "R2" V 2905 5500 50  0000 C CNN
F 1 "Resistor" V 2814 5500 50  0000 C CNN
F 2 "" H 2700 5500 50  0001 C CNN
F 3 "~" H 2700 5500 50  0001 C CNN
	1    2700 5500
	0    -1   -1   0   
$EndComp
Wire Wire Line
	1950 4250 1950 6750
Wire Wire Line
	1500 3950 1500 1800
Connection ~ 1500 3950
Wire Wire Line
	1500 5500 1500 3950
Connection ~ 1500 5500
Wire Wire Line
	1500 7300 1500 5500
Wire Wire Line
	2850 3950 1500 3950
Wire Wire Line
	2450 5500 1500 5500
$Comp
L timer_RC-rescue:+3.3V-SchemaBoard_Library-555-timer_RC-rescue PWR30
U 1 1 5D4D6C05
P 3300 1450
F 0 "PWR30" H 3300 1300 50  0001 C CNN
F 1 "+3.3V-SchemaBoard_Library-555-timer_RC-rescue" H 3388 1487 50  0001 L CNN
F 2 "" H 3300 1450 50  0001 C CNN
F 3 "" H 3300 1450 50  0001 C CNN
	1    3300 1450
	1    0    0    -1  
$EndComp
$Comp
L timer_RC-rescue:GND-SchemaBoard_Library-555-timer_RC-rescue GND20
U 1 1 5D4DA759
P 2150 1450
F 0 "GND20" H 2150 1200 50  0001 C CNN
F 1 "GND-SchemaBoard_Library-555-timer_RC-rescue" H 2238 1413 50  0001 L CNN
F 2 "" H 2150 1450 50  0001 C CNN
F 3 "" H 2150 1450 50  0001 C CNN
	1    2150 1450
	-1   0    0    1   
$EndComp
$Comp
L timer_RC-rescue:SwitchPush-SchemaBoard_Library-555-timer_RC-rescue SW11
U 1 1 5D4E4B63
P 5800 1800
F 0 "SW11" H 5800 2085 50  0000 C CNN
F 1 "SwitchPush" H 5800 1994 50  0000 C CNN
F 2 "" H 5800 2000 50  0001 C CNN
F 3 "~" H 5800 2000 50  0001 C CNN
	1    5800 1800
	1    0    0    -1  
$EndComp
$Comp
L timer_RC-rescue:LM555-SchemaBoard_Library-555-timer_RC-rescue U1-8
U 1 1 5D4D75EC
P 3350 4200
F 0 "U1-8" H 3350 3733 50  0000 C CNN
F 1 "LM555" H 3350 3824 50  0000 C CNN
F 2 "" H 3350 4200 50  0001 C CNN
F 3 "http://www.ti.com/lit/ds/symlink/lm555.pdf" H 3350 4200 50  0001 C CNN
	1    3350 4200
	-1   0    0    1   
$EndComp
$EndSCHEMATC
