using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AntsTSP
{
    class Ant
    {

        //double [,]pheromone;
        double[,] cities;
        double[,] pheromone;
        private int Q = 5000; // wspolczynnik pozostawianego feromonu

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

        public Ant(double [,] cities, double [,] pher, int iniCity, float beta, float alfa, float p)
        {
            this.pheromone = new double[pher.GetLength(0), pher.GetLength(0)];
            for (int i = 0; i < pher.GetLength(0); i++)
            {
                for (int j = 0; j < pher.GetLength(0); j++)
                {
                    this.pheromone[i, j] = pher[i, j];
                    // Console.WriteLine(this.pheromone[i, j] + "   " + pheromone[i, j] );
                }
            }


            best = new double[cities.GetLength(1), 2];
            this.cities = cities;
            //this.pheromone = pheromone.Clone() as double[,];

            this.ActiveCity = iniCity;
             
            this.beta = beta;
            this.alfa = alfa;
            this.p = p;

            visited = new ArrayList();
        }


        public void UpdateArrays(double [,] pher)
        {// this.pheromone = pher;
            //this.pheromone = pher.Clone() as double[,];
            this.pheromone = new double[pher.GetLength(0), pher.GetLength(0)];
            for (int i = 0; i < pher.GetLength(0); i++)
            {
                for(int j = 0; j < pher.GetLength(0); j++)
                {
                    this.pheromone[i, j] = pher[i, j];
                   // Console.WriteLine(this.pheromone[i, j] + "   " + pheromone[i, j] );
                }
            }

            if(pheromone != pheromone)
            {
                throw new System.ArgumentException("Updated pheromone == null");
            }

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
                 
                    nominator = Math.Pow(pheromone[ActiveCity, i], alfa) * Math.Pow(1 / cities[ActiveCity, i], beta); // licznik ułamka
                    //Console.WriteLine("N " +nominator + " = " + pheromone[ActiveCity, i] +"^alfa" + " * " + 1 / cities[ActiveCity, i] + "^beta");
                    
                    j = 0;
                    
                    while (j < length)
                    {
                        if (check(j, visited))
                        {

                            denominator += Math.Pow(pheromone[ActiveCity, j], alfa) * Math.Pow(1 / cities[ActiveCity, j], beta); // mianownik ułamka
                                                                                                                                 // Console.WriteLine("D " +denominator + " = " + pheromone[ActiveCity, j] + "^alfa" + " * " + 1 / cities[ActiveCity, j] + "^beta");
                            
                        }
                       
                        j++;
                    }


                    try
                    {
                        decisionArr[i] = nominator / denominator; // zmiana
                        
                        if (denominator == 0 || nominator == 0)
                            throw new DivideByZeroException();
                    }
                    catch (DivideByZeroException)
                    {
                        
                        throw new System.ArgumentException("Divide by zero countMethod(), probably pheromone = 0 ");
                    }
                    
                    
                    nominator = 0;
                    denominator = 0;
                    counter++;



                }
                i++;
            }

            

            return decisionArr;
        }

        public int makeDecision()//nie wlatuje
        {
             

           // Console.WriteLine("-----------------------------------Make decision-----------------");
            bool returning = false;
            double[] decisionArr = null;

            this.length = cities.GetLength(1);

             visited.Add(ActiveCity); // dodaje aktywne miasto do miast odwiedzonych, tablica miast - x,x,0,x etc, 0 to odwiedzone
            if (length == visited.Count) { returning = true; }// }visited.RemoveAt(0);  } // końcowe usuwanie punktu startu z listy by mrówka mogła do niego wrócić
 
         
            switch (returning)
            {
                case false:
                    decisionArr = countMethod();
                    break;

                case true:
                    visited.RemoveAt(0);      
                    decisionArr = countMethod();
                    //   Console.WriteLine("--------------------------------------Returning true---------Returning home-----------------------------------------------------");  
                    Finish = true; // wszystkie miasta zostaly odwiedzone wiec nie trzeba szukac dalej

                    break;

            }

          //  Console.WriteLine("-----------------------------------Going to roulette-----------------");
           return Roulette(decisionArr); // wieksza szansa na wybranie lepszej ścieżki
           //this.decision = decisionArr.Max();

        }


        public void move()
        {


            int nextCity = 0;

            //while(Finish)
            //{
            if (!Finish)
            {
                //Console.WriteLine("!FINISH");
                nextCity = makeDecision();
                distance += cities[ActiveCity, nextCity];
                //Console.WriteLine();
                //Console.Write("Przed " + pheromone[ActiveCity, nextCity] + "  ");
                pheromone[ActiveCity, nextCity] += (Q / distance);// (1 - p) * pheromone[ActiveCity, nextCity] + (Q / distance); // parowanie i zostawianie feromonu
                                                                  //Console.Write("  0.1 * " + pheromone[ActiveCity, nextCity] + "  Q " + Q + "  dist " + distance);
                                                                  //Console.WriteLine();
                int j = 0;



                best[bestCounter, 0] = ActiveCity;
                best[bestCounter, 1] = pheromone[ActiveCity, nextCity];
                bestCounter++;

                ActiveCity = nextCity;// ?

                if (Finish)
                {
                    //   Console.WriteLine("-------------------------------------------------------------------------------------------------------" + best.Length + " " );
                    // Console.WriteLine("Visited count " + visited.Count);
                    visited = null;
                    visited = new ArrayList();
                    bestCounter = 0;
                    Finish = false;

                    //Console.WriteLine(Finish);
                    // Console.WriteLine("-------------------------------------------------------------------------------------------------------");
                }

                //  Console.WriteLine("-------------------------------------------------------------------------------------------------------");

            }

        }


        public void moveLocal()
        {
            int nextCity = 0;

            //while(Finish)
            //{
            if (!Finish)
            {
                //Console.WriteLine("!FINISH");
                nextCity = makeDecision();
                distance += cities[ActiveCity, nextCity];
               /// Console.WriteLine();
               // Console.Write(ActiveCity + " - " + nextCity + "  Przed " + pheromone[ActiveCity, nextCity] + "  ");
                pheromone[ActiveCity, nextCity] = pheromone[ActiveCity, nextCity] + (Q / distance);// (1 - p) * pheromone[ActiveCity, nextCity] + (Q / distance); // parowanie i zostawianie feromonu
                pheromone[nextCity, ActiveCity] = pheromone[ActiveCity, nextCity];

                 

                 //Console.Write("Po    " + pheromone[ActiveCity, nextCity] + "  ");                                            //Console.Write("  0.1 * " + pheromone[ActiveCity, nextCity] + "  Q " + Q + "  dist " + distance);
               // Console.WriteLine();                                                                                                            //Console.WriteLine();
                int j = 0;



                best[bestCounter, 0] = ActiveCity;
                best[bestCounter, 1] = pheromone[ActiveCity, nextCity];
                bestCounter++;

                ActiveCity = nextCity;// ?

                if (Finish)
                {
                    //   Console.WriteLine("-------------------------------------------------------------------------------------------------------" + best.Length + " " );
                    // Console.WriteLine("Visited count " + visited.Count);
                    visited = null;
                    visited = new ArrayList();
                    bestCounter = 0;
                    Finish = false;


                   
                    //Console.WriteLine(Finish);
                    // Console.WriteLine("-------------------------------------------------------------------------------------------------------");
                }
            }

        }

        

        public double[,] getPheromone()
        {
            double [,] pher  = new double[pheromone.GetLength(0), pheromone.GetLength(0)];
            for (int i = 0; i < pheromone.GetLength(0); i++)
            {
                for (int j = 0; j < pheromone.GetLength(0); j++)
                {
                    pher[i, j] = pheromone[i, j];
                    pher[i, j] = (pher[i, j] * (1 - p)) < 0.0001 ? 0.0001 : pher[i, j] * (1 - p);
                  //  Console.Write(pher[i, j] + "  "  );
                }
               // Console.WriteLine();
            }


            return pher;
        }

       public double getDist()
        {
             
            //Console.WriteLine("-------------------------------------------------------------------------------------------------------");
            return distance;
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
            double[]  paths = new double[refPaths.Length - visited.Count]; // = refPaths.Clone() as double[];
            //Console.WriteLine(" PL " + paths.Length + " visited " + visited.Count);
             

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

             //  Console.WriteLine("VIISISITED " + k);
            }
            //Console.WriteLine("visited" + visited.Count + " paths " + paths.Length);
            //Console.WriteLine("refPaths.Length " + refPaths.Length + " - " + visited.Count);
            while (i < refPaths.Length)
            {
               
                if (check(i, visited))
                {
                   // Console.WriteLine(paths.Length + " " + counter + "  miasto nr" + i);
                    //Console.WriteLine("counter " + counter + "  " +  i);
                    resCount[counter] = i; // spierdala czasem pozazakres
                     
                    paths[counter] = refPaths[i];
                  //Console.WriteLine(paths[counter] + " | " + resCount[counter] + "   i = " + i);
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
           //Console.WriteLine("       RandomPoint - " + randomPoint + "  lastV - " + lastValue + "   thisV " + value  + "   paths[i] " + paths[i] + "  xxx " + cities[ActiveCity, resCount[i]] );
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
