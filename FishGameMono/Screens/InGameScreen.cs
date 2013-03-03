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
            GameUpdate(gameTime);

            KeyboardState keyState = Keyboard.GetState();
            if (lastKeyState.IsKeyUp(Keys.K) && keyState.IsKeyDown(Keys.K))
            {
                currentState = State.GameOverName;
            }
        }

        public void GameUpdate(GameTime gameTime)
        {

            if (ShouldCreateNewFish())
            {
                fish.Add(Fish.CreateRandomFish());
            }

            foreach (Fish f in fish)
            {
                f.Update(gameTime);
                if (BoxCollision(f, boat))
                {
                    /*if (IntersectPixels(new Rectangle(hookX, hookY, (int)(hook.Width * hookScale), (int)(hook.Height * hookScale)), hookData,
                        new Rectangle((int)f.location.X, (int)f.location.Y, (int)(f.text.Width * f.scale), (int)(f.text.Height * f.scale)), f.data.colorData[0]))
                    {*/

                    boat.isLineFull = true;
                    f.velocity = new Vector2(0.0f, -2.0f);
                    f.isCaught = true;
                    f.rotation = (float)(90 * Math.PI / 180);

                    // }
                }

                if (f.AtBoat())
                {
                    switch (f.type)
                    {
                        case (Fish.FishType.Plain):
                            score += 100;
                            break;
                        case (Fish.FishType.Pointy):
                            score += 200;
                            break;
                        case (Fish.FishType.Jelly):
                            score += 500;
                            break;
                        case (Fish.FishType.Crab):
                            score += 1000;
                            break;
                        case (Fish.FishType.Boot):
                            score -= 500;
                            break;
                    }
                }
            }

            for (int i = fish.Count - 1; i >= 0; i--)
            {
                Fish f = fish.ElementAt(i);
                if (f.IsOffScreen())
                    fish.Remove(fish.ElementAt(i));
            }

            boat.Update(gameTime);
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
            spriteBatch.Draw(sky, new Vector2(0, -100), Color.White);
            spriteBatch.DrawString(Game1.roundedFont, "Score: " + score, new Vector2(10.0f, 25.0f), Color.DarkMagenta);

            spriteBatch.Draw(water, new Vector2(0, 200), Color.White);
            spriteBatch.Draw(coral, new Vector2(0, 200), new Color(200, 200, 200, 200));
            spriteBatch.Draw(sand, new Vector2(0, Game1.screenHeight - sand.Height), Color.White);

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
