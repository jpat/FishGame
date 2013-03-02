using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Net;
using System.Net.Http;
using System.IO;

namespace FishGameMono
{
    class HighScoreMenu
    {
        public Dictionary<string, int> Scores;
        KeyboardState lastKeyState;
        int yOffset = 200;

        public HighScoreMenu()
        {
            //Scores = GetHighScores("http://192.168.0.14/fishgame/sqlscores.php");
            Scores = new Dictionary<string, int>() 
            {
                {"Julz", 11101},
                {"Mandz", 5293},
                {"Poop", 9009}
           };

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int i = 0;
            foreach (var v in Scores)
            {
                string s = v.Key + " " + v.Value;
                Vector2 stringSize = Game1.roundedFont.MeasureString(s);

                spriteBatch.DrawString(Game1.roundedFont, v.Key + " " + v.Value, new Vector2(100, yOffset + i * stringSize.Y), Color.White);
                i++;
            }
        }

        /*public Dictionary<string, int> GetHighScores(string url)
        {
            WebClient client = new WebClient();
            string reply = client.DownloadString(url);
            var lines = reply.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var pairs = lines.Select(line => line.Split(' '));
            var scores = pairs.Select(pair => new KeyValuePair<string, int>(pair[0], int.Parse(pair[1])));

            var dict = new Dictionary<string, int>();
            
            foreach (var score in scores)
            {
                dict.Add(score.Key, score.Value);
            }

            return dict;
       }*/

        
      /*  public async void GetHighScores(object sender, EventArgs e)
        {
            string url = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
        }*/
    }
}
