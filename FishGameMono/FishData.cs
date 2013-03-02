using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FishingGame
{
    class FishData
    {
        public Texture2D[] textures { get; set; }
        
        public Color[][] colorData; //List<Color[]>

        public FishData(Texture2D[] texts)
        {
            textures = texts;
            colorData = this.PopulateData();
        }

        public Color[][] PopulateData()
        {
            Color[][] allData = new Color[textures.Length][];
            for (int i = 0; i < textures.Length; i++)
            {
                Color[] cols = new Color[textures[i].Width * textures[i].Height];
                textures[i].GetData(cols);
                allData[i] = cols;
            }
            return allData;
        }
    }
}
