using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using EngineHelper;

namespace EngineHelper.EHGraphics
{
    class PlayUnSelectedButton
    {
        Texture2D button;
        Vector2 position;

        public void Initialize()
        {
            button = EngineHelper.EHManagers.EHEngineManager.Game.Content.Load<Texture2D>(@"Graphics/playUnSelectedButton");
            position.X = 50;
            position.Y = 300;
        }

        public void Update()
        {

        }

        public void Draw()
        {
            EngineHelper.EHManagers.EHEngineManager.Graphics.SpriteBatch.Draw(button, position, Color.White);
        }
    }
}
