import time
from Sculpture import Gallery
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
gallery.rate(16, 4);
gallery.rate(16, 0);
gallery.push(12, "Shit drawing");
gallery.rate(12,1);
gallery.rate(12,0);
gallery.rate(12,0);
gallery.push(1, "Ya, okay");
gallery.rate(1, 2);
gallery.rate(1, 2);
gallery.push(1, "NEW piece");

gallery.test_display();
