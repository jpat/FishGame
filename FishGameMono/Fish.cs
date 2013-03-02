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
        public enum FishType { jelly, pointy, plain, crab, boot };
        public static Dictionary<FishType, Texture2D[]> textures;

        public Rectangle rect;

        public FishType type;

        public Vector2 location;
        public Vector2 velocity;

        public bool isCaught;

        public Texture2D text;

        public float scale;
        public float rotation = 0.0f;

        public FishData data;

        public Fish(Vector2 loc, Vector2 vel, FishType f, float s)
        {
            this.location = loc;
            this.velocity = vel;
            this.scale = s;
            this.isCaught = false;
            this.type = f;
            this.text = textures[f][0];

            rect = new Rectangle((int)location.X, (int)location.Y, text.Width, text.Height);

            data = new FishData(textures[f]);

        }

        public override void Update(GameTime gameTime)
        {
            rect = new Rectangle((int)location.X, (int)location.Y, text.Width, text.Height);
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
            Texture2D currentTexture;
            if (textures[type].Count() == 2)
            {
                if (gameTime.TotalGameTime.Milliseconds % 1000 < 500)
                    currentTexture = textures[type][0];
                else
                    currentTexture = textures[type][1];
            }

            else if (textures[type].Count() == 3)
            {
                if (gameTime.TotalGameTime.Milliseconds % 1000 < 332) //332
                    currentTexture = textures[type][0];
                else if (gameTime.TotalGameTime.Milliseconds % 1000 < 665) //665
                    currentTexture = textures[type][1];
                else
                    currentTexture = textures[type][2];
            }

            else currentTexture = text;

            text = currentTexture;
            sb.Draw(currentTexture, location, null, Color.White, this.rotation, new Vector2(text.Width * 0.5f, text.Height * 0.5f), scale, SpriteEffects.None, 0);
        }
    }


}
