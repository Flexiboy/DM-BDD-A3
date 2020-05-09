using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace Cooking_TDF_Eq14
{
    public class Program
    {
        static void MainMenu(MySqlConnection connection)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\"\"\"\"\"\"\" This is... COOKING ! \"\"\"\"\"\"\"\n\n");
            Console.ForegroundColor = ConsoleColor.White;

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
                catch { Console.Write("> Please enter a valid number\n"); }
            } while (choice != 0 && choice != 1 && choice != 2 && choice != 3 && choice != 4);

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

        #region Demo Mode
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
                catch { Console.Write("> Please enter a valid number\n"); }
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
                catch { Console.Write("> Please enter a valid number\n"); }
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
                catch { Console.Write("> Please enter a valid number\n"); }
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
                catch { }
            } while (!listProduct.Contains(product));
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

                while (reader.Read()) // a username has to be unique
                {
                    usernameC = reader.GetString(0);
                    if (username == usernameC)
                    {
                        unique = false;
                        break;
                    }
                }
                reader.Close();
                connection.Close(); // --> CLOSE CO
                if (!unique || !(username.Length > 0))
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
                Console.Write("\n>>> Confirm the creation of the account (y/n) ?\n> ");
                Console.ForegroundColor = ConsoleColor.White;
                input = Console.ReadKey().KeyChar;
                Console.ReadKey();
            } while (input != 'y' && input != 'n');
            if (input == 'n') { MainMenu(connection); }

            #region Create codeClient and Account

            // Get the last client code
            connection.Open(); // --> OPEN CO

            MySqlCommand commandLastCodeC = connection.CreateCommand();
            commandLastCodeC.CommandText =
             "SELECT codeClient FROM client ORDER BY codeClient DESC LIMIT 1;"; // Limit the order to only one element

            MySqlDataReader readerLastCodeC;
            readerLastCodeC = commandLastCodeC.ExecuteReader();

            readerLastCodeC.Read();
            string codeC = readerLastCodeC.GetString(0); // get the last client code

            readerLastCodeC.Close();
            connection.Close(); // --> CLOSE CO

            // Create new client code
            string newCode = "";
            int numeralpart = Convert.ToInt32(codeC.Substring(1));
            numeralpart++;
            if (numeralpart < 10)
            {
                newCode = "C000" + Convert.ToString(numeralpart);
            }
            else if (numeralpart < 100)
            {
                newCode = "C00" + Convert.ToString(numeralpart);
            }
            else if (numeralpart < 1000)
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

            while (readerCo.Read())
            {
                codeC = readerCo.GetString(0);
                usernameC = readerCo.GetString(1);
                passwordC = readerCo.GetString(2);
                if (username == usernameC && password == passwordC) // match the user input with the database
                {
                    end = false;
                    break;
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
                    Console.Write("\nUsername or password incorrect, would you like to try again (y/n) ?\n> ");
                    Console.ForegroundColor = ConsoleColor.White;
                    input = Console.ReadKey().KeyChar;
                    Console.ReadKey();
                } while (input != 'y' && input != 'n');
                if (input == 'n') { MainMenu(connection); }
                Login(connection);
            }
            #endregion
        }

        static void Client(MySqlConnection connection, string codeClient) // Main method for the cooking, act as a dashboard where we can call various method
        {
            Console.Clear();
            Console.WriteLine("\"\"\"\"\"\"\" Client Dashboard \"\"\"\"\"\"\"\n");
            Console.WriteLine("client : " + codeClient + "\n\n");

            int choice = -1;
            Console.WriteLine("1. Take order" +
                "\n2. CdR account" +
                "\n" +
                "\n0. Main menu\n");
            do
            {
                Console.Write("> ");
                try { choice = Convert.ToInt32(Console.ReadLine()); } // make sure the input is an integer
                catch { Console.Write("> Please enter a valid number\n"); }
            } while (choice != 0 && choice != 1 && choice != 2);

            switch (choice)
            {
                case 0:
                    MainMenu(connection);
                    break;
                case 1:
                    string codeCommand = CodeBasket(connection);
                    Console.Clear();
                    Console.WriteLine("\"\"\"\"\"\"\" Take Order \"\"\"\"\"\"\"\n");
                    Order(connection, codeClient, codeCommand);
                    break;
                case 2:
                    if (IsCdR(connection, codeClient)) { MyCdR(connection, codeClient); } // check if he is a CdR first
                    else { BecomeCdR(connection, codeClient); }
                    break;
            }
        }

        // Take Order
        static void Order(MySqlConnection connection, string codeClient, string codeCommand)
        {
            Console.WriteLine("code command : " + codeCommand + "\n");
            int choice = -1;
            Console.WriteLine("1. Add a meal to your basket" +
                "\n2. Browse meals" +
                "\n" +
                "\n0. Client dashboard\n");
            do
            {
                Console.Write("> ");
                try { choice = Convert.ToInt32(Console.ReadLine()); } // make sure the input is an integer
                catch { Console.Write("> Please enter a valid number\n"); }
            } while (choice != 0 && choice != 1 && choice != 2);

            switch (choice)
            {
                case 0:
                    Client(connection, codeClient);
                    break;
                case 1:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\n___________________________ Adding\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    Basket(connection, codeClient, codeCommand);
                    break;
                case 2:
                    DisplayMeal(connection, true); // Display meals wether you are vegetarien or not, without displaying the meal code
                    Order(connection, codeClient, codeCommand);
                    break;
            }
        }

        /// <summary>
        /// Get the last command code and create another one
        /// </summary>
        /// <param name="connection"></param>
        /// <returns>Unique command code</returns>
        static string CodeBasket(MySqlConnection connection)
        {
            // Get the last basket code
            connection.Open(); // --> OPEN CO

            MySqlCommand commandLastCodeB = connection.CreateCommand();
            commandLastCodeB.CommandText =
             "SELECT codeCommande FROM panier ORDER BY codeCommande DESC LIMIT 1;"; // Limit the order to only one element

            MySqlDataReader readerLastCodeB;
            readerLastCodeB = commandLastCodeB.ExecuteReader();

            readerLastCodeB.Read();
            string codeC = readerLastCodeB.GetString(0); // get the last command code

            readerLastCodeB.Close();
            connection.Close(); // --> CLOSE CO

            // Create new basket code
            string newCode = "";
            int numeralpart = Convert.ToInt32(codeC.Substring(2));
            numeralpart++;
            if (numeralpart < 10)
            {
                newCode = "CC000" + Convert.ToString(numeralpart);
            }
            else if (numeralpart < 100)
            {
                newCode = "CC00" + Convert.ToString(numeralpart);
            }
            else if (numeralpart < 1000)
            {
                newCode = "CC0" + Convert.ToString(numeralpart);
            }
            else { newCode = "CC" + Convert.ToString(numeralpart); }
            return newCode;
        }
        static void Basket(MySqlConnection connection, string codeClient, string codeCommand)
        {
            SortedList<string, int> listBasket = new SortedList<string, int>(); // meal code and quantity
            SortedList<string, double> listRemuneration = new SortedList<string, double>(); // client code and remuneration
            double totalprice = 0;

            bool quit = false;
            do
            {
                Console.Write("Add a meal to your basket : ");
                string meal = Convert.ToString(Console.ReadLine()).ToLower();

                connection.Open(); // --> OPEN CO

                MySqlCommand command = connection.CreateCommand();
                command.CommandText =
                 "SELECT nomR, codeRecette, prixR, codeClient, remuneration FROM recette WHERE nomR = \"" + meal + "\";";

                MySqlDataReader reader;
                reader = command.ExecuteReader();

                string codeMeal = "";
                double price = 0;
                string codeC = "";
                double remuneration = 0;
                int quantity = 0;

                bool exist = false;

                while (reader.Read() && !exist)
                {
                    if (meal == reader.GetString(0).ToLower())
                    {
                        codeMeal = reader.GetString(1);
                        price = reader.GetDouble(2);
                        codeC = reader.GetString(3);
                        remuneration = reader.GetDouble(4);

                        exist = true;
                    }
                }
                reader.Close();
                connection.Close(); // --> CLOSE CO

                if (!exist)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("> Meal doesn't exists");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    do
                    {
                        Console.Write("Enter the quantity : ");
                        try { quantity = Convert.ToInt32(Console.ReadLine()); } // Make sure the input is an integer
                        catch { Console.Write("> Please enter a valid number\n"); }
                    } while (quantity < 0); // 0 will not take it into account


                    if (quantity != 0)
                    {
                        if (!SuficientStock(connection, codeMeal, quantity)) // do we have enough stock
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("> Sorry we do not have enough stock for that quantity.\n");
                            Console.ForegroundColor = ConsoleColor.White;
                            continue;
                        }
                        try { listBasket.Add(codeMeal, quantity); } // Cannot add twice the same key (meal)
                        catch
                        {
                            int idx = listBasket.IndexOfKey(codeMeal);
                            int newQ = listBasket.ElementAt(idx).Value + quantity;

                            listBasket[codeMeal] = newQ;
                            //listBasket[codeRecette] = listBasket.TryGetValue(codeRecette) + quantity;                        
                        }
                        try { listRemuneration.Add(codeC, remuneration * quantity); } // cannot add twice the same key (codeClient)
                        catch
                        {
                            int idx = listRemuneration.IndexOfKey(codeC);
                            double newRemun = listRemuneration.ElementAt(idx).Value + remuneration * quantity;

                            listRemuneration[codeC] = newRemun;
                        }
                        totalprice += price * quantity;
                    }
                }

                // print basket
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n------ Current basket ------");
                for (int i = 0; i < listBasket.Count(); i++)
                {
                    connection.Open(); // --> OPEN CO

                    MySqlCommand commandMeal = connection.CreateCommand();
                    commandMeal.CommandText =
                     "SELECT nomR FROM recette where codeRecette = \"" + listBasket.ElementAt(i).Key + "\";";

                    MySqlDataReader readerMeal;
                    readerMeal = commandMeal.ExecuteReader();
                    readerMeal.Read();

                    string thisMeal = readerMeal.GetString(0);

                    readerMeal.Close();
                    connection.Close(); // --> CLOSE CO

                    Console.WriteLine(thisMeal + " : " + listBasket.ElementAt(i).Value);
                }
                Console.WriteLine("\n--- Price = " + totalprice + " cook");
                Console.ForegroundColor = ConsoleColor.White;

                string input;
                do
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("\n> 1.Continue | 2.Confirm basket | 0.Quit\n> ");
                    Console.ForegroundColor = ConsoleColor.White;
                    input = Console.ReadLine();
                } while (input != "1" && input != "2" && input != "0");
                if (input == "2")
                {
                    if (listBasket.Count() != 0) { quit = true; }
                    else { Console.WriteLine("> Add at least one meal to your basket.\n"); }
                }
                else if (input == "0") { Client(connection, codeClient); }

                Console.WriteLine();

            } while (!quit);

            CreateBasket(connection, codeCommand, totalprice, codeClient);
            CreateBasketComposition(connection, codeCommand, listBasket);
            PayCdR(connection, listRemuneration);
            UpdateMeal(connection, listBasket); // update total order, price and remuneration if needed
            UpdateStock(connection, listBasket); //Update stock

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n------ Your meal will now arrive anytime soon. ------");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();
            Client(connection, codeClient);
        }

        static bool SuficientStock(MySqlConnection connection, string codeMeal, int quantity) // Check if the stocks are suficient
        {
            bool suficient = true;

            connection.Open(); // --> OPEN CO

            MySqlCommand commandCo = connection.CreateCommand();
            commandCo.CommandText = "SELECT p.stock - (cr.quantiteProduit*" + quantity + ") FROM produit p, constitutionRecette cr " +
                                        "WHERE p.codeProduit = cr.codeProduit AND cr.codeRecette = \"" + codeMeal + "\";";

            MySqlDataReader readerCo;
            readerCo = commandCo.ExecuteReader();

            while (readerCo.Read())
            {
                double stock = readerCo.GetDouble(0);
                if (stock < 0) // not enough stock
                {
                    connection.Close(); // --> CLOSE CO
                    return false;
                }
            }
            readerCo.Close();
            connection.Close(); // --> CLOSE CO

            return suficient;
        }

        static void CreateBasket(MySqlConnection connection, string codeCommand, double price, string codeClient)
        {
            connection.Open(); // --> OPEN CO
            MySqlCommand commandCreate = connection.CreateCommand();
            commandCreate.CommandText = "INSERT INTO `cooking`.`panier` (`codeCommande`,`date`,`prixP`,`codeClient`) " +
                                            "VALUES(\"" + codeCommand + "\", \"" + GetDate() + "\", " + price + ", \"" + codeClient + "\");";
            MySqlDataReader readerCreate;
            readerCreate = commandCreate.ExecuteReader();
            readerCreate.Read();
            readerCreate.Close();
            connection.Close(); // --> CLOSE CO
        }
        /// <summary>
        /// Create the constitution of an order
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="codeCommand">Meals will be associated with this codeCommand</param>
        /// <param name="listBasket">All ordered meals and its quantities associated with the upper codeCommand</param>
        static void CreateBasketComposition(MySqlConnection connection, string codeCommand, SortedList<string, int> listBasket)
        {
            for (int i = 0; i < listBasket.Count(); i++)
            {
                connection.Open(); // --> OPEN CO
                MySqlCommand commandCreate = connection.CreateCommand();
                commandCreate.CommandText = "INSERT INTO `cooking`.`constitutionPanier` (`codeCommande`,`codeRecette`,`quantiteRecette`) " +
                                                "VALUES (\"" + codeCommand + "\", \"" + listBasket.ElementAt(i).Key + "\", " + listBasket.ElementAt(i).Value + ");";
                MySqlDataReader readerCreate;
                readerCreate = commandCreate.ExecuteReader();
                readerCreate.Read();
                readerCreate.Close();
                connection.Close(); // --> CLOSE CO
            }
        }

        static void PayCdR(MySqlConnection connection, SortedList<string, double> listRemuneration)
        {
            for (int i = 0; i < listRemuneration.Count(); i++)
            {
                connection.Open(); // --> OPEN CO
                MySqlCommand commandPay = connection.CreateCommand();
                commandPay.CommandText = "UPDATE client SET cook = cook + " + listRemuneration.ElementAt(i).Value + " WHERE codeClient = \"" + listRemuneration.ElementAt(i).Key + "\"; ";
                MySqlDataReader readerPay;
                readerPay = commandPay.ExecuteReader();
                readerPay.Read();
                readerPay.Close();
                connection.Close(); // --> CLOSE CO
            }
        }

        /// <summary>
        /// Meal's price + 2 cook if its order exceed 10. And + 5 if it exceed 50 and CdR's remuneration = 4
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="listBasket">A sortedlist that contains all the meal of a certain order and its quantities</param>
        static void UpdateMeal(MySqlConnection connection, SortedList<string, int> listBasket)
        {
            for (int i = 0; i < listBasket.Count(); i++)
            {
                int totalCommand = 0;
                string codeM = listBasket.ElementAt(i).Key;
                int quantity = listBasket.ElementAt(i).Value;

                // Get total command
                connection.Open(); // --> OPEN CO
                MySqlCommand commandGet = connection.CreateCommand();
                commandGet.CommandText = "SELECT nombreCommande fROM recette WHERE codeRecette = \"" + codeM + "\";";
                MySqlDataReader readerGet;
                readerGet = commandGet.ExecuteReader();
                readerGet.Read();
                totalCommand = readerGet.GetInt32(0);
                readerGet.Close();
                connection.Close(); // --> CLOSE CO

                int newTotalCommand = totalCommand + quantity;
                double plusCook = 0;
                double plusRemuneration = 0;

                if (totalCommand < 10 && newTotalCommand >= 10)
                {
                    plusCook = 2;
                    // plus remuneration doesn't change
                }
                if (totalCommand < 50 && newTotalCommand >= 50)
                {
                    plusCook = 5;
                    plusRemuneration = 4;
                }

                // update meal
                connection.Open(); // --> OPEN CO
                MySqlCommand commandUpdate = connection.CreateCommand();
                commandUpdate.CommandText = "UPDATE recette SET prixR = prixR + " + plusCook + ", remuneration = remuneration + " + plusRemuneration + ", nombreCommande = " + newTotalCommand + " WHERE codeRecette = \"" + codeM + "\";";
                MySqlDataReader readerUpdate;
                readerUpdate = commandUpdate.ExecuteReader();
                readerUpdate.Read();
                readerUpdate.Close();
                connection.Close(); // --> CLOSE CO
            }
        }
        static void UpdateStock(MySqlConnection connection, SortedList<string, int> listBasket)
        {
            for (int i = 0; i < listBasket.Count(); i++)
            {
                string codeMeal = listBasket.ElementAt(i).Key;
                int quantity = listBasket.ElementAt(i).Value;

                connection.Open(); // --> OPEN CO

                MySqlCommand commandCo = connection.CreateCommand();
                commandCo.CommandText = "UPDATE produit p, constitutionRecette cr SET p.stock = p.stock - (cr.quantiteProduit * " + listBasket.ElementAt(i).Value + ") " +
                                            "WHERE p.codeProduit = cr.codeProduit AND cr.codeRecette = \"" + listBasket.ElementAt(i).Key + "\";";
                MySqlDataReader readerCo;
                readerCo = commandCo.ExecuteReader();

                readerCo.Close();
                connection.Close(); // --> CLOSE CO
            }
        }

        // CdR's methods
        static void MyCdR(MySqlConnection connection, string codeClient) // MyCdR's menu
        {
            Console.Clear();
            Console.WriteLine("\"\"\"\"\"\"\" My CdR \"\"\"\"\"\"\"\n");

            int choice = -1;
            Console.WriteLine("\n1. Create a meal" +
                "\n2. My cook/meals" +
                "\n3. Delete CdR's account" +
                "\n" +
                "\n0. Client dashboard\n");
            do
            {
                Console.Write("> ");
                try { choice = Convert.ToInt32(Console.ReadLine()); } // make sure the input is an integer
                catch { Console.Write("> Please enter a valid number\n"); }
            } while (choice != 0 && choice != 1 && choice != 2 && choice != 3 && choice != 4);

            switch (choice)
            {
                case 0:
                    Client(connection, codeClient);
                    break;
                case 1:
                    CreateMeal(connection, codeClient);
                    break;
                case 2:
                    CdrDetail(connection, codeClient);
                    break;
                case 3:
                    DeleteMyCdR(connection, codeClient);
                    break;
            }
        }

        static public bool IsCdR(MySqlConnection connection, string codeClient) // Check if a customer is a CdR before getting access
        {
            bool isCdr = true;

            connection.Open(); // --> OPEN CO

            MySqlCommand commandCdR = connection.CreateCommand();
            commandCdR.CommandText =
             "SELECT createur FROM client where codeClient = \"" + codeClient + "\";"; // get the creator status

            MySqlDataReader readerCdR;
            readerCdR = commandCdR.ExecuteReader();
            readerCdR.Read();
            isCdr = readerCdR.GetBoolean(0); // get the creator status

            readerCdR.Close();
            connection.Close(); // --> CLOSE CO

            return isCdr;
        }
        static void BecomeCdR(MySqlConnection connection, string codeClient)
        {
            char input;
            do
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("\nYou are not a CdR, do you want to create an account (y/n) ?\n> ");
                Console.ForegroundColor = ConsoleColor.White;
                input = Console.ReadKey().KeyChar;
                Console.ReadKey();
            } while (input != 'y' && input != 'n');
            if (input == 'n') { Client(connection, codeClient); }

            connection.Open(); // --> OPEN CO

            MySqlCommand commandCdR = connection.CreateCommand();
            commandCdR.CommandText =
             "UPDATE client SET createur = True, cook = 2 WHERE codeClient = \"" + codeClient + "\";"; // update the creator status

            MySqlDataReader readerCdR;
            readerCdR = commandCdR.ExecuteReader();
            readerCdR.Read();

            readerCdR.Close();
            connection.Close(); // --> CLOSE CO

            MyCdR(connection, codeClient);
        }

        static void CreateMeal(MySqlConnection connection, string codeClient)
        {
            Console.Clear();
            Console.WriteLine("\"\"\"\"\"\"\" Creation Zone \"\"\"\"\"\"\"\n");

            Console.Write("Meal's name : ");
            string mealName = Console.ReadLine();
            Console.Write("Meal's description : ");
            string descr = Console.ReadLine();
            string type = "";
            int choice = -1;
            do
            {
                Console.Write("\nMeal's type\n> 1.Appetizer | 2.Meal | 3.Dessert | 4.Cheese  :  ");
                try { choice = Convert.ToInt32(Console.ReadLine()); } // make sure the input is an integer
                catch { }
            } while (choice != 1 && choice != 2 && choice != 3 && choice != 4);

            switch (choice)
            {
                case 1:
                    type = "entrée";
                    break;
                case 2:
                    type = "plat";
                    break;
                case 3:
                    type = "dessert";
                    break;
                case 4:
                    type = "fromage";
                    break;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n___________________________ Recipe\n");
            Console.ForegroundColor = ConsoleColor.White;

            bool veg = true;
            SortedList<string, double> listRecipe = new SortedList<string, double>();
            string newCode = CodeMeal(connection);

            #region Recipe
            bool quit = false;
            do
            {
                Console.Write("Add a product : ");
                string product = Convert.ToString(Console.ReadLine()).ToLower();

                connection.Open(); // --> OPEN CO

                MySqlCommand command = connection.CreateCommand();
                command.CommandText =
                 "SELECT nomP, codeProduit, unite, categorie FROM produit WHERE nomP = \"" + product + "\";";

                MySqlDataReader reader;
                reader = command.ExecuteReader();

                string codeProduct = "";
                double quantity = 0;

                string unit = "";
                bool exist = false;

                while (reader.Read() && !exist)
                {
                    if (product == reader.GetString(0).ToLower())
                    {
                        codeProduct = reader.GetString(1);
                        unit = reader.GetString(2);
                        if (reader.GetString(3).ToLower() == "viande") { veg = false; } // the meal contains meat, so it is not adapted for vegetarians
                        exist = true;
                    }
                }
                reader.Close();
                connection.Close(); // --> CLOSE CO

                if (!exist)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("> Product doesn't exists");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    do
                    {
                        Console.Write("Enter the quantity (" + unit + ") : ");
                        try { quantity = Convert.ToDouble(Console.ReadLine().Replace('.', ',')); } // make sure the input is a double even if we type x.x instead of x,x
                        catch { Console.Write("> Please enter a valid number\n"); }
                    } while (quantity < 0); // 0 will not take it into account

                    if (quantity != 0)
                    {
                        try { listRecipe.Add(codeProduct, quantity); } // Cannot add twice the same key (product)
                        catch
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("Product already entered.\n");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                }

                string input;
                do
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("\n> 1.Continue | 2.Confirm recipe | 0.Quit\n> ");
                    Console.ForegroundColor = ConsoleColor.White;
                    input = Console.ReadLine();
                } while (input != "1" && input != "2" && input != "0");
                if (input == "2")
                {
                    if (listRecipe.Count() != 0) { quit = true; }
                    else { Console.WriteLine("> Add at least one product to your meal.\n"); }
                }
                else if (input == "0") { MyCdR(connection, codeClient); }

                Console.WriteLine();

            } while (!quit);
            #endregion

            CreateMealCreate(connection, newCode, mealName, type, descr, veg, codeClient);
            CreateMealRecipe(connection, newCode, listRecipe);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n------ Your meal was successfully create. ------");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();
            MyCdR(connection, codeClient);
        }
        static public string CodeMeal(MySqlConnection connection) // Create a new meal code
        {
            // Get the last meal code
            connection.Open(); // --> OPEN CO

            MySqlCommand commandLastCodeM = connection.CreateCommand();
            commandLastCodeM.CommandText =
             "SELECT codeRecette FROM recette ORDER BY codeRecette DESC LIMIT 1;"; // Limit the order to only one element

            MySqlDataReader readerLastCodeM;
            readerLastCodeM = commandLastCodeM.ExecuteReader();

            readerLastCodeM.Read();
            string codeM = readerLastCodeM.GetString(0); // get the last meal code

            readerLastCodeM.Close();
            connection.Close(); // --> CLOSE CO

            // Create new meal code
            string newCode = "";
            int numeralpart = Convert.ToInt32(codeM.Substring(1));
            numeralpart++;
            if (numeralpart < 10)
            {
                newCode = "R000" + Convert.ToString(numeralpart);
            }
            else if (numeralpart < 100)
            {
                newCode = "R00" + Convert.ToString(numeralpart);
            }
            else if (numeralpart < 1000)
            {
                newCode = "R0" + Convert.ToString(numeralpart);
            }
            else { newCode = "R" + Convert.ToString(numeralpart); }

            return newCode;
        }
        static void CreateMealCreate(MySqlConnection connection, string codeM, string mealName, string type, string descr, bool veg, string codeClient)
        {
            connection.Open(); // --> OPEN CO
            MySqlCommand commandCreate = connection.CreateCommand();
            commandCreate.CommandText = "INSERT INTO `cooking`.`recette` " +
                                            "(`codeRecette`,`nomR`,`type`,`descriptif`,`veg`,`prixR`,`remuneration`,`codeClient`, `nombreCommandeSemaine`, `nombreCommande`) " +
                                                "VALUES(\"" + codeM + "\", \"" + mealName + "\", \"" + type + "\", \"" + descr + "\", " + veg + ", 10, 2, \"" + codeClient + "\", 0, 0);";
            MySqlDataReader readerCreate;
            readerCreate = commandCreate.ExecuteReader();
            readerCreate.Read();
            readerCreate.Close();
            connection.Close(); // --> CLOSE CO
        }
        /// <summary>
        /// Create the constitution of a meal
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="codeM">Unique code of a certain meal</param>
        /// <param name="listRecipe">A sortedlist that contains all the product of a certain unique meal and its quantities</param>
        static void CreateMealRecipe(MySqlConnection connection, string codeM, SortedList<string, double> listRecipe)
        {
            for (int i = 0; i < listRecipe.Count(); i++)
            {
                connection.Open(); // --> OPEN CO
                MySqlCommand commandCreate = connection.CreateCommand();
                commandCreate.CommandText = "INSERT INTO `cooking`.`constitutionRecette` (`codeRecette`,`codeProduit`,`quantiteProduit`) VALUES (\"" + codeM + "\", \"" + listRecipe.ElementAt(i).Key + "\", " + Convert.ToString(listRecipe.ElementAt(i).Value).Replace(',', '.') + ");"; // SQL takes float with a '.'
                MySqlDataReader readerCreate;
                readerCreate = commandCreate.ExecuteReader();
                readerCreate.Read();
                readerCreate.Close();
                connection.Close(); // --> CLOSE CO
            }
        }

        static void CdrDetail(MySqlConnection connection, string codeClient)
        {
            Console.Clear();
            Console.WriteLine("\"\"\"\"\"\"\" My details \"\"\"\"\"\"\"\n");

            #region Get Cook
            int cook = 0;

            connection.Open(); // --> OPEN CO

            MySqlCommand commandCook = connection.CreateCommand();
            commandCook.CommandText =
             "SELECT cook FROM client where codeClient = \"" + codeClient + "\";";

            MySqlDataReader readerCook;
            readerCook = commandCook.ExecuteReader();
            readerCook.Read();

            cook = readerCook.GetInt32(0); // get cook

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("cook : " + cook + "\n");
            Console.ForegroundColor = ConsoleColor.White;

            readerCook.Close();
            connection.Close(); // --> CLOSE CO
            #endregion

            #region Get Meals
            string nameM = "";
            string type = "";
            string desciption = "";
            bool veg = false;
            float price = 0;
            int order = 0;
            int orderWeek = 0;

            connection.Open(); // --> OPEN CO

            MySqlDataReader readerMeal;
            MySqlCommand commandMeal = connection.CreateCommand();
            commandMeal.CommandText =
                "SELECT nomR, type, descriptif, veg, prixR, nombreCommande, nombreCommandeSemaine FROM recette WHERE codeClient = \"" + codeClient + "\" ORDER BY nombreCommande DESC;";

            readerMeal = commandMeal.ExecuteReader();

            Console.WriteLine("Your most wanted meals are at the top :\n");

            Console.ForegroundColor = ConsoleColor.Cyan;
            while (readerMeal.Read())
            {
                nameM = readerMeal.GetString(0);
                type = readerMeal.GetString(1);
                desciption = readerMeal.GetString(2);
                veg = readerMeal.GetBoolean(3);
                price = readerMeal.GetFloat(4);
                order = readerMeal.GetInt32(5);
                orderWeek = readerMeal.GetInt32(6);

                Console.WriteLine("Meal : " + nameM + " | Type : " + type +
                    " | Description : " + desciption + " | Vegetarien : " + veg + " | price (cook) : " + price +
                    " | Total order : " + order + " | Order in last week : " + orderWeek);
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            readerMeal.Close();
            connection.Close(); // --> CLOSE CO
            #endregion

            Console.WriteLine("Click on any button to return.");
            Console.ReadKey();
            MyCdR(connection, codeClient);
        }

        static void DeleteMyCdR(MySqlConnection connection, string codeClient)
        {
            char input;
            do
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("\n>>> Do you really want to delete your CdR account (y/n) ?\n> ");
                Console.ForegroundColor = ConsoleColor.White;
                input = Console.ReadKey().KeyChar;
                Console.ReadKey();
            } while (input != 'y' && input != 'n');
            if (input == 'n') { MyCdR(connection, codeClient); }

            DeleteCdRDelete(connection, codeClient, true);

            Client(connection, codeClient);
        }
        #endregion

        #region Cooking
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
                "\n0. Main menu\n");
            do
            {
                Console.Write("> ");
                try { choice = Convert.ToInt32(Console.ReadLine()); } // make sure the input is an integer
                catch { Console.Write("> Please enter a valid number\n"); }
            } while (choice != 0 && choice != 1 && choice != 2 && choice != 3 && choice != 4 && choice != 5 && choice != 6 && choice != 7);

            switch (choice)
            {
                case 0:
                    MainMenu(connection);
                    break;
                case 1:
                    DisplayClient(connection);
                    LittleMenuCooking(connection); // little menu
                    break;
                case 2:
                    DisplayMeal(connection);
                    LittleMenuCooking(connection);
                    break;
                case 3:
                    CdRofTheW(connection);
                    LittleMenuCooking(connection);
                    break;
                case 4:
                    Top5Meals(connection);
                    LittleMenuCooking(connection);
                    break;
                case 5:
                    TopCdR(connection);
                    LittleMenuCooking(connection);
                    break;
                case 6:
                    DeleteCdR(connection);
                    break;
                case 7:
                    DeleteMeal(connection);
                    break;
            }
        }

        static public string GetDate()
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
                catch { Console.Write("> Please enter a valid number\n"); }
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
                catch { Console.Write("> Please enter a valid number\n"); }
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
             "SELECT codeClient, nomC, prenomC, cook, nombreCommandeCdR FROM client WHERE createur = TRUE ORDER BY codeClient;";
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
        }

        static void DisplayMeal(MySqlConnection connection, bool displayclient = false)
        {
            char input;
            bool veg = false;
            do
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("\n> All meals (a) | Only vegetarian (v)\n> ");
                Console.ForegroundColor = ConsoleColor.White;
                input = Console.ReadKey().KeyChar;
                Console.ReadKey();
            } while (input != 'a' && input != 'v');
            if (input == 'v') { veg = true; }

            Console.Clear();

            string codeMeal = "";
            string nameM = "";
            string type = "";
            string desciption = "";
            float price = 0;
            int order = 0;
            string lastNameCreator = "";
            string fistNameCreator = "";

            connection.Open(); // --> OPEN CO
            MySqlDataReader reader;
            MySqlCommand command = connection.CreateCommand();
            if (veg == true)
            {
                command.CommandText =
                    "SELECT r.codeRecette, r.nomR, r.type, r.descriptif, r.veg, r.prixR, r.nombreCommande, c.nomC, c.prenomC FROM recette r, client c WHERE r.codeClient = c.codeClient AND r.veg = True ORDER BY r.codeRecette;";
            }
            else
            {
                command.CommandText =
                    "SELECT r.codeRecette, r.nomR, r.type, r.descriptif, r.veg, r.prixR, r.nombreCommande, c.nomC, c.prenomC FROM recette r, client c WHERE r.codeClient = c.codeClient ORDER BY r.codeRecette;";
            }
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

                if (displayclient) // no need to display the meal code for the client
                {
                    Console.WriteLine("Meal : " + nameM + " | Type : " + type +
                    " | Description : " + desciption + " | Vegetarien : " + veg + " | price (cook) : " + price +
                    " | Total order : " + order + " | Creator's last name : " + lastNameCreator + " | Creator's first name : " + fistNameCreator);
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Code Meal : " + codeMeal + " | Meal : " + nameM + " | Type : " + type +
                    " | Description : " + desciption + " | Vegetarien : " + veg + " | price (cook) : " + price +
                    " | Total order : " + order + " | Creator's last name : " + lastNameCreator + " | Creator's first name : " + fistNameCreator);
                    Console.WriteLine();
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            reader.Close();
            connection.Close(); // --> CLOSE CO
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
        }
        static void TopCdR(MySqlConnection connection)
        {
            Console.Clear();
            Console.OutputEncoding = System.Text.Encoding.Unicode; // € euro sign instead instead of cook

            string mealCode = "";
            string mealName = "";
            string mealType = "";
            float mealPrice = -1;
            int count = 0;

            connection.Open(); // --> OPEN CO
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

            MySqlDataReader reader;
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

            Console.ForegroundColor = ConsoleColor.DarkGray;
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
            connection.Close(); // --> CLOSE CO
            Console.WriteLine();
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

            while (reader.Read()) // find the CdR to delete
            {
                codeC = reader.GetString(0);
                if (idCustomer == codeC.ToLower() && idCustomer != "c0000") // don't delete cooking
                {
                    end = false;
                    break;
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
                    Console.Write("Stay customer (y/n) ?\n> "); // stay customer or delete account
                    input = Console.ReadKey().KeyChar;
                    Console.ReadKey();
                } while (input != 'y' && input != 'n');
                if (input == 'y') { stayCustomer = true; }

                DeleteCdRDelete(connection, codeC, stayCustomer); // delete CdR
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("> Database updated.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadKey();
                Dashboard(connection); // Back to the dashboard
            }
            else
            {
                char input;
                do
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Client code not found, would you like to try again (y/n) ?\n> ");
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
                reader.Close();

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("> CdR account deleted with success.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadKey();
            }
            else // delete the client
            {
                MySqlCommand delete = connection.CreateCommand();
                delete.CommandText = "DELETE FROM client WHERE codeClient = \"" + idCustomer + "\";";
                reader = delete.ExecuteReader();
                reader.Read();
                reader.Close();

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("> Customer deleted.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadKey();
            }
            connection.Close(); // --> CLOSE CO
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

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("> Meal deleted with success.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadKey();
                Dashboard(connection);
            }
            else
            {
                char input;
                do
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Meal code not found, would you like to try again (y/n) ?\n> ");
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

            while (reader.Read())
            {
                if (DateDistance(reader.GetString(1)) > 30)
                {
                    MySqlCommand update = connection.CreateCommand();
                    int stockMax = reader.GetInt32(2) % 2;
                    int stockMin = reader.GetInt32(3) % 2;
                    update.CommandText = "UPDATE produit SET stockMax = \"" + Convert.ToString(stockMax) + "\", stockMin = \"" + Convert.ToString(stockMin) + "\" WHERE codeProduit = \"" + reader.GetString(0) + "\";";
                    MySqlDataReader reader2 = update.ExecuteReader();
                    reader2.Read();
                }
            }
        }

        static void UpdateProduct(MySqlConnection connection, string mealCode)
        {
            MySqlDataReader reader;
            connection.Open();
            MySqlCommand infos = connection.CreateCommand();
            infos.CommandText = "SELECT codeProduit FROM constitutionRecette WHERE codeRecette = \"" + mealCode + "\";";
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
                update.CommandText = "UPDATE produit SET derniereUtilisation = \"" + date + "\" WHERE codeProduit = \"" + reader.GetString(0) + "\";";
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
            infos.CommandText = "SELECT codeRecette, quantiteRecette FROM constitutionPanier WHERE codeCommande = \"" + orderNumber + "\";";
            reader = infos.ExecuteReader();
            reader.Read();

            while (reader.Read())
            {
                MySqlCommand infos2 = connection.CreateCommand();
                infos2.CommandText = "SELECT codeClient, nombreCommandeSemaine, nombreCommande FROM recette WHERE codeRecette = \"" + reader.GetString(0) + "\";";
                MySqlDataReader reader2 = infos2.ExecuteReader();
                reader2.Read();

                int weeklyOrders = reader2.GetInt32(1) + reader.GetInt32(1);
                int orders = reader2.GetInt32(2) + reader.GetInt32(1);
                string customerNumber = reader2.GetString(0);

                reader2.Close();

                MySqlCommand update1 = connection.CreateCommand();
                update1.CommandText = "UPDATE recette SET nombreCommandeSemaine = \"" + Convert.ToString(weeklyOrders) + "\", nombreCommande = \"" + Convert.ToString(orders) + "\" WHERE codeRecette = \"" + reader.GetString(0) + "\";";
                reader2 = update1.ExecuteReader();
                reader2.Read();
                reader2.Close();
                MySqlCommand update2 = connection.CreateCommand();
                update2.CommandText = "UPDATE client SET nombreCommandeCdR = \"" + Convert.ToString(orders) + "\" WHERE codeClient = \"" + customerNumber + "\";";
                reader2 = update2.ExecuteReader();
                reader2.Read();
                UpdateProduct(connection, reader.GetString(0));
            }
            connection.Close();
        }
            #endregion

            #region XML



            #endregion


            // _______________________________ MAIN

            static void Main(string[] args)
        {
            string connectionString = "SERVER=localhost;PORT=3306;DATABASE=cooking;UID=cookingmama;PASSWORD=coco;";
            MySqlConnection connection = new MySqlConnection(connectionString);


            MainMenu(connection);


            Console.ReadKey();
        }
    }
}
