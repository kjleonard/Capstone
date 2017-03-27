#!/usr/bin/env python

import socket
import struct

TCP_IP = '127.0.0.1'
TCP_PORT = 50040
BUFFER_SIZE = 4096
MESSAGE = ""

s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.connect((TCP_IP, TCP_PORT))

data = s.recv(BUFFER_SIZE)
print "received data:", data

MESSAGE = "ENDIAN BIG\r\n\r\n"
s.send(MESSAGE)
data = s.recv(BUFFER_SIZE)
print "received data:", data

#s.close()
#quit()

BUFFER_SIZE = 24

MESSAGE = "START\r\n\r\n"
s.send(MESSAGE)
data = s.recv(BUFFER_SIZE)
print "received data:", data



#Acceleromter
TCP_PORT_ACC = 50042

s_acc = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s_acc.connect((TCP_IP, TCP_PORT_ACC))

while True:
	data = s_acc.recv(BUFFER_SIZE)
	data.replace(" ", "")
	data1 = data[:4]
	print len(data), "\t", data
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

s.close()
