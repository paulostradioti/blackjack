using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BlackJack.Api.Models.Constants;

namespace BlackJack.Api.Models
{
    public class BlackJackGameLogic
    {
      
        public static void BeginGame(Game game)
        {
            game.GameStatus.CurrentPlayer = CurrentPlayer.User;

            // Shuffles the Deck
            game.GameDeck.Shuffle(); //shuffle is unit tested

            // Give the Player's cards
            game.UserHand= new Deck(game.GameDeck.GetCards(2));

            // Give the Dealer's Cards
            game.DealerHand = new Deck(game.GameDeck.GetCards(2));
            RevealDealersCards(game.DealerHand.Cards);


            UpdateGameStatus(game);
        }



    public static bool HasWinner(Game game)
        {
            return !(WhoWins(game) == Winner.GameStillInProgress);
        }

        public static Winner WhoWins(Game game)
        {
            var winner = Winner.GameStillInProgress;

            var isUserBlackJack = HasBlackJack(game.UserHand.Cards.ToList());
            var isDealerBlackJack = HasBlackJack(game.DealerHand.Cards.ToList());

            if (isUserBlackJack && !isDealerBlackJack)
                winner = Winner.Player;
            else if (isDealerBlackJack && !isUserBlackJack)
                winner = Winner.Dealer;
            else if (isUserBlackJack && isDealerBlackJack)
                winner = Winner.Push;

            if (winner == Winner.GameStillInProgress &&
                GetTotal(game.DealerHand.Cards) > 21)
            {
                winner = Winner.Player;
            }

            if (winner == Winner.GameStillInProgress)
            {
                // ou conseguir obter mais pontos que o Dealer, caso o mesmo já tenha virado as 5 cartas ou totalizado qualquer valor superior a 17 pontos.
                var dealerTotal = GetTotal(game.DealerHand.Cards);
                var userTotal = GetTotal(game.UserHand.Cards);

                if (userTotal > dealerTotal && (game.DealerHand.Cards.Count >= 5 || dealerTotal > 17))
                {
                    winner = Winner.Player;
                }
                else if (userTotal > 21)
                {
                    winner = Winner.Dealer;
                }
            }


            return winner;

        }

        private static void RevealDealersCards(List<Card> dealersHand)
        {
            // Initally the Dealer should have 2 cards
            if (dealersHand.Count != 2)
                return;

            bool hasTen = HasTenInHand(dealersHand);

            if (hasTen)
                dealersHand.ForEach(x => x.IsRevealed = true);
            else
                dealersHand.First().IsRevealed = true;
        }


        public static bool HasBlackJack(List<Card> cards)
        {
            var sum = GetTotal(cards);
            return sum == 21;
        }

        private static bool HasTenInHand(List<Card> hand)
        {
            return hand.FirstOrDefault(x => Constants.ValueTen.Contains(x.Name)) != null;
        }

        internal static void Hit(Game game)
        {
            if (game.GameStatus.Winner == Winner.GameStillInProgress)
            {
                game.UserHand.Cards.AddRange(game.GameDeck.GetCards(1));
                UpdateGameStatus(game);
            }
        }

        internal static void Stand(Game game)
        {
            game.GameStatus.CurrentPlayer = CurrentPlayer.Dealer;

            if (game.GameStatus.Winner == Winner.GameStillInProgress)
            {
                bool stop = false;
                while (game.DealerHand.Cards.Count < 5 && GetTotal(game.DealerHand.Cards) <= 17 && game.GameStatus.Winner == Winner.GameStillInProgress)
                {
                    game.DealerHand.Cards.AddRange(game.GameDeck.GetCards(1));
                    UpdateGameStatus(game);
                }
            }

            game.DealerHand.Cards.ForEach(c => c.IsRevealed = true);
        }

        private static void UpdateGameStatus(Game game)
        {
            var winner = WhoWins(game);
            if (winner != Winner.GameStillInProgress)
            {
                game.GameStatus.Winner = winner;
                game.GameStatus.CurrentPlayer = CurrentPlayer.NoOne;
            }

        }

        /// <summary>
        /// Returns the value of the card in the game.
        /// </summary>
        /// <param name="card">The Card being evaluated</param>
        /// <param name="CombineACE">Boolean parameters that indicates wheated ACE should be combined with a TEN value card (if so, ACE value will return 11, otherwise returns 1)</param>
        /// <returns></returns>
        public static int GetCardValue(Card card, bool combineACE)
        {
            int value;

            switch (card.Name)
            {
                case Constants.CardName.Ace:
                    value = combineACE ? 11 : 1;
                    break;
                case Constants.CardName.Two:
                    value = 2;
                    break;
                case Constants.CardName.Three:
                    value = 3;
                    break;
                case Constants.CardName.Four:
                    value = 4;
                    break;
                case Constants.CardName.Five:
                    value = 5;
                    break;
                case Constants.CardName.Six:
                    value = 6;
                    break;
                case Constants.CardName.Seven:
                    value = 7;
                    break;
                case Constants.CardName.Eight:
                    value = 8;
                    break;
                case Constants.CardName.Nine:
                    value = 9;
                    break;
                case Constants.CardName.Ten:
                case Constants.CardName.Jack:
                case Constants.CardName.Queen:
                case Constants.CardName.King:
                    value = 10;
                    break;
                default:
                    value = 0;
                    break;
            }

            return value;
        }

        public static int GetTotal(List<Card> cards)
        {
            var hasTen = HasTenInHand(cards);
            var sum = cards.Sum(x => GetCardValue(x, hasTen));

            return sum;
        }
    }
}
