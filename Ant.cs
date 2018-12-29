using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntsTSP
{
    class Ant
    {

        //double [,]pheromone;
        double[,] cities;
        double[,] pheromone;

        private int ActiveCity;
        private float beta;
        private float alfa;
        private float p;

        private ArrayList visited;

        double decision;
       //double []decisionArr;

        public Ant(double [,] cities, double [,] pheromone, int iniCity, float beta, float alfa, float p)
        {
            this.cities = cities;
            //this.pheromone = pheromone.Clone() as double[,];

            this.ActiveCity = iniCity;
             
            this.beta = beta;
            this.alfa = alfa;
            this.p = p;
        }


        public void UpdateArrays(double [,] pheromone)
        {// this.pheromone = pher;
            this.pheromone = pheromone.Clone() as double[,];

            visited = null;
            visited = new ArrayList();


        }

        public int makeDecision()
        {
            //this.decision = 0;
            

            int length = cities.GetLength(1);
             
            int i = 0;
            int j = 0;

            double [] decisionArr = new double[length];
            double nominator = 0;
            double denominator = 0;


            while(i < length)
            {

                nominator = Math.Pow(pheromone[ActiveCity, i], alfa) * Math.Pow(1 / cities[ActiveCity, i], beta); // licznik ułamka

                while ( j < length)
                {
                    if (check(j, visited))
                    {
                        denominator += Math.Pow(pheromone[ActiveCity, j], alfa) * Math.Pow(1 / cities[ActiveCity, j], beta); // mianownik ułamka
                        j++;
                    }
                }


                decisionArr[i] = nominator / denominator;
                nominator = 0;
                denominator = 0;
                i++;
            }

            visited.Add(ActiveCity); // dodaje aktywne miasto do miast odwiedzonych

            this.decision = decisionArr.Max(); // nie jestem pewien czy po prostu, od razu powinienem wybierac najelpszą ścieżke------------------------------------
            int k = 0;
            while(k < decisionArr.Length)
            {
                if ( decisionArr[k] == decision )
                {
                    return k;
                }
                    k++;
            }

            return -1;
             
        }

        public void move(int decision)
        {
            int nextCity;

            int i = 0;
            while(condition(i))
            {

               nextCity = makeDecision();
                if(nextCity < 0) { Console.WriteLine("Something goes wrong in move"); }
                //Program.pheromone[ActiveCity, nextCity] = (1 - p) * Program.pheromone[ActiveCity, nextCity];

                i++;
            }

        }

       
        public bool condition(int i)
        {
            if(i == 100)
            {
                return false;
            }
            return true;
        }

        public bool check(int value, ArrayList randCities)
        {

            foreach (int x in randCities)
            {
                if (value == x)
                {
                    return false;
                }
            }

            return true;
        }

    }
}
