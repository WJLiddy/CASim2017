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
		

print(time.time())
