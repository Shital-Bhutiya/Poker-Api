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

namespace Poker.Models
{
    public class Card
    {
        public string Suit { get; set; }
        public string Rank { get; set; }
    }
}