using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;

using EasyStorage;

using EngineHelper;
using EngineHelper.EHGraphics;
using EngineHelper.EHStates;
using EngineHelper.EHObjects;
//using EngineHelper.EHInput;

namespace EngineHelper.EHStates
{
    class EHIntroState : EHBaseGameState
    {
        //Save variables
        IAsyncSaveDevice saveDevice;
        PlayerSaveDevice sharedSaveDevice;

        //Game states used to determine buttons
        GamePadState currentGamePadState;
        GamePadState previousGameState;

        //Player control
        public PlayerIndex player;

        //Message / Object Manager
        private EHManagers.EHObjectManager objectManager;
        private EHManagers.EHMessageManager msgManager;

        //Screen variables
        Vector2 screenPosition;
        SpriteFont spriteFont;
        private Viewport viewport;
        Rectangle wholeScreen;

        //Menu variable
        List<String> menuOptions = new List<String>();
        Texture2D IntroBackground;
        bool goToMainMenu;
        bool aWasPressed;

        public EHIntroState()
        {
            screenPosition = new Vector2(10.0f, 30.0f);
            objectManager = new EHManagers.EHObjectManager();
            msgManager = new EHManagers.EHMessageManager();
            wholeScreen = new Rectangle(0, 0, 1280, 720);
            goToMainMenu = false;
            aWasPressed = false;
        }

        public override void LoadContent()
        {
            spriteFont = EHManagers.EHEngineManager.Game.Content.Load<SpriteFont>(@"Fonts/SpriteFont1");
            IntroBackground = EHManagers.EHEngineManager.Game.Content.Load<Texture2D>(@"Graphics/IntroBackground");
            LoadPercentage = 100.0f;
        }

        public override void OnEnter(string previousState)
        {
            //This keeps it from flickering
            currentGamePadState = GamePad.GetState(player);
            viewport = EHManagers.EHEngineManager.Graphics.ScreenViewport;
            menuOptions.Add("BLANK");
        }

        public override void OnExit(string nextState)
        {
            //removed needed objects
        }

        public override void Update()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                EHManagers.EHEngineManager.Game.Exit();
            objectManager.UpdateObjects((float)EHManagers.EHEngineManager.Time.GameTime.ElapsedGameTime.TotalSeconds);
        }

        public override void Draw()
        {
            //Start drawing
            EHManagers.EHEngineManager.Graphics.SpriteBatch.Begin();

            EHManagers.EHEngineManager.Graphics.SpriteBatch.Draw(IntroBackground, wholeScreen, Color.White);
            EHManagers.EHEngineManager.Graphics.SpriteBatch.DrawString(spriteFont, @"IntroState Prototype", screenPosition, Color.White);

            //Stop drawing
            EHManagers.EHEngineManager.Graphics.SpriteBatch.End();
        }

        public override void Input()
        {
            if (EHManagers.EHEngineManager.Input.Keyboard.KeyPressed(Keys.Enter))
            {
                EHManagers.EHEngineManager.State.ChangeState(@"MainMenuState");
                return;
            }

            if (goToMainMenu)
            {
                EHManagers.EHEngineManager.State.ChangeState(@"MainMenuState");
                return;
            }

            previousGameState = currentGamePadState;
            currentGamePadState = GamePad.GetState(player);

            if (currentGamePadState.Buttons.A == ButtonState.Pressed)
            {
                aWasPressed = true;

                for (int i = 0; i < 4; i++)
                {
                    if (GamePad.GetState((PlayerIndex)i).Buttons.A == ButtonState.Pressed)
                    {
                        player = (PlayerIndex)i;
                    }
                }

                //create and add our SaveDevice
                sharedSaveDevice = new PlayerSaveDevice(player);

                //make sure we hold on to the device
                saveDevice = sharedSaveDevice;
            }

            if (aWasPressed)
            {
                // hook two event handlers to force the user to choose a new device if they cancel the device selector or if they disconnect the storage device after selecting it
                sharedSaveDevice.DeviceSelectorCanceled +=
                    (s, e) => e.Response = SaveDeviceEventResponse.Force;
                sharedSaveDevice.DeviceDisconnected +=
                    (s, e) => e.Response = SaveDeviceEventResponse.Force;

                //prompt for a device on the first Update we can
                sharedSaveDevice.PromptForDevice();
                sharedSaveDevice.Update(EHManagers.EHEngineManager.Time.GameTime);

                sharedSaveDevice.DeviceSelected += (s, e) =>
                {
                    //Save our save device to the global counterpart, so we can access it anywhere we want to save/load
                    EHUtility.EHGlobal.SaveDevice = (SaveDevice)s;

                    //Once they select a storage device, we can load the main menu.
                    //You'll notice I hard coded PlayerIndex.One here. You'll need to
                    //change that if you plan on releasing your game. I linked to an
                    //example on how to do that but here's the link if you need it.
                    //<a href="http://blog.nickgravelyn.com/2009/03/basic-handling-of-multiple-controllers/">http://blog.nickgravelyn.com/2009/03/basic-handling-of-multiple-controllers/</a>
                    //We need to perform a check to see if we're on the Press Start Screen.
                    //If a storage device is selected NOT from this page, we don't want to
                    //create a new Main Menu screen! (Thanks @FreelanceGames for the mention)
                    //if (this.IsActive)
                    //   ScreenManager.AddScreen(new MainMenuScreen(), PlayerIndex.One);
                    goToMainMenu = true;
                };

                //#if XBOX
                // add the GamerServicesComponent
                //EHManagers.EHEngineManager.Game.Components.Add(new Microsoft.Xna.Framework.GamerServices.GamerServicesComponent(ScreenManager.Game));
                //#endif
            }
            if (goToMainMenu)
            {
                EHManagers.EHEngineManager.State.ChangeState(@"MainMenuState");
                return;
            }

        }
    }
}
