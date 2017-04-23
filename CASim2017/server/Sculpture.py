import time
import os, sys
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
	
	Gallery:
		push(int ID, data) - Push back a sculpture
		rate(ID, score) - give sculpture a rating
		hot_list(int X) - top X hot items, from not to hot
		cont_list(int x) - top X controversials, from normal to CRazy 	
		new_list(int X) - Most recent things - doe



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
	def getHeat(self, seconds = False):
		curr_time = time.time()
		#three hour default:
		if (seconds == False):
			seconds = 10800
		
		positive_reviews=0	

		for i in range(len(self.ratings)):
			index = len(self.ratings) - 1 - i
			diffTime = curr_time - self.ratings[index].time
			#If the review was recent enough: Use it - It must also be good
			if diffTime < seconds:
				if (self.ratings[index].score >= 2.5):
					positive_reviews+=1;
			

		return positive_reviews;

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

	def save(self):
		filepath = "Scp_Saves/" + str(self.id)
		save_file = open(filepath, 'w')

		save_file.write("--ID--\n")
		save_file.write(str(self.id));
		
		save_file.write("\n")
		
		save_file.write("--Timestamp--\n")
		save_file.write(str(self.time))
	
		save_file.write("\n")

		save_file.write("--Reviews(score, timestamp)--\n")
		save_file.write("[")
		for rating in self.ratings:
			save_file.write("(")
			save_file.write(str(rating.score))
			save_file.write(",")
			save_file.write(str(rating.time))
			save_file.write(")")
		save_file.write("]\n")
		

		save_file.write("--Data--\n")
		save_file.write(self.data)
		
		save_file.write("\n")


	def load(self, file_name, directory):
 		if not(directory[len(directory) - 1] == '/'):
			directory+='/'
			 
		print "Loading file %s" % file_name
		r_file = open(directory + file_name, 'r')
		#first line is --ID--
		line = r_file.readline()
		#second is id
		scp_id   = r_file.readline()
		scp_id = scp_id[:-1] #remove \n
		
		
		line     = r_file.readline()
		scp_time = r_file.readline()
		scp_time = scp_time[:-1] # remove \n 
		
		line     = r_file.readline()
		reviews_str = r_file.readline()
		reviews_str = reviews_str[:-1] #Remove \n

		line     = r_file.readline()
		scp_data = r_file.readline()
		scp_data = scp_data[:-1]
		

		#cast each type of variable
		scp_id = int(scp_id);
		scp_time = float(scp_time);
		#data is already a string	
	
		#position of left parentheses
		pos_lpar = reviews_str.find('(')
		pos_comma= reviews_str.find(',')
		pos_rpar = reviews_str.find(')')
		scp_scores = []
		scp_times    = []
		while (pos_lpar != -1):
			
			this_score = int(reviews_str[pos_lpar + 1: pos_comma])
			scp_scores += [this_score]
			scp_times  += [float(reviews_str[pos_comma + 1: pos_rpar])]
			
			
			reviews_str = reviews_str[pos_rpar + 1:]			

			pos_lpar = reviews_str.find('(')
			pos_comma= reviews_str.find(',')
			pos_rpar = reviews_str.find(')')
			
		self.id = scp_id
		self.time = scp_time
		for i in range(len(scp_scores)):
			new_review = Rate(scp_scores[i], scp_times[i])
			self.ratings.append(new_review)
		
		#scp_reviews

		

  


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
		hot_data = []

		for i in range(count):
			hotness = self.scp_list[i].getHeat()
			position = bisect(hot_data, (hotness, -1));
			hot_data.insert(position, (hotness , self.scp_list[i].data))

		
			
		for i in range(count, len(self.scp_list)):
			#If something is more controversial: Delete the bottom
			#of controversialness list and add new thing
			if (self.scp_list[i].getHeat() > hot_data[0][0]):
				hot_data.pop(0)
				hotness = self.scp_list[i].getHeat()
				position = bisect(hot_data, (hotness, -1));
				hot_data.insert(position, (hotness , self.scp_list[i].data))
		
			#Return item numbers only
		for i in range (count):
			hot_data[i] = hot_data[i][1]
	
		return hot_data
		
	
	

	#Return a list of Tuples of form (controversy)
	def cont_list(self, count = False ):
		if count == False:
			count = len(self.scp_list)
		
		#controversial item id's
		cont_data = []

		for i in range(count):
			controversy = self.scp_list[i].getCont()
			position = bisect(cont_data, (controversy, -1));
			cont_data.insert(position, (controversy , self.scp_list[i].data))

		
			
		for i in range(count, len(self.scp_list)):
			#If something is more controversial: Delete the bottom
			#of controversialness list and add new thing
			if (self.scp_list[i].getCont() > cont_data[0][0]):
				cont_data.pop(0)
				controversy = self.scp_list[i].getCont()
				position = bisect(cont_data, (controversy, -1));
				cont_data.insert(position, (controversy , self.scp_list[i].data))
		
			#Return item numbers only
		for i in range (count):
			cont_data[i] = cont_data[i][1]
	
		return cont_data
	#list new items in gallery
	def new_list(self, count = False):
		if count == False:
			count = len(self.scp_list)
		
		#controversial item id's
		new_data = []

		for i in range(count):
			time = self.scp_list[i].time
			position = bisect(new_data, (time, -1));
			new_data.insert(position, (time , self.scp_list[i].data))

		
			
		for i in range(count, len(self.scp_list)):
			#If something is more controversial: Delete the bottom
			#of controversialness list and add new thing
			if (self.scp_list[i].time > new_data[0][0]):
				new_data.pop(0)
				time = self.scp_list[i].time
				position = bisect(new_data, (time, -1));
				new_data.insert(position, (time , self.scp_list[i].data))
		
			#Return item numbers only
		for i in range (count):
			new_data[i] = new_data[i][1]
	
		return new_data
	
		

	#For testing purpose, display the gallery
	def test_display(self):
		for scp in self.scp_list:
			print "ID: %s\n\t data[10]: %s\n\tHeat: %s\n\tCont: %s\n" % (scp.id, scp.data, scp.getHeat(), scp.getCont())
	

	def save_all(self):
		for scp in self.scp_list:
			scp.save();

	#Load one file
	def load_one(self, file_name, directory):
		scp = Sculpture(-1, "NULL")
		scp.load(file_name, directory)
		self.scp_list.append(scp)


	def load_all(self, directory = False):
		if (directory == False):
			directory = "Scp_Saves";
		files = []
		files = os.listdir(directory)
		#load each file
		for i in range(len(files)):
			self.load_one(files[i], directory)
		

print os.listdir("Scp_Saves")
