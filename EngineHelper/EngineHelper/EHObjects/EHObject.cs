using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineHelper.EHObjects
{
    public enum Object_Type { PLAYER = 0, ENEMY }
    public enum Obstacle_Type { OTHER = 0 }

    public class EHObject
    {
        Vector2 m_fPos;
        bool alive = true;

        public bool Alive
        {
            get { return alive; }
            set { alive = value; }
        }

        public Vector2 fPos
        {
            get { return m_fPos; }
            set { m_fPos = value; }
        }
        Rectangle colRect;

        public Rectangle ColRect
        {
            get { return colRect; }
            set { colRect = value; }
        }

        Object_Type objectID;

        internal Object_Type ObjectID
        {
            get { return objectID; }
            set { objectID = value; }
        }

        Obstacle_Type obstacleID;

        internal Obstacle_Type ObstacleID
        {
            get { return obstacleID; }
            set { obstacleID = value; }
        }

        public virtual void Update(float deltaTime) { }

        public virtual void Draw(SpriteBatch spriteBatch) { }

        public virtual bool Collide(Object tar)
        {
            return false;
        }
    }
}
