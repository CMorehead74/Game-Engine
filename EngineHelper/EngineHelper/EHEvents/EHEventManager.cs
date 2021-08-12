using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EngineHelper.EHEvents;

namespace EngineHelper.EHManagers
{
    enum EVENTID { EXAMPLE_EVENT }
    public class EHEventManager
    {
        private static EHEventManager s_Instance;

        public static EHEventManager GetInstance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = new EHEventManager();
                }
                return s_Instance;
            }
        }

        List<KeyValuePair<EVENTID, Listener>> ClientDatabase;
        List<EHEvent> currentEvents;

        public EHEventManager()
        {
            currentEvents = new List<EHEvent>();
            ClientDatabase = new List<KeyValuePair<EVENTID, Listener>>();
        }

        void RegisterClient(EVENTID eventID, Listener client)
        {
            if (client == null || AlreadyRegistered(eventID, client))
            {
                return;
            }
            ClientDatabase.Add(new KeyValuePair<EVENTID, Listener>(eventID, client));
        }

        bool AlreadyRegistered(EVENTID eventID, Listener client)
        {
            for (int i = 0; i < ClientDatabase.Count(); i++)
            {
                if (ClientDatabase[i].Key == eventID)
                    if (ClientDatabase[i].Value == client)
                        return true;
            }

            return false;
        }

        void UnRegisterClient(EVENTID eventID, Listener client)
        {
            for (int i = 0; i < ClientDatabase.Count(); i++)
            {
                if (ClientDatabase[i].Key == eventID)
                    if (ClientDatabase[i].Value == client)
                        ClientDatabase.Remove(ClientDatabase[i]);
            }
        }

        void UnRegisterAllClients(Listener client)
        {
            ClientDatabase.Clear();
        }

        void SendEvent(EVENTID eventID, EventArgs Data)
        {
            for (int i = 0; i < ClientDatabase.Count(); i++)
            {
                if (ClientDatabase[i].Key == eventID)
                    currentEvents.Add(new EngineHelper.EHEvents.EHEvent(ClientDatabase[i].Value.ListenerProc, Data));
            }
        }

        void ProcessEvents()
        {
            for (int i = 0; i < currentEvents.Count(); i++)
            {
                currentEvents[i].lstner(currentEvents[i].eventArg);
            }
            ClearEvents();
        }

        void ClearEvents()
        {
            currentEvents.Clear();
        }
    }

    public class Listener
    {
        public delegate void LISTENERPROC(EventArgs Data);
        public LISTENERPROC ListenerProc;

        public void Subscribe(LISTENERPROC lstn)
        {
            ListenerProc = lstn;
        }
    }
}
