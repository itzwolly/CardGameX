using System;
using System.Collections;
using System.Collections.Generic;

namespace CardGame {

    public class EventResponse {
        public readonly byte EventCode;
        public readonly Hashtable Data;

        public EventResponse(byte pEventCode, Hashtable pData) {
            EventCode = pEventCode;
            Data = pData;
        }
    }

}

