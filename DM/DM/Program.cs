using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
//using System.IO;

namespace Cooking_TDF_Eq14
{
    class Program
    {
        static void MainMenu(MySqlConnection connection)
        {
            Console.Clear();
            Console.WriteLine("\"\"\"\"\"\"\" This is... COOKING ! \"\"\"\"\"\"\"\n\n");
            int choice = Selection();

            switch (choice)
            {
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    Login(connection);
                    break;
                case 2:
                    Signing(connection);
                    break;
                case 3:
                    DemoMode(connection);
                    break;
                case 4:
                    Dashboard(connection);
                    break;
            }
        }

        static int Selection()
        {
            int choice = -1;
            Console.WriteLine("1. Login" +
                "\n2. Sign up" +
                "\n3. Demo mode" +
                "\n4. Cooking manager" +
                "\n" +
                "\n0. Exit\n");
            do
            {
                Console.Write("> ");
                try { choice = Convert.ToInt32(Console.ReadLine()); } // make sure the input is an integer
                catch { Console.Write("Please enter a number\n"); }
            } while (choice != 0 && choice != 1 && choice != 2 && choice != 3 && choice != 4);
            return choice;
        }

        #region Demo Mode
        static void LittleMenuDemo(MySqlConnection connection) // A little menu at the bottom to chose what to do next
        {
            int choice = -1;
            Console.WriteLine("1. Continue demo mode" +
                "\n2. Main menu" +
                "\n0. Exit\n");
            do
            {
                Console.Write("> ");
                try { choice = Convert.ToInt32(Console.ReadLine()); } // make sure the input is an integer
                catch { Console.Write("Please enter a number\n"); }
            } while (choice != 0 && choice != 1 && choice != 2);
            Console.WriteLine();

            switch (choice)
            {
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    DemoMode(connection);
                    break;
                case 2:
                    MainMenu(connection);
                    break;
            }
        }

        static void DemoMode(MySqlConnection connection) // Main method of the Demo Mode, act as a dashboard where we can call various method in order to complete tasks
        {
            Console.Clear();
            Console.WriteLine("\"\"\"\"\"\"\" Demo Mode \"\"\"\"\"\"\"\n\n");

            int Choice = -1;
            Console.WriteLine("1. Display how many client" +
                "\n2. Display number and name of CdR as well as their total number of meal ordered" +
                "\n3. Display how many meal" +
                "\n4. Display list of product having a stock number less or equal twice their minimal stock" +
                "\n\n0. Main menu\n");
            do
            {
                Console.Write("> ");
                try { Choice = Convert.ToInt32(Console.ReadLine()); } // make sure the input is an integer
                catch { Console.Write("Please enter a number\n"); }
            } while (Choice != 0 && Choice != 1 && Choice != 2 && Choice != 3 && Choice != 4);
            Console.WriteLine();

            switch (Choice)
            {
                case 0:
                    MainMenu(connection);
                    break;
                case 1:
                    DemoClient(connection);
                    break;
                case 2:
                    DemoCdR(connection);
                    break;
                case 3:
                    NumberMeals(connection);
                    break;
                case 4:
                    MinProduct(connection);
                    break;
            }
        }

        static void DemoClient(MySqlConnection connection) // Display the total number of clients registered
        {
            connection.Open(); // --> OPEN CO

            MySqlCommand commandCl = connection.CreateCommand();
            commandCl.CommandText =
             "SELECT COUNT(*) FROM client;";

            MySqlDataReader readerCl;
            readerCl = commandCl.ExecuteReader();
            readerCl.Read();
            int nbClient = readerCl.GetInt32(0); // get number of clients

            Console.ForegroundColor = ConsoleColor.Cyan;
            if (nbClient == 1) { Console.WriteLine("There is " + nbClient + " client registered\n\n"); }
            else { Console.WriteLine("There are " + nbClient + " clients registered\n\n"); }
            Console.ForegroundColor = ConsoleColor.White;

            readerCl.Close();
            connection.Close(); // --> CLOSE CO

            LittleMenuDemo(connection); // little menu
        }
        static void DemoCdR(MySqlConnection connection) // Display the total number of CdR and how many of their meals got ordered
        {
            connection.Open(); // --> OPEN CO
            #region CdR Number

            MySqlCommand commandCdR = connection.CreateCommand();
            commandCdR.CommandText =
             "SELECT COUNT(*) FROM client WHERE createur = True;"; // only the creators

            MySqlDataReader readerCdR;
            readerCdR = commandCdR.ExecuteReader();
            readerCdR.Read();
            int nbCdR = readerCdR.GetInt32(0); // get number of CdR

            Console.ForegroundColor = ConsoleColor.Cyan;
            if (nbCdR == 1) { Console.WriteLine("There is " + nbCdR + " client creator of Meals\n"); }
            else { Console.WriteLine("There are " + nbCdR + " clients creators of Meal\n"); }
            Console.ForegroundColor = ConsoleColor.White;

            readerCdR.Close();

            #endregion
            #region List of all CdR and the amount of their meal ordered

            MySqlCommand commandListCdR = connection.CreateCommand();
            commandListCdR.CommandText =
             "SELECT nomC, prenomC, nombreCommandeCdR FROM client WHERE createur = True GROUP BY codeClient ORDER BY nombreCommandeCdR DESC;";

            readerCdR = commandListCdR.ExecuteReader();

            string lastName = "";
            string firstName = "";
            int order = -1;

            Console.ForegroundColor = ConsoleColor.Cyan;
            while (readerCdR.Read())
            {
                lastName = readerCdR.GetString(0);
                firstName = readerCdR.GetString(1);
                order = readerCdR.GetInt32(2);
                if (order > 1) { Console.WriteLine("Last Name : " + lastName + " | First Name : " + firstName + " | Amounts of meals ordered : " + order); }
                else { Console.WriteLine("Last Name : " + lastName + " | First Name : " + firstName + " | Amount of meal ordered : " + order); }   
            }
            Console.ForegroundColor = ConsoleColor.White;
            readerCdR.Close();
            Console.WriteLine("\n\n");

            #endregion
            connection.Close(); // --> CLOSE CO

            LittleMenuDemo(connection); // little menu
        }
        static void NumberMeals(MySqlConnection connection) // Display the total number of meal 
        {
            connection.Open(); // --> OPEN CO

            MySqlCommand commandRec = connection.CreateCommand();
            commandRec.CommandText =
             "SELECT COUNT(*) FROM recette;";

            MySqlDataReader readerRec;
            readerRec = commandRec.ExecuteReader();
            readerRec.Read();
            int nbMeal = readerRec.GetInt32(0); // get the numer of meals

            Console.ForegroundColor = ConsoleColor.Cyan;
            if (nbMeal == 1) { Console.WriteLine("There is " + nbMeal + " meal available\n\n"); }
            else { Console.WriteLine("There are " + nbMeal + " meals available\n\n"); }
            Console.ForegroundColor = ConsoleColor.White;

            readerRec.Close();
            connection.Close(); // --> CLOSE CO

            LittleMenuDemo(connection); // little menu
        }
        static void MinProduct(MySqlConnection connection) // Display the products with a stock <= 2x minimal stock and the meal it composed
        {
            List<string> listProduct = new List<string>(); // all product that should be displayed

            connection.Open(); // --> OPEN CO
            #region Stock and stockMin

            MySqlCommand commandStock = connection.CreateCommand();
            commandStock.CommandText =
             "SELECT nomP, stock, stockMin FROM produit WHERE stock <= 2*stockMin ORDER BY nomP;";

            MySqlDataReader readerStock;
            readerStock = commandStock.ExecuteReader();

            string nameP = "";
            int stock = -1;
            int stockMin = -1;

            Console.ForegroundColor = ConsoleColor.Cyan;
            while (readerStock.Read())
            {
                nameP = readerStock.GetString(0);
                listProduct.Add(nameP.ToLower()); // add in our list, lowered to be compared more easily later
                stock = readerStock.GetInt32(1);
                stockMin = readerStock.GetInt32(2);

                Console.WriteLine("Product : " + nameP + " | Stock : " + stock + " | Minimal stock : " + stockMin);
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n");
            #endregion

            #region Choice

            int choice = -1;
            Console.WriteLine("1. Input a product\n" +
                "\n2. Continue demo mode" +
                "\n3. Main menu" +
                "\n0. Exit\n");
            do
            {
                Console.Write("> ");
                try { choice = Convert.ToInt32(Console.ReadLine()); } // make sure the input is an integer
                catch { Console.Write("Please enter a number\n"); }
            } while (choice != 0 && choice != 1 && choice != 2 && choice != 3);
            Console.WriteLine();

            switch (choice)
            {
                case 0:
                    Environment.Exit(0);
                    break;
                case 2:
                    DemoMode(connection);
                    break;
                case 3:
                    MainMenu(connection);
                    break;
            }
            #endregion

            #region Product list

            string product = "";
            do
            {
                Console.Write("Input the product that you would like to display : ");
                try { product = Convert.ToString(Console.ReadLine()).ToLower(); } // check if the input product is in our list
                catch {}
            } while (! listProduct.Contains(product));
            Console.WriteLine();


            MySqlCommand commandProd = connection.CreateCommand();
            commandProd.CommandText =
             "SELECT r.nomR, cr.quantiteProduit, p.unite FROM recette r, constitutionRecette cr, produit p WHERE p.nomP = \"" + product + "\" AND p.codeProduit = cr.codeProduit AND r.codeRecette = cr.codeRecette GROUP BY r.codeRecette;";

            readerStock.Close();
            readerStock = commandProd.ExecuteReader();

            string nameMeal = "";
            float amount = -1;
            string unit = "";

            Console.ForegroundColor = ConsoleColor.Cyan;
            while (readerStock.Read())
            {
                nameMeal = readerStock.GetString(0);
                amount = readerStock.GetFloat(1);
                unit = readerStock.GetString(2);
                Console.WriteLine("Meal : " + nameMeal + " | Quantity : " + amount + " | Unit : " + unit);
            }
            Console.ForegroundColor = ConsoleColor.White;           
            Console.WriteLine("\n\n");

            #endregion
            connection.Close(); // --> CLOSE CO

            LittleMenuDemo(connection); // little menu
        }
        #endregion

        #region Client
        static void Signing(MySqlConnection connection) // Create a customer
        {
            Console.Clear();
            Console.WriteLine("\"\"\"\"\"\"\" Signing up \"\"\"\"\"\"\"\n\n");
            string lastName = "";
            string firstName = "";
            string username = "";
            string password = "";
            string phone = "";
            #region Names

            bool valideN = false;
            do
            {
                Console.Write("Last name : ");
                lastName = Convert.ToString(Console.ReadLine());
                valideN = lastName.All(Char.IsLetter); // name can only be made of letters
            } while (!valideN || !(lastName.Length > 0)); // name has "at least" one caracter

            bool valideP = false;
            do
            {
                Console.Write("First name : ");
                firstName = Convert.ToString(Console.ReadLine());
                valideP = firstName.All(Char.IsLetter); // name can only be made of letters
            } while (!valideP || !(firstName.Length > 0)); // name has "at least" one caracter

            #endregion
            #region Unique username
            bool end = false;
            do
            {
                Console.Write("Username : ");
                username = Convert.ToString(Console.ReadLine());

                connection.Close(); // --> OPEN CO
                connection.Open();

                MySqlCommand command = connection.CreateCommand();
                command.CommandText =
                 "SELECT usernameC FROM client;";

                MySqlDataReader reader;
                reader = command.ExecuteReader();

                string usernameC = "";
                bool unique = true;

                while (reader.Read() && unique) // a username has to be unique
                {
                    usernameC = reader.GetString(0);
                    if (username == usernameC)
                    {
                        unique = false;
                    }
                }
                reader.Close();
                connection.Close(); // --> CLOSE CO
                if (!unique || !(username.Length>0))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Username already taken or too short, please select another");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else { end = true; }
            } while (!end);
            #endregion
            #region Password and Phone number
            
            do
            {
                Console.Write("Password (> 4 caracteres) : ");
                password = Convert.ToString(Console.ReadLine());
            } while (!(password.Length > 3));

            do
            {
                Console.Write("Phone number : ");
                try { phone = Convert.ToString(Convert.ToInt32(Console.ReadLine())); } // make sure the input is an integer
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Non valid phone number");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            } while (phone.Length != 9); // phone number has 10 digits, the first 0 isn't taken into account
            #endregion
        
            char input;
            do
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("\n>>> Confirm the creation of the account (y/n) ?");
                Console.ForegroundColor = ConsoleColor.White;
                input = Console.ReadKey().KeyChar;
                Console.ReadKey();
            } while (input != 'y' && input != 'n');
            if (input == 'n') { MainMenu(connection); }

            #region Create codeClient and Account

            // Get the last code client
            connection.Open(); // --> OPEN CO

            MySqlCommand commandLastCodeC = connection.CreateCommand();
            commandLastCodeC.CommandText =
             "SELECT codeClient FROM client ORDER BY codeClient DESC LIMIT 1;"; // Limit the order to only one element

            MySqlDataReader readerLastCodeC;
            readerLastCodeC = commandLastCodeC.ExecuteReader();

            readerLastCodeC.Read();
            string codeC = readerLastCodeC.GetString(0); // get the last code client

            readerLastCodeC.Close(); 
            connection.Close(); // --> CLOSE CO

            // Create new code client
            string newCode = "";
            int numeralpart = Convert.ToInt32(codeC.Substring(1));
            numeralpart++;
            if (numeralpart < 1000)
            {
                newCode = "C0" + Convert.ToString(numeralpart);
            }
            else { newCode = "C" + Convert.ToString(numeralpart); }

            // Update database
            connection.Open(); // --> OPEN CO
            MySqlCommand commandUpdate = connection.CreateCommand();
            commandUpdate.CommandText = "INSERT INTO `cooking`.`client` " +
                "(`codeClient`,`nomC`,`prenomC`,`telephoneC`,`usernameC`,`mdpC`,`createur`,`cook`, `nombreCommandeCdR`) " +
                    "VALUES (\"" + newCode + "\", \"" + lastName + "\", \"" + firstName + "\", \"" + phone + "\", " +
                        "\"" + username + "\", \"" + password + "\", False, 0, 0);";
            MySqlDataReader readerUpdate;
            readerUpdate = commandUpdate.ExecuteReader();
            readerUpdate.Read();
            readerUpdate.Close();
            connection.Close(); // --> CLOSE CO

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("> Account successfully creater.");
            Console.ForegroundColor = ConsoleColor.White;

            Client(connection, newCode); // connect to client
            #endregion
        }
        static void Login(MySqlConnection connection) // Sign in
        {
            Console.Clear();
            Console.WriteLine("\"\"\"\"\"\"\" Login \"\"\"\"\"\"\"\n\n");

            Console.Write("Username : ");
            string username = Convert.ToString(Console.ReadLine());

            Console.Write("Password : ");
            string password = "";
            while (true) // hide the user input
            {
                var key = Console.ReadKey(true);       
                if (key.Key == ConsoleKey.Enter)
                    break;
                //if (key.Key == ConsoleKey.Backspace) { }
                Console.Write("*");
                password += key.KeyChar;
            }
            Console.WriteLine();

            #region Connection

            connection.Open(); // --> OPEN CO

            MySqlCommand commandCo = connection.CreateCommand();
            commandCo.CommandText =
             "SELECT codeClient, usernameC, mdpC FROM client;";

            MySqlDataReader readerCo;
            readerCo = commandCo.ExecuteReader();

            string codeC = "";
            string usernameC = "";
            string passwordC = "";
            bool end = true;

            while (readerCo.Read() && end)
            {
                codeC = readerCo.GetString(0);
                usernameC = readerCo.GetString(1);
                passwordC = readerCo.GetString(2);
                if (username == usernameC && password == passwordC) // match the user input with the database
                {
                    end = false;
                }
            }

            readerCo.Close();
            connection.Close(); // --> CLOSE CO
            if (!end) { Client(connection, codeC); } // connect to client
            else
            {
                char input;
                do
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Username or password incorrect, would you like to try again (y/n) ?");
                    Console.ForegroundColor = ConsoleColor.White;
                    input = Console.ReadKey().KeyChar;
                    Console.ReadKey();
                } while (input != 'y' && input != 'n');
                if (input == 'n') { MainMenu(connection); }
                Login(connection);
            }          
            #endregion
        }

        static void Client(MySqlConnection connection, string codeClient)
        {
            Console.Clear();
            Console.WriteLine("\"\"\"\"\"\"\" Client Dashboard \"\"\"\"\"\"\"\n\n");
            Console.WriteLine("client : " + codeClient);
        }
        #endregion

        #region Cooking
        
        static void LittleMenuCooking(MySqlConnection connection) // A little menu at the bottom to chose what to do next
        {
            int choice = -1;
            Console.WriteLine("1. Continue with cooking manager" +
                "\n2. Main menu" +
                "\n0. Exit\n");
            do
            {
                Console.Write("> ");
                try { choice = Convert.ToInt32(Console.ReadLine()); } // make sure the input is an integer
                catch { Console.Write("Please enter a number\n"); }
            } while (choice != 0 && choice != 1 && choice != 2);

            switch (choice)
            {
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    Dashboard(connection);
                    break;
                case 2:
                    MainMenu(connection);
                    break;
            }
        }

        static void Dashboard(MySqlConnection connection) // Main method for the cooking, act as a dashboard where we can call various method in order to complete requests
        {
            Console.Clear();
            Console.WriteLine("\"\"\"\"\"\"\" Cooking Dashboard \"\"\"\"\"\"\"\n\n");

            int choice = -1;
            Console.WriteLine("1. Display clients" +
                "\n2. Display meals" +
                "\n" +
                "\n3. CdR of the week" +
                "\n4. Top 5 meals" +
                "\n5. Top CdR" +
                "\n" +
                "\n6. Delete CdR" +
                "\n7. Delete Meal" +
                "\n" +
                "\n0. Exit\n");
            do
            {
                Console.Write("> ");
                try { choice = Convert.ToInt32(Console.ReadLine()); } // make sure the input is an integer
                catch { Console.Write("Please enter a number\n"); }
            } while (choice != 0 && choice != 1 && choice != 2 && choice != 3 && choice != 4 && choice != 5 && choice != 6 && choice != 7);

            switch (choice)
            {
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    DisplayClient(connection);
                    break;
                case 2:
                    DisplayMeal(connection);
                    break;
                case 3:
                    CdRofTheW(connection);
                    break;
                case 4:
                    Top5Meals(connection);
                    break;
                case 5:
                    TopCdR(connection);
                    break;
                case 6:
                    DeleteCdR(connection);
                    break;
                case 7:
                    DeleteMeal(connection);
                    break;
            }
        }

        static void DisplayClient(MySqlConnection connection) // Chose to display all client or only CdR
        {
            Console.WriteLine();

            int choice = -1;
            Console.WriteLine("\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"");
            Console.WriteLine("1. All client" +
                "\n2. Only CdR" +
                "\n3. Cooking manager" +
                "\n0. Exit\n");
            do
            {
                Console.Write("> ");
                try { choice = Convert.ToInt32(Console.ReadLine()); } // make sure the input is an integer
                catch { Console.Write("Please enter a number\n"); }
            } while (choice != 0 && choice != 1 && choice != 2 && choice != 3);

            switch (choice)
            {
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    DisplayAllClient(connection);
                    break;
                case 2:
                    DisplayCdR(connection);
                    break;
                case 3:
                    Dashboard(connection);
                    break;
            }
        }
        static void DisplayAllClient(MySqlConnection connection)
        {
            Console.Clear();

            string codeClient = "";
            string lastName = "";
            string firstName = "";
            bool cdr = false;
            float cook = 0;
            int order = 0;

            connection.Open(); // --> OPEN CO
            MySqlDataReader reader;
            MySqlCommand command = connection.CreateCommand();
            command.CommandText =
             "SELECT codeClient, nomC, prenomC, createur, cook, nombreCommandeCdR FROM client ORDER BY codeClient;";
            reader = command.ExecuteReader();

            Console.WriteLine("\"\"\"\"\"\"\" All client \"\"\"\"\"\"\"\n");
            Console.ForegroundColor = ConsoleColor.Cyan;
            while (reader.Read())
            {
                codeClient = reader.GetString(0);
                lastName = reader.GetString(1);
                firstName = reader.GetString(2);
                cdr = reader.GetBoolean(3);
                cook = reader.GetFloat(4);
                order = reader.GetInt32(5);

                Console.WriteLine("Code client : " + codeClient + " | Last name : " + lastName + " | First name : " + firstName + " | Creator : " + cdr + " | Cook : " + cook + " | Amount of created meal ordered : " + order);
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n");
            reader.Close();
            connection.Close(); // --> CLOSE CO

            LittleMenuCooking(connection); // little menu
        }
        static void DisplayCdR(MySqlConnection connection)
        {
            Console.Clear();

            string codeClient = "";
            string lastName = "";
            string firstName = "";
            float cook = 2;
            int order = 0;

            connection.Open(); // --> OPEN CO
            MySqlCommand command = connection.CreateCommand();
            command.CommandText =
             "SELECT codeClient, nomC, prenomC, createur, cook, nombreCommandeCdR FROM client WHERE createur = TRUE ORDER BY codeClient;";
            MySqlDataReader reader;
            reader = command.ExecuteReader();

            Console.WriteLine("\"\"\"\"\"\"\" Creator of Meal \"\"\"\"\"\"\"\n");
            Console.ForegroundColor = ConsoleColor.Cyan;
            while (reader.Read())
            {
                codeClient = reader.GetString(0);
                lastName = reader.GetString(1);
                firstName = reader.GetString(2);
                cook = reader.GetFloat(3);
                order = reader.GetInt32(4);

                Console.WriteLine("Code client : " + codeClient + " | Last name : " + lastName + " | First name : " + firstName + " | Cook : " + cook + " | Amount of created meal ordered : " + order);
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n");
            reader.Close();
            connection.Close(); // --> CLOSE CO

            LittleMenuCooking(connection); // little menu
        }

        static void DisplayMeal(MySqlConnection connection)
        {
            Console.Clear();

            string codeMeal = "";
            string nameM = "";
            string type = "";
            string desciption = "";
            bool veg = false;
            float price = 0;
            int order = 0;
            string lastNameCreator = "";
            string fistNameCreator = "";

            connection.Open(); // --> OPEN CO
            MySqlDataReader reader;
            MySqlCommand command = connection.CreateCommand();
            command.CommandText =
             "SELECT r.codeRecette, r.nomR, r.type, r.descriptif, r.veg, r.prixR, r.nombreCommande, c.nomC, c.prenomC FROM recette r, client c WHERE r.codeClient = c.codeClient ORDER BY r.codeRecette;";
            reader = command.ExecuteReader();

            Console.WriteLine("\"\"\"\"\"\"\" Meals \"\"\"\"\"\"\"\n");
            Console.ForegroundColor = ConsoleColor.Cyan;
            while (reader.Read())
            {
                codeMeal = reader.GetString(0);
                nameM = reader.GetString(1);
                type = reader.GetString(2);
                desciption = reader.GetString(3);
                veg = reader.GetBoolean(4);
                price = reader.GetFloat(5);
                order = reader.GetInt32(6);
                lastNameCreator = reader.GetString(7);
                fistNameCreator = reader.GetString(8);

                Console.WriteLine("Code Meal : " + codeMeal + " | Meal : " + nameM + " | Type : " + type + 
                    " | Description : " + desciption + " | Vegetarien : " + veg + " | price (cook) : " + order +
                    " | Total order : " + order + " | Creator's last name : " + lastNameCreator + " | Creator's first name : " + fistNameCreator);
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n");
            reader.Close();
            connection.Close(); // --> CLOSE CO

            LittleMenuCooking(connection); // little menu
        }

        static void CdRofTheW(MySqlConnection connection) // Display CdR of the week based on their total amount of meal ordered during the week
        {
            Console.Clear();
            connection.Open(); // --> OPEN CO

            MySqlCommand cdrOfTheWeek = connection.CreateCommand();
            cdrOfTheWeek.CommandText =
                "SELECT c.codeClient, c.nomC, c.prenomC, SUM(r.nombreCommandeSemaine) AS total " +
                    "FROM client c, recette r " +
                        "WHERE c.codeClient = r.codeClient " +
                            "AND c.createur = TRUE " +
                                "GROUP BY c.codeClient " +
                                "ORDER BY total DESC " +
                                "LIMIT 1;";

            MySqlDataReader reader;
            reader = cdrOfTheWeek.ExecuteReader();
            reader.Read();
            string customerCode = reader.GetString(0);
            string lastName = reader.GetString(1);
            string firstName = reader.GetString(2);
            int orderNumber = reader.GetInt32(3);
            reader.Close();
            connection.Close(); // --> CLOSE CO

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\"\"\"\"\"\"\"\" Best Meal Creator of the week \"\"\"\"\"\"\"\"\n");
            Console.WriteLine($"Customer number: {customerCode}\nLast name: {lastName}\nFirst name: {firstName}\nNumber of order this week: {orderNumber}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n");

            LittleMenuCooking(connection); // little menu
        }
        static void Top5Meals(MySqlConnection connection)
        {
            Console.Clear();
            Console.OutputEncoding = System.Text.Encoding.Unicode; // € euro sign instead of cook
            connection.Open(); // --> OPEN CO
            
            MySqlCommand top5Meal = connection.CreateCommand();
            top5Meal.CommandText =
                "SELECT r.codeRecette, r.nomR, r.type, r.prixR, r.nombreCommandeSemaine, c.nomC, c.prenomC " +
                    "FROM recette r, client c " +
                        "WHERE c.codeClient = r.codeClient " +
                            "ORDER BY r.nombreCommandeSemaine DESC " +
                            "LIMIT 5; "; // keep only 5

            MySqlDataReader reader;
            reader = top5Meal.ExecuteReader();
            reader.Read();
            string mealCode = "";
            string mealName = "";
            string mealType = "";
            float mealPrice = -1;
            int weekOrders = -1;
            string creatorsLastName = "";
            string creatorsFirstName = "";
            int count = 0;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\"\"\"\"\"\"\"\" Top 5 Meals this week \"\"\"\"\"\"\"\"\n");
            while (reader.Read())
            {
                mealCode = reader.GetString(0);
                mealName = reader.GetString(1);
                mealType = reader.GetString(2);
                mealPrice = reader.GetFloat(3);
                weekOrders = reader.GetInt32(4);
                creatorsLastName = reader.GetString(5);
                creatorsFirstName = reader.GetString(6);
                count++;

                Console.WriteLine($"\"\"\"\"\"\"\"\" Meal number {count} \"\"\"\"\"\"\"\"\n");
                Console.WriteLine($"Meal's code: {mealCode}\nMeal's name: {mealName}\nMeal's type: {mealType}\nMeal's price: {mealPrice} cook\nNumber of orders this week: {weekOrders}\nCreator's last name: {creatorsLastName}\nCreator's first name: {creatorsFirstName}\n\n");
            }
            Console.ForegroundColor = ConsoleColor.White;
            reader.Close();
            connection.Close(); // --> CLOSE CO
            Console.WriteLine();

            LittleMenuCooking(connection); // little menu
        }
        static void TopCdR(MySqlConnection connection)
        {
            Console.Clear();
            Console.OutputEncoding = System.Text.Encoding.Unicode; // € euro sign
            connection.Open();
            MySqlDataReader reader;

            string mealCode = "";
            string mealName = "";
            string mealType = "";
            float mealPrice = -1;
            int count = 0;

            MySqlCommand topCdR = connection.CreateCommand();
            topCdR.CommandText =
                "SELECT c.codeClient, c.nomC, c.prenomC " +
                    "FROM client c " +
                        "ORDER BY c.nombreCommandeCdR DESC " +
                        "LIMIT 1;";

            MySqlCommand top5MealTopCdR = connection.CreateCommand();
            top5MealTopCdR.CommandText =
                "SELECT r.codeRecette, r.nomR, r.type, r.prixR, r.nombreCommande " +
                    "FROM recette r " +
                        "WHERE r.codeClient = (" +
                            "SELECT c.codeClient " +
                                "FROM client c " +
                                    "ORDER BY c.nombreCommandeCdR DESC " +
                                        "LIMIT 1) " +
                            "ORDER BY r.nombreCommande DESC " +
                            "LIMIT 5;";

            reader = topCdR.ExecuteReader();
            reader.Read();
            string customerCode = reader.GetString(0);
            string firstName = reader.GetString(1);
            string lastName = reader.GetString(2);
            reader.Close();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\"\"\"\"\"\"\"\" Best Meal Creator \"\"\"\"\"\"\"\"\n");
            Console.WriteLine($"Customer code: {customerCode}\nLast name: {lastName}\nFirst name: {firstName}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n");

            reader = top5MealTopCdR.ExecuteReader();
            int numberOfOrders = -1;

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"\"\"\"\"\"\"\"\" His/Her Top 5 Meals \"\"\"\"\"\"\"\"\n");
            Console.ForegroundColor = ConsoleColor.Cyan;
            while (reader.Read())
            {
                mealCode = reader.GetString(0);
                mealName = reader.GetString(1);
                mealType = reader.GetString(2);
                mealPrice = reader.GetFloat(3);
                numberOfOrders = reader.GetInt32(4);
                count++;

                Console.WriteLine($"\"\"\"\"\"\"\"\" Meal number {count} \"\"\"\"\"\"\"\"\n");
                Console.WriteLine($"Meal's code: {mealCode}\nMeal's name: {mealName}\nMeal's type: {mealType}\nMeal's price: {mealPrice} cook\nNumber of orders: {numberOfOrders}\n\n");
            }
            Console.ForegroundColor = ConsoleColor.White;
            reader.Close();
            connection.Close();
            Console.WriteLine();

            LittleMenuCooking(connection); // little menu
        }

        static void DeleteMeal(MySqlConnection connection) // Take the input and make sure it exists
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\"\"\"\"\"\"\" Deleting Meal \"\"\"\"\"\"\"\n\n");
            Console.ForegroundColor = ConsoleColor.White;

            Console.Write("Code : ");
            string mealCode = Convert.ToString(Console.ReadLine()).ToLower();

            connection.Open(); // --> OPEN CO

            MySqlCommand command = connection.CreateCommand();
            command.CommandText =
             "SELECT codeRecette FROM recette;";

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            string codeM = "";
            bool end = true;

            while (reader.Read() && end) // find the meal to delete
            {
                codeM = reader.GetString(0);
                if (mealCode == codeM.ToLower())
                {
                    end = false;
                }
            }
            reader.Close();
            connection.Close(); // --> CLOSE CO

            if (!end) 
            { 
                DeleteMealDelete(connection, codeM); // delete the meal                
                
                Console.WriteLine("> Meal deleted with success.");
                Console.ReadKey();
                Dashboard(connection);
            }
            else
            {
                char input;
                do
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Code meal not found, would you like to try again (y/n) ?");
                    Console.ForegroundColor = ConsoleColor.White;
                    input = Console.ReadKey().KeyChar;
                    Console.ReadKey();
                } while (input != 'y' && input != 'n');
                if (input == 'n') { Dashboard(connection); }
                DeleteMeal(connection); // repeat the method
            }
        }
        static void DeleteMealDelete(MySqlConnection connection, string mealCode) // Delete the meal
        {
            connection.Open(); // --> OPEN CO
            MySqlCommand delete = connection.CreateCommand();           
            delete.CommandText = "DELETE FROM recette WHERE codeRecette = \"" + mealCode + "\";";
            MySqlDataReader reader;
            reader = delete.ExecuteReader();
            reader.Read();
            connection.Close(); // --> CLOSE CO
        }

        static void DeleteCdR(MySqlConnection connection) // Take the input and make sure it exists
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\"\"\"\"\"\"\" Deleting CdR \"\"\"\"\"\"\"\n\n");
            Console.ForegroundColor = ConsoleColor.White;

            Console.Write("Code : ");
            string idCustomer = Convert.ToString(Console.ReadLine()).ToLower();

            connection.Open(); // --> OPEN CO

            MySqlCommand command = connection.CreateCommand();
            command.CommandText =
             "SELECT codeClient FROM client WHERE createur = true;"; // will select only the client who are CdR

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            string codeC = "";
            bool end = true;

            while (reader.Read() && end) // find the CdR to delete
            {
                codeC = reader.GetString(0);
                if (idCustomer == codeC.ToLower())
                {
                    end = false;
                }
            }
            reader.Close();
            connection.Close(); // --> CLOSE CO

            if (!end)
            {
                bool stayCustomer = false;
                char input;
                do
                {
                    Console.WriteLine("Stay customer (y/n) ?"); // stay customer or delete account
                    input = Console.ReadKey().KeyChar;
                    Console.ReadKey();
                } while (input != 'y' && input != 'n');
                if (input == 'y') { stayCustomer = true; }

                DeleteCdRDelete(connection, codeC, stayCustomer); // delete CdR
                Console.WriteLine("> Database updated");
                Dashboard(connection); // Back to the dashboard
            }
            else
            {
                char input;
                do
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Code client not found, would you like to try again (y/n) ?");
                    Console.ForegroundColor = ConsoleColor.White;
                    input = Console.ReadKey().KeyChar;
                    Console.ReadKey();
                } while (input != 'y' && input != 'n');
                if (input == 'n') { Dashboard(connection); } // Back to the dashboard

                DeleteCdR(connection); // repeat the method
            }
        }
        static void DeleteCdRDelete(MySqlConnection connection, string idCustomer, bool stayCustomer) // Delete the meal
        {
            connection.Open(); // --> OPEN CO

            MySqlCommand meals = connection.CreateCommand();
            meals.CommandText = "SELECT r.codeRecette FROM recette r, client c WHERE r.codeClient = \"" + idCustomer + "\";";

            MySqlDataReader reader;
            reader = meals.ExecuteReader();
            reader.Read();

            List<string> listCodeMeal = new List<string>(); // list all the meal that has to be deleted
            while (reader.Read())
            {
                listCodeMeal.Add(reader.GetString(0)); // add in list
            }
            reader.Close();
            connection.Close(); // --> CLOSE CO
            
            foreach (string codeMeal in listCodeMeal)
            {
                DeleteMealDelete(connection, codeMeal); // delete all the meal in list
            }

            connection.Open(); // --> OPEN CO
            if (stayCustomer) // only update his status
            {
                MySqlCommand update = connection.CreateCommand();
                update.CommandText = "UPDATE client SET createur = False, cook = 0, nombreCommandeCdR = 0 WHERE codeClient = \"" + idCustomer + "\";";
                reader = update.ExecuteReader();
                reader.Read();

                Console.WriteLine("> Customer updated");
            }
            else // delete the client
            {
                MySqlCommand delete = connection.CreateCommand();
                delete.CommandText = "DELETE FROM client WHERE codeClient = \"" + idCustomer + "\";";
                reader = delete.ExecuteReader();
                reader.Read();

                Console.WriteLine("> Customer deleted");            
            }
            connection.Close(); // --> CLOSE CO
        }
        #endregion

            
        // _______________________________ MAIN

        static void Main(string[] args)
        {
            string connectionString = "SERVER=localhost;PORT=3306;DATABASE=cooking;UID=root;PASSWORD=Hjbtqt24@!;";
            MySqlConnection connection = new MySqlConnection(connectionString);

            MenuPrincipal(connection);


            Console.ReadKey();
        }

        
        static string GetDate()
        {
            DateTime firstDayOfWeek;
            int day = 0;
            int month = 0;
            int year = 0;

            switch (DateTime.Today.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    firstDayOfWeek = DateTime.Today;
                    day = firstDayOfWeek.Day;
                    month = firstDayOfWeek.Month;
                    year = firstDayOfWeek.Year - 2000;
                    break;

                case DayOfWeek.Tuesday:
                    firstDayOfWeek = DateTime.Today.AddDays(-1);
                    day = firstDayOfWeek.Day;
                    month = firstDayOfWeek.Month;
                    year = firstDayOfWeek.Year - 2000;
                    break;

                case DayOfWeek.Wednesday:
                    firstDayOfWeek = DateTime.Today.AddDays(-2);
                    day = firstDayOfWeek.Day;
                    month = firstDayOfWeek.Month;
                    year = firstDayOfWeek.Year - 2000;
                    break;

                case DayOfWeek.Thursday:
                    firstDayOfWeek = DateTime.Today.AddDays(-3);
                    day = firstDayOfWeek.Day;
                    month = firstDayOfWeek.Month;
                    year = firstDayOfWeek.Year - 2000;
                    break;

                case DayOfWeek.Friday:
                    firstDayOfWeek = DateTime.Today.AddDays(-4);
                    day = firstDayOfWeek.Day;
                    month = firstDayOfWeek.Month;
                    year = firstDayOfWeek.Year - 2000;
                    break;

                case DayOfWeek.Saturday:
                    firstDayOfWeek = DateTime.Today.AddDays(-5);
                    day = firstDayOfWeek.Day;
                    month = firstDayOfWeek.Month;
                    year = firstDayOfWeek.Year - 2000;
                    break;

                case DayOfWeek.Sunday:
                    firstDayOfWeek = DateTime.Today.AddDays(-6);
                    day = firstDayOfWeek.Day;
                    month = firstDayOfWeek.Month;
                    year = firstDayOfWeek.Year - 2000;
                    break;

                default:
                    break;
            }

            return $"{day}-{month}-{year}";
        }

        static int DateDistance(string date)
        {
            int year = Convert.ToInt32(date[6]) * 10 + Convert.ToInt32(date[7]) + 2000;
            int month = Convert.ToInt32(date[3]) * 10 + Convert.ToInt32(date[4]);
            int day = Convert.ToInt32(date[0]) * 10 + Convert.ToInt32(date[1]);
            DateTime enteredDate = new DateTime(year, month, day);

            TimeSpan delay = DateTime.Today - enteredDate;

            return Convert.ToInt32(delay.TotalDays);
        }

        static void Reapprovisionnement(MySqlConnection connection)
        {
            MySqlCommand retrieve = connection.CreateCommand();
            retrieve.CommandText = "SELECT codeProduit, derniereUtilisation, stockMax, stockMin FROM produit;";
            MySqlDataReader reader = retrieve.ExecuteReader();
            reader.Read();

            while(reader.Read())
            {
                if (DateDistance(reader.GetString(1)) > 30)
                {
                    MySqlCommand update = connection.CreateCommand();
                    int stockMax = reader.GetInt32(2) % 2;
                    int stockMin = reader.GetInt32(3) % 2;
                    update.CommandText = "UPDATE produit SET stockMax = " + Convert.ToString(stockMax) + ", stockMin = " + Convert.ToString(stockMin) + "WHERE codeProduit = " + reader.GetString(0) + ";";
                }
            }
        }

        static void UpdateProduct(MySqlConnection connection, string mealCode)
        {
            MySqlDataReader reader;
            connection.Open();
            MySqlCommand infos = connection.CreateCommand();
            infos.CommandText = "SELECT codeProduit FROM constitutionRecette WHERE codeRecette = " + mealCode + ";";
            reader = infos.ExecuteReader();
            reader.Read();

            DateTime today = DateTime.Today;
            string day = Convert.ToString(today.Day);
            string month = Convert.ToString(today.Month);
            string year = Convert.ToString(today.Year - 2000);
            string date = day + "-" + month + "-" + year;

            while (reader.Read())
            {
                MySqlCommand update = connection.CreateCommand();
                update.CommandText = "UPDATE produit SET derniereUtilisation = " + date + " WHERE codeProduit = " + reader.GetString(0) + ";";
                MySqlDataReader reader2 = update.ExecuteReader();
                reader2.Read();
            }

            connection.Close();
        }

        static bool Check()
        {
            DayOfWeek day = DateTime.Today.DayOfWeek;
            int hour = DateTime.Now.Hour;
            int minute = DateTime.Now.Minute;
            int seconds = DateTime.Now.Second;

            bool test = false;

            if (day == DayOfWeek.Sunday)
            {
                if (hour == 23 && minute == 59 && seconds == 59)
                {
                    test = true;
                }
            }
            return test;
        }

        static void UpdateWeeklyOrders(MySqlConnection connection)
        {
            MySqlDataReader reader;
            connection.Open();

            if (Check())
            {
                MySqlCommand update = connection.CreateCommand();
                update.CommandText = "UPDATE recette SET nombreCommandeSemaine = 0;";
                reader = update.ExecuteReader();
                reader.Read();
            }
            connection.Close();
        }

        static void Update(MySqlConnection connection, string orderNumber)
        {
            MySqlDataReader reader;
            connection.Open();

            MySqlCommand infos = connection.CreateCommand();
            infos.CommandText = "SELECT codeRecette, quantiteRecette FROM constitutionPanier WHERE codeCommande = " + orderNumber + ";";
            reader = infos.ExecuteReader();
            reader.Read();

            while (reader.Read())
            {
                MySqlCommand infos2 = connection.CreateCommand();
                infos2.CommandText = "SELECT codeClient, nombreCommandeSemaine, nombreCommande FROM recette WHERE codeRecette = " + reader.GetString(0) + ";";
                MySqlDataReader reader2 = infos2.ExecuteReader();
                reader2.Read();

                int weeklyOrders = reader2.GetInt32(1) + reader.GetInt32(1);
                int orders = reader2.GetInt32(2) + reader.GetInt32(1);
                string customerNumber = reader2.GetString(0);

                MySqlCommand update1 = connection.CreateCommand();
                update1.CommandText = "UPDATE recette SET nombreCommandeSemaine = " + Convert.ToString(weeklyOrders) + ", nombreCommande = " + Convert.ToString(orders) + " WHERE codeRecette = " + reader.GetString(0) + ";";
                reader2 = update1.ExecuteReader();
                reader2.Read();
                MySqlCommand update2 = connection.CreateCommand();
                update2.CommandText = "UPDATE client SET nombreCommandeCdR = " + Convert.ToString(orders) + " WHERE codeClient = " + customerNumber + ";";
                reader2 = update2.ExecuteReader();
                reader2.Read();
                UpdateProduct(connection, reader.GetString(0));
            }
            connection.Close();
        }
    }
}
