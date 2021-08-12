using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using EngineHelper.EHObjects;
using EngineHelper;


namespace EngineHelper.EHManagers
{
    public class Node
    {
        protected EHObjects.EHObject data;
        public EHObjects.EHObject Data
        {
            get { return data; }
            set { data = value; }
        }

        protected Node next;
        internal Node Next
        {
            get { return next; }
            set { next = value; }
        }
    }

    class EHObjectManager
    {
        private static EHObjectManager s_Instance;
        
        public static EHObjectManager GetInstance
        {
            get
            {
                if (s_Instance == null)
                    s_Instance = new EHObjectManager();
                return s_Instance;
            }
        }

        private Node head;
        internal Node Head
        {
            get { return head; }
            set { head = value; }
        }

        public void AddObject(EHObjects.EHObject data)
        {
            Node newNode = new Node();
            if (head == null)
            {
                newNode.Data = data;
                newNode.Next = null;
                head = newNode;
            }
            else
            {
                newNode.Data = data;
                newNode.Next = head;
                head = newNode;
            }
        }

        public void RemoveObject(EngineHelper.EHObjects.EHObject data)
        {
            Node prev = null;
            Node cur = head;
            while (cur != null)
            {
                if (cur.Data == data)
                {
                    if (prev == null)
                    {
                        head = cur.Next;
                        break;
                    }
                    prev.Next = cur.Next;
                    cur.Next = null;
                    break;
                }
                prev = cur;
                cur = cur.Next;
            }
        }

        public void UpdateObjects(float deltaTime)
        {
            Node cur = head;
            while (cur != null)
            {
                if (cur.Data.fPos.Y > EngineHelper.EHManagers.EHEngineManager.Graphics.ScreenViewport.Height)      
                    RemoveObject(cur.Data);

                cur.Data.Update(deltaTime);

                cur = cur.Next;
            }
        }

        public void DrawObjects(SpriteBatch spriteBatch)
        {
            Node cur = head;
            while (cur != null)
            {
                cur.Data.Draw(spriteBatch);
                cur = cur.Next;
            }
        }

        public void CheckCollisions()
        {
            Node outerCur = head;
            Node innerCur = head;
            while (outerCur != null)
            {
                if (outerCur.Data.ObjectID == Object_Type.PLAYER)
                {
                    innerCur = head;
                    while (innerCur != null)
                    {
                        if (outerCur.Data.ObjectID != innerCur.Data.ObjectID)
                        {
                            if (outerCur.Data.Collide(innerCur.Data))
                            {
                                if (innerCur.Data.ObjectID == Object_Type.ENEMY)
                                {
                                    outerCur.Data.Alive = false;
                                    break;
                                }
                            }
                        }
                        innerCur = innerCur.Next;
                    }
                }
                outerCur = outerCur.Next;
            }
        }

        public void RemoveAllObjects()
        {
            head = null;
        }
    }
}
