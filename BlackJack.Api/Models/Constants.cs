using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJack.Api.Models
{
    public static class Constants
    {
        public enum Suit
        {
            Hearts = 1,
            Diamonds,
            Clubs,
            Spades
        }

        public enum CardName
        {
            Ace,
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Ten,
            Jack,
            Queen,
            King
        }

        public enum Winner
        {
            Player, 
            Dealer,
            Push,
            GameStillInProgress
        }

        public enum CurrentPlayer
        {
            User,
            Dealer,
            NoOne
        }

        public static readonly List<CardName> ValueTen = new List<CardName> { CardName.Ten, CardName.Jack, CardName.Queen, CardName.King };
    }
}
