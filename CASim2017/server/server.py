import socket
import sys
from thread import *
from Sculpture import *
 
class Server:

	HOST = ''   # Symbolic name meaning all available interfaces
	PORT = 44320 # Arbitrary non-privileged port
	s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
	current_id = 0

	def __init__(self, size):
		
		self.size = size
		self.gallery = Gallery(self.size)
		print 'Socket created'
		 
		#Bind socket to local host and port
		try:
		    self.s.bind((self.HOST, self.PORT))
		except socket.error as msg:
		    print 'Bind failed. Error Code : ' + str(msg[0]) + ' Message ' + msg[1]
		    sys.exit()
		     
		print 'Socket bind complete'

		self.s.listen(10)
		print 'Socket now listening'
		self.start()

	def start(self):
		#now keep talking with the client
		while True:
		    #wait to accept a connection - blocking call
		    conn, addr = self.s.accept()
		    print 'Connected with ' + addr[0] + ':' + str(addr[1])
		     
		    #start new thread takes 1st argument as a function name to be run, second is the tuple of arguments to the function.
		    start_new_thread(self.clientthread ,(conn,))

		self.s.close()
	 
	#Function for handling connections. This will be used to create threads
	def clientthread(self, conn):

	    #infinite loop so that function do not terminate and thread do not end.
	    while True:

	        data = 0 
	        #Receiving from client
	        updown = conn.recv(10)

	        if updown == "upload    ":
		        size = conn.recv(10)
		        data = conn.recv(int(size))
		        self.gallery.push(self.current_id, data)
		        self.current_id += 1
		        print self.current_id, updown
		        conn.sendall('OKAY.')

	        elif updown == "download  ":
	        	conn.sendall(gallery.hot_list())
				print updown
				cconn.sendall('OKAY.')

	        if not data:
	            break
	     
	    #came out of loop
	    conn.close()

se = Server(10)
