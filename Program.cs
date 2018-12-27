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
        public int x = 1;
        public string errorMsg = "";
        private string fileName;
        private int antNum;
        private double decision;
        private Random q;
        private const float q0 = 0.9f;

        private double[,] cityArr;
        private double[,] pheromone;

        private double [,,,] antsArr; // , , ,this city number
        public Ants(string fileName, int antNum)
        {
            this.fileName = fileName;
            this.antNum = antNum;

            //q = new Random(100);
        }

        public double [,] Cities()
        {
        

            int length = 0;

            try
            {
                string input = File.ReadAllText(fileName);
                int i = 0, j = 0;

                foreach (var row in input.Split('\n')) // cities number
                {
                    length++;
                }
                cityArr = new double[length, length];
                pheromone = new double[length, length];

                foreach (string row in input.Split('\n'))
                {
                    
                    j = 0;
                    foreach (string col in row.Trim().Split(' '))
                    {
                            cityArr[i, j] = double.Parse(col.Trim());
                        j++;
                    }
                    i++;
                }

                return cityArr;

            }   
            
            catch (Exception ex)
            {
                errorMsg = "Could not read the file: " + ex;
                Console.Write(errorMsg);

                return null;
            }

            
        }


    }
}
