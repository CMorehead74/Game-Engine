using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineHelper.EHGraphics
{
    public class EHLineRenderer
    {
        RenderTarget2D pixelTexture;

        public EHLineRenderer(GraphicsDevice graphicsDevice)
        {
            pixelTexture = new RenderTarget2D(graphicsDevice, 2, 3);
            graphicsDevice.SetRenderTarget(pixelTexture);
            graphicsDevice.Clear(Color.White);
            graphicsDevice.SetRenderTarget(null);
        }

        //Draws a line between 2 points.
        public void DrawLine(SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, float thickness, Color color)
        {
            Vector2 difference = point2 - point1;
            float length = difference.Length();
            float angle = (float)System.Math.Atan2(difference.Y, difference.X);

            spriteBatch.Draw(pixelTexture, point1, null, color, angle,
                                new Vector2(0, 1),
                                new Vector2(length / 2, thickness / 3),
                                SpriteEffects.None, 0);
        }

        //Draws a Rectangle from the passed in parameters.
        public void DrawRect(SpriteBatch spriteBatch, Rectangle r, float thickness, Color color)
        {
            //  top
            DrawLine(spriteBatch, new Vector2(r.Left, r.Top), new Vector2(r.Right, r.Top), thickness, color);
            //  bottom
            DrawLine(spriteBatch, new Vector2(r.Left, r.Bottom), new Vector2(r.Right, r.Bottom), thickness, color);
            //  left side
            DrawLine(spriteBatch, new Vector2(r.Left, r.Top), new Vector2(r.Left, r.Bottom), thickness, color);
            //  right side
            DrawLine(spriteBatch, new Vector2(r.Right, r.Top), new Vector2(r.Right, r.Bottom), thickness, color);
        }

        //Draws a circle from the passed in parameters.
        public void DrawCircle(SpriteBatch spriteBatch, Vector2 center, float radius, int sides, float thickness, Color color)
        {
            if (sides < 2)
                return;

            float max = 2 * (float)Math.PI + 1;
            float step = max / (float)sides;

            Vector2 prev;
            Vector2 next;

            prev = new Vector2(radius * (float)Math.Cos(0), radius * (float)Math.Sin(0));

            for (float theta = step; theta < max; theta += step)
            {
                next = new Vector2(radius * (float)Math.Cos((double)theta), radius * (float)Math.Sin((double)theta));

                // calculate the distance between the two vectors
                float distance = Vector2.Distance(prev, next);

                // calculate the angle between the two vectors
                float angle = (float)Math.Atan2((double)(next.Y - prev.Y),
                    (double)(next.X - prev.X));

                // stretch the pixel between the two vectors
                spriteBatch.Draw(pixelTexture, center + prev, null, color, angle, Vector2.Zero,
                    new Vector2(distance / 2, thickness / 3), SpriteEffects.None, 0);

                prev = next;
            }
        }

        /// Draws a closed polygon from the given points.
        public void DrawPolygon(SpriteBatch spriteBatch, Vector2[] points, float thickness, Color color)
        {
            //  Should AT LEAST be a triangle.
            if (points.Length < 3)
                return;

            //  Connect the dots...LA LA LA!
            for (int i = 0; i < points.Length - 1; i++)
            {
                DrawLine(spriteBatch, points[i], points[i + 1], thickness, color);
            }

            //  Draw closing line between last and first point
            DrawLine(spriteBatch, points[points.Length - 1], points[0], thickness, color);
        }
    }
}
