using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Poker.Models
{
    public class PokerTable
    {
        public int TableId { get; set; }
        public ICollection<PokerHand> PokerHands { get; set; }
        public PokerTable()
        {
            PokerHands = new HashSet<PokerHand>();
        }
    }
}