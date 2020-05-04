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
        static void MenuPrincipal(MySqlConnection connection)
        {
            Console.Clear();
            Console.WriteLine("\"\"\"\"\"\"\" Bienvenue chez... COOKING ! \"\"\"\"\"\"\"\n\n");

            int choix = Selection();

            switch (choix)
            {
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    ModeDemo(connection);
                    break;
            }
        }

        static int Selection()
        {
            int choix = -1;
            Console.WriteLine("1. Se connecter" +
                "\n2. S'inscrire" +
                "\n3. Mode Démo" +
                "\n" +
                "\n0. Sortir\n");
            do
            {
                Console.Write("> ");
                try { choix = Convert.ToInt32(Console.ReadLine()); } // On vérifie si c'est bien un entier qui a été entré
                catch { Console.Write("Veuillez entrer un chiffre\n"); }
            } while (choix != 0 && choix != 1 && choix != 2 && choix != 3);
            return choix;
        }

        #region Mode Demo

        static void ModeDemo(MySqlConnection connection)
        {
            Console.Clear();
            Console.WriteLine("\"\"\"\"\"\"\" Mode Démo \"\"\"\"\"\"\"\n\n");
            #region Choix

            int choix = -1;
            Console.WriteLine("1. Afficher le nombre de clients" +
                "\n2. Afficher le nombre et le nom des CdR ainsi que leurs nombres total de recettes commandées" +
                "\n3. Afficher le nombre de recettes" +
                "\n4. Afficher la liste des produits ayant une quantité inférieur ou égale à 2x leur quantité minimale" +
                "\n\n0. Sortir\n");
            do
            {
                Console.Write("> ");
                try { choix = Convert.ToInt32(Console.ReadLine()); } // On vérifie si c'est bien un entier qui a été entré
                catch { Console.Write("Veuillez entrer un chiffre\n"); }
            } while (choix != 0 && choix != 1 && choix != 2 && choix != 3 && choix != 4);
            Console.WriteLine();

            #endregion
            switch (choix)
            {
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    DemoClients(connection);
                    break;
                case 2:
                    DemoCdR(connection);
                    break;
                case 3:
                    NombreRecettes(connection);
                    break;
                case 4:
                    MinProduit(connection);
                    break;
            }
        } // Méthode appelant les autres méthodes de Démo selon le choix effectué

        static void DemoClients(MySqlConnection connection)
        {
            connection.Open();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText =
             "SELECT COUNT(*) FROM client;";

            MySqlDataReader reader;
            reader = command.ExecuteReader();
            reader.Read();
            int nbClient = reader.GetInt32(0);

            if (nbClient > 1) { Console.WriteLine("Il y a " + nbClient + " clients inscrits chez Cooking\n\n"); }
            else { Console.WriteLine("Il y a " + nbClient + " client inscrit chez Cooking\n\n"); }

            connection.Close();

            #region Choix Suite

            int choix = -1;
            Console.WriteLine("1. Continuer en Démo" +
                "\n2. Revenir au menu principal" +
                "\n0. Sortir\n");
            do
            {
                Console.Write("> ");
                try { choix = Convert.ToInt32(Console.ReadLine()); } // On vérifie si c'est bien un entier qui a été entré
                catch { Console.Write("Veuillez entrer un chiffre\n"); }
            } while (choix != 0 && choix != 1 && choix != 2);
            Console.WriteLine();

            #endregion
            switch (choix)
            {
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    ModeDemo(connection);
                    break;
                case 2:
                    MenuPrincipal(connection);
                    break;
            }
        }

        static void DemoCdR(MySqlConnection connection)
        {
            #region Nombre de CdR

            connection.Open();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText =
             "SELECT COUNT(*) FROM client WHERE createur = True;";

            MySqlDataReader reader;
            reader = command.ExecuteReader();
            reader.Read();
            int nbCdR = reader.GetInt32(0);

            if (nbCdR > 1) { Console.WriteLine("Il y a " + nbCdR + " clients créateurs de recettes chez Cooking\n"); }
            else { Console.WriteLine("Il y a " + nbCdR + " client créateur de recettes chez Cooking\n"); }

            connection.Close();

            #endregion
            #region Liste CdR et nombres recettes

            connection.Open();

            MySqlCommand command2 = connection.CreateCommand();
            command2.CommandText =
             "select c.nomC, c.prenomC, sum(r.nombreCommande) from client c, recette r where c.codeClient = r.codeClient and c.createur = True group by c.codeClient;";

            MySqlDataReader reader2;
            reader2 = command2.ExecuteReader();

            string nom = "";
            string prenom = "";
            int nbCommande = -1;

            while (reader2.Read())
            {
                nom = reader2.GetString(0);
                prenom = reader2.GetString(1);
                nbCommande = reader2.GetInt32(2);

                Console.WriteLine("Nom : " + nom + " | Prenom : " + prenom + " | Nombre de recette(s) commandée(s) : " + nbCommande);
            }
            connection.Close();
            Console.WriteLine("\n\n");

            #endregion


            #region Choix Suite

            int choix = -1;
            Console.WriteLine("1. Continuer en Démo" +
                "\n2. Revenir au menu principal" +
                "\n0. Sortir\n");
            do
            {
                Console.Write("> ");
                try { choix = Convert.ToInt32(Console.ReadLine()); } // On vérifie si c'est bien un entier qui a été entré
                catch { Console.Write("Veuillez entrer un chiffre\n"); }
            } while (choix != 0 && choix != 1 && choix != 2);
            Console.WriteLine();

            #endregion
            switch (choix)
            {
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    ModeDemo(connection);
                    break;
                case 2:
                    MenuPrincipal(connection);
                    break;
            }
        }

        static void NombreRecettes(MySqlConnection connection)
        {
            connection.Open();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText =
             "SELECT COUNT(*) FROM recette;";

            MySqlDataReader reader;
            reader = command.ExecuteReader();
            reader.Read();
            int nbRecette = reader.GetInt32(0);

            if (nbRecette > 1) { Console.WriteLine("Il y a " + nbRecette + " recettes disponibles chez Cooking\n\n"); }
            else { Console.WriteLine("Il y a " + nbRecette + " recette disponible chez Cooking\n\n"); }

            connection.Close();

            #region Choix Suite

            int choix = -1;
            Console.WriteLine("1. Continuer en Démo" +
                "\n2. Revenir au menu principal" +
                "\n0. Sortir\n");
            do
            {
                Console.Write("> ");
                try { choix = Convert.ToInt32(Console.ReadLine()); } // On vérifie si c'est bien un entier qui a été entré
                catch { Console.Write("Veuillez entrer un chiffre\n"); }
            } while (choix != 0 && choix != 1 && choix != 2);
            Console.WriteLine();

            #endregion
            switch (choix)
            {
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    ModeDemo(connection);
                    break;
                case 2:
                    MenuPrincipal(connection);
                    break;
            }
        }

        static void MinProduit(MySqlConnection connection)
        {
            List<string> listeProduit = new List<string>();

            #region Stock et stockMin

            connection.Open();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText =
             "SELECT nomP, stock, stockMin FROM produit WHERE stock <= 2*stockMin ORDER BY nomP;";

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            string nomP = "";
            int stock = -1;
            int stockMin = -1;

            while (reader.Read())
            {
                nomP = reader.GetString(0);
                listeProduit.Add(nomP.ToLower());
                stock = reader.GetInt32(1);
                stockMin = reader.GetInt32(2);

                Console.WriteLine("Produit : " + nomP + " | Stock : " + stock + " | Stock minimal : " + stockMin);
            }
            connection.Close();
            Console.WriteLine("\n");

            #endregion

            #region Choix Suite

            int choix = -1;
            Console.WriteLine("1. Saisie d'un produit\n" +
                "\n2. Continuer en Démo" +
                "\n3. Revenir au menu principal" +
                "\n0. Sortir\n");
            do
            {
                Console.Write("> ");
                try { choix = Convert.ToInt32(Console.ReadLine()); } // On vérifie si c'est bien un entier qui a été entré
                catch { Console.Write("Veuillez entrer un chiffre\n"); }
            } while (choix != 0 && choix != 1 && choix != 2 && choix != 3);
            Console.WriteLine();

            #endregion
            switch (choix)
            {
                case 0:
                    Environment.Exit(0);
                    break;
                case 2:
                    ModeDemo(connection);
                    break;
                case 3:
                    MenuPrincipal(connection);
                    break;
            }
            #region Liste Produit

            string produit = "";
            do
            {
                Console.Write("Entrer le produit que vous souhaitez afficher : ");
                try { produit = Convert.ToString(Console.ReadLine()).ToLower(); }
                catch { }
            } while (!listeProduit.Contains(produit));
            Console.WriteLine();

            connection.Open();

            MySqlCommand command2 = connection.CreateCommand();
            command2.CommandText =
             "SELECT r.nomR, cr.quantiteProduit, p.unite FROM recette r, constitutionRecette cr, produit p WHERE p.nomP = \"" + produit + "\" AND p.codeProduit = cr.codeProduit AND r.codeRecette = cr.codeRecette GROUP BY r.codeRecette;";

            MySqlDataReader reader2;
            reader2 = command2.ExecuteReader();

            string nomR = "";
            float quantite = -1;
            string unite = "";

            while (reader2.Read())
            {
                nomR = reader2.GetString(0);
                quantite = reader2.GetFloat(1);
                unite = reader2.GetString(2);

                Console.WriteLine("Recette : " + nomR + " | Quantité : " + quantite + " | Unité : " + unite);
            }
            connection.Close();
            Console.WriteLine("\n\n");

            #endregion

            #region Choix Suite

            int choix2 = -1;
            Console.WriteLine("1. Continuer en Démo" +
                "\n2. Revenir au menu principal" +
                "\n0. Sortir\n");
            do
            {
                Console.Write("> ");
                try { choix2 = Convert.ToInt32(Console.ReadLine()); } // On vérifie si c'est bien un entier qui a été entré
                catch { Console.Write("Veuillez entrer un chiffre\n"); }
            } while (choix2 != 0 && choix2 != 1 && choix2 != 2);
            Console.WriteLine();

            #endregion
            switch (choix2)
            {
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    ModeDemo(connection);
                    break;
                case 2:
                    MenuPrincipal(connection);
                    break;
            }
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

        static void TableauDeBord(MySqlConnection connection)
        {
            connection.Open();
            MySqlDataReader reader;

            #region CdR Of The Week
            MySqlCommand cdrOfTheWeek = connection.CreateCommand();
            cdrOfTheWeek.CommandText =
                "SELECT c.codeClient, c.nomC, c.prenomC, SUM(r.nombreCommandeSemaine) AS total " +
                    "FROM client c, recette r" +
                        "WHERE c.codeClient = r.codeClient" +
                            "AND c.createur = TRUE" +
                                "GROUP BY c.codeClient" +
                                "ORDER BY total DESC" +
                                "LIMIT 1;";

            reader = cdrOfTheWeek.ExecuteReader();
            reader.Read();
            string customerCode = reader.GetString(0);
            string lastName = reader.GetString(1);
            string firstName = reader.GetString(2);
            int orderNumber = reader.GetInt32(3);

            Console.WriteLine($"\"\"\"\"\"\"\"\"Best Meal Creator of the week\"\"\"\"\"\"\"\"\n");
            Console.WriteLine($"Customer number: {customerCode}\nLast name: {lastName}\nFirst name: {firstName}\nNumber of order this week: {orderNumber}");
            
            Console.WriteLine("\n\n\n\n");
            #endregion

            #region Top 5 Meals
            MySqlCommand top5Meal = connection.CreateCommand();
            top5Meal.CommandText =
                "SELECT r.codeRecette, r.nomR, r.type, r.prixR, r.nombreCommandeSemaine, c.nomC, c.prenomC" +
                    "FROM recette r, client c" +
                        "WHERE c.codeClient = r.codeClient" +
                            "ORDER BY r.nombreCommandeSemaine DESC" +
                            "LIMIT 5; ";

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

            Console.WriteLine($"\"\"\"\"\"\"\"\"Top 5 Meals this week\"\"\"\"\"\"\"\"\n");
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

                Console.WriteLine($"\"\"\"\"\"\"\"\"Meal number {count}\"\"\"\"\"\"\"\"\n");
                Console.WriteLine($"Meal's code: {mealCode}\nMeal's name: {mealName}\nMeal's type: {mealType}\nMeal's price: {mealPrice} €\nNumber of orders this week: {weekOrders}\nCreator's last name: {creatorsLastName}\nCreator's first name: {creatorsFirstName}\n\n");
            }

            Console.WriteLine("\n\n\n\n");
            #endregion

            #region Top CdR
            MySqlCommand topCdR = connection.CreateCommand();
            topCdR.CommandText =
                "SELECT c.codeClient, c.nomC, c.prenomC" +
                    "FROM client c" +
                        "ORDER BY c.nombreCommandeCdR DESC" +
                        "LIMIT 1;";

            MySqlCommand top5MealTopCdR = connection.CreateCommand();
            top5MealTopCdR.CommandText =
                "SELECT r.codeRecette, r.nomR, r.type, r.prixR, r.nombreCommande" +
                    "FROM recette r" +
                        "WHERE r.codeClient = (" +
                            "SELECT c.codeClient" +
                                "FROM client c" +
                                    "ORDER BY c.nombreCommandeCdR DESC" +
                                        "LIMIT 1)" +
                            "ORDER BY r.nombreCommande DESC" +
                            "LIMIT 5;";

            reader = topCdR.ExecuteReader();
            reader.Read();
            customerCode = reader.GetString(0);
            firstName = reader.GetString(1);
            lastName = reader.GetString(2);

            Console.WriteLine($"\"\"\"\"\"\"\"\"Best Meal Creator\"\"\"\"\"\"\"\"\n");
            Console.WriteLine($"Customer code: {customerCode}\nLast name: {lastName}\nFirst name: {firstName}");

            reader = top5MealTopCdR.ExecuteReader();
            reader.Read();
            int numberOfOrders = -1;

            Console.WriteLine($"\"\"\"\"\"\"\"\"His/Her Top 5 Meals\"\"\"\"\"\"\"\"\n");
            while (reader.Read())
            {
                mealCode = reader.GetString(0);
                mealName = reader.GetString(1);
                mealType = reader.GetString(2);
                mealPrice = reader.GetFloat(3);
                numberOfOrders = reader.GetInt32(4);
                count++;

                Console.WriteLine($"\"\"\"\"\"\"\"\"Meal number {count}\"\"\"\"\"\"\"\"\n");
                Console.WriteLine($"Meal's code: {mealCode}\nMeal's name: {mealName}\nMeal's type: {mealType}\nMeal's price: {mealPrice} €\nNumber of orders: {numberOfOrders}\n\n");
            }
            Console.WriteLine("\n\n\n\n");
            #endregion

            connection.Close();
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

        static void DeleteMeal(MySqlConnection connection, string meal, string mealCode)
        {
            connection.Open();
            MySqlDataReader reader;
            MySqlCommand verify = connection.CreateCommand();
            verify.CommandText = "SELECT nomR FROM recette WHERE codeRecette = " + mealCode + ";";
            
            reader = verify.ExecuteReader();
            reader.Read();
            if (reader.GetString(0) == meal)
            {
                MySqlCommand delete = connection.CreateCommand();
                delete.CommandText = "DELETE FROM recette WHERE codeRecette = " + mealCode;
                reader = delete.ExecuteReader();
                reader.Read();
            }
            else
            {
                Console.WriteLine("Cette recette n'existe pas");
            }
            connection.Close();
        }

        static void DeleteCdR(MySqlConnection connection, string customer, bool stayCustomer)
        {
            MySqlDataReader reader;
            connection.Open();
            MySqlCommand meals = connection.CreateCommand();
            meals.CommandText = "SELECT nomR, codeRecette FROM recette WHERE codeClient = " + customer + ";";

            reader = meals.ExecuteReader();
            reader.Read();

            while(reader.Read())
            {
                DeleteMeal(connection, reader.GetString(0), reader.GetString(1));
            }

            if (stayCustomer)
            {
                MySqlCommand update = connection.CreateCommand();
                update.CommandText = "UPDATE client SET createur = False, cook = 0, nombreCommandeCdR = 0 WHERE  codeClient = " + customer + ";";
                reader = update.ExecuteReader();
                reader.Read();

                Console.WriteLine("Customer updated");
            }
            else
            {
                MySqlCommand delete = connection.CreateCommand();
                delete.CommandText = "DELETE FROM client WHERE codeClient = " + customer + ";";
                reader = delete.ExecuteReader();
                reader.Read();

                Console.WriteLine("Customer deleted");
            }

            Console.WriteLine("Database updated");
            connection.Close();
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
