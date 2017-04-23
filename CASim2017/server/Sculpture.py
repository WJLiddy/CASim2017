import time
from bisect import bisect
from standard_dev import standard_deviation

"""
A quick summery of Useful stuff:
	Rate - Don't worry about it

	Sculpture:
		id - sculpture id
		data - Mystrious string mickey wants for describing picture
		ratings - list of time-stamped ratings
		time - time item was initialized
		getHeat() - How hot is this item (double)
		getCont() - How controversial? (doube)
		getScores() - What are the scores without the timestamps, in list form
		




"""

"""Python scripture"""
#Rating contains score and timestamp
class Rate:
	def __init__(self, score, timestamp):
		self.score = score
		self.time = timestamp


class Sculpture:    
	#id of statue -ideee   
	# __ = pleasedonttouch 

	#Give initial score of 4 - then remove after 3 ratings
	def __init__(self, idee, scp_data):
		self.data = scp_data
		self.id = idee
		self.ratings = []
		self.time = time.time()

	#rate from 1 to 4
	def addRating(self, rate):
		rating = Rate(rate, time.time())
		self.ratings.append(rating)

	#How hot is the item?
	def getHeat(self):
		return 1.337

	def getScores(self):
		scores = []
		for rating in self.ratings:
			scores.append(rating.score)
		return scores

	#how controversial?
	def getCont(self):
		return standard_deviation(self.getScores())

	def getAvRating(self):
		return sum(self.getScores()) / max(len(self.getScores()),1)

	def returnDate(self):
		return self.time

#A gallery of Scuplptures:
class Gallery:
	
	#Size constructor
	def __init__(self, size = 1000):
		self.size = size
		self.scp_list = []	
	

	#Add a new item to the museum
	def push(self, idee, scp_data):
		#if over full list, remove least liked item
		if len(self.scp_list) >= self.size:

			#Remove worst item
			worst_average = self.scp_list[0].getAvRating()
			index = 0
			worst_index=0

			for scp in self.scp_list:
				if (scp.getAvRating() < worst_average):
				#Check if not an unrated item:
					if not(len(scp.ratings) == 0):
						worst_average = scp.getAvRating()
						worst_index = index
				index+=1
			self.scp_list.pop(worst_index);
	
		new_scp =  Sculpture(idee, scp_data)	
		self.scp_list.append(new_scp)

	def get_sculpt(self, idee):
		for scp in self.scp_list:
			if scp.id == idee:
				return scp

	# scp - sculpture
	def rate(self, idee, rate):
		for scp in self.scp_list:
			if scp.id == idee:
				scp.addRating(rate)

	def hot_list(self, count = False):
		if count == False:
			count = len(self.scp_list)
		
		#controversial item id's
		hot_ids = []

		for i in range(count):
			hotness = self.scp_list[i].getHeat()
			position = bisect(hot_ids, (hotness, -1));
			hot_ids.insert(position, (hotness , self.scp_list[i].data))

		
			
		for i in range(count, len(self.scp_list)):
			#If something is more controversial: Delete the bottom
			#of controversialness list and add new thing
			if (self.scp_list[i].getHeat() > hot_ids[0][0]):
				cont_ids.pop(0)
				hotness = self.scp_list[i].getHeat()
				position = bisect(hot_ids, (hotness, -1));
				hot_ids.insert(position, (hotness , self.scp_list[i].data))
		
			#Return item numbers only
		for i in range (count):
			hot_ids[i] = hot_ids[i][1]
	
		return hot_ids


		#Create function to get the hottest items
		hot_ids = []
		# ...
		# ...
		return hot_ids

	#Return a list of Tuples of form (controversy)
	def cont_list(self, count = False ):
		if count == False:
			count = len(self.scp_list)
		
		#controversial item id's
		cont_ids = []

		for i in range(count):
			controversy = self.scp_list[i].getCont()
			position = bisect(cont_ids, (controversy, -1));
			cont_ids.insert(position, (controversy , self.scp_list[i].data))

		
			
		for i in range(count, len(self.scp_list)):
			#If something is more controversial: Delete the bottom
			#of controversialness list and add new thing
			if (self.scp_list[i].getCont() > cont_ids[0][0]):
				cont_ids.pop(0)
				controversy = self.scp_list[i].getCont()
				position = bisect(cont_ids, (controversy, -1));
				cont_ids.insert(position, (controversy , self.scp_list[i].data))
		
			#Return item numbers only
		for i in range (count):
			cont_ids[i] = cont_ids[i][1]
	
		return cont_ids

	#list new items in gallery
	def new_list(self, count = False):
		if count == False:
			count = len(self.scp_list)

		new_ids = []
		for i in range(count):
			index = len(self.scp_list) - count + i
			new_ids.append(self.scp_list[index].data)
			
		return new_ids

	#For testing purpose, display the gallery
	def test_display(self):
		for scp in self.scp_list:
			print "ID: %s\n\t data[10]: %s\n\tHeat: %s\n\tCont: %s\n" % (scp.id, scp.data, scp.getHeat(), scp.getCont())
	




