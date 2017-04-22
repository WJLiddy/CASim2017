# -*- coding: utf-8 -*-
from lxml import html
from bs4 import BeautifulSoup
import requests, sys, os, codecs, zipfile
from selenium import webdriver
from os import listdir
from os.path import join

def download_file(url, id):
	local_filename = i + '.zip'
	# Repeatedly download file until timeout
	r = requests.get(url, stream=True)
	kb_downloaded = 0
	with open(local_filename, 'wb') as f:
		for chunk in r.iter_content(chunk_size=1024): 
			if chunk: # filter out keep-alive new chunks
				f.write(chunk)

web_url = 'https://archive3d.net/'
cwd = join(os.getcwd() + '\\models')

for pg in range(1893):
	page_url = 'https://archive3d.net/?page=' + str(1+(pg*24))
	page = requests.get(page_url)
	soup = BeautifulSoup(page.content, 'html.parser')
	p = soup.find_all('a')
	for j in range(48,72):
		s = str(p[j])
		t = '?a=download&do=get&id='
		i = ''
		for k in range(28,36):
			i += s[k]

		dl_url = web_url + t + i
		download_file(dl_url, i)
		print "DL!"
		zip_ref = zipfile.ZipFile(i + '.zip', 'r')
		nwd = join(cwd, i)
		zip_ref.extractall(nwd)
		zip_ref.close()

		for item in os.listdir(nwd):
		    if not (item.endswith(".3DS") or item.endswith(".png") or item.endswith(".jpg") or item.endswith(".tif")):
		        os.remove(join(nwd, item))

		os.remove(i + '.zip')
