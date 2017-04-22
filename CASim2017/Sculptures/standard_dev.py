from math import sqrt

def standard_deviation(lst):
	"""Calculates the standard deviation for a list of numbers."""
	num_items = len(lst)
	mean = sum(lst) / num_items
	differences = [x - mean for x in lst]
	sq_differences = [d ** 2 for d in differences]
	ssd = sum(sq_differences)
	return ssd
