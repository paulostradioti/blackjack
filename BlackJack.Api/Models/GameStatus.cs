using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BlackJack.Api.Models.Constants;

namespace BlackJack.Api.Models
{
    public class GameStatus
    {
        public long Id { get; set; }
        public Winner Winner { get; set; }
        public CurrentPlayer CurrentPlayer { get; set; }
    }
}
