using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EngineHelper.EHManagers
{
    class EHMessageManager
    {
        private static EHMessageManager s_Instance;

        public static EHMessageManager GetInstance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = new EHMessageManager();
                }
                return s_Instance;
            }
        }

        public delegate void MESSAGEPROC(EngineHelper.EHMessages.EHMessage msg);

        private MESSAGEPROC messageProc;

        private Queue<EHMessages.EHMessage> msgQueue;

        public void Init(MESSAGEPROC msgFunc)
        {
            messageProc = msgFunc;
            msgQueue = new Queue<EHMessages.EHMessage>();
        }

        public void SendMessage(EHMessages.EHMessage msg)
        {
            msgQueue.Enqueue(msg);
        }

        public void ClearMessages()
        {
            msgQueue.Clear();
        }

        public void ProcessMessage()
        {
            for (int i = 0; i < msgQueue.Count; i++)
            {
                messageProc(msgQueue.Dequeue());
            }
        }
    }
}
