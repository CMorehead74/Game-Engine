//Based on Shawn Hargreaves implementation at:
//http://blogs.msdn.com/b/shawnhar/archive/2007/06/08/displaying-the-framerate.aspx
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace EngineHelper.EHUtility
{
    public class EHFPSCounter : DrawableGameComponent
    {
        SpriteFont spriteFont;
        string[] numbers;

        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;

        Vector2 screenPosition;

        public EHFPSCounter(Game game, Vector2 screenPos) : base(game)
        {
            numbers = new String[10];
            for (int j = 0; j < 10; j++)
            {
                numbers[j] = j.ToString();
            }

            screenPosition = screenPos;// new Vector2(32, 32);
        }

        protected override void LoadContent()
        { 
            spriteFont = EHManagers.EHEngineManager.Game.Content.Load<SpriteFont>(@"Fonts/FPSFont");
        }

        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
        }

        //private Vector2 printOffset = Vector2.Zero;
        private int[] fpsDigits = new int[4];

        public override void Draw(GameTime gameTime)
        {
            frameCounter++;
            //Cap the framerate at 9999 fps
            if (frameRate > 9999)
            {
               frameRate = 9999;
            }

            //Thousands digit 
            fpsDigits[0] = frameRate / 1000;
            //Hundreds digit
            fpsDigits[1] = (frameRate - fpsDigits[0] * 1000) / 100;
            //Tens digit
            fpsDigits[2] = (frameRate - fpsDigits[0] * 1000 - fpsDigits[1] * 100) / 10;
            //Ones digit
            fpsDigits[3] = frameRate - fpsDigits[0] * 1000 - fpsDigits[1] * 100 - fpsDigits[2] * 10;

            EHManagers.EHEngineManager.Graphics.SpriteBatch.Begin();

            EHManagers.EHEngineManager.Graphics.SpriteBatch.DrawString(spriteFont, "fps: ", screenPosition, Color.White);
            Vector2 offset = spriteFont.MeasureString("fps: ");
            Vector2 printOffset = screenPosition;// Vector2.Zero;
            printOffset.X += offset.X;

            for (int i = 0; i < 4; i++)
            {
                DrawShadowedNumber(fpsDigits[i], printOffset);

                if (i < 3)
                    printOffset.X += spriteFont.MeasureString(numbers[fpsDigits[i + 1]]).X;              
            }
            EHManagers.EHEngineManager.Graphics.SpriteBatch.End();
        }

        private void DrawShadowedNumber(int number, Vector2 screenPos)
        {
            //Draw shadow
            EHManagers.EHEngineManager.Graphics.SpriteBatch.DrawString(spriteFont, numbers[number], screenPos + Vector2.One, Color.Black);

            //Draw number
            EHManagers.EHEngineManager.Graphics.SpriteBatch.DrawString(spriteFont, numbers[number], screenPos, Color.White);
        }
    }
}
