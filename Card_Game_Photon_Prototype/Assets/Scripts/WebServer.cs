using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class WebServer {
    private const string SAVE_PLAYER_DECK = "http://card-game.gearhostpreview.com/saveplayerdeck.php";
    private const string GET_DECK_FROM_DB_URL = "http://card-game.gearhostpreview.com/getdeck.php";
    private const string GET_CARDS_USING_DECK_CODE = "http://card-game.gearhostpreview.com/getcardsusingdeckcode.php";
    private const string ADD_DECK = "http://card-game.gearhostpreview.com/adddeck.php";
    private const string REMOVE_DECK = "http://card-game.gearhostpreview.com/removedeck.php";
    private const string UPDATE_DECK_NAME = "http://card-game.gearhostpreview.com/updatedeckname.php";

    public static IEnumerator SaveDeckToDB(string pDeckCode) {
        WWWForm form = new WWWForm();
        form.AddField("deckcode", pDeckCode);

        using (WWW hs_get = new WWW(SAVE_PLAYER_DECK, form)) {
            yield return hs_get;

            if (hs_get.error != null) {
                Debug.Log(hs_get.error);
            } else {
                if (hs_get.isDone) {
                    Debug.Log(hs_get.text);
                }
            }
        }
    }

    public static IEnumerator GetDeckFromDB(string pUserName, Action<string[]> pCallBack) {
        WWWForm form = new WWWForm();
        form.AddField("username", pUserName);

        using (WWW hs_get = new WWW(GET_DECK_FROM_DB_URL, form)) {
            yield return hs_get;

            if (hs_get.error != null) {
                Debug.Log(hs_get.error);
            } else {
                if (hs_get.isDone) {
                    string[] rows = hs_get.text.Split('\n');
                    pCallBack(rows);
                }
            }
        }
    }

    public static IEnumerator SaveCardsUsingDeckCode(int pId, string pNewDeckName, string pUserName, string pPrevDeckCode, string pNewDeckCode, string pTurboDeckCode, Action<bool> pCallBack) {
        WWWForm form = new WWWForm();
        form.AddField("deckid", pId);
        form.AddField("newdeckname", pNewDeckName);
        form.AddField("username", pUserName);
        form.AddField("prevdeckcode", pPrevDeckCode);
        form.AddField("newdeckcode", pNewDeckCode);
        form.AddField("newturbocode", pTurboDeckCode);

        using (WWW hs_get = new WWW(SAVE_PLAYER_DECK, form)) {
            yield return hs_get;

            if (hs_get.error != null) {
                Debug.Log(hs_get.error);
            } else {
                if (hs_get.isDone) {
                    Debug.Log(hs_get.text);
                    pCallBack(true);
                }
            }
        }
    }

    public static IEnumerator GetCardsUsingDeckCode(string pBase64RegDeckCode, string pBase64TurboDeckCode, Action<List<CardData>, List<CardData>> pCallBack) {
        WWWForm form = new WWWForm();
        form.AddField("deckcode", pBase64RegDeckCode);
        form.AddField("turbocode", pBase64TurboDeckCode);

        using (WWW hs_get = new WWW(GET_CARDS_USING_DECK_CODE, form)) {
            yield return hs_get;

            if (hs_get.error != null) {
                Debug.Log(hs_get.error);
            } else {
                if (hs_get.isDone) {
                    CardInfoTypeList infoList = CardInfoTypeList.CreateFromJSON(hs_get.text);
                    CardInfoType infoType = infoList.CardTypes;

                    List<CardData> regCards = infoType.GetCardInfo(infoType.RegCards);
                    List<CardData> turboCards = infoType.GetCardInfo(infoType.TurboCards);

                    pCallBack(regCards, turboCards);
                }
            }
        }
    }

    public static IEnumerator RemoveDeck(string pUsername, int pDeckId) {
        WWWForm form = new WWWForm();
        form.AddField("username", pUsername);
        form.AddField("deckid", pDeckId);

        using (WWW hs_get = new WWW(REMOVE_DECK, form)) {
            yield return hs_get;

            if (hs_get.error != null) {
                Debug.Log(hs_get.error);
            } else {
                if (hs_get.isDone) {
                    Debug.Log(hs_get.text);
                }
            }
        }
    }

    public static IEnumerator AddDeck(string pUsername, Action<DeckData> pCallBack) {
        WWWForm form = new WWWForm();
        form.AddField("username", pUsername);

        using (WWW hs_get = new WWW(ADD_DECK, form)) {
            yield return hs_get;

            if (hs_get.error != null) {
                Debug.Log(hs_get.error);
            } else {
                if (hs_get.isDone) {
                    Debug.Log(hs_get.text);

                    DeckInfoList deckInfoList = DeckInfoList.CreateFromJSON(hs_get.text);
                    DeckInfo deckInfo = deckInfoList.Decks[0];

                    DeckData deckData = new DeckData(deckInfo.Id, deckInfo.DeckName, deckInfo.DeckCode, deckInfo.TurboCode);
                    pCallBack(deckData);
                }
            }
        }
    }

    public static IEnumerator UpdateDeckName(string pUsername, int pDeckId, string pNewDeckName) {
        WWWForm form = new WWWForm();
        form.AddField("username", pUsername);
        form.AddField("deckid", pDeckId);
        form.AddField("newdeckname", pNewDeckName);

        using (WWW hs_get = new WWW(UPDATE_DECK_NAME, form)) {
            yield return hs_get;

            if (hs_get.error != null) {
                Debug.Log(hs_get.error);
            } else {
                if (hs_get.isDone) {
                    Debug.Log(hs_get.error);
                }
            }
        }
    }
}
