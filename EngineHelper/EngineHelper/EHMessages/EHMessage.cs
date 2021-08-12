using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EngineHelper.EHMessages
{
    // Add Messages to the enum as they are made.
    public enum MSG_TYPE { Msg_EMPTY }

    class EHMessage
    {
        private MSG_TYPE msgID;

        public MSG_TYPE MsgID
        {
            get { return msgID; }
            set { msgID = value; }
        }

        float x;

        public float X
        {
            get { return x; }
            set { x = value; }
        }

        float y;

        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        public EHMessage()
        {

        }

        public EHMessage(MSG_TYPE id)
        {
            msgID = id;
        }
    }
}
