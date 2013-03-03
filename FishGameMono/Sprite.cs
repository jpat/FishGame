using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishGameMono
{
    class Sprite
    {
        private Texture2D[] frames;

        public Sprite(Texture2D[] _frames)
        {
            frames = _frames;
        }

        public Texture2D GetCurrentFrame(GameTime gameTime)
        {
            int numMS = 1000 / frames.Length;
            int textIndex = (gameTime.TotalGameTime.Milliseconds / numMS) % (frames.Length);
            return frames[textIndex];
        }
    }
}
