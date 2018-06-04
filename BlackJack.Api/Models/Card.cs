using Newtonsoft.Json;
using System;
using static BlackJack.Api.Models.Constants;

namespace BlackJack.Api.Models
{
    public class Card
    {
        public long CardId { get; set; }
        public CardName Name { get; private set; }
        public Suit Suit { get; private set; }
        public bool IsRevealed { get; set; }
        public long DeckfId { get; set; }
        public Deck Deck { get; set; }
    
        public Card(CardName _name, Suit _suit)
        {
            Name = _name;
            Suit = _suit;
            IsRevealed = false;
        }

        public override string ToString()
        {
            return $"{Name} of {Suit}";
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        private Card()
        {

        }
    }
}
