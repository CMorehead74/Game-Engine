using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace EngineHelper.EHTimes
{
    public sealed class EHTime
    {
        GameTime time = new GameTime();

        float deltaTime = 0.0f;
        float realDeltaTime = 0.0f;

        float speed = 1.0f;

        public bool IsFixedTimeStep { get; set; }

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public float DeltaTime
        {
            get { return deltaTime; }
        }

        public float RealDeltaTime
        {
            get { return realDeltaTime; }
        }

        public GameTime GameTime
        {
            get { return time; }
            set
            {
                time = value;

                if (!IsFixedTimeStep)
                {
                    realDeltaTime = (float)time.ElapsedGameTime.TotalSeconds;
                    deltaTime = realDeltaTime * speed;
                }
            }
        }

        public EHTime()
        {
            IsFixedTimeStep = false;
        }

        public void SetFixedTimeStep(float fixedTimeStep)
        {
            IsFixedTimeStep = true;
            realDeltaTime = deltaTime = fixedTimeStep;
        }
    }
}
