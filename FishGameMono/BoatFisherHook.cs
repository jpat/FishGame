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
    class BoatFisherHook : GameObject
    {
        public static Texture2D boatText, fisherText, hookText, lineText;

        private float rotation;
        private int rotDirection = 1;
        private int boatX = (int)(Game1.screenWidth * 0.5f);
        private int lineLength = 10;
        private Vector2 lineOffset = new Vector2(80, 100);
        private Vector2 hookOffset = new Vector2(67, 110);
        private Vector2 hookLoc; 
        float hookScale = 0.5f;

        bool isLowering = false;
        public bool isLineFull = false;
                
        private Vector2 center = new Vector2(boatText.Width / 2, boatText.Height / 2);

        public BoatFisherHook()
        {
            hookLoc = new Vector2((int)hookOffset.X + boatX, lineOffset.Y + lineLength);
            //hookrect, for now
            rect = new Rectangle((int)hookLoc.X, (int)hookLoc.Y, (int)(hookText.Width * hookScale), (int)(hookText.Height * hookScale));
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Left) && boatX >= 0)
            {
                boatX -= 3;
                hookLoc.X -= 3;
            }
            else if (keyState.IsKeyDown(Keys.Right) && boatX <= Game1.screenWidth)
            {
                boatX += 3;
                hookLoc.X += 3;
            }

            rect = new Rectangle((int)hookLoc.X, (int)hookLoc.Y, (int)(hookText.Width * hookScale), (int)(hookText.Height * hookScale));

            RotateBoat(gameTime);
            CheckLine();
            UpdateLine();
        }

        private void RotateBoat(GameTime gameTime)
        {
            rotation += (float)(rotDirection * Math.PI / 180);
            if ((rotation * Math.PI / 180) > 3.0f || (rotation * Math.PI / 180) < -3.0f)
            {
                rotDirection *= -1;
            }
        }

        private void CheckLine()
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Space))
                isLowering = true;
        }

        private void UpdateLine()
        {
            if (lineLength < (Game1.screenHeight - 2 * hookText.Height) && isLowering)
            {
                lineLength += 2;
                hookLoc.Y += 2;
            }

            if (lineLength >= (Game1.screenHeight - 2 * hookText.Height) || isLineFull)
                isLowering = false;

            if (lineLength > 10 && !isLowering)
            {
                lineLength -= 2;
                hookLoc.Y -= 2;
            }

            if (isLineFull && lineLength <= 10)
                isLineFull = false;

        }

        //rectangle collision
        

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(fisherText, new Vector2(boatX - 30, 100), null, Color.White, 0.05f * (float)Math.Cos(rotation), new Vector2(0, 0), 0.3f, SpriteEffects.None, 0);
            spriteBatch.Draw(boatText, new Vector2(boatX, 190), null, Color.White, 0.08f * (float)Math.Cos(rotation), center, 0.6f, SpriteEffects.None, 0);
            spriteBatch.Draw(lineText, new Vector2(boatX + lineOffset.X, lineOffset.Y), null, Color.White, 0.0f, new Vector2(0, 0), new Vector2(1, lineLength), SpriteEffects.None, 0);
            spriteBatch.Draw(hookText, new Vector2(hookLoc.X, hookLoc.Y), null, Color.White, 0.0f, new Vector2(0, 0), hookScale, SpriteEffects.None, 0);
            
        }
    }
}
