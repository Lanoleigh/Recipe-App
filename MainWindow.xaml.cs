using POE;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace POE_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Ingridients ingridients = new Ingridients();
        public static Ingridients i = new Ingridients();
        public MainWindow()
        { 
            InitializeComponent();
            foreach (var item in ingridients.food_groups)
                lstFoodG.Items.Add(item);
            foreach (var item2 in ingridients.measurements)
                lstUnits.Items.Add(item2);
            foreach(var item3 in ingridients.food_groups)
                lstFGroups.Items.Add(item3);
        }
        int numOfIns;
        int steps;
        int counter = 0;


        private void btnEnterNumOfIns_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Int32.TryParse(txtNumOfIngs.Text,out int result) && !String.IsNullOrWhiteSpace(txtRepName.Text))
                {
                    numOfIns = result;
                    i.ingArray = new Ingridients[numOfIns];
                    
                    txtRepName.IsReadOnly = true;
                    txtNumOfIngs.IsReadOnly = true;
                    lblError1.Visibility = Visibility.Hidden;
                    txtIngriedientName.IsEnabled = true;
                    txtIngriedientName.Focus();
                    txtQuantity.IsEnabled = true;
                    txtCalories.IsEnabled = true;
                    lstFoodG.IsEnabled = true;
                    lstUnits.IsEnabled = true;
                    lstUnits.IsEnabled = true;
                }
            }catch(Exception ex)
            {
                lblError1.Visibility = Visibility.Visible;
                txtRepName.Clear();
                txtRepName.Focus();
                txtNumOfIngs.Clear();
            }

        }
        //function to recieve inputs.
        private void btnAddIngridient_Click(object sender, RoutedEventArgs e)
        {
            try
            {               
                i.ingArray[counter] = new Ingridients();
                i.recipeName = txtRepName.Text;
                i.ingArray[counter].ingridientName = txtIngriedientName.Text;
                i.ingArray[counter].unitOfMeasurement = lstUnits.SelectedItem.ToString();
                i.ingArray[counter].foodGroup = lstFoodG.SelectedItem.ToString();
                
                if (Int32.TryParse(txtQuantity.Text, out int result) && Double.TryParse(txtCalories.Text, out double result1))
                {
                    i.ingArray[counter].quatity = result;
                    i.ingArray[counter].calories = result1;
                    ingridients.calculateCalories(result1);
                    lblError2.Visibility = Visibility.Hidden;
                }
                else
                {
                    txtQuantity.Clear();
                    txtCalories.Clear();
                    txtQuantity.Focus();
                    lblError2.Visibility = Visibility.Visible;
                }
                counter++;
                txtIngriedientName.Focus();
                txtIngriedientName.Clear();
                txtCalories.Clear();
                txtQuantity.Clear();
                lstFoodG.UnselectAll();
                lstUnits.UnselectAll();
            }
            catch(Exception ex)
            {
                txtQuantity.Clear();
                txtCalories.Clear();
                txtQuantity.Focus();
                lblError2.Visibility = Visibility.Visible;
            }


        }

        private void btnEnterNoSteps_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               steps = Int32.Parse(txtNumOfSteps.Text);
               i.steps = new string[steps];
               txtNumOfSteps.IsReadOnly = true;
               btnEnterNoSteps.IsEnabled = false;
                txtStep.Focus(); 
               
            }catch(Exception ex)
            {
                txtNumOfSteps.Clear();
                txtNumOfSteps.Focus();
                lblError3.Visibility = Visibility.Visible;
            }
        }
        //used to add a step to the array.
        private void btnAddStep_Click(object sender, RoutedEventArgs e)
        {
            i.steps[counter] = txtStep.Text;
            txtStep.Clear();
            txtStep.Focus();
            counter++;
        }

        private void txtIngriedientName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(counter == numOfIns)
            {

                txtIngriedientName.IsReadOnly = true;
                txtCalories.IsReadOnly = true;
                txtQuantity.IsReadOnly = true;
                lstFoodG.IsEnabled = false;
                lstUnits.IsEnabled = false;
                txtNumOfSteps.IsEnabled = true;
                txtNumOfSteps.Focus();
                btnEnterNoSteps.IsEnabled = true;
                txtStep.IsEnabled = true;
                counter = 0;
            }
        }
        //function dedicated to add to the list.
        private void btnAddReps_Click(object sender, RoutedEventArgs e)
        {
            ingridients.ingridientsList.Add(i);
            
            tbViewReps.IsEnabled = true;
            tbAdvanced.IsEnabled = true;
            btnNextPage.Visibility = Visibility.Visible;
            txtRepName.IsReadOnly = false;
            txtNumOfIngs.Clear();
            txtNumOfIngs.IsReadOnly = false;
            txtRepName.Clear();
            txtRepName.Focus();
            txtIngriedientName.IsReadOnly = false;
            txtQuantity.IsReadOnly = false;
            txtCalories.IsReadOnly = false;
            txtIngriedientName.IsEnabled = true;
            txtQuantity.IsEnabled = true;
            txtCalories.IsEnabled = true;
            lstUnits.IsEnabled = true;
            lstFoodG.IsEnabled = true;
            txtStep.IsReadOnly = false;
            txtNumOfSteps.IsReadOnly = false;
            btnAddStep.IsEnabled = true;
            
            counter = 0;
            i = new Ingridients();
            
        }

        private void txtStep_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(counter == steps)
            {
                txtStep.IsReadOnly = true;
                btnAddStep.IsEnabled = false;
                
            }
        }

        private void btnViewRecipes_Click(object sender, RoutedEventArgs e)
        {
            foreach(var item in ingridients.ingridientsList)
            {
                lstRecipes.Items.Add(item.recipeName);
            }
        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            tbControl.SelectedIndex = 1;
        }

        private void lstRecipes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtDisplay.Clear();
            string s = lstRecipes.SelectedItem.ToString();
            string output = "";
            foreach(var item in ingridients.ingridientsList)
            {
                if (s.Equals(item.recipeName))
                {
                    output += $"Recipe Name: {item.recipeName}\n\n" +
                   $"Ingridients:\n";                   
                    foreach(var item2 in item.ingArray)
                        output += $"{item2.ingridientName} {item2.quatity} {item2.unitOfMeasurement} -- {item2.calories} cal, Food Group: {item2.foodGroup} \n";
                    
                    int x = 1;
                    output += "Steps:\n";
                    foreach (string ss in item.steps)
                    {
                        output += $"{x}) {ss}\n";
                        x++;
                    }
                    output += $"\nTotal Calories: {item.totalCalories}";
                       
                }
            }
            txtDisplay.Text = output;
        }
        //scaling by half the value
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tbxScaleDisplay.Text = "";
           
            string s = lstRecipes.SelectedItem.ToString();
            string output = "Scaled Ingridients by half: \n";
            foreach(var item in ingridients.ingridientsList)
            {
                if (s.Equals(item.recipeName))
                {
                    foreach (var item2 in item.ingArray)
                    {
                        output += $"{item2.ingridientName} {item2.quatity * 0.5}{item2.unitOfMeasurement}--{item2.calories * 0.5} cal, Food Group:{item2.foodGroup} \n";
                    }

                }
            }
            tbxScaleDisplay.Text = output;
        }
        //scaling by Double the value
        private void btnDouble_Click(object sender, RoutedEventArgs e)
        {
            tbxScaleDisplay.Text = "";
            string s = lstRecipes.SelectedItem.ToString();
            string output = "Scaled Ingridients by Double: \n";
            foreach (var item in ingridients.ingridientsList)
            {

                if (s.Equals(item.recipeName))
                {
                    foreach (var item2 in item.ingArray)
                    {
                        output += $"{item2.ingridientName} {item2.quatity * 2}{item2.unitOfMeasurement}--{item2.calories * 2} cal, Food Group:{item2.foodGroup} \n";
                    }
                    
                }
            }
            tbxScaleDisplay.Text = output;
        }
        //scaling by triple the value
        private void btnTriple_Click(object sender, RoutedEventArgs e)
        {
            tbxScaleDisplay.Text = "";
            string s = lstRecipes.SelectedItem.ToString();
            string output = "Scaled Ingridients by Triple: \n";
            foreach (var item in ingridients.ingridientsList)
            {
                if (s.Equals(item.recipeName))
                {
                    foreach (var item2 in item.ingArray)
                    {
                        output += $"{item2.ingridientName} {item2.quatity * 2}{item2.unitOfMeasurement}--{item2.calories * 2} cal, Food Group:{item2.foodGroup} \n";
                    }

                }
            }
            tbxScaleDisplay.Text = output;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            tbxScaleDisplay.Text = "";
            txtDisplay.Clear();
        }
        //searching for specific ingridient in recipe.
        private void btnSearchIngridient_Click(object sender, RoutedEventArgs e)
        {
            txtDisplay2.Clear();
            lstReps2.UnselectAll();
            lstReps2.Items.Clear();
            string search = txtSearch.Text;
            bool found = false;
            foreach(var item in ingridients.ingridientsList)
            {
                foreach(var item2 in item.ingArray)
                {
                    if (item2.ingridientName.ToLower().Equals(search.ToLower()))
                    {
                        found = true;
                        lblNotFound.Visibility = Visibility.Hidden;
                        lstReps2.Items.Add(item.recipeName);
                    }
                }
            }
            if (found == false)
                lblNotFound.Visibility = Visibility.Visible;
        }

        //Filtering list by the limit of calories set by user
        private void btnSearchCalories_Click(object sender, RoutedEventArgs e)
        {
            txtDisplay2.Clear();
            lstReps2.UnselectAll();
            lstReps2.Items.Clear();
            try
            {
                double userLimit = Double.Parse(txtSearchCalos.Text);
                foreach(var item in ingridients.ingridientsList)
                {
                    if(userLimit < item.totalCalories)
                    {
                        lstReps2.Items.Add(item.recipeName);
                    }
                }
            }
            catch(Exception ex)
            {
                txtSearchCalos.Clear();
                txtSearchCalos.Focus();
            }
        }
        //here to display the recipe details as you select the different recipes
        private void lstReps2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstReps2.SelectedItem == null)
                return;
            txtDisplay2.Text = "";
            try
            {
                string s = lstReps2.SelectedItem.ToString();
                string output = "";
                foreach (var item in ingridients.ingridientsList)
                {
                    if (s.Equals(item.recipeName))
                    {
                        output += $"Recipe Name: {item.recipeName}\n\n" +
                       $"Ingridients:\n";
                        foreach (var item2 in item.ingArray)
                            output += $"{item2.ingridientName} {item2.quatity} {item2.unitOfMeasurement} -- {item2.calories} cal, Food Group: {item2.foodGroup} \n";

                        int x = 1;
                        output += "Steps:\n";
                        foreach (string ss in item.steps)
                        {
                            output += $"{x}) {ss}\n";
                            x++;
                        }
                        output += $"\nTotal Calories: {item.totalCalories}";

                    }
                }
                txtDisplay2.Text = output;
               
            }
            catch(Exception ex)
            {
                lstReps2.UnselectAll();
            }
           
        }

        private void lstFGroups_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            txtDisplay2.Clear();
            lstReps2.UnselectAll();
            lstReps2.Items.Clear();
            string selectedItem = lstFGroups.SelectedItem.ToString();
            bool bfound = false;
            foreach (var item in ingridients.ingridientsList)
            {
                foreach (var item2 in item.ingArray)
                {
                    if (selectedItem.Equals(item2.foodGroup))
                    {
                        bfound = true;
                        lblNotFound.Visibility = Visibility.Hidden;
                        lstReps2.Items.Add(item.recipeName);
                    }
                }
            }
            if (bfound == false)
                lblNotFound.Visibility = Visibility.Visible;
        }
    }
}