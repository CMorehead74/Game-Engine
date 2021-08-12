using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using EngineHelper.EHAudio;
using EngineHelper.EHGraphics;
using EngineHelper.EHAnimations;
using EngineHelper.EHTimes;
using EngineHelper.EHInputs;
using EngineHelper.EHUtility;


namespace EngineHelper.EHManagers
{
    public static class EHEngineManager
    {
        static Game game = null;
        public static Game Game
        {
            get { return game; }
        }

        static EHRandom random = null;

        public static EHRandom Random
        {
            get { return random; }
        }

        static EHAudioManager audio = null;

        public static EHAudioManager Audio
        {
            get { return audio; }
        }

        static EHGraphic graphics = null;  

        public static EHGraphic Graphics
        {
            get { return graphics; }
        }

        static EHTime time = null;

        public static EHTime Time
        {
            get { return time; }
        }

        static EHMath math = null;

        public static EHMath Math
        {
            get { return math; }
        }

        static EHInputManager input = null;

        public static EHInputManager Input
        {
            get { return input; }
        }

        static EHGameStateManager gameStateManager = null;

        public static EHGameStateManager State
        {
            get { return gameStateManager; }
        }

        //static YHighScores highScores = null;

        //public static YHighScores HighScores
        //{
        //    get { return highScores; }
        //}

        //static YAnimationLoader animLoader = null;

        //public static YAnimationLoader Animation
        //{
        //    get { return animLoader; }
        //}

        public static bool Initialize(Game game, GraphicsDeviceManager graphicsDeviceManager)
        {
            EHEngineManager.game = game;

            audio = new EHAudioManager();
            audio.Initialize(game);

            graphics = new EHGraphic();
            graphics.Initialize(graphicsDeviceManager);

            time = new EHTime();

            random = new EHRandom();

            math = new EHMath();

            input = new EHInputManager();

            gameStateManager = new EHGameStateManager();

            //highScores = new YHighScores();
            //animLoader = YAnimationLoader.Instance;

            return true;
        }

        public static void Update(GameTime gameTime)
        {
            input.ReadDeviceStates();
            time.GameTime = gameTime;
            gameStateManager.Update(time.RealDeltaTime);
        }

        public static void Draw(GameTime gameTime)
        {
            time.GameTime = gameTime;
            gameStateManager.Draw();
        }
    }
}
