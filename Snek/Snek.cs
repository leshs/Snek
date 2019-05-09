using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameDemo
{
    class Snek
    {
        private List<Symbol> snekList = new List<Symbol>();
        private Char character;
        private String playerName;
        private ConsoleColor color;
        private int strafPonits;
        private int foodPoints;
        private int winPoints = 0;
        public Snek(int x, int y, char symbol, String name, ConsoleColor clr)
        {
            playerName = name;
            color = clr;
            character = symbol;
            Symbol first = new Symbol(x, y, symbol, clr);
            snekList.Add(first);
        }

        /* Bewegen der Schlange
         * Der Methode wird ein Symbol übergeben, welches der neue "Kopf" der Schlange ist
         * Außerdem wird ein bool übergeben, der anzeigt ob bei dem Schritt auf ein Essen getroffen wurde.
         * Wenn kein Essen getroffen wurde, wird das letzte Symbol der Schlange (das erste in der Liste) entfernt.
         */
        public void Move(bool food, Symbol newHead)
        {
            snekList.Add(newHead);
            newHead.Show(true);
            if (!food)
            {
                snekList.ElementAt(0).Show(false);
                snekList.RemoveAt(0);
            }
            else foodPoints++;
        }

        //Gibt den "Kopf" der Schlange zurück, dies entspricht dem letzten Symbol in der Liste
        public Symbol GetHead()
        {
            return snekList.ElementAt(snekList.Count - 1);
        }

        //Überprüft, ob an einer bestimmten Stelle eine Schlange bzw. ein Schlangen-Symbol ist -> wichtig zum überprüfen, ob jemand gewonnen hat
        public bool CheckForSnek(Symbol symbol)
        {
            bool snekImWeg = false;
            foreach (Symbol snekSymbol in snekList)
            {
                if (symbol.GetX() == snekSymbol.GetX() && symbol.GetY() == snekSymbol.GetY())
                {
                    snekImWeg = true;
                }
            }
            return snekImWeg;
        }

        public void Show(bool show)
        {
            foreach (Symbol symbol in snekList)
            {
                symbol.Show(true);
            }
        }

        public Char GetChar()
        {
            return character;
        }

        public String GetName()
        {
            return playerName;
        }

        public ConsoleColor GetConsoleColor()
        {
            return color;
        }

        public void AddStrafPoints()
        {
            strafPonits--;
        }

        public ConsoleColor GetColor()
        {
            return color;
        }

        public void SetWinPoints()
        {
            winPoints = 20;
        }

        public int GetWinPoints()
        {
            return winPoints;
        }

        public int GetFoodPoints()
        {
            return foodPoints;
        }

        public int GetStrafPoints()
        {
            return strafPonits;
        }
    }
}
