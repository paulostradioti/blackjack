using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static BlackJack.Api.Models.Constants;

namespace BlackJack.Api.Models
{
    public class Deck
    {
        public long DeckId { get; set; }
        public List<Card> Cards { get; set; }
        public string DeckName { get; set; }
        public static List<Card> AllCards()
        {
            var cards = new List<Card>();

            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (CardName name in Enum.GetValues(typeof(CardName)))
                {
                    var card = new Card(name, suit);
                    cards.Add(card);
                }
            }

            return cards;
        }
        public Deck()
        {
            Cards = new List<Card>();
        }

        public Deck(List<Card> cards)
        {
            Cards = cards;
        }

        public List<Card> GetCards(int count)
        {
            List<Card> returnValue = new List<Card>();

            if (count <= 0)
                return returnValue;

            // if count is > then the number of available cards, all cards are returned as per the Take<> definition
            var cards = Cards.Take(count).AsEnumerable();

            foreach (var card in cards)
            {
                returnValue.Add(card);
                Cards.Remove(card);
            }

            return returnValue;
        }

        public void Shuffle()
        {
            List<Card> shuffledDeck = new List<Card>();

            //copies the cards to avoid changing in the exposed Cards property before shuffling is concluded
            List<Card> currentDeck = new List<Card>(Cards.Count);
            Cards.ToList().ForEach(card => currentDeck.Add(card));

            //pulls a random card from the current deck and moves it to the shuffled deck
            Random randomGenerator = new Random();

            while (currentDeck.Count() > 0)
            {
                var randomPosition = randomGenerator.Next(currentDeck.Count()); // from 0 up to Count-1
                shuffledDeck.Add(currentDeck[randomPosition]);
                currentDeck.RemoveAt(randomPosition);
            }

            Cards = shuffledDeck;
        }

        public override string ToString()
        {
            List<string> cards = new List<string>();
            Cards.ToList().ForEach(x => cards.Add(x.ToString()));

            return String.Join(", ", cards);
        }
    }
}
