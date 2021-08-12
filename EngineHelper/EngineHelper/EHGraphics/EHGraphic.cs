using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineHelper.EHGraphics
{
    public sealed class EHGraphic
    {
        GraphicsDeviceManager manager = null;
        EHLineRenderer lineRenderer = null;
        Texture2D pixel = null;
        SpriteBatch spriteBatch = null;

        public GraphicsDeviceManager Manager
        {
            get { return manager; }
        }

        public GraphicsDevice Device
        {
            get { return manager.GraphicsDevice; }
        }

        public Viewport ScreenViewport
        {
            get { return manager.GraphicsDevice.Viewport; }
        }

        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        public EHLineRenderer Line
        {
            get { return lineRenderer; }
        }

        public Texture2D Pixel
        {
            get { return pixel; }
        }

        public void Initialize(GraphicsDeviceManager graphics)
        {
            manager = graphics;
            lineRenderer = new EHLineRenderer(manager.GraphicsDevice);
            pixel = new Texture2D(manager.GraphicsDevice, 1, 1);
            Color[] whitePixels = new Color[] { Color.White };
            pixel.SetData<Color>(whitePixels);

            spriteBatch = new SpriteBatch(manager.GraphicsDevice);

            manager.DeviceReset += new EventHandler<EventArgs>(ResetDevice);
        }

        public void ToggleFullScreen()
        {
            manager.IsFullScreen = !manager.IsFullScreen;

            manager.ApplyChanges();
        }

        //  Event
        private void ResetDevice(object sender, EventArgs eventargs)
        {
           lineRenderer = new EHLineRenderer(manager.GraphicsDevice);

            pixel = new Texture2D(manager.GraphicsDevice, 1, 1);
            Color[] whitePixels = new Color[] { Color.White };
            pixel.SetData<Color>(whitePixels);
        }
    }
}
