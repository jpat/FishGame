using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Net;
using System.Text;

namespace FishGameMono
{
    public enum State { Menu, InGame, Pause, HighScores, GameOverName, Exiting }

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Matrix spriteScale;

        public static int screenWidth;// = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        public static int screenHeight;// = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        public static Rectangle screenRect;

        State currentState;

        public static SpriteFont font, roundedFont, bigRoundedFont;

        public KeyboardState lastKeyState;

        MenuScreen menu;
        GameOverNameScreen gameOver;
        HighScoreScreen highScores;
        InGameScreen inGameScreen;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            //screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            //screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1366;
            screenWidth = 1366;
            screenHeight = 768;
        }

        protected override void Initialize()
        {
            currentState = State.Menu;
            screenRect = new Rectangle(0, 0, screenWidth, screenHeight);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);


            /*float screenscale =
            (float)graphics.GraphicsDevice.Viewport.Width / 800f;
            // Create the scale transform for Draw. 
            // Do not scale the sprite depth (Z=1).
            spriteScale = Matrix.CreateScale(screenscale, screenscale, 1);*/


            font = Content.Load<SpriteFont>("font");
            roundedFont = Content.Load<SpriteFont>("roundedfont"); 
            bigRoundedFont = Content.Load<SpriteFont>("BigRoundFont");
            
            Fish.sprites = new Dictionary<Fish.FishType, Sprite>
            {
                {
                    Fish.FishType.Jelly, new Sprite(new[]
                                                {
                                                    Content.Load<Texture2D>("jelly"),
                                                    Content.Load<Texture2D>("jelly2a"),
                                                    Content.Load<Texture2D>("jelly2b")
                                                })
                },
                {
                    Fish.FishType.Plain, new Sprite(new[]
                                                {
                                                    Content.Load<Texture2D>("plainfish"),
                                                    Content.Load<Texture2D>("plainfish2b"),
                                                })
                },
                {
                    Fish.FishType.Pointy, new Sprite(new[]
                                                {
                                                    Content.Load<Texture2D>("pointyfish"),
                                                    Content.Load<Texture2D>("pointyfish2"),
                                                })
                },
                {
                    Fish.FishType.Boot, new Sprite(new[]
                                            {
                                                Content.Load<Texture2D>("boot")
                                            })
                },
                {
                    Fish.FishType.Crab, new Sprite(new[]
                                                {
                                                    Content.Load<Texture2D>("crab_lg"),
                                                    Content.Load<Texture2D>("crab2_lg"),
                                                    Content.Load<Texture2D>("crab3_lg"),
                                                })
                }
            };
            Kelp.sprites = new Dictionary<Kelp.KelpType, Sprite>
            {   
                {
                    Kelp.KelpType.Single, new Sprite(new[] 
                                                { 
                                                    Content.Load<Texture2D>("kelp1"), 
                                                    Content.Load<Texture2D>("kelp2"), 
                                                    Content.Load <Texture2D>("kelp2b"), 
                                                    Content.Load<Texture2D>("kelp3"), 
                                                })
                },

                {
                    Kelp.KelpType.Multi, new Sprite(new[]
                                                {
                                                    Content.Load<Texture2D>("bigkelp1_m"),
                                                    Content.Load<Texture2D>("bigkelp2_m"),
                                                    Content.Load<Texture2D>("bigkelp3_m"),
                                                })
                }
            };

            BoatFisherHook.boatText = Content.Load<Texture2D>("ship");
            BoatFisherHook.fisherText = Content.Load<Texture2D>("fisher_m");
            BoatFisherHook.hookText = Content.Load<Texture2D>("hook_m");
            BoatFisherHook.lineText = Content.Load<Texture2D>("pixel");
            
            InGameScreen.sky = Content.Load<Texture2D>("sky_lg");
            InGameScreen.water = Content.Load<Texture2D>("sea_lg");
            InGameScreen.sand = Content.Load<Texture2D>("sand1366");
            InGameScreen.coral = Content.Load<Texture2D>("coral2_lg");

            menu = new MenuScreen(bigRoundedFont);
            gameOver = new GameOverNameScreen();
            highScores = new HighScoreScreen();
            inGameScreen = new InGameScreen();

            MenuScreen.menuBG = Content.Load<Texture2D>("menubg_lg");
            GameOverNameScreen.gameOverBG = Content.Load<Texture2D>("gameover2");
            HighScoreScreen.highScoreBG = Content.Load<Texture2D>("highscore_lg");

        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            switch (currentState)
            {
                case(State.Menu):
                    menu.Update(gameTime);
                    currentState = menu.currentState;
                    break;

                case(State.InGame):
                    inGameScreen.Update(gameTime);
                    currentState = inGameScreen.currentState;
                    break;

                case(State.GameOverName):
                    gameOver.Update(gameTime);
                    currentState = gameOver.currentState;
                    break;

                case(State.Pause):
                    
                    break;

                case(State.HighScores):
                    highScores.Update(gameTime);
                    currentState = highScores.currentState;
                    break;

                case(State.Exiting):
                    this.Exit();
                    break;
                    
            }

            lastKeyState = keyState;

            base.Update(gameTime);
        }

        /*static bool IntersectPixels(Rectangle rectangleA, Color[] dataA,
                                    Rectangle rectangleB, Color[] dataB)
        {
            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point
                    Color colorA = dataA[(x - rectangleA.Left) +
                                         (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) +
                                         (y - rectangleB.Top) * rectangleB.Width];

                    // If both pixels are not completely transparent,
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        // then an intersection has been found
                        System.Diagnostics.Debug.WriteLine("daf");
                        return true;
                    }
                }
            }

            // No intersection found
            return false;
        }*/

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, spriteScale);

            switch(currentState){
                case(State.Menu):
                    menu.Draw(gameTime, spriteBatch);
                    break;

                case(State.InGame):
                    inGameScreen.Draw(gameTime, spriteBatch);
                    break;

                case(State.GameOverName):
                    gameOver.Draw(gameTime, spriteBatch);
                    break;

                case(State.HighScores):
                    highScores.Draw(gameTime, spriteBatch);
                    break;

                case(State.Pause):
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        

        /*private void PostHighScore(string name, int score)
        {
            string url = "http://192.168.0.14/fishgame/fishpost.php";
            using (WebClient wc = new WebClient())
            {
                System.Collections.Specialized.NameValueCollection values = new System.Collections.Specialized.NameValueCollection();
                values.Add("Name", name);
                values.Add("Score", score.ToString());
                var result = wc.UploadValues(url, values);
                var returnString = Encoding.ASCII.GetString(result);
            }
        }*/
    }
}
