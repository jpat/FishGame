using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FishGameMono
{
    class Fish : GameObject
    {
        public int ScoreValue { get; set; }

        public enum FishType { Jelly, Pointy, Plain, Crab, Boot };
        public static Dictionary<FishType, Sprite> sprites;
        public static Dictionary<FishType, int> scores = new Dictionary<FishType,int> {
            { FishType.Plain, 100 }, 
            { FishType.Pointy, 200 },
            { FishType.Jelly, 500 },
            { FishType.Crab, 1000 },
            { FishType.Boot, -500 },
        };

        public FishType type;
        public int score;

        public Vector2 location;
        public Vector2 velocity;

        public bool isCaught;

        public float scale;
        public float rotation = 0.0f;

        public Fish(Vector2 loc, Vector2 vel, FishType f, float s)
        {
            this.location = loc;
            this.velocity = vel;
            this.scale = s;
            this.isCaught = false;
            this.type = f;
            this.score = scores[type];

            var text = sprites[f].GetCurrentFrame(new GameTime());

            rect = new Rectangle((int)location.X, (int)location.Y, (int)(text.Width * scale), (int)(text.Height * scale));
        }

        public static Fish CreateRandomFish()
        {
            Random r = new Random();
            Fish.FishType type;
            int randomType = r.Next(0,150);
            int yLoc;

            if (randomType < 20)
                type = Fish.FishType.Boot;
            else if (randomType < 60)
                type = Fish.FishType.Plain;
            else if (randomType < 80)
                type = Fish.FishType.Pointy;
            else if (randomType < 120)
                type = Fish.FishType.Jelly;
            else
                type = Fish.FishType.Crab;

            int randomY = r.Next((int)(Game1.screenHeight * 0.4f), (int)(Game1.screenHeight * 0.8f));
            int vel;

            vel = (type != FishType.Boot) ? r.Next(2, 6) : 2;

            yLoc = (type != Fish.FishType.Crab) ? randomY : (int)(Game1.screenHeight * 0.9f);

            return new Fish(new Vector2(Game1.screenWidth + 100, yLoc), new Vector2(-vel, 0), type, 0.5f);
        }

        public override void Update(GameTime gameTime)
        {
            rect = new Rectangle((int)location.X, (int)location.Y, (int)(rect.Width * scale), (int)(rect.Height * scale));
            location += velocity;

            if (this.isCaught == true)
            {
                KeyboardState keyState = Keyboard.GetState();
                if (keyState.IsKeyDown(Keys.Left))
                    location.X -= 3;
                else if (keyState.IsKeyDown(Keys.Right))
                    location.X += 3;
            }
        }

        public bool IsOffScreen()
        {
            if (
                (velocity.X < 0 && location.X < -100) ||
                    (velocity.X > 0 && location.X > Game1.screenWidth) ||
                AtBoat()
                )
                return true;
            else return false;
        }

        public bool AtBoat()
        {
            return location.Y <= 130;
        }

        public override void Draw(GameTime gameTime, SpriteBatch sb)
        {
            var texture = sprites[this.type].GetCurrentFrame(gameTime);
            sb.Draw(texture, location, null, Color.White, this.rotation, new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), scale, SpriteEffects.None, 0);
        }
    }


}
