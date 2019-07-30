EESchema Schematic File Version 4
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
P 6750 4250
F 0 "U1-8" H 6750 4717 50  0000 C CNN
F 1 "8pinChip" H 6750 4626 50  0000 C CNN
F 2 "" H 6750 4250 50  0001 C CNN
F 3 "http://www.ti.com/lit/ds/symlink/lm555.pdf" H 6750 4250 50  0001 C CNN
	1    6750 4250
	1    0    0    -1  
$EndComp
$Comp
L SchemaBoard_Library:LED LED1
U 1 1 5D4552BA
P 3500 3700
F 0 "LED1" H 3493 3916 50  0000 C CNN
F 1 "LED" H 3493 3825 50  0000 C CNN
F 2 "" H 3500 3700 50  0001 C CNN
F 3 "~" H 3500 3700 50  0001 C CNN
	1    3500 3700
	1    0    0    -1  
$EndComp
$Comp
L SchemaBoard_Library:Battery_Cell BT1
U 1 1 5D455990
P 3450 4500
F 0 "BT1" V 3195 4550 50  0000 C CNN
F 1 "Battery_Cell" V 3286 4550 50  0000 C CNN
F 2 "" V 3450 4560 50  0001 C CNN
F 3 "~" V 3450 4560 50  0001 C CNN
	1    3450 4500
	0    1    1    0   
$EndComp
Wire Wire Line
	6250 4500 3650 4500
Wire Wire Line
	1650 4500 1650 3700
$Comp
L SchemaBoard_Library:CP CP1
U 1 1 5D45475C
P 3500 3050
F 0 "CP1" V 3245 3050 50  0000 C CNN
F 1 "CP" V 3336 3050 50  0000 C CNN
F 2 "" H 3538 2900 50  0001 C CNN
F 3 "~" H 3500 3050 50  0001 C CNN
	1    3500 3050
	0    1    1    0   
$EndComp
Wire Wire Line
	5650 4350 6250 4350
Wire Wire Line
	3350 4500 1650 4500
Wire Wire Line
	1650 3050 3350 3050
Connection ~ 1650 3700
Wire Wire Line
	1650 3700 1650 3050
Wire Wire Line
	3650 3050 5650 3050
Wire Wire Line
	1650 3700 3350 3700
Wire Wire Line
	5650 3050 5650 3700
Wire Wire Line
	3650 3700 5650 3700
Connection ~ 5650 3700
Wire Wire Line
	5650 3700 5650 4350
$EndSCHEMATC
