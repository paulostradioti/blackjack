using BlackJack.Api.Models;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using Xunit;
using static BlackJack.Api.Models.Constants;

namespace BlackJack.Api.Tests
{
    public class BlackJackModelTests
    {
        [Fact]
        public void TestShuffle()
        {
            Deck deck = new Deck();

            var deckBeforeShuffle = deck.Cards;
            deck.Shuffle();

            deck.Cards.Should().HaveCount(deckBeforeShuffle.Count);
            //deck.Cards.Should().NotEqual(deckBeforeShuffle);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public void TestGetCard(int value)
        {
            Deck deck = new Deck();

            var initialCount = deck.Cards.Count;
            var card = deck.GetCards(value);
            var finalCount = deck.Cards.Count;

            if (value >= 0)
                deck.Cards.Should().HaveCountLessOrEqualTo(initialCount);
            else
                deck.Cards.Should().HaveCount(finalCount);
        }
    }
}
