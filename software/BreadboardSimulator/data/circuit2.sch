EESchema Schematic File Version 4
LIBS:circuit2-cache
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
L SchemaBoard_Library:C C1
U 1 1 5D3E4A94
P 1950 2050
F 0 "C1" H 2065 2096 50  0000 L CNN
F 1 "C" H 2065 2005 50  0000 L CNN
F 2 "" H 1988 1900 50  0001 C CNN
F 3 "~" H 1950 2050 50  0001 C CNN
	1    1950 2050
	0    1    1    0   
$EndComp
$Comp
L SchemaBoard_Library:+3.3V PWR1
U 1 1 5D3E25B9
P 3050 1700
F 0 "PWR1" H 3050 1550 50  0001 C CNN
F 1 "+3.3V" H 3138 1737 50  0000 L CNN
F 2 "" H 3050 1700 50  0001 C CNN
F 3 "" H 3050 1700 50  0001 C CNN
	1    3050 1700
	1    0    0    -1  
$EndComp
$Comp
L SchemaBoard_Library:R_PHOTO LDR1
U 1 1 5D3CE114
P 4300 2050
F 0 "LDR1" V 3975 2050 50  0000 C CNN
F 1 "R_PHOTO" V 4066 2050 50  0000 C CNN
F 2 "" V 4350 1800 50  0001 L CNN
F 3 "~" H 4300 2000 50  0001 C CNN
	1    4300 2050
	0    1    1    0   
$EndComp
$Comp
L SchemaBoard_Library:Resistor R1
U 1 1 5D3CF2F7
P 5900 2050
F 0 "R1" V 5695 2050 50  0000 C CNN
F 1 "Resistor" V 5786 2050 50  0000 C CNN
F 2 "" H 5900 2050 50  0001 C CNN
F 3 "~" H 5900 2050 50  0001 C CNN
	1    5900 2050
	0    1    1    0   
$EndComp
$Comp
L SchemaBoard_Library:Speaker SP1
U 1 1 5D3CD94C
P 8150 2050
F 0 "SP1" H 8320 2046 50  0000 L CNN
F 1 "Speaker" H 8320 1955 50  0000 L CNN
F 2 "" H 8150 1850 50  0001 C CNN
F 3 "~" H 8140 2000 50  0001 C CNN
	1    8150 2050
	1    0    0    -1  
$EndComp
$Comp
L SchemaBoard_Library:OpAmp OPAMP1
U 1 1 5D3E6608
P 8700 3500
F 0 "OPAMP1" H 8725 3867 50  0000 C CNN
F 1 "OpAmp" H 8725 3776 50  0000 C CNN
F 2 "" H 8700 3500 50  0001 C CNN
F 3 "~" H 8700 3500 50  0001 C CNN
	1    8700 3500
	1    0    0    -1  
$EndComp
$Comp
L SchemaBoard_Library:6pinRelay U2-6
U 1 1 5D3E2F18
P 4700 3750
F 0 "U2-6" V 4033 3750 50  0000 C CNN
F 1 "6pinRelay" V 4124 3750 50  0000 C CNN
F 2 "Relay_THT" H 5200 3650 50  0001 L CNN
F 3 "https://standexelectronics.com/wp-content/uploads/datasheet_reed_relay_DIP.pdf" H 4600 3750 50  0001 C CNN
	1    4700 3750
	0    1    1    0   
$EndComp
$Comp
L SchemaBoard_Library:8pinChip U1-8
U 1 1 5D3CCD89
P 6600 3650
F 0 "U1-8" H 6600 4117 50  0000 C CNN
F 1 "8pinChip" H 6600 4026 50  0000 C CNN
F 2 "" H 6600 3650 50  0001 C CNN
F 3 "http://www.ti.com/lit/ds/symlink/lm555.pdf" H 6600 3650 50  0001 C CNN
	1    6600 3650
	1    0    0    -1  
$EndComp
$Comp
L SchemaBoard_Library:16pinChip U3-16
U 1 1 5D3E3E98
P 2000 3650
F 0 "U3-16" H 2000 4417 50  0000 C CNN
F 1 "16pinChip" H 2000 4326 50  0000 C CNN
F 2 "" H 2000 3250 50  0001 C CNN
F 3 "" H 2000 3250 50  0001 C CNN
	1    2000 3650
	1    0    0    -1  
$EndComp
$Comp
L SchemaBoard_Library:Diode D1
U 1 1 5D3E50B4
P 2550 5650
F 0 "D1" H 2550 5866 50  0000 C CNN
F 1 "Diode" H 2550 5775 50  0000 C CNN
F 2 "" H 2550 5650 50  0001 C CNN
F 3 "~" H 2550 5650 50  0001 C CNN
	1    2550 5650
	1    0    0    -1  
$EndComp
$Comp
L SchemaBoard_Library:SwitchPush SW1
U 1 1 5D3E6FAF
P 3750 5650
F 0 "SW1" H 3750 5935 50  0000 C CNN
F 1 "SwitchPush" H 3750 5844 50  0000 C CNN
F 2 "" H 3750 5850 50  0001 C CNN
F 3 "~" H 3750 5850 50  0001 C CNN
	1    3750 5650
	1    0    0    -1  
$EndComp
$Comp
L SchemaBoard_Library:CP CP1
U 1 1 5D3FE055
P 4450 6850
F 0 "CP1" H 4568 6896 50  0000 L CNN
F 1 "CP" H 4568 6805 50  0000 L CNN
F 2 "" H 4488 6700 50  0001 C CNN
F 3 "~" H 4450 6850 50  0001 C CNN
	1    4450 6850
	0    1    1    0   
$EndComp
$Comp
L SchemaBoard_Library:Transistor Q1
U 1 1 5D3CFB42
P 7250 5500
F 0 "Q1" H 7441 5546 50  0000 L CNN
F 1 "Transistor" H 7441 5455 50  0000 L CNN
F 2 "" H 7450 5375 50  0001 L CIN
F 3 "http://www.onsemi.com/pub_link/Collateral/2N2219-D.PDF" H 7250 5500 50  0001 L CNN
	1    7250 5500
	1    0    0    -1  
$EndComp
$Comp
L SchemaBoard_Library:Inductor L1
U 1 1 5D3E5584
P 10050 5450
F 0 "L1" H 10103 5496 50  0000 L CNN
F 1 "Inductor" H 10103 5405 50  0000 L CNN
F 2 "" H 10050 5450 50  0001 C CNN
F 3 "~" H 10050 5450 50  0001 C CNN
	1    10050 5450
	1    0    0    -1  
$EndComp
$Comp
L SchemaBoard_Library:GND GND1
U 1 1 5D3D0965
P 950 6850
F 0 "GND1" H 950 6600 50  0001 C CNN
F 1 "GND" H 955 6677 50  0000 C CNN
F 2 "" H 950 6850 50  0001 C CNN
F 3 "" H 950 6850 50  0001 C CNN
	1    950  6850
	1    0    0    -1  
$EndComp
$Comp
L SchemaBoard_Library:LED LED1
U 1 1 5D3E5DAB
P 1600 5650
F 0 "LED1" H 1593 5866 50  0000 C CNN
F 1 "LED" H 1593 5775 50  0000 C CNN
F 2 "" H 1600 5650 50  0001 C CNN
F 3 "~" H 1600 5650 50  0001 C CNN
	1    1600 5650
	1    0    0    -1  
$EndComp
$Comp
L SchemaBoard_Library:Battery_Cell BT1
U 1 1 5D3EC35E
P 5450 6150
F 0 "BT1" V 5195 6200 50  0000 C CNN
F 1 "Battery_Cell" V 5286 6200 50  0000 C CNN
F 2 "" V 5450 6210 50  0001 C CNN
F 3 "~" V 5450 6210 50  0001 C CNN
	1    5450 6150
	0    1    1    0   
$EndComp
Wire Wire Line
	3050 1700 3050 3150
Wire Wire Line
	3050 3150 2500 3150
Wire Wire Line
	2100 2050 4150 2050
Wire Wire Line
	4450 2050 5650 2050
Wire Wire Line
	1800 2050 900  2050
Wire Wire Line
	900  2050 900  3300
Wire Wire Line
	900  3300 1500 3300
Wire Wire Line
	6150 2050 7950 2050
Wire Wire Line
	7950 2150 7400 2150
Wire Wire Line
	7400 2150 7400 3450
Wire Wire Line
	7400 3450 7100 3450
Wire Wire Line
	7100 3600 8400 3600
Wire Wire Line
	9050 3400 10050 3400
Wire Wire Line
	10050 3400 10050 5300
Wire Wire Line
	10050 5600 10050 6150
Wire Wire Line
	10050 6150 7350 6150
Wire Wire Line
	7350 6150 7350 5700
Wire Wire Line
	7100 3750 7350 3750
Wire Wire Line
	7350 3750 7350 5300
Wire Wire Line
	5000 3450 6100 3450
Wire Wire Line
	6100 3750 5500 3750
Wire Wire Line
	5500 3750 5500 4150
Wire Wire Line
	5500 4150 5000 4150
Wire Wire Line
	4400 4150 4200 4150
Wire Wire Line
	4200 4150 4200 5650
Wire Wire Line
	4200 5650 3950 5650
Wire Wire Line
	2700 5650 3150 5650
Wire Wire Line
	1750 5650 2400 5650
Wire Wire Line
	5650 6150 7350 6150
Connection ~ 7350 6150
Wire Wire Line
	5350 6150 4900 6150
Wire Wire Line
	4900 6150 4900 6850
Wire Wire Line
	4900 6850 4600 6850
Wire Wire Line
	4300 6850 3150 6850
Wire Wire Line
	3150 6850 3150 5650
Connection ~ 3150 5650
Wire Wire Line
	3150 5650 3550 5650
Wire Wire Line
	1450 5650 950  5650
Wire Wire Line
	950  5650 950  6850
Wire Wire Line
	7100 3900 7850 3900
Wire Wire Line
	7850 3900 7850 3800
Wire Wire Line
	7850 3800 8400 3800
$EndSCHEMATC