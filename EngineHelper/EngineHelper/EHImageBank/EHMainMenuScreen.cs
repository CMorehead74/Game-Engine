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
    class EHMainMenuScreen
    {
        //Background
        Texture2D backgroundTexture;
        Vector2 backgroundPosition;

        public void Initialize()
        {
            backgroundPosition.X = 0;
            backgroundPosition.Y = 0;
            backgroundTexture = EHManagers.EHEngineManager.Game.Content.Load<Texture2D>(@"Graphics/MainMenuBackground");
        }

        public void Update()
        {

        }
        public void Draw()
        {
            EHManagers.EHEngineManager.Graphics.SpriteBatch.Draw(backgroundTexture, backgroundPosition, Color.White);
        }
    }
}
