using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Cuivre.Code.Utils
{
    static class TextWrap
    {
        public static List<string> Wrap(string text, int width, SpriteFont font)
        {
            List<string> list = new List<string>();
            if (text == null) return list;

            string[] words = text.Split(new char[] { ' ' });

            int curWidth = 0;
            string curText = "";

            foreach (string word in words)
            {
                curWidth += (int)font.MeasureString(word + " ").X;
                if (word.Equals("\n"))
                {
                    list.Add(curText);
                    curText = "";
                    curWidth = 0;
                }
                else if (curWidth > width)
                {
                    list.Add(curText);
                    curText = word + " ";
                    curWidth = (int)font.MeasureString(word + " ").X;
                }
                else
                {
                    curText += word + " ";
                }
            }
            list.Add(curText);

            return list;
        }

        public static List<string> Wrap(string text, int width, SpriteFont font, int letterCount)
        {
            return Wrap(text.Substring(0, Math.Min(letterCount, text.Length)), width, font);
        }
    }
}
