using System;
using System.Collections;
using System.Collections.Generic;

namespace CardGame {
    public class EventResponse {
        public readonly byte EventCode;
        public readonly object Data;
        public List<int> Receivers;

        public EventResponse(byte pEventCode, object pData) {
            EventCode = pEventCode;
            Data = pData;
            Receivers = new List<int>();
        }
    }

}

