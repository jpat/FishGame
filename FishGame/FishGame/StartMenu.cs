using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FishingGame
{
    class StartMenu
    {
        string[] menuItems = {"Play", "View High Scores", "Quit"};

        Color normalColor = Color.White;
        Color hilightColor = Color.Black;
        Color currentColor;
        SpriteFont font;
        KeyboardState lastKeyState;
        int xOffset = 535;
        int yOffset = 215;

        public int selected;

        public StartMenu(SpriteFont _font)
        {
            font = _font;
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState currentKeyState = Keyboard.GetState();

            if (currentKeyState.IsKeyUp(Keys.Up) && lastKeyState.IsKeyDown(Keys.Up))
            {
                selected--;
                if (selected < 0)
                    selected = menuItems.Length - 1;
            }

            if (currentKeyState.IsKeyUp(Keys.Down) && lastKeyState.IsKeyDown(Keys.Down))
            {
                selected++;
                if (selected == menuItems.Length)
                    selected = 0;
            }

            lastKeyState = currentKeyState;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < menuItems.Length; i++)
            {
                double width = font.MeasureString(menuItems[i]).X;
                double height = font.MeasureString(menuItems[i]).Y;
                if (i == selected)
                    currentColor = hilightColor;
                else currentColor = normalColor;

                Vector2 location = new Vector2((float)xOffset, (float)(yOffset + i * height));
                spriteBatch.DrawString(font, menuItems[i], location, currentColor);
            }
        }



    }
}
