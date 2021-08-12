using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using EngineHelper;

namespace EngineHelper
{
    //This is the main type for your game
    public class Game1 : EngineHelper.EHUtility.EHGame
    {
        //Allows the game to perform any initialization it needs to before starting to run.
        //This is where it can query for any required services and load any non-graphic
        //related content.  Calling base.Initialize will enumerate through any components
        //and initialize them as well.
        protected override void Initialize()
        {
            this.graphics.PreferredBackBufferWidth = 1280;
            this.graphics.PreferredBackBufferHeight = 720;
#if DEBUG
            //graphics.IsFullScreen = false;
#else 
            //graphics.IsFullScreen = true;
#endif

            this.graphics.ApplyChanges();

#if XBOX
            base.Components.Add(new Microsoft.Xna.Framework.GamerServices.GamerServicesComponent(this));
#endif

            base.Initialize();
        }

        //LoadContent will be called once per game and is the place to load all of your content.
        protected override void LoadContent()
        {
            base.LoadContent();
            EHManagers.EHEngineManager.State.Initialize(this.Content, new EHUtility.EHLoadScreen());
            EHManagers.EHEngineManager.State.RegisterState("IntroState", new EHStates.EHIntroState());
            EHManagers.EHEngineManager.State.RegisterState("MainMenuState", new EHStates.EHMainMenuState());
        }
    }
}
