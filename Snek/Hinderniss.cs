using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameDemo
{

    class Hindernis
    {
        private List<Symbol> hindernisse = new List<Symbol>();
        private int xPos;
        private int yPos;
        private int w;
        private int h;
        private Snek snek1;
        private Snek snek2;
        private ConsoleColor color = ConsoleColor.White;
        public Hindernis(int x, int y, int width, int heigth, Snek s1, Snek s2)
        {
            xPos = x;
            yPos = y;
            w = width;
            h = heigth;
            snek1 = s1;
            snek2 = s2;
            Generate();
        }

        public void Generate()
        {
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    Symbol symbol = new Symbol(xPos + i, yPos + j, ' ', color, true);
                    if (!snek1.CheckForSnek(symbol) && !snek2.CheckForSnek(symbol))
                        symbol.Show(true);
                    hindernisse.Add(symbol);
                }
            }
        }

        public bool CheckForHinderniss(Symbol symbol)
        {
            foreach (Symbol hindernissTemp in hindernisse)
            {
                if (hindernissTemp.GetX() == symbol.GetX() && hindernissTemp.GetY() == symbol.GetY())
                {
                    Remove();
                    return true;
                }

            }
            return false;
        }

        public void Remove()
        {
            foreach (Symbol hindernissTemp in hindernisse)
            {
                hindernissTemp.SetIsHinderniss(false);
                hindernissTemp.Show(false);
            }
        }
    }
}
