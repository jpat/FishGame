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
    class InGameScreen : GameScreen
    {
        private KeyboardState lastKeyState;

        private List<Fish> fish;
        private List<Kelp> kelp;
        private BoatFisherHook boat;

        private int score;

        public static Texture2D sky, sand, coral, water;

        public InGameScreen()
        {
            currentState = State.InGame;
            fish = new List<Fish>();
            kelp = new List<Kelp>
            {
                new Kelp(Kelp.KelpType.Multi, new Vector2(570, 380), 0.8f, 0.5f),
                new Kelp(Kelp.KelpType.Single, new Vector2(150, 380), 0.8f, 0.5f),
                new Kelp(Kelp.KelpType.Single, new Vector2(160, 390), 0.8f, 0.5f),
                new Kelp(Kelp.KelpType.Single, new Vector2(140, 420), 0.8f, 0.5f),
            };
            boat = new BoatFisherHook();
        }
        
        public override void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            if (lastKeyState.IsKeyUp(Keys.K) && keyState.IsKeyDown(Keys.K))
            {
                currentState = State.GameOverName;
            }

            if (ShouldCreateNewFish())
            {
                fish.Add(Fish.CreateRandomFish());
            }

            UpdateFish(gameTime);

            boat.Update(gameTime);
        }

        private void UpdateFish(GameTime gameTime)
        {
            foreach (Fish f in fish)
            {
                f.Update(gameTime);

                if (BoxCollision(f, boat))
                {
                    boat.isLineFull = true;
                    f.velocity = new Vector2(0.0f, -2.0f);
                    f.isCaught = true;
                    f.rotation = (float)(90 * Math.PI / 180);
                }

                if (f.AtBoat())
                {
                    score += f.score;  
                }
            }

            // have to use indexing; can't modify collection within foreach
            for (int i = fish.Count - 1; i >= 0; i--)
            {
                Fish f = fish.ElementAt(i);
                 if (f.IsOffScreen())
                 {
                     fish.Remove(fish.ElementAt(i));
                }
            }
        }

        private bool ShouldCreateNewFish()
        {
            Random r = new Random();
            return r.Next(0, 1000) % 499 == 0;
        }

        public static bool BoxCollision(GameObject obj1, GameObject obj2)
        {
            if (obj1.rect.Intersects(obj2.rect))
            {
                return true;
            }
            else return false;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(sky, new Vector2(0, -100), Color.White);
            spriteBatch.Draw(sky, new Rectangle(0, 0, (int)Game1.screenWidth, (int)(Game1.screenHeight * 0.3f)), Color.White);
            spriteBatch.DrawString(Game1.roundedFont, "Score: " + score, new Vector2(10.0f, 25.0f), Color.DarkMagenta);

            //spriteBatch.Draw(water, new Vector2(0, 200), Color.White);
            spriteBatch.Draw(water, new Rectangle(0, (int)(Game1.screenHeight * 0.3f), Game1.screenWidth, Game1.screenHeight), Color.White);

            //spriteBatch.Draw(coral, new Vector2(0, 200), new Color(200, 200, 200, 200));
            spriteBatch.Draw(coral, new Rectangle(0, (int)(Game1.screenHeight * 0.5f), Game1.screenWidth, (int)(Game1.screenHeight * 0.5f)), Color.White);
            
            //spriteBatch.Draw(sand, new Vector2(0, Game1.screenHeight - sand.Height), Color.White);

            boat.Draw(gameTime, spriteBatch);

            foreach (Kelp k in kelp)
            {
                k.Draw(gameTime, spriteBatch);
            }

            foreach (Fish f in fish)
            {
                f.Draw(gameTime, spriteBatch);
            }
                    
        }

    }
}
