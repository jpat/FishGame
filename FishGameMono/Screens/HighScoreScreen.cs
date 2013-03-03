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
    class HighScoreScreen : GameScreen
    {
        public Dictionary<string, int> Scores;
        private KeyboardState lastKeyState;
        public static Texture2D highScoreBG;
        private int yOffset = 200;


        public HighScoreScreen()
        {
            currentState = State.HighScores;
            //Scores = GetHighScores("http://192.168.0.14/fishgame/sqlscores.php");
            Scores = new Dictionary<string, int>() 
            {
                {"Julz", 11101},
                {"Mandz", 5293},
                {"Poop", 9009}
           };

        }

        public override void Update(GameTime gameTime)
        {   
            KeyboardState currentKeyState = Keyboard.GetState();

            if (currentKeyState.IsKeyDown(Keys.X) && lastKeyState.IsKeyUp(Keys.X))
            {
                currentState = State.Menu;
            }
            lastKeyState = currentKeyState;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(highScoreBG, Game1.screenRect, Color.White);
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
