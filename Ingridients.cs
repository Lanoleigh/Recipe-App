using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static POE.Ingridients;

namespace POE
{
    public class Ingridients : Recipe
    {
        //part 2 variable
        public delegate void AlertDelegate(double c, Ingridients[] ins);
        AlertDelegate alertDelegate = alertUser;
        public List<string> recipeNames = new List<string>();
        public List<Ingridients> ingridientsList = new List<Ingridients>();
        public string? recipeName;
        public double calories;
        public double totalCalories;
        public string? foodGroup;
        public static Ingridients i = new Ingridients();
        public  Ingridients[] ingArray;
        public string[] food_groups = {"Protein","Vegetables","Dairy","Fruit","Carbohydrate",};
        //part 1 variables
        public string? ingridientName;
        public int quatity;
        public string? unitOfMeasurement;
        public string[]? steps;      
        //used in chooseUnitOfMeasurement method
        public string[] measurements = {"Cups", "Grams","TableSpoons","Liters","Mililiters","Miligrams","TeaSpoons","Kilograms","Other"};
        
        //delegate method
        public static void alertUser(double c, Ingridients[] ins)
        {
            if(c > 300)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("This Recipe's calory intake exceeds 300");
                foreach(var item in ins)
                {
                    Console.WriteLine($"{item.ingridientName} = {item.calories} cal");
                }
                Console.ResetColor();
            }
        }

        private void displaySpecificRecipe()
        {
            Console.WriteLine("Enter the number of the recipe you would like to see.");
            string input = Console.ReadLine();
            int choice = checkInt(input);
            if(choice >= 0 && choice < ingridientsList.Count)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"\nRecipe:{ingridientsList[choice].recipeName} ");
                Console.WriteLine("Ingridients:");
                int z = 1;
                foreach(Ingridients items in ingridientsList[choice].ingArray)
                {
                    Console.WriteLine($"Ingridient{z}) {items.ingridientName}  {items.quatity}  {items.unitOfMeasurement} " +
                        $"{items.calories} cal, Food Group: {items.foodGroup}");
                    z++;
                }
                int x = 1;
                Console.WriteLine("Steps:");
                foreach (string ss in ingridientsList[choice].steps)
                    Console.WriteLine($"{x}){ss}");
                
                Console.WriteLine("\n");
            }
            else
            {
                Console.WriteLine("Please enter a number within the range provided.");
                displaySpecificRecipe();
            }
            Console.WriteLine("Enter the number for the option you want to choose.\n1)Scale Quantities\n2)Main Menu");
            string s = Console.ReadLine();
            int o = checkInt(s);
            if (o <= 2 && o > 0)
            {
                switch (o)
                {
                    case 1: ScaleIngridients(ingridientsList[choice].ingArray); break;
                    case 2: Menu();break;
                }
            }
            else
            {
                Console.WriteLine("You entered a digit out of bounds");
                Menu();
            }

        }
        public Boolean PromptUser()
        {
            
            Boolean result = false;
            Console.WriteLine("Please type 1 to launch application and any other key to terminate program.");
            string ans = Console.ReadLine();
            if (ans == "1") {
                result = true;
                Menu();
            }
            else
                Environment.Exit(0);
           
            return result;

        }
        //part 2 method
        private static void Menu()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Please enter the corresponding number for the menu option \ne.g type and enter number 1 to select 1)Publish Recipe\n");
            string menu = "1) View all Recipes" +
                " \n2) Enter new Recipe " +
                " \n3) Exit Application\n" +
                "Menu option: ";
            Console.WriteLine(menu);
            Console.ResetColor();
            int optionSelected = Convert.ToInt32(Console.ReadLine());

            if (optionSelected >= 1 && optionSelected <= 4)
            {
                switch (optionSelected)
                {
                    case 1: i.displayRecipes(); break;
                    case 2: i.receiveInput(); break;
                    case 3: Environment.Exit(0); break;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please enter a number between 1 and 3");
                Console.ResetColor();
                Menu();//recursive programming
            }
        }
        //receives user input.
        public void receiveInput()
        {
            totalCalories = 0;

            Console.Write("Name of Recipe: ");
            recipeName = Console.ReadLine();
            Console.Write("how many ingridients are in this recipe: ");
            string numOfIngridients = Console.ReadLine();
            int num = checkInt(numOfIngridients);
            ingArray = new Ingridients[num];

            for (int x = 0; x < num; x++)
            {
                ingArray[x] = new Ingridients();
                Console.WriteLine($"Ingridient #{x + 1}");
                Console.Write("Name of Ingridient: ");
                ingArray[x].ingridientName = Console.ReadLine();
                //Error Handling
                ingArray[x].ingridientName = checkString(ingArray[x].ingridientName);
                Console.Write("Enter the quantity of this ingridient: ");
                string check = Console.ReadLine();
                //error Handling
                ingArray[x].quatity = checkInt(check);
                Console.Write("What is the unit of measurement: \n");
                ingArray[x].unitOfMeasurement = ChooseUnitOfMasurement();
                Console.Write("How many calories does this ingridient have: ");
                string cal = Console.ReadLine();
                
                //error handling
                ingArray[x].calories = checkDouble(cal);
                calculateCalories(Double.Parse(cal));
                Console.WriteLine("Select the Food Group Below");
                ingArray[x].foodGroup = FoodGroup();
            }
            Console.WriteLine("How many step in the recipe:");
            int numOfSteps = Int32.Parse(Console.ReadLine());
            steps = new string[numOfSteps];
            for(int y = 0; y < numOfSteps; y++)
            {
                Console.Write($"{y+1})");
                string s = Console.ReadLine();
                steps[y] = checkString(s);//validation on the steps ensuring irs only string.
            }

            Ingridients inObject = new Ingridients();
            inObject.recipeName = recipeName;
            inObject.ingArray = ingArray;
            inObject.steps = steps;



            ingridientsList.Add(inObject);
            Console.WriteLine($"The total Calories for this recipe is: {totalCalories}");
            alertDelegate(totalCalories,ingArray);
            Console.WriteLine("You have successfully entered the recipe!");
            //user will be directed to the menu.
            Menu();

        }
        public double calculateCalories(double cal)
        {
            return totalCalories += cal;
        }
        //error handling
        private double checkDouble(string d)
        {
            double x;
            while(!double.TryParse(d,out x))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Please enter a valid digit:");
                Console.ResetColor();
                d = Console.ReadLine();
            }
            return x;
        }
        private int checkInt(string input)
        {

            int x;
            while(!int.TryParse(input, out x))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Please enter a valid digit:");
                Console.ResetColor();
                input = Console.ReadLine();
            }

            return x;

        }
        //error handling
        private string checkString(string s)
        {
            //https://stackoverflow.com/questions/18251875/in-c-how-to-check-whether-a-string-contains-an-integer
            bool check = s.Any(char.IsDigit);
            while (check)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Please enter a valid string:");
                Console.ResetColor();
                s = Console.ReadLine(); 
                check = s.Any(char.IsDigit);
            }
            return s;
        }
        //method to scale the ingridients
        public void ScaleIngridients(Ingridients[] rep)
        {
            string scalingOption = "1)Half the quantity\n2)Double the quantity\n3)Triple the quantity";
            double scaling = 0;
            double[] newArray = new double[rep.Length];
            double[] scaledCalories = new double[rep.Length];
                    Console.WriteLine(scalingOption);
                    int response = Convert.ToInt32(Console.ReadLine());
                    if(response >= 1 && response <= 3)
                    {
                        switch (response)
                        {
                            case 1: scaling = 0.5; break;
                            case 2: scaling = 2; break;
                            case 3: scaling = 3; break;
                        }
                        int count = 0;
                        foreach (Ingridients recipe in rep)
                        {
                            newArray[count] = recipe.quatity;
                            newArray[count] *= scaling;
                            scaledCalories[count] = recipe.calories * scaling;
                            count++;
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Please enter a valid input between 1 and 3.");
                        ScaleIngridients(rep);
                        Console.ResetColor();
                    }
                   
                    //displaying the scaled ingridients.
                    Console.WriteLine("Here is the scaled ingridients:");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    for (int x = 0; x < rep.Length; x++)
                    {
                        Console.WriteLine($"{rep[x].ingridientName}  {newArray[x]} {rep[x].unitOfMeasurement}. Calory intake: {scaledCalories[x]}");
                    }
                    Console.ResetColor();
                    Console.WriteLine("Would you like to revert the quantity back to the original state? 1 for yes and any other key for no.");
                    string backToOriginal = Console.ReadLine();

            if (backToOriginal == "1")
            {
                foreach (Ingridients recipe in rep)
                {
                    Console.WriteLine($"Ingridient Name: {recipe.ingridientName} Quantity: {recipe.quatity} {recipe.unitOfMeasurement}");
                }
            }

            Menu();

            
        }
        //method to prompt the user to enter a new recipe
        public void promptUserToEnterNewRecipe(Ingridients[] rep)
        {
            Console.WriteLine("Would you like to enter a new Recipe? 1 for yes and any other key for no.");
            string ans = Console.ReadLine();
            if(ans == "1") 
            { 
                Console.WriteLine("Are you sure you want to enter a new array. This will delete the previous recipe. 1 for yes and any other key for no.");
                string ans2 = Console.ReadLine();
                if(ans2 == "1") 
                {
                    Array.Clear(rep);
                    Console.WriteLine("\n\n Please enter new recipe.");
                    receiveInput();
                }
            } 
            
        }
        //method to display the recipe
        public void addToNamesList()
        {
            foreach(var recipe in ingridientsList)
            {
                recipeNames.Add(recipe.recipeName);
            }
        }
        public void displayRecipes()
        {
            
            int c = ingridientsList.Count;
            if(c > 0)
            {
                //ordering the list alphebetically
                var sortedRecipe = ingridientsList.OrderBy(rep => rep.recipeName);
                Console.WriteLine("\nRecipes:\n***********************************");
                int repIng = 0, step2 = 1;
                foreach (var recipe in sortedRecipe)
                {
                    Console.WriteLine($"{repIng}) {recipe.recipeName}");
                    repIng++;
                }
                displaySpecificRecipe();
            }
            else
            {
                Console.WriteLine("Please enter a recipe before using this function");
                Menu();
            }

            Console.ResetColor();
        }
        private string ChooseUnitOfMasurement()
        {
            string result = "";
            for(int i = 0;i < measurements.Length; i++)
            {
                Console.WriteLine($"{i+1}){measurements[i]}");
            }
            int choice = Convert.ToInt16(Console.ReadLine());
            //checks if user entered the correct value.
            if(choice >= 0 && choice <= 9)
            {
     
                switch (choice)
                {
                    case 1: result = measurements[0]; break;
                    case 2: result = measurements[1]; break;
                    case 3: result = measurements[2]; break;
                    case 4: result = measurements[3]; break;
                    case 5: result = measurements[4]; break;
                    case 6: result = measurements[5]; break;
                    case 7: result = measurements[6]; break;
                    case 8: result = measurements[7]; break;
                    case 9: result = OtherOption(); break;
                }
            }else 
            {
                //exception handling
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please provide a valid input (between 1 and 9)");
                result = ChooseUnitOfMasurement();
                Console.ResetColor();
            }

            return result;
        }
        private string OtherOption()
        {
            string result = "";
            Console.Write("What other unit of measurement will you be using: ");
            result = Console.ReadLine();
            return result;
        }
        private string FoodGroup()
        {
            string result = "";
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Please type and enter the number that the food group corresponds with.\n" +
                "e.g press 1 and press enter to select 1)Protein");
            Console.ResetColor();
            for(int x = 0; x < food_groups.Length; x++)
            {
                Console.WriteLine($"{x+1}){food_groups[x]}");
            }
            int choice = Int32.Parse(Console.ReadLine());
            if(choice >=1 && choice <= 5)
            {
                switch (choice)
                {
                    case 1: result = food_groups[0]; break;
                    case 2: result = food_groups[1]; break;
                    case 3: result = food_groups[2]; break;
                    case 4: result = food_groups[3]; break;
                    case 5: result = food_groups[4]; break;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please select a number between 1 and 5");
                Console.ResetColor();
                FoodGroup();
            }
            
            return result;
        }

     
    }
}
