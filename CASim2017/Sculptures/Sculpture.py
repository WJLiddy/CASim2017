import time
"""Python scripture"""
class Sculpture:    
	#id of statue -ideee   
	# __ = pleasedonttouch 

	def __init__(self, idee):
		self.__id = idee
		self.ratings = []
		self.time = time.time()

	def addRating(self, rate):
		self.ratings.append(rate)

	#How hot is the item?
	def getHeat():
		return 1.337

	#how controversial?
	def getCont():
		return 1.337

	def getAvRating():
		return 1.337

	def returnDate():
		return self.time

#A gallery of Scuplptures:
class Sculptures:
	
	#Size constructor
	def __init__(self, size):
		self.size = size
		self.scp_list = []	
	
	#Default size constructor
	def __init__(self):
		self.size = 1000
		self.scp_list = []

	#Add a new item to the museum
	def push(self, idee):
		#if len(self.scp_list) >= self.size:
		
		self.scp_list.append(idee)

	# scp - sculpture
	def rate(self, idee, rate):
		for scp in self.scp_list:
			if scp.__id == idee:
				scp.addRating(rate)

	def hot_list(self, count):
		#Create function to get the hottest items
		hot_ids = []
		# ...
		# ...
		return hot_ids

	def cont_list(self, count):
		cont_ids = []
		# ...
		# ...
		return cont_ids
	
	def new_list(self, count):
		new_ids = []
		# ...
		# ...



print(time.time())

Sculptures 
