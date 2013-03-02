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

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static int screenWidth = 800;
        public static int screenHeight = 600;

        public static int score;

        enum State { Menu, InGame, Pause, HighScore, GameOver, Name };
        State currentState;

        StartMenu startMenu;
        HighScoreMenu highScoreMenu;

        public static SpriteFont font, roundedFont;

        Texture2D menubg, hsbg, gameoverbg;

        Texture2D fisher, boat, hook;
        Texture2D sky, water, sand, coral;
        Texture2D dot, pixel;

        public Texture2D jelly1, jelly2, jelly3;
        public Texture2D plain1, plain2;
        public Texture2D pointy1, pointy2;
        public Texture2D boot1;
        public Texture2D crab1, crab2, crab3;

        public Texture2D[] smallkelp, bigkelp;

        Color[] hookData;

        
        int boatX;
        int lineLength;
        Vector2 lineOffset, hookOffset;
        bool isLowering, lineFull;

        int hookX, hookY;
        float hookScale;

        public float boatRotation = 0.0f;
        public float rotDirection = 1;

        List<Fish> fish;

        public KeyboardState lastKeyState;
        char[] name = { 'A', 'A', 'A' };
        int nameIndex = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.PreferredBackBufferWidth = screenWidth;
        }

        protected override void Initialize()
        {
            currentState = State.Menu;

            lineLength = 10;
            hookScale = 0.5f;
            boatX = (int)(screenWidth * 0.5);
            lineOffset = new Vector2(80, 100);

            //hookX = boatX + 67;
            hookOffset = new Vector2(67, 110);
            hookX = (int)hookOffset.X + boatX;
            hookY = (int)lineOffset.Y + lineLength;
            //hookY = 110 + lineLength;

            isLowering = false;
            lineFull = false;
            fish = new List<Fish>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("font");
            roundedFont = Content.Load<SpriteFont>("roundedFont");

            Fish.textures = new Dictionary<Fish.FishType, Texture2D[]>
            {
                {
                    Fish.FishType.jelly, new Texture2D[3]
                                                {
                                                    Content.Load<Texture2D>("jelly"),
                                                    Content.Load<Texture2D>("jelly2a"),
                                                    Content.Load<Texture2D>("jelly2b")
                                                }
                },
                {
                    Fish.FishType.plain, new Texture2D[2]
                                                {
                                                    Content.Load<Texture2D>("plainfish"),
                                                    Content.Load<Texture2D>("plainfish2b"),
                                                }
                },
                {
                    Fish.FishType.pointy, new Texture2D[2]
                                                {
                                                    Content.Load<Texture2D>("pointyfish"),
                                                    Content.Load<Texture2D>("pointyfish2"),
                                                }
                },
                {
                    Fish.FishType.boot, new Texture2D[1]
                                            {
                                                Content.Load<Texture2D>("boot")
                                            }
                },
                {
                    Fish.FishType.crab, new Texture2D[3]
                                                {
                                                    Content.Load<Texture2D>("crab"),
                                                    Content.Load<Texture2D>("crab2"),
                                                    Content.Load<Texture2D>("crab3"),
                                                }
                }
            };

            smallkelp = new Texture2D[4] { Content.Load<Texture2D>("kelp1"), 
                Content.Load<Texture2D>("kelp2"), 
                Content.Load <Texture2D>("kelp2b"), 
                Content.Load<Texture2D>("kelp3") 
            };

            bigkelp = new Texture2D[3] { Content.Load<Texture2D>("bigkelp1"), 
                Content.Load<Texture2D>("bigkelp2"), 
                Content.Load<Texture2D>("bigkelp3") 
            };

            fisher = Content.Load<Texture2D>("fisher");
            boat = Content.Load<Texture2D>("ship");
            sky = Content.Load<Texture2D>("sky");
            water = Content.Load<Texture2D>("water");
            sand = Content.Load<Texture2D>("sand2");
            coral = Content.Load<Texture2D>("coral");
            hook = Content.Load<Texture2D>("hook");

            dot = Content.Load<Texture2D>("dot");
            pixel = Content.Load<Texture2D>("pixel");

            hookData = new Color[hook.Width * hook.Height];
            hook.GetData(hookData);

            startMenu = new StartMenu(roundedFont);
            highScoreMenu = new HighScoreMenu();

            menubg = Content.Load<Texture2D>("menubg");
            hsbg = Content.Load<Texture2D>("scorefiller");
            gameoverbg = Content.Load<Texture2D>("gameover");

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

            switch(currentState){

                case(State.Menu):
                    startMenu.Update(gameTime);
                    if (keyState.IsKeyDown(Keys.Enter))
                    {
                        switch (startMenu.selected)
                        {
                            case(0):
                                currentState = State.InGame;
                                break;
                            case(1):
                                currentState = State.HighScore;
                                break;
                            case(2):
                                this.Exit();
                                break;
                        }
                    }
                    break;

                case(State.InGame):
                    GameUpdate(gameTime);
                    if (lastKeyState.IsKeyUp(Keys.K) && keyState.IsKeyDown(Keys.K))
                    {
                        currentState = State.Name;
                    }
                    break;

                case(State.Name):

                    if (keyState.IsKeyDown(Keys.Down) && lastKeyState.IsKeyUp(Keys.Down))
                    {
                        if (name[nameIndex] >= 'Z')
                        {
                            name[nameIndex] = 'A';
                        }
                        else
                        {
                            name[nameIndex]++;
                        }
                    }
                    if (keyState.IsKeyDown(Keys.Up) && lastKeyState.IsKeyUp(Keys.Up))
                    {
                        if (name[nameIndex] <= 'A')
                        {
                            name[nameIndex] = 'Z';
                        }
                        else
                        {
                            name[nameIndex]--;
                        }
                    }
                    if (keyState.IsKeyDown(Keys.Enter) && lastKeyState.IsKeyUp(Keys.Enter))
                    {
                        if (nameIndex < 2)
                        {
                            nameIndex++;
                        }
                        else
                        {
                            string finalName = new string(name);
                            //PostHighScore(finalName, score);
                            //highScoreMenu.GetHighScores("http://192.168.0.14/fishgame/sqlscores.php");
                            currentState = State.HighScore;
                        }
                    }
                    break;

                case(State.Pause):
                    
                    break;

                case(State.HighScore):
                    if (keyState.IsKeyDown(Keys.X) && lastKeyState.IsKeyUp(Keys.X))
                        currentState = State.Menu;
                    break;
                    
            }
            lastKeyState = keyState;

            base.Update(gameTime);
        }

        public void GameUpdate(GameTime gameTime)
        {
            boatRotation += (float)(rotDirection * Math.PI / 180);
            if ((boatRotation * Math.PI / 180) > 3.0f || (boatRotation * Math.PI / 180) < -3.0f)
                rotDirection *= -1;

            RandomFish(gameTime);

            UpdateBoat();
            CheckLine();
            UpdateLine();

            foreach (Fish f in fish)
            {
                f.Update();
                if (BoxCollision(f.location, f.text.Width, f.text.Height, f.scale))
                {
                    /*if (IntersectPixels(new Rectangle(hookX, hookY, (int)(hook.Width * hookScale), (int)(hook.Height * hookScale)), hookData,
                        new Rectangle((int)f.location.X, (int)f.location.Y, (int)(f.text.Width * f.scale), (int)(f.text.Height * f.scale)), f.data.colorData[0]))
                    {*/

                        lineFull = true;
                        f.velocity = new Vector2(0.0f, -2.0f);
                        f.isCaught = true;
                        f.rotation = (float)(90 * Math.PI / 180);
                        
                   // }
                }

                if (f.AtBoat())
                {
                    switch (f.type)
                    {
                        case (Fish.FishType.plain):
                            score += 100;
                            break;
                        case (Fish.FishType.pointy):
                            score += 200;
                            break;
                        case (Fish.FishType.jelly):
                            score += 500;
                            break;
                        case (Fish.FishType.crab):
                            score += 1000;
                            break;
                        case (Fish.FishType.boot):
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
        }

        public void UpdateBoat()
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Left))
            {
                boatX -= 3;
                hookX -= 3;
            }
            else if (keyState.IsKeyDown(Keys.Right))
            {
                boatX += 3;
                hookX += 3;
            }
        }

        public void CheckLine()
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Space))
                isLowering = true;
        }

        public void UpdateLine()
        {
            if (lineLength < (screenHeight - 2 * hook.Height) && isLowering)
            {
                lineLength += 2;
                hookY += 2;
            }

            if (lineLength >= (screenHeight - 2 * hook.Height) || lineFull)
                isLowering = false;

            if (lineLength > 10 && !isLowering)
            {
                lineLength -= 2;
                hookY -= 2;
            }

            if (lineFull && lineLength <= 10)
                lineFull = false;

        }

        //rectangle collision
        public bool BoxCollision(Vector2 vec, int width, int height, float scale2)
        {
            Rectangle hookRect = new Rectangle(hookX, hookY+20, (int)(hook.Width * hookScale), (int)(hook.Height * hookScale)-20);
            Rectangle otherRect = new Rectangle((int)vec.X+5, (int)vec.Y+5, (int)(width * scale2)-5, (int)(height * scale2)-5);

            if (hookRect.Intersects(otherRect))
            {
                return true;
            }

            else return false;
        }

        static bool IntersectPixels(Rectangle rectangleA, Color[] dataA,
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
        }

        protected override void Draw(GameTime gameTime)
        {
            
            Rectangle screenRect = new Rectangle(0, 0, screenWidth, screenHeight);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            switch(currentState){
                case(State.Menu):

                    spriteBatch.Draw(menubg, screenRect, Color.White);
                    startMenu.Draw(gameTime, spriteBatch);
                    break;

                case(State.InGame):

                    spriteBatch.Draw(sky, new Vector2(0, -100), Color.White);

                    spriteBatch.DrawString(roundedFont, "Score: " + score, new Vector2(10.0f, 25.0f), Color.DarkMagenta);

                    spriteBatch.Draw(water, new Vector2(0, 200), Color.White);
                    spriteBatch.Draw(coral, new Vector2(0, 200), new Color(200,200,200,200));
                    spriteBatch.Draw(sand, new Vector2(0, screenHeight - sand.Height), Color.White);

                    spriteBatch.Draw(fisher, new Vector2(boatX - 30, 100), null, Color.White, 0.05f * (float)Math.Cos(boatRotation), new Vector2(0, 0), 0.3f, SpriteEffects.None, 0);
                    spriteBatch.Draw(boat, new Vector2(boatX, 190), null, Color.White, 0.08f*(float)Math.Cos(boatRotation), new Vector2(boat.Width/2, boat.Height/2),0.6f,SpriteEffects.None, 0 );

                    spriteBatch.Draw(pixel, new Vector2(boatX + lineOffset.X, lineOffset.Y), null, Color.White, 0.0f, new Vector2(0, 0), new Vector2(1, lineLength), SpriteEffects.None, 0);
                    spriteBatch.Draw(hook, new Vector2(hookX, hookY), null, Color.White, 0.0f, new Vector2(0, 0), hookScale, SpriteEffects.None, 0);

                    DrawSeaweed(gameTime);

                    foreach (Fish f in fish)
                    {
                        f.Draw(gameTime, spriteBatch);
                    }
                    break;

                case(State.Name):
                    spriteBatch.Draw(gameoverbg, screenRect, Color.White);
                    string finalName = new string(name);
                    spriteBatch.DrawString(roundedFont, finalName, new Vector2(550, 200), Color.Black);
                    break;

                case(State.HighScore):
                    spriteBatch.Draw(hsbg, screenRect, Color.White);
                    highScoreMenu.Draw(spriteBatch);
                    break;

                case(State.Pause):
                    break;

                case(State.GameOver):
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void DrawSeaweed(GameTime gameTime)
        {
            DrawSeaweed_Helper(gameTime, bigkelp, new Vector2(570, 380), 0.8f, 0.5f);
            DrawSeaweed_Helper(gameTime, smallkelp, new Vector2(150, 380), 0.8f, 0.5f);
            DrawSeaweed_Helper(gameTime, smallkelp, new Vector2(160, 380), 0.8f, 0.5f);
            DrawSeaweed_Helper(gameTime, smallkelp, new Vector2(140, 420), 0.8f, 0.5f);  
        }

        public void DrawSeaweed_Helper(GameTime gameTime, Texture2D[] texts, Vector2 location, float alpha, float scale)
        {
            int numMS = 1000 / texts.Length;
            int textIndex = (gameTime.TotalGameTime.Milliseconds / numMS) % (texts.Length);
            
            spriteBatch.Draw(texts[textIndex], location, null, new Color(1.0f, 1.0f, 1.0f, alpha), 0.0f, new Vector2(0,0), scale, SpriteEffects.None, 0);
        }

        public void RandomFish(GameTime gameTime)
        {
            Random r = new Random();
            if (r.Next(0,1000) % 499 == 0)
            {
                Fish.FishType type;
                int randomType = r.Next(0,150);
                int yLoc;

                if (randomType < 20)
                    type = Fish.FishType.boot;
                else if (randomType < 60)
                    type = Fish.FishType.plain;
                else if (randomType < 80)
                    type = Fish.FishType.pointy;
                else if (randomType < 120)
                    type = Fish.FishType.jelly;
                else
                    type = Fish.FishType.crab;

                int randomY = r.Next(300,500);
                int vel;

                if (type != Fish.FishType.boot)
                    vel = r.Next(1, 4);
                else vel = 1;

                if (type != Fish.FishType.crab)
                    yLoc = randomY;

                else
                    yLoc = 550;

                fish.Add(new Fish(new Vector2(900, yLoc), new Vector2(-vel, 0), type, 0.5f));
            }
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
