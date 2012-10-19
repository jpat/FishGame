using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace FishingGame
{
    class HighScoreMenu
    {
        public static Dictionary<string, int> scores;
        KeyboardState lastKeyState;
        int yOffset = 200;

        public HighScoreMenu()
        {
            scores = new Dictionary<string, int>() 
            {
                {"Julz", 11101},
                {"Mandz", 5293},
                {"Poop", 9009}
            };
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int i = 0;
            foreach (var v in scores)
            {
                string s = v.Key + " " + v.Value;
                Vector2 stringSize = Game1.roundedFont.MeasureString(s);

                spriteBatch.DrawString(Game1.roundedFont, v.Key + " " + v.Value, new Vector2(100, yOffset + i * stringSize.Y), Color.White);
                i++;
            }
        }
    }
}
