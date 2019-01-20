using System;
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
            int antNum = 15;
            //ns1.Ants a = new ns1.Ants();
            //Console.WriteLine(a.x);
            Ants a = new Ants(path, antNum);
            a.Cities();
            a.iniAnts("local_global"); // local_global, global

          

            Console.ReadKey();
        }
    }

 


    class Ants
    {
        private int choosenOne = 0;
        private ArrayList distanceArr = new ArrayList();
        public const float beta = 0.4f;
        public const float alfa = 0.9f;
        public const float p = 0.9f;
        double shortest = 0;
        int length;

        public string errorMsg = "";
        private string fileName;
        private int antNum;
         
        
        private const float q0 = 0.9f;

        public double[,] cityArr;
        public double[,] pheromone;

        private Ant[] antsArr; // , , ,this city number

        public void iniAnts(string pheromoneMethod)
        {
            Random r = new Random();
            double srednia = 0;
             
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
            
            int j = 0;
            //switch(pheromoneMethod)
            //{ }
            while (j < 10000)
            {
                 

                //Console.WriteLine("---------------------------- " + j + " -------------------------------");
                switch (pheromoneMethod)
                {
                    case "global":
                        globalPheromoneMethod(pheromone);

                        break;

                    case "local_global":
                        localglobalPheromoneMethod(pheromone);

                        break;
                }
                 
                /////////////////////////////////////////////////////////////////////////////////////////////////////
                Thread.Sleep(50);
                Console.Clear();
                  Console.WriteLine("Srednia: " + srednia);
                for (int y = 0; y < pheromone.GetLength(0); y++)
                {
                    for (int z = 0; z < pheromone.GetLength(0); z++)
                    {
                        if (y != z)
                        {
                            //Console.WriteLine(" FIRST " + pheromone[y, z] + " FIRST ");
                           // pheromone[y, z] = (pheromone[y, z] * (1 - p) ) < 0.2 ? 0.2 : pheromone[y, z] * (1 - p); // zeruje sie po parowaniu
                            //Console.WriteLine(" SECOND " + pheromone[y, z] + " SECOND ");
                            if (pheromone[y, z] == 0) { throw new System.ArgumentException("Pheromone[" + y + "," + z + "] == 0"); } // pheromone[y, z] = double.MinValue;

                            //pheromone[y, z] = Math.Round(pheromone[y, z], 4, MidpointRounding.ToEven);
                            //Console.Write(pheromone[y, z] + " ");

                             Console.Write(Math.Round(pheromone[y, z], 4, MidpointRounding.AwayFromZero)  + " ");
                        }
                        else
                        {
                            Console.Write("X ");
                        }
                        }
                      Console.WriteLine();
                }


                i = 0;
                srednia = 0;
                foreach (Ant ant in antsArr)
                {
                    distanceArr.Add(ant.getDist());
                    ant.getDist(); // do tablicy
                    srednia += ant.getDist();
                    Console.WriteLine("DISTANCE " + ant.getDist()); // to leci do tablicy i na jej podstawie biorę najlepszą trase
                     
                    ant.distance = 0;

                    //x = r.Next(length);
                    //antsArr[i] = new Ant(cityArr, pheromone, x, beta, alfa, p);
                    i++;
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
                        choosenOne = licznik; // najlepsza trasa, lub jedna z najlepszych
                        licznik++;
                    }
                }
                if (pheromoneMethod == "local_global") {  pheromone = antsArr[choosenOne].getPheromone(); } //Console.WriteLine("PL " + pheromone.Length); }
                Console.WriteLine(licznik + "   " + Min);
                distanceArr.Clear();


                int c = 0;
                while( c < length)
                {
                    antsArr[c] = new Ant(cityArr, pheromone, r.Next(length), beta, alfa, p);
                    c++;
                }

/////////////////////////////////////////////////////////////////////////////////////////////////////
            }



        }
       
        private void globalPheromoneMethod(double [,] pheromone)
        {

            double[,] pheromoneCopy = new double[pheromone.GetLength(0), pheromone.GetLength(0)];
            //this.pheromone = pher.Clone() as double[,];
             
            for (int i = 0; i < pheromone.GetLength(0); i++)
            { 
                for (int j = 0; j < pheromone.GetLength(0); j++)
                {
                    pheromoneCopy[i, j] = pheromone[i, j];
                    // Console.WriteLine(this.pheromone[i, j] + "   " + pheromone[i, j] );
                }
            }


            int k = 0;
            
            //Console.WriteLine("---------------------------- " + j + " -------------------------------");
            while (k < length) // kroki do przejscia po kazdym miescie
            {

                foreach (Ant ant in antsArr)
                {

                    ant.UpdateArrays(pheromone); // updating new pheromon \/
                    ant.move();
                    pheromone = ant.getPheromone();

                }


                k++;
            }
            



            for(int i = 0; i < pheromone.GetLength(0); i++) // sprawdzanie czy czy dana sciężka była uczeszczana - jesli tak, parowanie feromonu
            {
                for (int j = 0; j < pheromone.GetLength(0); j++)
                {
                    if (pheromoneCopy[i, j] != pheromone[i, j])
                    {
                       pheromone[i, j] = pheromone[i, j] * (1 - p); 
                        // Console.WriteLine(this.pheromone[i, j] + "   " + pheromone[i, j] );
                    }
                }
            }

            this.pheromone = pheromone; // przyoisanie feromonu lok do glob



        }

        private void localglobalPheromoneMethod(double [,] pheromone)
        { 
            //Console.WriteLine("----------------------------------------------------------------------------------------");
            double[,] pheromoneCopy = new double[pheromone.GetLength(0), pheromone.GetLength(0)];

            for (int i = 0; i < pheromone.GetLength(0); i++)
            {
                for (int j = 0; j < pheromone.GetLength(0); j++)
                {
                    pheromoneCopy[i, j] = pheromone[i, j];
                    // Console.WriteLine(this.pheromone[i, j] + "   " + pheromone[i, j] );
                }
            }


  
            foreach (Ant ant in antsArr)
            { 

                 
                ant.UpdateArrays(pheromoneCopy);

            }

            int k = 0;
            while (k < length) // kroki do przejscia po kazdym miescie
            {

                foreach (Ant ant in antsArr)
                {

                   //ant.UpdateArrays(pheromone); // updating new pheromon \/
                    ant.moveLocal();
                   // pheromone = ant.getPheromone();

                }
                k++;
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
