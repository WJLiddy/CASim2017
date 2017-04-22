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
//TODO: TEST
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

class Sculptures
{
    

    //scp - sculpure
    private List<Sculpture> scp_list;
    //Museum size 
    private int Max_Size;
    //Default constructor: Set up with default list size of 1000
    public Sculptures(){
        Max_Size = 1000;
    }

    //Constructor with int to specify size of list.

    public void push(int id){
        if (scp_list.Count < Max_Size){        
            Sculpture new_scp = new Sculpture(id);
            scp_list.Add(new_scp);
        }
    }
}




