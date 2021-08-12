using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using EngineHelper;
using EngineHelper.EHStates;
using EngineHelper.EHGraphics;
using EngineHelper.EHUtility;

namespace EngineHelper.EHUtility
{
    public class EHGame : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        public EHGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        //Allows the game to perform any initialization it needs to before starting to run.
        //This is where it can query for any required services and load any non-graphic
        //related content.  Calling base.Initialize will enumerate through any components and initialize them as well.
        protected override void Initialize()
        {
            EHManagers.EHEngineManager.Initialize(this, this.graphics);

#if DEBUG
            //  Add FPS Counter Component
            Viewport v = GraphicsDevice.Viewport;
            Vector2 bottomRight = new Vector2(v.Width - 125, v.Height - 32);
            Components.Add(new EHFPSCounter(this, bottomRight));

#endif
            base.Initialize();
        }

        //LoadContent will be called once per game and is the place to load all of your content.
        protected override void LoadContent()
        {
            //Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        //UnloadContent will be called once per game and is the place to unload all content.
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        //Allows the game to run logic such as updating the world, checking for collisions, gathering input, and playing audio.
        protected override void Update(GameTime gameTime)
        {
            EHManagers.EHEngineManager.Update(gameTime);
            base.Update(gameTime);
        }

        //This is called when the game should draw itself.
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            EHManagers.EHEngineManager.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
