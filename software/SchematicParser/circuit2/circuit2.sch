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
P 1850 1650
F 0 "C1" H 1965 1696 50  0000 L CNN
F 1 "C" H 1965 1605 50  0000 L CNN
F 2 "" H 1888 1500 50  0001 C CNN
F 3 "~" H 1850 1650 50  0001 C CNN
	1    1850 1650
	1    0    0    -1  
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
P 4550 1800
F 0 "LDR1" V 4225 1800 50  0000 C CNN
F 1 "R_PHOTO" V 4316 1800 50  0000 C CNN
F 2 "" V 4600 1550 50  0001 L CNN
F 3 "~" H 4550 1750 50  0001 C CNN
	1    4550 1800
	0    1    1    0   
$EndComp
$Comp
L SchemaBoard_Library:Resistor R1
U 1 1 5D3CF2F7
P 6050 1750
F 0 "R1" V 5845 1750 50  0000 C CNN
F 1 "Resistor" V 5936 1750 50  0000 C CNN
F 2 "" H 6050 1750 50  0001 C CNN
F 3 "~" H 6050 1750 50  0001 C CNN
	1    6050 1750
	-1   0    0    1   
$EndComp
$Comp
L SchemaBoard_Library:Speaker SP1
U 1 1 5D3CD94C
P 7450 1600
F 0 "SP1" H 7620 1596 50  0000 L CNN
F 1 "Speaker" H 7620 1505 50  0000 L CNN
F 2 "" H 7450 1400 50  0001 C CNN
F 3 "~" H 7440 1550 50  0001 C CNN
	1    7450 1600
	1    0    0    -1  
$EndComp
$Comp
L SchemaBoard_Library:OpAmp OPAMP1
U 1 1 5D3E6608
P 8000 3600
F 0 "OPAMP1" H 8025 3967 50  0000 C CNN
F 1 "OpAmp" H 8025 3876 50  0000 C CNN
F 2 "" H 8000 3600 50  0001 C CNN
F 3 "~" H 8000 3600 50  0001 C CNN
	1    8000 3600
	1    0    0    -1  
$EndComp
$Comp
L SchemaBoard_Library:6pinRelay U2-6
U 1 1 5D3E2F18
P 4700 3600
F 0 "U2-6" V 4033 3600 50  0000 C CNN
F 1 "6pinRelay" V 4124 3600 50  0000 C CNN
F 2 "Relay_THT" H 5200 3500 50  0001 L CNN
F 3 "https://standexelectronics.com/wp-content/uploads/datasheet_reed_relay_DIP.pdf" H 4600 3600 50  0001 C CNN
	1    4700 3600
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
P 1850 5600
F 0 "D1" H 1850 5816 50  0000 C CNN
F 1 "Diode" H 1850 5725 50  0000 C CNN
F 2 "" H 1850 5600 50  0001 C CNN
F 3 "~" H 1850 5600 50  0001 C CNN
	1    1850 5600
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
P 3750 7000
F 0 "CP1" H 3868 7046 50  0000 L CNN
F 1 "CP" H 3868 6955 50  0000 L CNN
F 2 "" H 3788 6850 50  0001 C CNN
F 3 "~" H 3750 7000 50  0001 C CNN
	1    3750 7000
	-1   0    0    1   
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
P 8950 5450
F 0 "L1" H 9003 5496 50  0000 L CNN
F 1 "Inductor" H 9003 5405 50  0000 L CNN
F 2 "" H 8950 5450 50  0001 C CNN
F 3 "~" H 8950 5450 50  0001 C CNN
	1    8950 5450
	1    0    0    -1  
$EndComp
$Comp
L SchemaBoard_Library:GND GND1
U 1 1 5D3D0965
P 1800 6050
F 0 "GND1" H 1800 5800 50  0001 C CNN
F 1 "GND" H 1805 5877 50  0000 C CNN
F 2 "" H 1800 6050 50  0001 C CNN
F 3 "" H 1800 6050 50  0001 C CNN
	1    1800 6050
	1    0    0    -1  
$EndComp
$Comp
L SchemaBoard_Library:LED LED1
U 1 1 5D3E5DAB
P 1850 7150
F 0 "LED1" H 1843 7366 50  0000 C CNN
F 1 "LED" H 1843 7275 50  0000 C CNN
F 2 "" H 1850 7150 50  0001 C CNN
F 3 "~" H 1850 7150 50  0001 C CNN
	1    1850 7150
	1    0    0    -1  
$EndComp
$Comp
L SchemaBoard_Library:Battery_Cell BT1
U 1 1 5D3EC35E
P 5700 7000
F 0 "BT1" V 5445 7050 50  0000 C CNN
F 1 "Battery_Cell" V 5536 7050 50  0000 C CNN
F 2 "" V 5700 7060 50  0001 C CNN
F 3 "~" V 5700 7060 50  0001 C CNN
	1    5700 7000
	0    1    1    0   
$EndComp
$EndSCHEMATC
