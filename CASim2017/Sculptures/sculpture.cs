using System;
using System.Collections.Generic;

namespace Monotest
{
    class MainClass
    {
        public static void Main (string[] args)
        {
            Console.WriteLine ("Hello World!");
        }
    }
}

//TODO: Maybe make a rating class holding timestamps?
class Sculpture
{
    private int id;

    private List<int> ratings;

    private DateTime today; // Get time object was created

    //Initialize with the time of day:
    public Sculpture(int id)
    {
        today = DateTime.Now;
    }

    //Add a rating
    public void addRating(int rate)
    {
        ratings.Add(rate);
    }

    //How hot is this item?
    public double get_heat(){


        return 1.337;
    }

    //How controversial is this item?
    public double get_cont(){
    
        return 1.337;
    }

    //How new is th
    public double getAvRating()
    {
        return 1.337;    
    }

    public DateTime returnDate()
    {
        return today;
    }    
}
