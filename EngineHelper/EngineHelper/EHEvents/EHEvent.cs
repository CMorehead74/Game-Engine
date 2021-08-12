using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EngineHelper.EHEvents
{
    class EHEvent
    {
        public EngineHelper.EHManagers.Listener.LISTENERPROC lstner;
        public EventArgs eventArg;

        public EHEvent(EngineHelper.EHManagers.Listener.LISTENERPROC lsn, EventArgs arg)
        {
            lstner = lsn;
            eventArg = arg;
        }
    }
}
