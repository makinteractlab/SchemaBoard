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
L SchemaBoard_Library:LED LED1
U 1 1 5D4552BA
P 3200 3700
F 0 "LED1" H 3193 3916 50  0000 C CNN
F 1 "LED" H 3193 3825 50  0000 C CNN
F 2 "" H 3200 3700 50  0001 C CNN
F 3 "~" H 3200 3700 50  0001 C CNN
	1    3200 3700
	1    0    0    -1  
$EndComp
Wire Wire Line
	1350 3700 3050 3700
Wire Wire Line
	7800 3700 6950 3700
$Comp
L SchemaBoard_Library:LM555 U1-8
U 1 1 5D433B16
P 6450 3600
F 0 "U1-8" H 6450 4067 50  0000 C CNN
F 1 "LM555" H 6450 3976 50  0000 C CNN
F 2 "" H 6450 3600 50  0001 C CNN
F 3 "http://www.ti.com/lit/ds/symlink/lm555.pdf" H 6450 3600 50  0001 C CNN
	1    6450 3600
	1    0    0    -1  
$EndComp
Wire Wire Line
	1350 5500 7800 5500
Wire Wire Line
	7800 3700 7800 5500
Wire Wire Line
	1350 3700 1350 5500
Wire Wire Line
	3350 3700 5950 3700
$EndSCHEMATC
