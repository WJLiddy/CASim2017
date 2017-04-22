from math import sqrt

#Return -1 of the list of numbers is empty
def standard_deviation(lst):
	"""Calculates the standard deviation for a list of numbers."""
	num_items = len(lst)
	if num_items == 0:
		return -1
	mean = sum(lst) / num_items
	differences = [x - mean for x in lst]
	sq_differences = [d ** 2 for d in differences]
	ssd = sum(sq_differences)
	return ssd
	
	
