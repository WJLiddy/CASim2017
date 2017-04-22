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
	def getHeat(self):
		return 1.337

	#how controversial?
	def getCont(self):
		return 1.337

	def getAvRating(self):
		return sum(ratings) / len(ratings)

	def returnDate(self):
		return self.time

#A gallery of Scuplptures:
class Gallery:
	
	#Size constructor
	def __init__(self, size = 1000):
		self.size = size
		self.scp_list = []	
	

	#Add a new item to the museum
	def push(self, idee):
		#if over full list, remove least liked item
		if len(self.scp_list) >= self.size:

			#Remove worst item
			worst_average = self.scp_list[0].getAvRating()
			index = 0
			worst_index=0

			for scp in self.scp_list:
				if (scp.getAvRating() < worst_average):
					worst_average = scp.getAvRating()
					worst_index = index
				index+=1
			self.scp_list.pop(worst_index);
		
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

#Gallery of size 5
gallery = Gallery(5)

gallery.push(4);
gallery.push(6);
gallery.push(16);
gallery.push(12);
gallery.push(1);
