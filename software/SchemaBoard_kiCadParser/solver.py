import math
import json
import copy
import xml.etree.ElementTree as ET
import argparse
import os
from random import choice

#priority sequence 
def priority(comp):
    counter= 0
    for a in range(len(comp)):
        if comp[a][0].startswith(("OPAMP","U","RELAY","SW")):
            temp= comp[a]
            comp[a]=comp[counter]
            comp[counter]=temp
            counter+=1
    
    return(comp)

def sorting(comp):
    
    for i in range(len(comp)):
        if comp[i][0].startswith(("U","RELAY","OPAMP","SW")):
            counter=1
            for j in range(2,len(comp[i]),2):
                a=comp[i].index(str(int(counter)))
                temp=comp[i][a]
                comp[i][a]= comp[i][j]
                comp[i][j]=temp
                temp=comp[i][a-1]
                comp[i][a-1]=comp[i][j-1]
                comp[i][j-1]=temp
                if j<((len(comp[i])/2)-1):
                    counter+=1
                elif j<(len(comp[i])/2):
                    counter=counter+((len(comp[i])-1)/4)
                else:
                    counter-=1

    return(comp)

def no_of_rows(nets,comp,row_assign,no_rows_on_board,bb):#assignment of rows to nets
    next_empty=1
    free_rows=[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]
    i=0
    while(comp[i][0].startswith(("RELAY","U","OPAMP","SW"))):
        U_size= int(len(comp[i])/2)
        already_exist=-1
        counter=1
        counter_SW=1
        for a in range(int(U_size/2)):#no of pins in IC/2 for one side
            for j in range(len(row_assign)):
                if comp[i][(a*2)+1]==row_assign[j][0]:
                    already_exist=j
            if already_exist!=-1:


                if comp[i][0].startswith("SW"):
                    counter_SW+=1
                    if counter_SW==2:
                        if next_empty>(len(free_rows)/2):
                            bb[next_empty-1][0]=1
                            bb[next_empty+1][0]=1                         
                        else:
                            bb[next_empty-1][4]=1
                            bb[next_empty+1][4]=1
                        next_empty+=1

                row_assign[already_exist].append(str(next_empty))
                free_rows[next_empty-1]=1
                next_empty+=1
                if comp[i][0].startswith("SW"):
                    next_empty+=1
                if comp[i][0].startswith("RELAY"):
                    counter+=1
                    if counter==3:
                        if next_empty>(len(free_rows)/2):
                            bb[next_empty-1][0]=1
                            bb[next_empty][0]=1                         
                        else:
                            bb[next_empty-1][4]=1
                            bb[next_empty][4]=1
                        next_empty+=2
            else:
                if comp[i][0].startswith("SW"):
                    counter_SW+=1
                    if counter_SW==2:
                        if next_empty>(len(free_rows)/2):
                            bb[next_empty-1][0]=1
                            bb[next_empty+1][0]=1                         
                        else:
                            bb[next_empty-1][4]=1
                            bb[next_empty+1][4]=1
                        next_empty+=1

                row_assign.append([])
                row_assign[len(row_assign)-1].append(comp[i][(a*2)+1])
                row_assign[len(row_assign)-1].append(str(next_empty))
                free_rows[next_empty-1]=1
                next_empty+=1
                if comp[i][0].startswith("SW"):
                    next_empty+=1


                if comp[i][0].startswith("RELAY"):
                    counter+=1
                    if counter==3:
                        if next_empty>(len(free_rows)/2):
                            bb[next_empty-1][0]=1
                            bb[next_empty][0]=1                         
                        else:
                            bb[next_empty-1][4]=1
                            bb[next_empty][4]=1
                        next_empty+=2
            already_exist=-1
            if next_empty>(len(free_rows)/2):
                print("ciruit is too big ")
                exit()
        temp=next_empty   
        next_empty=int(next_empty-(U_size/2)+(no_rows_on_board/2))
        if comp[i][0].startswith(("RELAY","SW")):
            next_empty-=2
        counter=1
        counter_SW=1
        for a in range(int(U_size/2)):#no of pins in IC/2 for one side
            for j in range(len(row_assign)):
                if comp[i][(a*2)+1+U_size]==row_assign[j][0]:
                    already_exist=j
            if already_exist!=-1:

                if comp[i][0].startswith("SW"):
                    counter_SW+=1
                    if counter_SW==2:
                        if next_empty>(len(free_rows)/2):
                            bb[next_empty-1][0]=1
                            bb[next_empty+1][0]=1                         
                        else:
                            bb[next_empty-1][4]=1
                            bb[next_empty+1][4]=1
                        next_empty+=1

                row_assign[already_exist].append(str(next_empty))
                free_rows[next_empty-1]=1
                next_empty+=1
                if comp[i][0].startswith("SW"):
                    next_empty+=1
                if comp[i][0].startswith("RELAY"):
                    counter+=1
                    if counter==3:
                        if next_empty>(len(free_rows)/2):
                            bb[next_empty-1][0]=1
                            bb[next_empty][0]=1                         
                        else:
                            bb[next_empty-1][4]=1
                            bb[next_empty][4]=1
                        next_empty+=2
            else:
                if comp[i][0].startswith("SW"):
                    counter_SW+=1
                    if counter_SW==2:
                        if next_empty>(len(free_rows)/2):
                            bb[next_empty-1][0]=1
                            bb[next_empty+1][0]=1                         
                        else:
                            bb[next_empty-1][4]=1
                            bb[next_empty+1][4]=1
                        next_empty+=1

                row_assign.append([])
                row_assign[len(row_assign)-1].append(comp[i][(a*2)+1+U_size])
                row_assign[len(row_assign)-1].append(str(next_empty))
                free_rows[next_empty-1]=1
                next_empty+=1
                if comp[i][0].startswith("SW"):
                    next_empty+=1
                if comp[i][0].startswith("RELAY"):
                    counter+=1
                    if counter==3:
                        if next_empty>(len(free_rows)/2):
                            bb[next_empty-1][0]=1
                            bb[next_empty][0]=1                         
                        else:
                            bb[next_empty-1][4]=1
                            bb[next_empty][4]=1
                        next_empty+=2
            already_exist=-1
            if 0 not in free_rows:
                print("circuit to big")
                exit()
        next_empty=temp
        i+=1
    ##
    free_rows_list=[]
    for i in range(len(free_rows)):
            if free_rows[i]==0:
                free_rows_list.append(i)
##
    for i in range(len(nets)):
        #check if already assigned
        already_assign=0
        for j in range(len(row_assign)):
            if nets[i][0]==row_assign[j][0]:
                already_assign=len(row_assign[j])-1
        if already_assign==0:
            row_assign.append([])
            row_assign[len(row_assign)-1].append(nets[i][0])

        more_assign=math.ceil(((len(nets[i])-1)/2)/4)-already_assign
        if nets[i][0].startswith(("+3","GND","3V")) or len(nets[i])>17:
            more_assign=math.ceil(((len(nets[i])-1)/2)/3)-already_assign
        while more_assign>0:
            if 0 not in free_rows:
                print("circuit to big")
                exit()
                ## added for localised allotment of space
            if free_rows[next_empty-1]==0:
                next_empty=next_empty
            if free_rows[next_empty]==0:
                next_empty=next_empty+1
            elif free_rows[next_empty-2]==0:
                next_empty=next_empty-1
            else:
                #####
                next_empty=choice([i for i in range(0,no_rows_on_board) if free_rows[i] != 1])+1
            for k in range(len(row_assign)):
                if row_assign[k][0]==nets[i][0]:
                    row_assign[k].append(str(next_empty))
                    free_rows[next_empty-1]=1
                    more_assign-=1
                    break
                    
    return row_assign,bb
    

def correcting_pin_names(comp,file_type):
    for i in range(len(comp)):
        if comp[i][0].startswith(("LED","CP","D","J","SP")):
            for j in range(2,len(comp[i]),2):
                if comp[i][j]=="1":
                    if file_type=="fritzing":
                        comp[i][j]="cathode"
                    else:
                        comp[i][j]="anode"
                if comp[i][j]=="2":
                    if file_type=="fritzing":
                        comp[i][j]="anode"
                    else:
                        comp[i][j]="cathode"
        elif comp[i][0].startswith(("VCC","BT")):
            for j in range(2,len(comp[i]),2):
                if comp[i][j]=="1":
                    if file_type=="fritzing":
                        comp[i][j]="negative"
                    else:
                        comp[i][j]="positive"
                if comp[i][j]=="2":
                    if file_type=="fritzing":
                        comp[i][j]="positive"
                    else:
                        comp[i][j]="negative"
        elif comp[i][0].startswith("Q"):
            for j in range(2,len(comp[i]),2):
                if comp[i][j]=="1":
                    comp[i][j]="emitter"
                if comp[i][j]=="2":
                    comp[i][j]="base"
                if comp[i][j]=="3":
                    comp[i][j]="collector"
        else:
            for j in range(2,len(comp[i]),2):
                comp[i][j]="none"

    return(comp)



#final printing and adding columns
def adding_column(comp,row_assign,no_rows_on_board,bb):
    
    for i in range(len(comp)):
        #print()
        #print (comp[i][0],end=" ")
        for j in range (1,len(comp[i])):
            if j%2==1:
                for k in range (len(row_assign)):
                    if row_assign[k][0]== comp[i][j]:
                        for l in range(1,len(row_assign[k])):
                            assignment_done=0
                            if comp[i][0].startswith(("U","RELAY","OPAMP","SW")):
                                possible_rows=1
                            elif len(row_assign[k])==2:
                                possible_rows=5
                            elif l>1 and l<(len(row_assign[k])-1):
                                possible_rows=3
                            else:
                                possible_rows=4
                            for t in range(possible_rows):
                                if bb[int(row_assign[k][l])-1][t]==0 and int(row_assign[k][l])>(no_rows_on_board/2):
                                    #print(row_assign[k][l]+str(t+1),end=" ")
                                    comp[i][j]=[row_assign[k][l],str(t+1)]
                                    bb[int(row_assign[k][l])-1][t]=1

                                    assignment_done=1
                                    break

                                elif int(row_assign[k][l])<=int(no_rows_on_board/2) and bb[int(row_assign[k][l])-1][4-t]==0:
                                    #print(row_assign[k][l]+str(5-t),end=" ")
                                    comp[i][j]=[row_assign[k][l],str(5-t)]
                                    bb[int(row_assign[k][l])-1][4-t]=1

                                    assignment_done=1
                                    break

                                else:
                                    continue
                                
                            if assignment_done==1:
                                break
    
    return(comp)


#adding wires as components to the circuit
def addingwire(comp,row_assign,no_rows_on_board):    
    counter=1
    for i in range(len(row_assign)):

        if len(row_assign[i])>2:
            if int(row_assign[i][1])>(no_rows_on_board/2):
                x="5"
            else:
                x="1" 
            if int(row_assign[i][2])>(no_rows_on_board/2):
                y="5"
            else:
                y="1"
            comp.append([])
            comp[-1].append("wire"+str(counter))
            comp[-1].extend(([row_assign[i][1],x],"1",[row_assign[i][2],y],"2"))
            counter+=1

#adding second layer of wires
        for j in range(3, len(row_assign[i])):
            
            if int(row_assign[i][j-1])>(no_rows_on_board/2):
                x="4"
            else:
                x="2" 
            if int(row_assign[i][j])>(no_rows_on_board/2):
                y="5"
            else:
                y="1"
            comp.append([])
            comp[-1].append("wire"+str(counter))
            comp[-1].extend(([row_assign[i][j-1],x],"1",[row_assign[i][j],y],"2"))
            counter+=1

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


    for net in root.findall('net'):
        array.append([])
        for connector in net.findall('connector'):
            array[-1].append(connector.attrib)
            for part in connector.findall('part'):
                array[-1].append(part.attrib)
                file_type="fritzing"
    return generate_netlist(file_type,array),file_type

def generate_netlist(file_type,array):
    nets=[]
    #print(array)
    if file_type=="kicad":
        for i in range(len(array)):
            for j in range(len(array[i])):
                if j==0:
                    nets.append([])
                    nets[-1].append(array[i][j]['name'])
                else:
                    nets[-1].append(array[i][j]['ref'])
                    nets[-1].append(array[i][j]['pin'])
    if file_type=="fritzing":
        for i in range(len(array)):
            pin_number=""
            for j in range(len(array[i])):
                if j>0 and j%2==0:
                    continue
                elif j==0:
                    nets.append([])
                    nets[-1].append("net"+str(len(nets)))
                    pin_number=str(int(array[i][j]['id'][9:])+1)
                else:
                    nets[-1].append(array[i][j]['label'])
                    nets[-1].append(pin_number)
    return nets

def component(nets):
    comp=[]
    for i in range(len(nets)):
        for j in range(1, len(nets[i])):
            if j%2 ==1:#if component
                component_exist=0
                for a in range(len(comp)):
                    if nets[i][j]==comp[a][0]:
                        component_exist=1
                        break
                if component_exist==1:
                    continue
                else:
                    comp.append([])
                    comp[len(comp)-1].append(nets[i][j])
            else:
                temp_comp = nets[i][j-1]
                for a in range(len(comp)):
                    if comp[a][0]==temp_comp:
                        comp[a].append(nets[i][0])
                        comp[a].append(nets[i][j])
    
    return comp

def write_json(nets,comp,file_type,fileName):
    #writing onto json 
    fileName=fileName+".json"
    f=open(fileName,"w")
    list0=[]
    list3=[]
    for i in range(len(nets)):
        list1=[]
        for j in range(1,len(nets[i])-1,2):
            obj= {"component":nets[i][j],"pin":"connector"+str(int(nets[i][j+1])-1)}
            list1.append(obj)
        obj={"name":nets[i][0],"connector":list1}
        list0.append(obj)

    #CHANGING COMP TO SUIT YOONJI

    comp_new=copy.deepcopy(comp)
    comp=correcting_pin_names(comp,file_type)
    for i in range(len(comp_new)):
        list2=[]
        for j in range(1,len(comp_new[i])-1,2):
            obj2={"id":"connector"+str(int(comp_new[i][j+1])-1),"type":comp[i][j+1],"position":[int(x) for x in comp[i][j]]}
            list2.append(obj2)
        obj2 ={"label":comp[i][0],"connector":list2}
        list3.append(obj2)

    final_obj={"sketch":"test.fz","net":list0,"components":list3}
    json.dump(final_obj,f)

def manage_opamp(nets):
    temp=[]
    for i in range(len(nets)):
        for j in range(len(nets[i])):
            if nets[i][j].startswith("OPAMP") and not (nets[i][j] in temp):
                temp.append(nets[i][j])
                nets.append([])
                nets[-1].append("Net-"+nets[i][j]+"pin1")
                nets[-1].append(nets[i][j])
                nets[-1].append("1")
                nets.append([])
                nets[-1].append("Net-"+nets[i][j]+"pin8")
                nets[-1].append(nets[i][j])
                nets[-1].append("8")
                nets.append([])
                nets[-1].append("Net-"+nets[i][j]+"pin5")
                nets[-1].append(nets[i][j])
                nets[-1].append("5")
    return nets

def final_sorting(comp):
    print(comp)
    for i in range(len(comp)):
        counter=1
        for j in range(2,len(comp[i]),2):
            a=comp[i].index(str(counter))
            counter+=1
            temp= comp[i][j]
            comp[i][j]=comp[i][a]
            comp[i][a]=temp
            temp= comp[i][j-1]
            comp[i][j-1]=comp[i][a-1]
            comp[i][a-1]=temp
    return comp

def flipping_components(comp,nets):
    for i in range(len(comp)):
        if len(comp[i])==5 and comp[i][0].startswith(("R","CP","C","BT","L")) and (not(comp[i][0].startswith("LED"))) :            
            temp=comp[i][2]
            comp[i][2]=comp[i][4]
            comp[i][4]=temp

            
    for i in range(len(nets)):
        for j in range(1,len(nets[i]),2):
            
            if nets[i][j].startswith(("R","CP","C","BT","L")) and (not(nets[i][j].startswith("RELAY"))) and (not(nets[i][j].startswith("LED"))) :
                
                if nets[i][j+1]=="1":
                    nets[i][j+1]="2"
                   
                else:
                    nets[i][j+1]="1"


    return comp,nets


def solver(nets,file_type,fileName):

    nets= manage_opamp(nets)
    bb = [[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0]]
    no_rows_on_board=len(bb)
    row_assign=[]
    comp=component(nets)
    comp=sorting(comp)
    comp=priority(comp)
    row_assign,bb=no_of_rows(nets,comp,row_assign,no_rows_on_board,bb)
    comp=adding_column(comp,row_assign,no_rows_on_board,bb)
    addingwire(comp,row_assign,no_rows_on_board)


    comp,nets=flipping_components(comp,nets)

    comp=final_sorting(comp)
    write_json(nets,comp,file_type,fileName)
    print("executed")
    


