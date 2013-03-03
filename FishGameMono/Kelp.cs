using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishGameMono
{
    class Kelp : GameObject
    {
        public enum KelpType { Multi, Single };
        public static Dictionary<KelpType, Sprite> sprites;

        public KelpType type;
        public Vector2 location;
        public float alpha;
        public float scale;
        

        public Kelp(KelpType _type, Vector2 _loc, float _alpha, float _scale)
        {
            type = _type;
            location = _loc;
            alpha = _alpha;
            scale = _scale;
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var texture = sprites[this.type].GetCurrentFrame(gameTime);
            spriteBatch.Draw(texture, location, null, new Color(1.0f, 1.0f, 1.0f, alpha), 0.0f, new Vector2(0, 0), scale, SpriteEffects.None, 0);
        }
    }
}
