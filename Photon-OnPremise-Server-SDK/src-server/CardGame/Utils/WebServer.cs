using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CardGame {
    using System;
    using Newtonsoft.Json;

    public class WebServer {
        private static readonly HttpClient client = new HttpClient();
        private const string GET_CARDS_USING_DECK_CODE = "http://card-game.gearhostpreview.com/getfullcardsusingdeckcode.php";

        public static async Task<Deck> GetCardsUsingCodes(string pDeckCode, string pTurboCode) {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("deckcode", pDeckCode);
            postData.Add("turbocode", pTurboCode);

            FormUrlEncodedContent content = new FormUrlEncodedContent(postData);
            var response = await client.PostAsync(GET_CARDS_USING_DECK_CODE, content);
            string responseString = await response.Content.ReadAsStringAsync();

            CardsTypeContainer container = JsonConvert.DeserializeObject<CardsTypeContainer>(responseString);

            Deck deck = new Deck(container.CardTypes.RegCards, container.CardTypes.TurboCards);
            
            int count = deck.GetCards().Count;
            return (count == 0) ? null : deck; // (count == Deck.SIZE ? deck : null)
        }
    }

}

