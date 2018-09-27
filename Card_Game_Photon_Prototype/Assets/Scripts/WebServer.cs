using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class WebServer {
    private const string ADD_CARD_TO_DECK_URL = "http://card-game.gearhostpreview.com/addcardtodeck.php";
    private const string GET_DECK_FROM_DB_URL = "http://card-game.gearhostpreview.com/getdeck.php";

    public static IEnumerator AddCardToDeck(string pUserName, int pCardId, string pDeckName = "") {
        WWWForm form = new WWWForm();
        form.AddField("username", pUserName);
        form.AddField("cardid", pCardId);
        form.AddField("deckName", pDeckName);

        using (WWW hs_get = new WWW(ADD_CARD_TO_DECK_URL, form)) {
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
}
