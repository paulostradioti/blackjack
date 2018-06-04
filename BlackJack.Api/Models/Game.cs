using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJack.Api.Models
{
    public class Game
    {
        public long Id { get; set; }

        public Deck GameDeck { get; set; }
        public Deck DealerHand { get; set; }
        public Deck UserHand { get; set; }
        public GameStatus GameStatus { get; set; }

        public Game()
        {
            GameDeck = new Deck(Deck.AllCards());
            DealerHand = new Deck();
            UserHand = new Deck();
            GameStatus = new GameStatus {CurrentPlayer = Constants.CurrentPlayer.User, Winner = Constants.Winner.GameStillInProgress };
        }
    }
}
