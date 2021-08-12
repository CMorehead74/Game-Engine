using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EngineHelper;
using EngineHelper.EHGraphics;
using EngineHelper.EHStates;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Threading;

namespace EngineHelper.EHUtility
{
    class EHLoadScreen : EHManagers.EHILoadingScreen
    {
        Texture2D image;
        SpriteFont font;

        public EHLoadScreen() { }

        public void Update() { }

        public void Draw(float loadPercentage)
        {
            EHManagers.EHEngineManager.Graphics.Device.Clear(Color.Black);
            Thread.Sleep(15);
        }
    }
}
