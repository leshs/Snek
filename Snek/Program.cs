/*
 * fn@gso-koeln.de 2019
 */
using System;
using System.Collections.Generic;
using System.Timers;

namespace ConsoleGameDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();
            program.GameRun();
        }
        // Limits
        public const int LimitX = 80;
        public const int LimitY = 40;
        private const int TimeStep = 250;

        //Spieler und Liste mit Food
        private Snek player2 = new Snek(20, 20, '1', "Spieler 1", ConsoleColor.DarkBlue);
        private Snek player1 = new Snek(60, 20, '2', "Spieler 2", ConsoleColor.DarkRed);
        private List<Symbol> foodList = new List<Symbol>();
        private List<Hindernis> hindernissListe = new List<Hindernis>();

        private ConsoleKey timerKey = ConsoleKey.Home;
        private bool timerSignal = false;
        private bool player1Loose = false;
        private bool player2Loose = false;
        Random rnd = new Random();


        public void GameRun()
        {
            // Konsole einrichten
            SetGameConsole();
            PrintAnleitung();
            PrintBorder();

            // Timer einrichten und starten
            Timer timer = new Timer();
            timer.Interval = 250; // milli seconds
            timer.Elapsed += TimerElapsed; // Timer Handler registrieren
            timer.Start();
            player2.Show(true); //Player initialisieren (ausgeben)
            player1.Show(true);

            while (true)
            {
                ConsoleKey ck = ConsoleKey.Home;
                if (Console.KeyAvailable)
                {
                    // get key from user
                    ck = Console.ReadKey().Key;
                    timerKey = ck;
                }
                else if (timerSignal && timerKey != ConsoleKey.Home)
                {
                    // repeat key triggered by timer
                    ck = timerKey;
                    timerSignal = false;
                }
                else
                {
                    // continue waiting
                    continue;
                }
                if (ck == ConsoleKey.Escape) break;
                GameStep(ck);
                //Checken, ob jemand gewonnen hat
                if (player1Loose)
                {
                    player2.SetWinPoints();
                    GameOver(player2);
                    break;
                }
                else if (player2Loose)
                {
                    player2.SetWinPoints();
                    GameOver(player1);
                    break;
                }
            }
            timer.Dispose();
            Console.CursorLeft = 1;
            Console.CursorTop = 1;
            Console.CursorLeft = 1;
            Console.CursorTop = 1;
            Console.ReadLine();
        }
        public void PrintAnleitung()
        {
            Console.CursorLeft = 0;
            Console.CursorTop = 10;
            Console.WriteLine("\t Anleitung");
            Console.WriteLine();
            Console.WriteLine("\t Steuerung: Spieler 1: WASD - Spieler 2 Pfeiltasten");
            Console.WriteLine();
            Console.WriteLine("\t Auf dem Spielfeld erscheinen zufällig Symbole und farbige Vierecke");
            Console.WriteLine();
            Console.WriteLine("\t Symbole geben Pluspunkte und die Schlange wird länger");
            Console.WriteLine();
            Console.WriteLine("\t Vierecke geben Minuspunkte");
            Console.WriteLine();
            Console.WriteLine("\t Wenn ein Spieler in eine Schlange oder die Wand läuft, endet das Spiel");
            Console.WriteLine();
            Console.WriteLine("\t Der andere Spieler bekommt dann einen Punkte-Bonus");
            Console.WriteLine();
            Console.WriteLine("\t Der Spieler, der am Ende mehr Punkte hat, hat gewonnen");
            Console.WriteLine();
            Console.WriteLine("\t Zum beginnen des Spiels einen beliebigen Key drücken");
            Console.ReadKey();
            Console.Clear();
        }

        public void SetGameConsole()
        {
            Console.SetWindowSize(LimitX + 1, LimitY); // Etwas Rand, wegen Umbruch
            Console.SetBufferSize(LimitX + 1, LimitY);

            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.White;
            Console.CursorVisible = false;
            Console.Clear();
        }
        public void PrintBorder() //Den Rand ausgeben
        {
            //oben
            Console.CursorLeft = 3;
            Console.CursorTop = 1;
            for (int i = 0; i < LimitX - 3; i++)
            {
                Console.Write("X");
            }
            //rechts und links
            Console.CursorTop = 2;
            Console.CursorLeft = 0;
            for (int i = 0; i < LimitY - 3; i++)
            {
                Console.Write("   X");
                for (int j = 0; j < LimitX - 5; j++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine("X");
            }
            //unten
            Console.CursorLeft = 3;
            Console.CursorTop = LimitY - 2;
            for (int i = 0; i < LimitX - 3; i++)
            {
                Console.Write("X");
            }
        }

        //Ausgabe des Gewinners, wenn das Spiel zu Ende ist.
        public void GameOver(Snek player)
        {
            Console.Clear();
            Console.CursorLeft = 0;
            Console.CursorTop = 15;


            ConsoleColor w = ConsoleColor.White;
            ConsoleColor c1 = player1.GetColor();
            ConsoleColor c2 = player2.GetColor();
            int gesamtP1 = player1.GetWinPoints() + player1.GetStrafPoints() + player1.GetFoodPoints();
            int gesamtP2 = player2.GetWinPoints() + player2.GetStrafPoints() + player2.GetFoodPoints();
            Console.Write("\t\t\t\t");
            Console.ForegroundColor = c1;
            Console.Write("\t Spieler 1");
            Console.ForegroundColor = c2;
            Console.WriteLine("\t Spieler2");

            Console.ForegroundColor = w;
            Console.Write("\t\t\t Food-Punkte");
            Console.ForegroundColor = c1;
            Console.Write("\t    " + player1.GetFoodPoints());
            Console.ForegroundColor = c2;
            Console.WriteLine("\t\t    " + player2.GetFoodPoints());

            Console.ForegroundColor = w;
            Console.Write("\t\t\t Straf-Punkte");
            Console.ForegroundColor = c1;
            Console.Write("\t    " + player1.GetStrafPoints());
            Console.ForegroundColor = c2;
            Console.WriteLine("\t\t    " + player2.GetStrafPoints());

            Console.ForegroundColor = w;
            Console.Write("\t\t\t Bonus");
            Console.ForegroundColor = c1;
            Console.Write("\t\t    " + player1.GetWinPoints());
            Console.ForegroundColor = c2;
            Console.WriteLine("\t\t    " + player2.GetWinPoints());

            Console.ForegroundColor = w;
            Console.Write("\t\t\t Gesamt-Punkte");
            Console.ForegroundColor = c1;
            Console.Write("\t    " + gesamtP1);
            Console.ForegroundColor = c2;
            Console.WriteLine("\t\t    " + gesamtP2);

            Console.WriteLine();
            Console.ForegroundColor = player.GetColor();
            Console.WriteLine("\t\t\t\t " + player.GetName() + " hat gewonnen");
        }

        public void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            timerSignal = true;
        }

        //Die Methode nimnmt den eingelesenen Key und bestimmt, wohin sich der Spieler bewegt.
        public void GameStep(ConsoleKey ck)
        {
            if (rnd.Next(0, 2) == 1)
            {
                GenerateFood();
            }
            else
            {
                GenerateHindernisse();
            }
            Symbol symbolTemp;
            int p1X = player1.GetHead().GetX();
            int p1Y = player1.GetHead().GetY();
            Char p1C = player1.GetChar();
            ConsoleColor p1Col = player1.GetConsoleColor();
            int p2X = player2.GetHead().GetX();
            int p2Y = player2.GetHead().GetY();
            Char p2C = player2.GetChar();
            ConsoleColor p2Col = player2.GetConsoleColor();


            //Player 2
            if (ck == ConsoleKey.UpArrow)
            {
                symbolTemp = new Symbol(p1X, p1Y - 1, '#', p1Col);
            }
            else if (ck == ConsoleKey.DownArrow)
            {
                symbolTemp = new Symbol(p1X, p1Y + 1, '#', p1Col);
            }
            else if (ck == ConsoleKey.LeftArrow)
            {
                symbolTemp = new Symbol(p1X - 1, p1Y, '#', p1Col);
            }
            else if (ck == ConsoleKey.RightArrow)
            {
                symbolTemp = new Symbol(p1X + 1, p1Y, '#', p1Col);
            }

            //Player 1
            else if (ck == ConsoleKey.W)
            {
                symbolTemp = new Symbol(p2X, p2Y - 1, '#', p2Col);
            }
            else if (ck == ConsoleKey.S)
            {
                symbolTemp = new Symbol(p2X, p2Y + 1, '#', p2Col);
            }
            else if (ck == ConsoleKey.A)
            {
                symbolTemp = new Symbol(p2X - 1, p2Y, '#', p2Col);
            }
            else if (ck == ConsoleKey.D)
            {
                symbolTemp = new Symbol(p2X + 1, p2Y, '#', p2Col);
            }
            else
            {
                symbolTemp = new Symbol(0, 0, 'A', p2Col);
            }
            //Überprüfen ob GameOver
            if (symbolTemp.GetColor() == p1Col)
            {
                CheckForLoose(symbolTemp, player1);
                if (CheckForHinderniss(symbolTemp)) player1.AddStrafPoints();
                player1.Move(CheckForFood(symbolTemp), symbolTemp);
            }
            else if (symbolTemp.GetColor() == p2Col)
            {
                CheckForLoose(symbolTemp, player2);
                if (CheckForHinderniss(symbolTemp)) player2.AddStrafPoints();
                CheckForHinderniss(symbolTemp);
                player2.Move(CheckForFood(symbolTemp), symbolTemp);
            }
        }

        //Die Methode generiert zufällig Essen
        public void GenerateFood()
        {
            if (rnd.Next(0, 100) > 80 && foodList.Count < 8)
            {
                int xPos = rnd.Next(4, 78);
                int yPos = rnd.Next(3, 37);
                Symbol foodTemp = new Symbol(xPos, yPos, 'O', ConsoleColor.White);
                bool exists = false;
                foreach (Symbol symbol in foodList) //Checken, ob das zufällig generierte Symbol bereits in der Foodliste vorhanden ist
                {
                    if (symbol.GetX() == foodTemp.GetX() && symbol.GetY() == foodTemp.GetY())
                        exists = true;
                }
                if (!exists && !player1.CheckForSnek(foodTemp) && !player2.CheckForSnek(foodTemp)) //An der Stelle des Symbols darf keine Schlange stehen
                {
                    foodList.Add(foodTemp);
                    foodTemp.Show(true);
                }
            }
        }

        //Die Methode generiert zufällig Hindernisse; Die Hindernisse sind farbige Vierecke
        public void GenerateHindernisse()
        {
            if (rnd.Next(0, 100) > 93)
            {
                int xPos = rnd.Next(4, 72);
                int yPos = rnd.Next(3, 35);
                int width = rnd.Next(1, 10);
                int heigth = rnd.Next(1, 8);
                Hindernis hindernissTemp = new Hindernis(xPos, yPos, width, heigth, player1, player2);
                hindernissListe.Add(hindernissTemp);
            }
        }


        //_____________________ Hier beginnen die "Check"-Methoden - sie überprüfen u.a. , ob die Schlange "bei einem Schritt auf etwas trifft" ____________________

        //Die Methode überprüft, ob ein Spieler verloren hat (Ob er in sich selbst, den anderen Spieler oder die Wand gefahren ist)
        public void CheckForLoose(Symbol symbolTemp, Snek snekTemp)
        {
            if (player1.CheckForSnek(symbolTemp) || player2.CheckForSnek(symbolTemp) || CheckForBorder(symbolTemp))
            {
                if (snekTemp == player1)
                {
                    player1Loose = true;
                }
                else
                {
                    player2Loose = true;
                }
            }
        }

        //Es wird geprüft, ob der Spieler ein Food-Symbol getroffen hat
        public bool CheckForFood(Symbol symbolTemp)
        {
            foreach (Symbol foodTemp in foodList)
            {
                if (foodTemp.GetX() == symbolTemp.GetX() && foodTemp.GetY() == symbolTemp.GetY())
                {
                    foodList.Remove(foodTemp);
                    return true;
                }
            }
            return false;
        }

        //Die Methode überprüft, ob ein Spieler in eine Mauer gerannt ist
        public bool CheckForBorder(Symbol symbol)
        {
            bool border = false;
            if (symbol.GetX() == 3 || symbol.GetX() == 79 || symbol.GetY() == 1 || symbol.GetY() == 38) border = true;
            return border;
        }

        public bool CheckForHinderniss(Symbol symbol)
        {
            foreach (Hindernis hinderniss in hindernissListe)
            {
                if (hinderniss.CheckForHinderniss(symbol))
                {
                    hindernissListe.Remove(hinderniss);
                    return true;
                }
            }
            return false;
        }
    }
}

