# Write nice commments please of how to use this


from solver import *
import os 
import argparse
import xml.etree.ElementTree as ET

def parse_input(Input):
    tree= ET.parse(Input)
    root=tree.getroot()
    a=root.tag
    file_type=""
    array=[]
    for nets in root.findall('nets'):
        for net in nets.findall('net'):
            array.append([])
            array[-1].append(net.attrib)
            for node in net.findall('node'):
                array[-1].append(node.attrib)
                file_type="kicad"

    value_list=[]
    for components in root.findall("components"):
        for comp in components.findall("comp"):
            value_list.append([])
            value_list[-1].append(comp.attrib)
            for value in comp.findall('value'):
                value_list[-1].append(value.text)
    
    return generate_netlist(file_type,array),file_type,value_list


def generate_netlist(file_type,array):
    nets=[]
    for i in range(len(array)):
        for j in range(len(array[i])):
            if j==0:
                nets.append([])
                nets[-1].append(array[i][j]['name'])
            else:
                nets[-1].append(array[i][j]['ref'])
                nets[-1].append(array[i][j]['pin'])

    return nets

def generate_comp_value(value_list):
    comp_value=[]
    for i in range(len(value_list)):
        for j in range(2):
            if j%2==0:
                comp_value.append([])
                comp_value[-1].append(value_list[i][j]['ref'])
            else:
                comp_value[-1].append(value_list[i][j])

    return comp_value

# MAIN
# from the terminal input should be "python3 fritzing.py fritzing.xml location/name_of_output_file"
parser = argparse.ArgumentParser()
parser.add_argument('ids', nargs = '*', help = 'some ids')
args = parser.parse_args()
fileName=args.ids[1]
Input=args.ids[0]
nets,file_type,value_list=parse_input(Input)
print(value_list)
comp_value=generate_comp_value(value_list)
print(comp_value)
solver(nets,file_type,fileName,comp_value)



