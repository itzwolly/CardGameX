using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {
    private List<Card> _hand;

    private void Awake() {
        _hand = new List<Card>();
    }

    public List<Card> GetCards() {
        return _hand;
    }
}
