using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {
    private static Hand _instance;
    public static Hand Instance {
        get {
            if (!_instance) {
                _instance = FindObjectOfType(typeof(Hand)) as Hand;

                if (!_instance) {
                    Debug.Log("There should be an active object with the component Hand");
                } else {
                    _instance.Init();
                }
            }
            return _instance;
        }
    }

    private List<Card> _hand;

    private void Init() {
        if (_hand == null) {
            _hand = new List<Card>();
        }
    }

    public void AddCard(Card pCard) {
        _hand.Add(pCard);

        //DisplayCard(pCard);
    }

    public GameObject DisplayCard(int pCardId, int pHandSize) {
        HandManager manager = PhotonNetwork.player.TagObject as HandManager;
        GameObject gameCard = manager.DisplayPlayerCardDrawn(pHandSize);
        gameCard.GetComponent<CardGameBehaviour>().CardId = pCardId;

        return gameCard;
    }

    public void PlayCard(int pId) {
        Card card = GetCard(pId);
        card.ExecuteOnEnter();

        _hand.Remove(card);
    }

    public Card GetCard(int pId) {
        return _hand.Find(o => o.Data.Id == pId);
    }

    public List<Card> GetCards() {
        return _hand;
    }
}
