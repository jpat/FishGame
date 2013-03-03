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

        public enum FishType { jelly, pointy, plain, crab, boot };
        public static Dictionary<FishType, Sprite> sprites;

        public Rectangle rect;

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

            rect = new Rectangle((int)location.X, (int)location.Y, text.Width, text.Height);

        }

        public override void Update(GameTime gameTime)
        {
            rect = new Rectangle((int)location.X, (int)location.Y, rect.Width, rect.Height);
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
