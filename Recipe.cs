using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POE
{
    public class Recipe 
    {
        string[] steps;

        public string[] getSteps(int numOfSteps)
        {
            Console.WriteLine("Provide steps for recipe:");
            steps = new string[numOfSteps];
            for (int x = 0; x < numOfSteps; x++)
            {
                Console.Write($"{x+1})");
                steps[x] = Console.ReadLine();
            }

            return steps;
        }
       
    }
}
