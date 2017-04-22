import time
import standard_dev
"""Python scripture"""
class Sculpture:    
	#id of statue -ideee   
	# __ = pleasedonttouch 

	def __init__(self, idee, scp_data):
		self.data = scp_data
		self.id = idee
		self.ratings = []
		self.time = time.time()

	#rate from 1 to 4
	def addRating(self, rate):
		self.ratings.append(rate)

	#How hot is the item?
	def getHeat(self):
		return 1.337

	#how controversial?
	def getCont(self):
		return 1.337

	def getAvRating(self):
		return sum(self.ratings) / max(len(self.ratings),1)

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
					worst_average = scp.getAvRating()
					worst_index = index
				index+=1
			self.scp_list.pop(worst_index);
	
		new_scp =  Sculpture(idee, scp_data)	
		self.scp_list.append(new_scp)


	# scp - sculpture
	def rate(self, idee, rate):
		for scp in self.scp_list:
			if scp.id == idee:
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

	#For testing purpose, display the gallery
	def test_display(self):
		for scp in self.scp_list:
			print "ID: %s\n data[10]: %s" % (scp.id, scp.data)
	



print(time.time())

#Gallery of size 5
gallery = Gallery(5)

gallery.push(4, "Great piece");
gallery.rate(4, 3);
gallery.rate(4, 4);
gallery.rate(4, 4);
gallery.push(6, "Darn not good");
gallery.rate(6, 2);
gallery.rate(6, 1.5);
gallery.push(16, "This is CONTROVERISAL");
gallery.rate(16, 0);
gallery.rate(16, 4);
gallery.rate(16, 0);
gallery.rate(16, 4);
gallery.push(12, "Shit drawing");
gallery.rate(12,1);
gallery.rate(12,0);
gallery.rate(12,0);
gallery.push(1, "Ya, okay");
gallery.rate(1, 2);
gallery.rate(1, 2);
gallery.push(1, "NEW piece");

gallery.test_display();
