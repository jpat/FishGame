using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishGameMono
{
    class MenuScreen : GameScreen
    {
        private int selected;
        //public State currentState;
        public static Texture2D menuBG;

        string[] menuItems = { "Play", "View High Scores", "Quit" };

        Color normalColor = Color.White;
        Color hilightColor = Color.Black;
        Color currentColor;
        SpriteFont font;
        KeyboardState lastKeyState;
        int xOffset = 535;
        int yOffset = 215;

        public MenuScreen(SpriteFont _font)
        {
            font = _font;
        }

        public override void Update(GameTime gameTime)
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

            if (currentKeyState.IsKeyDown(Keys.Enter))
            {
                switch (selected)
                {
                    case (0):
                        currentState = State.InGame;
                        break;
                    case (1):
                        currentState = State.HighScores;
                        break;
                    case (2):
                        currentState = State.Exiting;
                        break;
                }
            }

            lastKeyState = currentKeyState;
        }

        public override void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Draw(menuBG, Game1.screenRect, Color.White);

            for (int i = 0; i < menuItems.Length; i++)
            {
                double width = font.MeasureString(menuItems[i]).X;
                double height = font.MeasureString(menuItems[i]).Y;
                if (i == selected)
                    currentColor = hilightColor;
                else currentColor = normalColor;

                Vector2 location = new Vector2((float)xOffset, (float)(yOffset + i * height));
                sb.DrawString(font, menuItems[i], location, currentColor);
            }
        }

    }
}
