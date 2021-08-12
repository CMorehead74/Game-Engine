using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Media;

using EngineHelper;
using EngineHelper.EHGraphics;
using EngineHelper.EHInputs;
using EngineHelper.EHStates;
using EngineHelper.EHUtility;
using EngineHelper.EHObjects;

namespace EngineHelper.EHStates
{
    class EHMainMenuState : EHBaseGameState
    {
        // Gamepad states used to determine button presses
        GamePadState currentGamePadState;
        GamePadState previousGamePadState;

        //Message / Object Manager
        private EHManagers.EHObjectManager objectManager;
        private EHManagers.EHMessageManager msgManager;

        //Player control
        public PlayerIndex player;

        //EHPLAYER playerObj;
        EHStates.EHGameState reset;             //CJM

        //Graphics
        Texture2D MainMenuBackground;
        private Viewport viewPort;
        //GraphicsDeviceManager graphics;
       //SpriteBatch spriteBatch;
        List<String> menuOptions = new List<String>();
        int selectedMenuOption = 1;
        SpriteFont spriteFont;
        Rectangle wholescreen;
        Vector2 screenPositionFont;


        //Buttons
        Texture2D playSelectedButton;
        Texture2D howToSelectedButton;
        Texture2D creditsSelectedButton;
        Texture2D playUnSelectedButton;
        Texture2D howToUnSelectedButton;
        Texture2D creditsUnSelectedButton;
        Texture2D exitSelectedButton;
        Texture2D exitUnSelectedButton;

        //Button Positions
        Vector2 screenPositionPlay;
        Vector2 screenPositionHowTo;
        Vector2 screenPositionCredits;
        Vector2 screenPositionExit;

        //Button Size
        Rectangle ButtonSize;

        //Input Variables
        //bool howToPlay;
        //bool credits;
        int myFrame;
        float myFrameTimer;

        //Sound Variables
        SoundEffect selectButton;
        SoundEffect selected;
        SoundEffect store;

        public EHMainMenuState()
        {
            player = (PlayerIndex)0;
            objectManager = new EHManagers.EHObjectManager();
            msgManager = new EHManagers.EHMessageManager();

            screenPositionPlay = new Vector2(480.0f, 260.0f);
            screenPositionHowTo = new Vector2(480.0f, 390.0f);
            screenPositionCredits = new Vector2(670.0f, 260.0f);
            screenPositionExit = new Vector2(670.0f, 390.0f);

            screenPositionFont = new Vector2(10.0f, 10.0f);

            reset = new EHStates.EHGameState();      
            wholescreen = new Rectangle(0, 0, 1280, 720);
            ButtonSize = new Rectangle(0, 0, 128, 128);
            //howToPlay = false;
            //credits = false;
            myFrame = 0;
            myFrameTimer = 0.0f;
        }

        public override void LoadContent()
        {
            //Background and title (fonts)
            spriteFont = EHManagers.EHEngineManager.Game.Content.Load<SpriteFont>(@"Fonts/SpriteFont1");
            MainMenuBackground = EHManagers.EHEngineManager.Game.Content.Load<Texture2D>(@"Graphics/MainMenuBackground");
                                

            //Buttons
            playSelectedButton = EHManagers.EHEngineManager.Game.Content.Load<Texture2D>(@"Graphics/playSelectedButton");
            howToSelectedButton = EHManagers.EHEngineManager.Game.Content.Load<Texture2D>(@"Graphics/howToSelectedButton");
            creditsSelectedButton = EHManagers.EHEngineManager.Game.Content.Load<Texture2D>(@"Graphics/creditsSelectedButton");
            playUnSelectedButton = EHManagers.EHEngineManager.Game.Content.Load<Texture2D>(@"Graphics/playUnSelectedButton");
            howToUnSelectedButton = EHManagers.EHEngineManager.Game.Content.Load<Texture2D>(@"Graphics/howToUnSelectedButton");
            creditsUnSelectedButton = EHManagers.EHEngineManager.Game.Content.Load<Texture2D>(@"Graphics/creditsUnSelectedButton");
            exitSelectedButton = EHManagers.EHEngineManager.Game.Content.Load<Texture2D>(@"Graphics/exitSelectedButton");
            exitUnSelectedButton = EHManagers.EHEngineManager.Game.Content.Load<Texture2D>(@"Graphics/exitUnSelectedButton"); 

            //Sound Stuff ****************************************
            selectButton = EHManagers.EHEngineManager.Game.Content.Load<SoundEffect>(@"Sounds/SFX_SELECTHIGHLIGHT");
            selected = EHManagers.EHEngineManager.Game.Content.Load<SoundEffect>(@"Sounds/SFX_BUTTONSELECT");
            store = EHManagers.EHEngineManager.Game.Content.Load<SoundEffect>(@"Sounds/SFX_STORE");

            LoadPercentage = 100.0f;
        }

        public override void OnEnter(string previousState)
        {
            currentGamePadState = GamePad.GetState(player);
            viewPort = EHManagers.EHEngineManager.Graphics.ScreenViewport;
            menuOptions.Add("TEST");

#if XBOX
            if (Guide.IsTrialMode)
                trialMode = true;

            // get the highscore data/read the save file
            if (Global.SaveDevice.FileExists(Global.containerName, Global.fileName_Levels))
            {
                Global.SaveDevice.Load(
                    Global.containerName,
                    Global.fileName_Levels,
                    stream =>
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            highscoreTime = int.Parse(reader.ReadLine());
                            onLevel = int.Parse(reader.ReadLine());
                        }
                    });
            }
#endif
        }

        public override void OnExit(string nextState)
        {
            reset.Initialize();
        }

        public override void Update()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                EHManagers.EHEngineManager.Game.Exit();

            objectManager.UpdateObjects((float)EHManagers.EHEngineManager.Time.GameTime.ElapsedGameTime.TotalSeconds);

            myFrameTimer += (float)EHManagers.EHEngineManager.Time.GameTime.ElapsedGameTime.TotalSeconds;

            if (myFrameTimer >= 0.1f)
            {
                myFrameTimer = 0.0f;
                myFrame++;
                if (myFrame >= 4)
                {
                    myFrame = 0;
                }
            }
        }

        public override void Draw()
        {
            EHManagers.EHEngineManager.Graphics.SpriteBatch.Begin();

            EHManagers.EHEngineManager.Graphics.SpriteBatch.Draw(MainMenuBackground, wholescreen, Color.White);
            EHManagers.EHEngineManager.Graphics.SpriteBatch.DrawString(spriteFont, @"MainMenuState Prototype", screenPositionFont, Color.White);

            EHManagers.EHEngineManager.Graphics.SpriteBatch.Draw(playUnSelectedButton, screenPositionPlay, ButtonSize, Color.White);
            EHManagers.EHEngineManager.Graphics.SpriteBatch.Draw(howToUnSelectedButton, screenPositionHowTo, ButtonSize, Color.White);
            EHManagers.EHEngineManager.Graphics.SpriteBatch.Draw(creditsUnSelectedButton, screenPositionCredits, ButtonSize, Color.White);
            EHManagers.EHEngineManager.Graphics.SpriteBatch.Draw(exitUnSelectedButton, screenPositionExit, ButtonSize, Color.White);


            if (selectedMenuOption == 1)
                EHManagers.EHEngineManager.Graphics.SpriteBatch.Draw(playSelectedButton, screenPositionPlay, ButtonSize, Color.White);

            else if (selectedMenuOption == 2)
                EHManagers.EHEngineManager.Graphics.SpriteBatch.Draw(howToSelectedButton, screenPositionHowTo, ButtonSize, Color.White);

            else if (selectedMenuOption == 3)
               EHManagers.EHEngineManager.Graphics.SpriteBatch.Draw(creditsSelectedButton, screenPositionCredits, ButtonSize, Color.White);

            else if (selectedMenuOption == 4)
               EHManagers.EHEngineManager.Graphics.SpriteBatch.Draw(exitSelectedButton, screenPositionExit, ButtonSize, Color.White);

            EHManagers.EHEngineManager.Graphics.SpriteBatch.End();
        }

        public override void Input()
        {
            previousGamePadState = currentGamePadState;
            currentGamePadState = GamePad.GetState(player);

            //if (howToPlay)
            //{
            //    if (EHManagers.EHEngineManager.Input.Keyboard.KeyPressed(Keys.Enter) || (currentGamePadState.Buttons.A  == ButtonState.Pressed && previousGamePadState.Buttons.A != ButtonState.Pressed))
            //    {
            //        howToPlay = false;
            //    }
            //    return;
            //}
            //if (credits)
            //{
            //    if (EHManagers.EHEngineManager.Input.Keyboard.KeyPressed(Keys.Enter) || (currentGamePadState.Buttons.A == ButtonState.Pressed && previousGamePadState.Buttons.A != ButtonState.Pressed))
            //    {
            //        credits = false;
            //    }
            //    return;
            //}

            //SOUND & WRAP

            /*
             * 1    3
             * 2    4
             * 
             * if 1, press down - += 1
             * if 2, press down - 2 = 2
             * if 3, press down - += 1
             * if 4, press down - 4 = 4
             * 
             * if 1, press right - += 2
             * if 2, press right - += 2
             * if 3 || 4, press right - 3 = 3, 4 = 4
             * 
             * if 1 || 2, press left - 1 = 1, 2=2
             * if 3, press left - -=2
             * if 4, press left - -=2
             * */


            //MOVE RIGHT
            if (EHManagers.EHEngineManager.Input.Keyboard.KeyPressed(Keys.Right) || (previousGamePadState.IsButtonUp(Buttons.LeftThumbstickRight) && currentGamePadState.IsButtonDown(Buttons.LeftThumbstickRight)))
            {
                if (selectedMenuOption <= 2)
                {
                    selectButton.Play(0.8f, 0.0f, 0.0f);
                    selectedMenuOption += 2;
                }
            }
            //MOVE LEFT
            if (EHManagers.EHEngineManager.Input.Keyboard.KeyPressed(Keys.Left) || (previousGamePadState.IsButtonUp(Buttons.LeftThumbstickLeft) && currentGamePadState.IsButtonDown(Buttons.LeftThumbstickLeft)))
            {
                if (selectedMenuOption >= 3)
                {
                    selectButton.Play(0.8f, 0.0f, 0.0f);
                    selectedMenuOption -= 2;
                }
            }
            //WRAP FROM BOTTOM to TOP
            if (EHManagers.EHEngineManager.Input.Keyboard.KeyPressed(Keys.Down) || (previousGamePadState.IsButtonUp(Buttons.LeftThumbstickDown) && currentGamePadState.IsButtonDown(Buttons.LeftThumbstickDown)))
            {
                if (selectedMenuOption == 2)
                {
                    selectedMenuOption = 0;
                }
                else if (selectedMenuOption == 4)  
                {
                    selectedMenuOption = 2;
                }
                selectButton.Play(0.8f, 0.0f, 0.0f);
                selectedMenuOption += 1;
            }
            //WRAP FROM TOP to BOTTOM
            if (EHManagers.EHEngineManager.Input.Keyboard.KeyPressed(Keys.Up) || (previousGamePadState.IsButtonUp(Buttons.LeftThumbstickUp) && currentGamePadState.IsButtonDown(Buttons.LeftThumbstickUp)))
            {
                if (selectedMenuOption == 1)
                {
                    selectedMenuOption = 3;
                }
                else if (selectedMenuOption == 3)
                {
                    selectedMenuOption = 5;
                }
                selectButton.Play(0.8f, 0.0f, 0.0f);
                selectedMenuOption -= 1;
            }

            //Selected options and their actions
            if (EHManagers.EHEngineManager.Input.Keyboard.KeyPressed(Keys.Escape) || (currentGamePadState.Buttons.A == ButtonState.Pressed && previousGamePadState.Buttons.A != ButtonState.Pressed))
            {
                EHManagers.EHEngineManager.Game.Exit();
            }

            if (selectedMenuOption == 1)
            {
                if (EHManagers.EHEngineManager.Input.Keyboard.KeyPressed(Keys.Enter) || (currentGamePadState.Buttons.A == ButtonState.Pressed && previousGamePadState.Buttons.A != ButtonState.Pressed))
                {
                    EHManagers.EHEngineManager.State.ChangeState("IntroState");
                    selected.Play(0.8f, 0.0f, 0.0f);
                    return;
                }
            }
            if (selectedMenuOption == 2)
            {
                if (EHManagers.EHEngineManager.Input.Keyboard.KeyPressed(Keys.Enter) || (currentGamePadState.Buttons.A == ButtonState.Pressed && previousGamePadState.Buttons.A != ButtonState.Pressed))
                {
                    //credits = true;
                    EHManagers.EHEngineManager.State.ChangeState("IntroState");
                    selected.Play(0.8f, 0.0f, 0.0f);
                    return;
                }
            }
            if (selectedMenuOption == 3)
            {
                if (EHManagers.EHEngineManager.Input.Keyboard.KeyPressed(Keys.Enter) || (currentGamePadState.Buttons.A == ButtonState.Pressed && previousGamePadState.Buttons.A != ButtonState.Pressed))
                {
                    //howToPlay = true;
                    EHManagers.EHEngineManager.State.ChangeState("IntroState");
                    selected.Play(0.8f, 0.0f, 0.0f);
                    return;
                }
            }
            if (selectedMenuOption == 4)
            {
                if (EHManagers.EHEngineManager.Input.Keyboard.KeyPressed(Keys.Enter) || (currentGamePadState.Buttons.A == ButtonState.Pressed && previousGamePadState.Buttons.A != ButtonState.Pressed))
                {
                    EHManagers.EHEngineManager.Game.Exit();
                    selected.Play(0.8f, 0.0f, 0.0f);
                    return;
                }
            }

        }   
    }
}
