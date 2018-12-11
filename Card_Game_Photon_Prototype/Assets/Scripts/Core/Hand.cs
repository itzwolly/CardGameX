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

    public List<Card> Cards = null;
    public CardGameBehaviour SelectedMonsterCardBehaviour = null;
    public CardGameBehaviour SelectedSpellCardBehaviour = null;

    private List<IInteractable> _targets;

    public void Init() {
        Cards = new List<Card>();
        _targets = new List<IInteractable>();
    }

    public List<IInteractable> GetTargets() {
        return _targets;
    }

    public GameObject AddCard(Card pCard, int pHandSize) {
        int count = Cards.Count;
        if (count >= Config.MAX_HAND_SIZE) {
            return null;
        }

        //Debug.Log("Adding card: " + pCard.Data.Id);
        Cards.Add(pCard);

        HandManager manager = PhotonNetwork.player.TagObject as HandManager;
        GameObject gameCard = manager.DisplayPlayerCardDrawn(pHandSize);
        CardGameBehaviour cgb = gameCard.GetComponent<CardGameBehaviour>();
        cgb.CardId = pCard.Data.Id;
        cgb.SetCardText(pCard);

        return gameCard;
    }
}
