

using System;
using System.Collections.Generic;

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
    private List<Sculpture> scp_list = new List<Sculpture>();
    //Museum size 
    private int Max_Size;
    //Default constructor: Set up with default list size of 1000
    public Sculptures(){
        Max_Size = 1000;
    }

    //Constructor with int to specify size of list.
    public Sculptures(int size){
        Max_Size = size;
    }
   
    public void push(int id){
        //TODO: If list is full , delete least popular item.
        //DEBUG
        Console.WriteLine("Size of gallery: {0}", scp_list.Count);

        if (scp_list.Count >= Max_Size) {
            //DEBUG:
            Console.WriteLine("Full Gallery!");      
            scp_list.RemoveAt(0);       
        }
        Sculpture new_scp = new Sculpture(id);
        scp_list.Add(new_scp);    
    }

    //List of top COUNT  hot items
    public List<int> hot(int count){
        List<int> hot_ids;
        //TODO: Figure out hot items
        return hot_ids;
    }
    //List of top COUNT controversial ITEMS
    public List<int> cont(){
        List<int> cont_ids;
        //TODO: Figure out controversial items
        return cont_ids;
    }
}


//Testing Function
namespace Monotest
{
    class MainClass
    {
        public static void Main (string[] args)
        {
            Sculptures gallery = new Sculptures(3);
            Console.WriteLine("Loading in item with ID 1");
            gallery.push(1);
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine("Loading in item with ID 2");
            gallery.push(2);
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine("Loading in item with ID 3");
            gallery.push(3);
            System.Threading.Thread.Sleep(2000);
            gallery.push(6);
            System.Threading.Thread.Sleep(2000);
            gallery.push(8);
            Console.WriteLine ("Hello World!");
        }
    }
}

