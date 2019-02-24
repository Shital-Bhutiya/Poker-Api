using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public enum Suit
{
    Clubs,
    Diamonds,
    Hearts,
    Spades
}
public enum Rank
{
    Two=2,
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
    King,
    Ace
}

namespace Poker.Models
{
    public class Card
    {
        public Suit Suit { get; set; }
        public Rank Rank { get; set; }
    }
}