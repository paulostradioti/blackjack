using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackJack.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlackJack.Api.Controllers
{
    [Route("api/[controller]")]
    public class BlackJackController : Controller
    {
        private readonly BlackJackContext context;
        //BlackJackGameState gameState;
        Game game;

        public BlackJackController(BlackJackContext _context)
        {
            context = _context;
        }

        /// <summary>
        ///  Creates a new game. If there's already an ongoing game it will be discarted and a new one will begin
        /// </summary>
        /// <returns></returns>
        [Route("newgame")]
        [HttpGet]
        public IActionResult NewGame()
        {
            Game game = new Game();
            BlackJackGameLogic.BeginGame(game);
            context.Games.Add(game);

            context.SaveChanges();

            var view = new GameView(game);

            return CreatedAtRoute("GetGame", new { id = game.Id }, view);
        }



        [HttpGet]
        public IEnumerable<string> Get()
        {
            var gameIds = context.Games.Select(x => x.Id.ToString()).ToList();
            return gameIds;
        }


        [HttpGet("{id:long:min(1)}", Name = "GetGame")]
        public IActionResult Get(long id)
        {

            var result = context.Games.Include(game => game.DealerHand.Cards)
                                       .Include(game => game.UserHand.Cards)
                                       .Include(game => game.GameDeck.Cards)
                                       .Include(game => game.GameStatus).First(game => game.Id == id);
            if (result != null)
            {
                return Ok(new GameView(result));
            }

            else
                return NotFound();
        }


        [HttpGet("{id:long:min(1)}/hit")]
        public IActionResult Hit(long id)
        {
            var result = context.Games.Include(game => game.DealerHand.Cards)
                           .Include(game => game.UserHand.Cards)
                           .Include(game => game.GameDeck.Cards)
                           .Include(game => game.GameStatus).First(game => game.Id == id);

            if (result != null)
            {
                BlackJackGameLogic.Hit(result);
            }

            context.Update(result);
            context.SaveChanges();

            return Ok(new GameView(result));
        }

        [HttpGet("{id:long:min(1)}/stand")]
        public IActionResult Stand(long id)
        {
            var result = context.Games.Include(game => game.DealerHand.Cards)
               .Include(game => game.UserHand.Cards)
               .Include(game => game.GameDeck.Cards)
               .Include(game => game.GameStatus).First(game => game.Id == id);

            if (result != null)
            {
                BlackJackGameLogic.Stand(result);
            }

            context.Update(result);
            context.SaveChanges();

            return Ok(new GameView(result));
        }
    }
}
