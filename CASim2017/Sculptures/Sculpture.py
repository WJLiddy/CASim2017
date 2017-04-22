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

	def getAvRating():
		return 1.337

	def returnDate():
		return self.time

#A gallery of Scuplptures:
class Sculpture:
	
	#Size constructor
	def __init__(self, size):
		self.size = size
		
	#Default size constructor
	def __init__(self):
		self.size = 1000

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



print(time.time())
