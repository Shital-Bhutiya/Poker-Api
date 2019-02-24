using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Poker.Models
{
    public class PokerHand
    {
        public string Name { get; set; }
        public ICollection<Card> Cards { get; set; }

        public PokerHand()
        {
            Cards = new HashSet<Card>();
        }
    }
}