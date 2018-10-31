using UnityEngine;
using System.Collections.Generic;

public class Card {
    private CardData _data;

    public CardData Data {
        get { return _data; }
    }

    public Card(CardData pCardData) {
        _data = pCardData;
    }
}
