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

        public FishType type;

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

            int randomY = r.Next(300,500);
            int vel;

            vel = (type != FishType.Boot) ? r.Next(1, 4) : 1;

            yLoc = (type != Fish.FishType.Crab) ? randomY : 550;

            return new Fish(new Vector2(900, yLoc), new Vector2(-vel, 0), type, 0.5f);
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
                AtBoat()//(location.Y < 120)
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
