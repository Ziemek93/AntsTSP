﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AntsTSP
{
   // using alias = ns1;
    class Program
    {
        public double[,] pheromone;
        

        static void Main(string[] args)
        {
            string path = @"C:\Users\Ziemowit\source\repos\AntsTSP\AntsTSP\antsdata.txt";
            int antNum = 1;
            //ns1.Ants a = new ns1.Ants();
            //Console.WriteLine(a.x);
            Ants a = new Ants(path, antNum);
            a.Cities();
            a.iniAnts();

          

            Console.ReadKey();
        }
    }

 


    class Ants
    {
        private ArrayList distanceArr = new ArrayList();
        public const float beta = 0.3f;
        public const float alfa = 0.7f;
        public const float p = 0.7f;

        int length;

        public string errorMsg = "";
        private string fileName;
        private int antNum;
         
        
        private const float q0 = 0.9f;

        public double[,] cityArr;
        public double[,] pheromone;

        private Ant[] antsArr; // , , ,this city number

        public void iniAnts()
        {
            double srednia = 0;
            Random r = new Random();
            int [] randCities = new int[antNum];
            antsArr = new Ant[antNum];


            int x;
            int i = 0;

            while (i < antNum)
            {
                x = r.Next(length);
                antsArr[i] = new Ant(cityArr, pheromone, x, beta, alfa, p);

                i++;
            }
            i = 0;
            int j = 0;

            while (j < 10000)
            {
 
                while (i < length  ) // kroki do przejscia po kazdym miescie
                {
                    
                    foreach (Ant ant in antsArr)
                    {

                        ant.UpdateArrays(pheromone);
                        pheromone = ant.move();
                     

                    }
                    
                   
                    i++;
                }

/////////////////////////////////////////////////////////////////////////////////////////////////////
                //  Thread.Sleep(10);
                 // Console.Clear();
                //  Console.WriteLine("Srednia: " + srednia);
                for (int y = 0; y < pheromone.GetLength(0); y++)
                {
                    for (int z = 0; z < pheromone.GetLength(0); z++)
                    {

                        //Console.Write(pheromone[y, z] + " ");
                        //Console.Write(Math.Round(pheromone[y, z], 1, MidpointRounding.AwayFromZero)  + " ");
                    }
                    //  Console.WriteLine();
                }


                i = 0;
                srednia = 0;
                foreach (Ant ant in antsArr)
                {
                    distanceArr.Add(ant.getDist());
                    ant.getDist(); // do tablicy
                    srednia += ant.getDist();
                   // Console.WriteLine("DISTANCE " + ant.getDist()); // to leci do tablicy i na jej podstawie biorę najlepszą trase
                     
                    ant.distance = 0;

                    x = r.Next(length);
                    antsArr[i] = new Ant(cityArr, pheromone, x, beta, alfa, p);
                }
                srednia = srednia / (i + 1);
                
                j++;


                double Max = double.MinValue;
                double Min = double.MaxValue;

                foreach (double k in distanceArr)
                {
                    if (Max < k)
                    {
                        Max = k;
                    }
                    if (Min > k)
                    {
                        Min = k;
                    }
                }
                int licznik = 0;
                foreach(double z in distanceArr)
                {
                    if(z == Min)
                    {
                        licznik++;
                    }
                }
                //Console.WriteLine(licznik + "   " + Min);
                distanceArr.Clear();
/////////////////////////////////////////////////////////////////////////////////////////////////////
            }



        }
       

        public Ants(string fileName, int antNum)
        {
            this.fileName = fileName;
            this.antNum = antNum;

            
            //q = new Random(100);
        }

        public void Cities()
        {


            this.length = 0;

            try
            {
                string input = File.ReadAllText(fileName);
                int i = 0, j = 0;

                foreach (var row in input.Split('\n')) // cities number
                {
                    this.length++;
                }
                cityArr = new double[length, length];
                pheromone = new double[length, length];
                
                foreach (string row in input.Split('\n'))
                {

                    j = 0;
                    foreach (string col in row.Trim().Split(' '))
                    {
                        cityArr[i, j] = double.Parse(col.Trim());
                        pheromone[i, j] = 0.2;

                        j++;
                    }
                    i++;
                }
                this.cityArr = cityArr;
               // Console.WriteLine("Length = " +  cityArr.GetLength(1));

               

            }

            catch (Exception ex)
            {
                errorMsg = "Could not read the file: " + ex;
                Console.Write(errorMsg);

               
            }


        }

        public bool check(int value, int []randCities)
        {

            foreach(int x in randCities)
            {
                if(value == x)
                {
                    return false;
                }
            }

            return true;
        }
     
        


        public void WinnerChickenDinner()
        {

        }

    }
}
