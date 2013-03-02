﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FishGameMono
{
    abstract class GameObject
    {
        public abstract void Draw(GameTime gameTime, SpriteBatch sb);
        public abstract void Update(GameTime gameTime);
    }
}
