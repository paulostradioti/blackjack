using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BlackJack.Api.Models.Constants;

namespace BlackJack.Api.Models
{
    public class GameView
    {
        [JsonProperty("Game Id")]
        public long GameId;
             
        [JsonProperty("Dealer Cards")]
        List<String> Dealer = new List<string>();

        [JsonProperty("Your Cards")]
        List<String> User = new List<string>();

        [JsonProperty("Your Total")]
        public int Total;

        [JsonProperty("Game Result")]
        public String GameResult;

        [JsonProperty("Who is Next")]
        public String WhoIsNext;

        public GameView(Game game)
        {
            GameId = game.Id;

            game.DealerHand.Cards.Where(c => c.IsRevealed).ToList().ForEach(x => Dealer.Add(x.ToString()));
            game.UserHand.Cards.ToList().ForEach(x => User.Add(x.ToString()));

            Winner winner = game.GameStatus.Winner;
            GameResult = GetGameResultText(winner);
            WhoIsNext = GetCurrentPlayingText(game.GameStatus.CurrentPlayer);

            Total = BlackJackGameLogic.GetTotal(game.UserHand.Cards);
            
        }

        private string GetGameResultText(Winner winner)
        {
            string retVal = string.Empty;

            switch (winner)
            {
                case Winner.Player:
                    retVal = "You Won! Congratulations";
                    break;
                case Winner.Dealer:
                    retVal = "The Dealer Won! Better Luck Next Time.";
                    break;
                case Winner.Push:
                    retVal = "It's a PUSH.";
                    break;
                case Winner.GameStillInProgress:
                    retVal = "The Game is still in Progress";
                    break;
            }

            return retVal;
        }

        private string GetCurrentPlayingText(CurrentPlayer player)
        {
            string retVal = string.Empty;

            switch (player)
            {
                case Constants.CurrentPlayer.User:
                    retVal = "It's your turn.";
                    break;
                case Constants.CurrentPlayer.Dealer:
                    retVal = "It's the Dealers turn.";
                    break;
                case Constants.CurrentPlayer.NoOne:
                    retVal = "Nobody";
                    break;
            }

            return retVal;
        }
    }
}
