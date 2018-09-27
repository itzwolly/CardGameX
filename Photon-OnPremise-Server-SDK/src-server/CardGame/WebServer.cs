using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CardGame {
    using System;

    public class WebServer {
        private static readonly HttpClient client = new HttpClient();
        private const string GET_DECK_FROM_DB_URL = "http://card-game.gearhostpreview.com/getdeck.php";

        public static async Task<Deck> GetDeckAsync(string pUserName) {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("username", pUserName);

            FormUrlEncodedContent content = new FormUrlEncodedContent(postData);
            var response = await client.PostAsync(GET_DECK_FROM_DB_URL, content);
            string responseString = await response.Content.ReadAsStringAsync();

            Deck deck = new Deck();
            string[] rows = responseString.Split('\n');
            for (int i = 0; i < rows.Length - 1; i++) {
                string[] cols = rows[i].Split('\t');
                Card card = new Card(Convert.ToInt32(cols[0]), cols[1], cols[2], cols[3]);
                deck.AddCard(card);
            }

            int count = deck.GetCards().Count;
            return (count == 0) ? null : deck; // (count == Deck.SIZE ? deck : null)
        }
    }

}

