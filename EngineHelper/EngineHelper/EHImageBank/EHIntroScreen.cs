using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using EngineHelper;

namespace EngineHelper.EHImageBank
{
    class EHIntroScreen
    {
        //the image respresenting the intro screen
        //Texture2D texture;

        //an array of positions fo the intro sccreen
        //Vector2 position;

        public void Initialize()
        {
            //Load the background texture
           // texture = EHManagers.EHEngineManager.Game.Content.Load<Texture2D>(@"Grpahics/testsize");        //This might be able to be deleted.. //CJM
           // position.X = 0;
           // position.Y = 0;
        }

        public void Update()
        {
            //No update needed for now, Might need update for the animation of the loading.
        }

        public void Draw()
        {
           // EHManagers.EHEngineManager.Graphics.SpriteBatch.Draw(texture, position, Color.White);
        }
    }
}
