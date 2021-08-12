using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using System.Threading;
using EngineHelper.EHManagers;

namespace EngineHelper.EHStates
{
    public class EHBaseGameState : EHIGameState
    {
        Thread loadThread;
        float loadPercentage;

        public string Name { get; set; }

        public void StartLoad()
        {
            loadThread = new Thread(new ThreadStart(LoadContent));
            loadThread.Start();
        }

        public bool IsLoading
        {
            get
            {
                return loadThread != null && loadThread.IsAlive;
            }
        }

        public virtual void LoadContent()
        {
            LoadPercentage = 100.0f;
        }

        public float LoadPercentage
        {
            get
            {
                float loadCopy;
                lock (this)
                {
                    loadCopy = loadPercentage;
                }
                return loadCopy;
            }
            set
            {
                lock (this)
                {
                    loadPercentage = value;
                }
            }
        }

        public virtual void OnEnter(string previousState) { }

        public virtual void OnExit(string nextState) { }

        public virtual void OnOverride(string nextState) { }

        public virtual void OnResume(string previousState) { }

        public virtual void Input() { }

        public virtual void Update() { }

        public virtual void DrawRenderTargets() { }

        public virtual void Draw() { }
    }
}
