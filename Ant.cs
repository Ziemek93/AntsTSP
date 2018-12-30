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
        private int Q = 25; // wspolczynnik pozostawianego feromonu

        private int ActiveCity;
        private float beta; //wspolczynnik
        private float alfa; // wspolczynnik
        private float p; //wspolczynnik
        private int length;
        private bool Finish = false;
        private ArrayList visited;
        public double [,] best;
        private int bestCounter;
        public double distance = 0;

        //double []decisionArr;

        public Ant(double [,] cities, double [,] pheromone, int iniCity, float beta, float alfa, float p)
        {
            best = new double[cities.GetLength(1), 2];
            this.cities = cities;
            //this.pheromone = pheromone.Clone() as double[,];

            this.ActiveCity = iniCity;
             
            this.beta = beta;
            this.alfa = alfa;
            this.p = p;

            visited = new ArrayList();
        }


        public void UpdateArrays(double [,] pheromone)
        {// this.pheromone = pher;
            this.pheromone = pheromone.Clone() as double[,];

           // visited = null; // czyszczona jest poprzednia lista odwiedzonych miast
          //  visited = new ArrayList();
           

        }

        public double [] countMethod()
        {

            int i = 0;
            int j = 0;
            int arrLenght = length - visited.Count;
            double[] decisionArr = new double[length];

            int counter1 = 0, counter2 = 0;
            double nominator = 0;
            double denominator = 0;

          //  Console.WriteLine("aktualne " + ActiveCity);
            int counter = 0;
            int newLength = length + 2 - visited.Count;
            while (i < length)
            {
                //Console.WriteLine("i = " + i);
                if (check(i, visited))
                {
                    //Console.WriteLine("i chcked = " + i);
                    nominator = Math.Pow(pheromone[ActiveCity, i], alfa) * Math.Pow(1 / cities[ActiveCity, i], beta); // licznik ułamka
                    //Console.WriteLine("nominator" + nominator);
                    j = 0;
                    
                    while (j < length)
                    {
                        if (check(j, visited))
                        {

                            denominator += Math.Pow(pheromone[ActiveCity, j], alfa) * Math.Pow(1 / cities[ActiveCity, j], beta); // mianownik ułamka
                            //Console.WriteLine("-----------------------------------------------------------------------" + cities[ActiveCity, j]);
                            //Console.WriteLine("ph " + pheromone[ActiveCity, j] + "   ci " + cities[ActiveCity, j] + "      c - "+ ActiveCity + " co - " + j);
                            //Console.WriteLine("----------------------");
                            //Console.WriteLine("  Vi " + visited.Count + "  " + "Sprawdzam sciezke nr " + j);

                            //Console.WriteLine("denominator " + denominator);
                             
                        }
                       
                        j++;
                    }


                     
                    decisionArr[i] = nominator / denominator; // zmiana
                    nominator = 0;
                    denominator = 0;
                    counter++;



                }
                i++;
            }

            

            return decisionArr;
        }

        public int makeDecision()
        {
           // Console.WriteLine("-----------------------------------Make decision-----------------");
            //this.decision = 0;
            bool returning = false;
            double[] decisionArr = null;

            this.length = cities.GetLength(1);

             visited.Add(ActiveCity); // dodaje aktywne miasto do miast odwiedzonych, tablica miast - x,x,0,x etc, 0 to odwiedzone
            //Console.WriteLine(visited.Count + "----------------------------------------------------------------------------" + length);
            if (length == visited.Count) { returning = true; }// }visited.RemoveAt(0);  } // końcowe usuwanie punktu startu z listy by mrówka mogła do niego wrócić

            
           
          //  Console.WriteLine(length + "---------------------------------------------------" + visited.Count);
            // Console.WriteLine("-----------------------------------------------" + "DODAJE");
            switch (returning)
            {
                case false:
                    decisionArr = countMethod();
                    break;

                case true:
                    visited.RemoveAt(0);
                  //  Console.WriteLine(length + "---------------------------------------------------" + visited.Count);
                    decisionArr = countMethod();
                    //   Console.WriteLine("--------------------------------------Returning true---------Returning home-----------------------------------------------------");
                    
                    Finish = true; // wszystkie miasta zostaly odwiedzone wiec nie trzeba szukac dalej
                  //  Console.WriteLine(Finish);

                    break;

            }


             
            //foreach (double x in decisionArr)
            //{
            //    Console.WriteLine(x);
            //}
          //  Console.WriteLine("-----------------------------------Going to roulette-----------------");
           return Roulette(decisionArr); // wieksza szansa na wybranie lepszej ścieżki
           //this.decision = decisionArr.Max();

        }

        public double [,] move()
        {


            int nextCity;
            
            //while(Finish)
            //{
            if(!Finish)
            {
                //Console.WriteLine(Finish);
               nextCity = makeDecision();
                //if(nextCity < 0) { Console.WriteLine("Something goes wrong in move"); }
               // Console.WriteLine("DECISION " + nextCity);
                pheromone[ActiveCity, nextCity] = (1 - p) * pheromone[ActiveCity, nextCity] + Q / cities[ActiveCity, nextCity]; // parowanie i zostawianie feromonu
                int j = 0;

                distance += cities[ActiveCity, nextCity];
               // Console.WriteLine(distance);
                best[bestCounter, 0] = ActiveCity;
                best[bestCounter, 1] = pheromone[ActiveCity, nextCity];
                bestCounter++;

                ActiveCity = nextCity;// ?

                if(Finish)
                {
                    //   Console.WriteLine("-------------------------------------------------------------------------------------------------------" + best.Length + " " );
                    //foreach (double w in best)
                    //{
                    //    Console.WriteLine(w);
                    //}
                    visited = null;
                    visited = new ArrayList();
                    bestCounter = 0;
                    Finish = false;
                    //Console.WriteLine(Finish);
                    // Console.WriteLine("-------------------------------------------------------------------------------------------------------");
                }

                //  Console.WriteLine("-------------------------------------------------------------------------------------------------------");

            }
            


            return pheromone;

        }

       public double getDist()
        {
            
            return distance;
        }

        public bool condition(int i)
        {
            if(i == 1)
            {
                return false;
            }
            return true;
        }

        public bool check(int value, ArrayList randCities)
        {

            foreach (int x in randCities)
            {
                // Console.WriteLine("                                     " + value + "  ==  " + x);
                if (value == x)
                {
                    return false;
                }
            }

            return true;
        }
     

        public int Roulette(double[] refPaths)
        {
            double[]  paths= new double[refPaths.Length - visited.Count]; // = refPaths.Clone() as double[];
            //Console.WriteLine(" PL " + paths.Length);
             

            Random r = new Random();
            double randomPoint;
            double resultPh = 0;

            double sum = paths.Sum();
            int[] resCount = new int[paths.Length];
            int i = 0;
            // Console.WriteLine("S");
         //   Console.WriteLine("---------------------------");
            int counter = 0;

            foreach(int k in visited)
            {

          //      Console.WriteLine("VIISISITED " + k);
            }
            //Console.WriteLine("visited" + visited.Count + " paths " + paths.Length);
            //Console.WriteLine("refPaths.Length " + refPaths.Length + " - " + visited.Count);
            while (i < refPaths.Length)
            {
               
                if (check(i, visited))
                {
                   
                 //Console.WriteLine("counter " + counter + "  " +  i);
                    resCount[counter] = i; // spierdala czasem pozazakres
                    paths[counter] = refPaths[i];
                  //   Console.WriteLine(paths[counter] + " | " + resCount[counter] + "   i = " + i);
                    counter++;
                }
                i++;
            }
             i = 0;
          //  Console.WriteLine("---------------------------");
            //Console.WriteLine("S");
            //foreach (double x in paths)
            //{
            //    Console.WriteLine("Paths   "+x);
            //}


            for (i = 0; i < paths.Length - 1; i++) // skad ten -1
            {

                int index = i;
                for (int j = i + 1; j < paths.Length; j++)
                    if (paths[j] < paths[index])  //min ? sprawdzic - musi byc od min do max
                        index = j;



                int smallerN = resCount[index];
                resCount[index] = resCount[i];
                resCount[i] = smallerN;

                double smallerNumber = paths[index];
                paths[index] = paths[i];
                paths[i] = smallerNumber;
            }

          //  Console.WriteLine("T");
            //foreach (double x in paths)
            //{
            //    Console.WriteLine(x);
            //}

            //Console.WriteLine("---------------------------");

            randomPoint = (r.Next(0, 100));
            randomPoint /= 100;

            int choosenRes = 0;
            i = 0;
            double lastValue = 0;


            double value = 0 ;
            while (i < paths.Length)
		 {
                
                value = paths[i] + lastValue;
             // Console.WriteLine("RandomPoint - " + randomPoint + "  lastV - " + lastValue + "   thisV " + value  + "   paths[i] " + paths[i]);
                if (randomPoint >= lastValue && randomPoint <= value)
                {
                  
                    choosenRes = resCount[i];
                    resultPh = paths[i];
                   //  Console.WriteLine("Hey " + choosenRes);
                    break;
                }
                lastValue = value;
                i++;
            }


            if(resultPh == 0)
            {
                Console.WriteLine("Ant - Roulette - Something goes wrong - resultPH = 0");
            }

          //  Console.WriteLine("ret");
            return choosenRes; //resultPh; wybieram wybraną scieżke

        }




    }
}
