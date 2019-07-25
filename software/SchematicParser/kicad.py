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

    polar_capacitor=[]
    for components in root.findall('components'):
        for comp in components.findall('comp'):
            if comp.attrib['ref'].startswith('C'):
                temp=comp.attrib['ref']
                for libsource in comp.findall('libsource'):
                    if libsource.attrib['part']=='CP':
                        polar_capacitor.append(temp)
    return generate_netlist(file_type,array,polar_capacitor),file_type


def generate_netlist(file_type,array,polar_capacitor):
    nets=[]
    for i in range(len(array)):
        for j in range(len(array[i])):
            if j==0:
                nets.append([])
                nets[-1].append(array[i][j]['name'])
            else:
                nets[-1].append(array[i][j]['ref'])
                nets[-1].append(array[i][j]['pin'])

    for i in range(len(nets)):
        for j in range(len(nets[i])):
            if nets[i][j] in polar_capacitor:
                nets[i][j]=nets[i][j][0]+"P"+nets[i][j][1:] 
    return nets


# MAIN
# from the terminal input should be "python3 fritzing.py fritzing.xml location/name_of_output_file"
parser = argparse.ArgumentParser()
parser.add_argument('ids', nargs = '*', help = 'some ids')
args = parser.parse_args()
fileName=args.ids[1]
Input=args.ids[0]
nets,file_type=parse_input(Input)
solver(nets,file_type,fileName)



