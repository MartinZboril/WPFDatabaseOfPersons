using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DatabazeOsob
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();           
            DisplayDatabase();
        }

        private int tempSelectedIndex;

        private static PersonDatabase _database;
        public static PersonDatabase Database
        {
            get
            {
                if (_database == null)
                {
                    var fileHelper = new FileHelper();
                    _database = new PersonDatabase(fileHelper.GetLocalFilePath("TodoSQLite.db3"));
                }
                return _database;
            }
        }

        //Přidá osobu do databáze
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            //Kontrola vstupů
            if(FirstName.Text.Equals("").Equals(false) && int.TryParse(FirstName.Text, out int Nothing).Equals(false) && LastName.Text.Equals("").Equals(false) && int.TryParse(LastName.Text, out int Nothing1).Equals(false) && int.TryParse(RN.Text, out int RNPart1) && int.TryParse(RN2.Text, out int RNPart2) && Sex.Text.Equals("").Equals(false))
            {
                Person item = new Person();
                item.FirstName = FirstName.Text;
                item.LastName = LastName.Text;
                item.RNnumber = RNPart1 + "/" + RNPart2;
                item.Sex = Sex.Text;
                item.TimeStamp = DateTime.Now;
                item.UpdateTimeStamp = DateTime.Now;
                Database.SaveItemAsync(item);
                ClearForm();           
                Message.Text = "Osoba byla úspěšně přidána!";
                DisplayDatabase();
            } else
            {
                Message.Text = "Špatně vypněno!";
                DisplayDatabase();
            }

        }

        //Vybrání určitého záznamu z listview
        private void PersonListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PersonListView.SelectedItem != null)
            {
                DateOfAdd.Visibility = Visibility.Visible;
                DateOfUpdate.Visibility = Visibility.Visible;
                AddButton.Visibility = Visibility.Hidden;
                UpdateButton.Visibility = Visibility.Visible;
                DeleteButton.Visibility = Visibility.Visible;
                RN.IsEnabled = false;
                RN2.IsEnabled = false;
                var person = PersonListView.SelectedItem as Person;
                tempSelectedIndex = PersonListView.SelectedIndex;
                FirstName.Text = person.FirstName;
                LastName.Text = person.LastName;
                string[] RNnum = person.RNnumber.Split(("/").ToCharArray());
                RN.Text = RNnum[0].ToString();
                RN2.Text = RNnum[1].ToString();
                Sex.Text = person.Sex;
                DateOfAdd.Text = "Přidán: " + person.TimeStamp;
                DateOfUpdate.Text = "Změnen: " + person.UpdateTimeStamp;
                Message.Text = "";
            } else
            {
                AddButton.Visibility = Visibility.Visible;
                UpdateButton.Visibility = Visibility.Hidden;
                DeleteButton.Visibility = Visibility.Hidden;
                DateOfAdd.Visibility = Visibility.Hidden;
                DateOfUpdate.Visibility = Visibility.Hidden;
            }
        }

        //Změna záznamu v databázi
        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            PersonListView.SelectedIndex = tempSelectedIndex;
            var person = PersonListView.SelectedItem as Person;
            //Ošetření vstupu
            if (FirstName.Text.Equals("").Equals(false) && int.TryParse(FirstName.Text, out int Nothing).Equals(false) && LastName.Text.Equals("").Equals(false) && int.TryParse(LastName.Text, out int Nothing1).Equals(false))
            {
                person.FirstName = FirstName.Text;
                person.LastName = LastName.Text;
                person.UpdateTimeStamp = DateTime.Now;
                await Database.SaveItemAsync(person);
                ClearForm();
                Message.Text = "Osoba byla úspěšně změněna!";
                DateOfAdd.Visibility = Visibility.Hidden;
                DateOfUpdate.Visibility = Visibility.Hidden;
                RN.IsEnabled = true;
                RN2.IsEnabled = true;
            } else
            {
                Message.Text = "Špatně vypněno!";
                DisplayDatabase();
            }
            DisplayDatabase();
        }

        //Odstranění záznamu
        private async void DeleteButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            PersonListView.SelectedIndex = tempSelectedIndex;
            var person = PersonListView.SelectedItem as Person;
            await Database.DeleteItemAsync(person);
            RN.IsEnabled = true;
            RN2.IsEnabled = true;
            ClearForm();
            Message.Text = "Osoba byla úspěšně smazána!";
            DisplayDatabase();
        }

        //Zobrazení záznamů z databáze
        private void DisplayDatabase()
        {
            var itemsFromDb = Database.GetItemsNotDoneAsync().Result;
            Debug.WriteLine(itemsFromDb.Count);
            foreach (Person Item in itemsFromDb)
            {
                Debug.WriteLine(Item);
            }
            PersonListView.ItemsSource = itemsFromDb;
            //Počet záznamů v databázi
            CountPerson.Text = "V databázi jsou " + itemsFromDb.Count + " osoby";
        }

        //Vyprázdnění formuláře po odeslání
        private void ClearForm()
        {
            FirstName.Text = " ";
            LastName.Text = " ";
            RN.Text = " ";
            RN2.Text = " ";
            Sex.Text = " "; 
        }
    }
}