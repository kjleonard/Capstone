#!/usr/bin/env python

import socket
import struct

TCP_IP = '127.0.0.1'

###Command Interface###

TCP_PORT_CI = 50040
BUFFER_SIZE_CI = 4096
MESSAGE = ""

s_ci = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s_ci.connect((TCP_IP, TCP_PORT_CI))

data = s_ci.recv(BUFFER_SIZE_CI)
print "received data:", data

#Commands server to send the bytes for floats in big endian.
MESSAGE = "ENDIAN BIG\r\n\r\n"
s_ci.send(MESSAGE)
data = s_ci.recv(BUFFER_SIZE_CI)
print "received data:", data

#Begins data collection on ports 50041-50044
MESSAGE = "START\r\n\r\n"
s_ci.send(MESSAGE)
data = s_ci.recv(BUFFER_SIZE_CI)
print "received data:", data


###Acceleromter###

TCP_PORT_ACC = 50042
BUFFER_SIZE_ACC = 24

s_acc = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s_acc.connect((TCP_IP, TCP_PORT_ACC))

while True:
	data = s_acc.recv(BUFFER_SIZE_ACC)
	data.replace(" ", "")
	data1 = data[:4]
	print len(data), "\t", data #Prints the length and data in byte format, could be removed/ignored for data collection
	if len(data1) == 4:
		print "data1: ", struct.unpack('>f',data1)
	data2 = data[4:-16]
	if len(data2) == 4:
		print "data2: ", struct.unpack('>f',data2)
	data3 = data[8:-12]
	if len(data3) == 4:
                print "data3: ", struct.unpack('>f',data3)
	data4 = data[12:-8]
	if len(data4) == 4:			
		print "data4: ", struct.unpack('>f',data4)
	data5 = data[16:-4]
	if len(data5) == 4:
		print "data5: ", struct.unpack('>f',data5)
	data6 = data[-4:]
	if len(data6) == 4:
		print "data6: ", struct.unpack('>f',data6)

