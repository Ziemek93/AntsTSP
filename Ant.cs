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
        Random r;
         double[,] cities;
        double[,] pheromone;
        private int Q = 500; // wspolczynnik pozostawianego feromonu

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

        public int test = 0;
 
        public Ant(double [,] cities, double [,] pher, int iniCity, float beta, float alfa, float p, Random r)
        {
            this.r = r;
            test = iniCity;
            this.pheromone = new double[pher.GetLength(0), pher.GetLength(0)];
            for (int i = 0; i < pher.GetLength(0); i++)
            {
                for (int j = 0; j < pher.GetLength(0); j++)
                {
                    this.pheromone[i, j] = pher[i, j];
                 }
            }


            best = new double[cities.GetLength(1), 2];
            this.cities = cities;
 
            this.ActiveCity = iniCity;
             
            this.beta = beta;
            this.alfa = alfa;
            this.p = p;

            visited = new ArrayList();

             
        }

        
        public void UpdateArrays(double [,] pher)
        { 
            this.pheromone = new double[pher.GetLength(0), pher.GetLength(0)];
            for (int i = 0; i < pher.GetLength(0); i++)
            {
                for(int j = 0; j < pher.GetLength(0); j++)
                {
                    this.pheromone[i, j] = pher[i, j];
                 }
            }

            if(pheromone != pheromone)
            {
                throw new System.ArgumentException("Updated pheromone == null");
            }
 
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

        //makeDecision() odpowiada za podejmowanie decyzji
        public int makeDecision()//nie wlatuje
        { 

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
                     Finish = true; // wszystkie miasta zostaly odwiedzone wiec nie trzeba szukac dalej

                    break;

            }

            return Roulette(decisionArr); // wieksza szansa na wybranie lepszej ścieżki
 
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


        //metoda move odpowiada za poruszanie się mrówki
        public void moveLocal()
        {
            int nextCity = 0;

             
            if (!Finish)
            {
               
                nextCity = makeDecision();
                distance += cities[ActiveCity, nextCity];
              
                pheromone[ActiveCity, nextCity] = pheromone[ActiveCity, nextCity] + (Q / distance);// (1 - p) * pheromone[ActiveCity, nextCity] + (Q / distance); // parowanie i zostawianie feromonu
                pheromone[nextCity, ActiveCity] = pheromone[ActiveCity, nextCity];
                                                                                                                           
                int j = 0;



                best[bestCounter, 0] = ActiveCity;
                best[bestCounter, 1] = pheromone[ActiveCity, nextCity];
                bestCounter++;

                ActiveCity = nextCity;// ?

                if (Finish)
                { 
                    visited = null;
                    visited = new ArrayList();
                    bestCounter = 0;
                    Finish = false;
                     
                   
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

        //W metodzie Roulette wybierania jest ścieżka przy pomocy losowania.Im bardziej korzystniejsza jest droga tym większe ma szanse na wylosowanie.
        public int Roulette(double[] refPaths)
        {
            double[]  paths = new double[refPaths.Length - visited.Count];  
         
             
            double randomPoint = 0;
            double resultPh = 0;

            double sum = paths.Sum();
            int[] resCount = new int[paths.Length];
            int i = 0;
  
            int counter = 0;
 
            while (i < refPaths.Length)
            {
               
                if (check(i, visited))
                { 
                    resCount[counter] = i; 
                     
                    paths[counter] = refPaths[i];
                    counter++;
                }
                i++;
            }
             i = 0;
         


            for (i = 0; i < paths.Length - 1; i++)  
            {

                int index = i;
                for (int j = i + 1; j < paths.Length; j++)
                    if (paths[j] < paths[index])   
                        index = j;



                int smallerN = resCount[index];
                resCount[index] = resCount[i];
                resCount[i] = smallerN;

                double smallerNumber = paths[index];
                paths[index] = paths[i];
                paths[i] = smallerNumber;
            }
             
            randomPoint = (r.Next(0, 100));
            randomPoint  = randomPoint /  100;
          
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
                    break;
                }
                lastValue = value;
                i++;
            }


            if(resultPh == 0)
            {
                Console.WriteLine("Ant - Roulette - Something goes wrong - resultPH = 0");
            }

             return choosenRes; 

        }




    }
}
