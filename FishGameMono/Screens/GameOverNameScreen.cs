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
    class GameOverNameScreen : GameScreen
    {
        private KeyboardState lastKeyState;
        public static Texture2D gameOverBG;
        private char[] name = { 'A', 'A', 'A' };
        private int nameIndex = 0;
        
        public GameOverNameScreen()
        {
            currentState = State.GameOverName;
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState currentKeyState = Keyboard.GetState();

            if (currentKeyState.IsKeyDown(Keys.Up) && lastKeyState.IsKeyUp(Keys.Up))
            {
                if (name[nameIndex] >= 'Z'){
                    name[nameIndex] = 'A';
                }
                else{
                    name[nameIndex]++;
                }
            }
            if (currentKeyState.IsKeyDown(Keys.Down) && lastKeyState.IsKeyUp(Keys.Down))
            {
                if (name[nameIndex] <= 'A'){
                    name[nameIndex] = 'Z';
                }
                else{
                    name[nameIndex]--;
                }
            }
            if (currentKeyState.IsKeyDown(Keys.Enter) && lastKeyState.IsKeyUp(Keys.Enter))
            {
                if (nameIndex < 2){
                    nameIndex++;
                }
                else{
                    string finalName = new string(name);
                    //PostHighScore(finalName, score);
                    //highScoreMenu.GetHighScores("http://192.168.0.14/fishgame/sqlscores.php");
                    currentState = State.HighScores;
                }
            }

            lastKeyState = currentKeyState;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(gameOverBG, Game1.screenRect, Color.White);
            string finalName = new string(name);
            spriteBatch.DrawString(Game1.bigRoundedFont, finalName, new Vector2((int)(Game1.screenWidth * 0.70f), (int)(Game1.screenHeight * 0.35f)), Color.Black);
        }

        
    }
}
