using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            int antNum = 20;
            //ns1.Ants a = new ns1.Ants();
            //Console.WriteLine(a.x);
            Ants a = new Ants(path, antNum);
            a.Cities();

            Console.ReadKey();
        }
    }




    class Ants
    {
        public const float beta = 0.6f;
        public const float alfa = 0.2f;
        public const float p = 0.6f;

        int length;

        public string errorMsg = "";
        private string fileName;
        private int antNum;
        private double decision;
        private Random q;
        private const float q0 = 0.9f;

        public double[,] cityArr;
        public double[,] pheromone;

        private Ant[] antsArr; // , , ,this city number

        public void iniAnts()
        {
            Random r = new Random();
            int [] randCities = new int[antNum];

            int x;
            int i = 0;
            while (i < antNum)
            {
                 x = r.Next(length);
                //if (check( x, randCities))
                // {
                //     antsArr[i] = new Ant(x);
                //     i++;
                // }
                antsArr[i] = new Ant(cityArr, pheromone, x, beta, alfa, p);
                i++;
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
                cityArr = new double[length, length - 1];
                pheromone = new double[length, length - 1];

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
        
    }
}
