/*
 * fn@gso-koeln.de 2019
 */
using System;

namespace ConsoleGameDemo
{
    public class Symbol
    {
        private int posX = 0;
        private int posY = 0;
        private ConsoleColor color;
        private char sign;
        private bool isHinderniss = false;

        public Symbol(int x, int y, char character, ConsoleColor clr, bool hinderniss = false)
        {
            posX = x;
            posY = y;
            color = clr;
            sign = character;
            isHinderniss = hinderniss;
        }

        public void Move(int x, int y)
        {
            // Symbol ausradieren
            Show(false);

            // Position umsetzen mit Überlauf
            posX = (posX + Program.LimitX + x) % Program.LimitX;
            posY = (posY + Program.LimitY + y) % Program.LimitY;
            Console.CursorLeft = posX;
            Console.CursorTop = posY;

            // Symbol an neuer Stelle ausgeben
            Show(true);
        }

        public void Show(bool show)
        {
            // Cursorposition setzen
            Console.CursorLeft = posX;
            Console.CursorTop = posY;
            Console.BackgroundColor = ConsoleColor.Green;

            // Ausgabefarbe bestimmen
            if (show) Console.ForegroundColor = color;
            else Console.ForegroundColor = Console.BackgroundColor;

            if (isHinderniss && show) Console.BackgroundColor = color;

            // Symbol ausgeben
            Console.Write(sign);
            Console.BackgroundColor = ConsoleColor.Green;
            // Cursorposition zurücksezten (wegen Blinkeffekt)
            Console.CursorLeft = posX;
            Console.CursorTop = posY;
        }

        public int GetX()
        {
            return posX;
        }

        public int GetY()
        {
            return posY;
        }

        public Char GetChar()
        {
            return sign;
        }

        public void SetIsHinderniss(bool hinderniss)
        {
            this.isHinderniss = hinderniss;
        }

        public ConsoleColor GetColor()
        {
            return color;
        }
    }
}
