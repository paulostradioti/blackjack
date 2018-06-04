using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static BlackJack.Api.Models.Constants;

namespace BlackJack.Api.Models
{
    public class BlackJackContext : DbContext
    {
        public BlackJackContext(DbContextOptions<BlackJackContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Card>()
                .HasOne(d => d.Deck)
                .WithMany(c => c.Cards)
                .HasForeignKey(p => p.DeckfId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Game> Games { get; set; }
    }
}
