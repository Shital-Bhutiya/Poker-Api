using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Poker.Models
{
    public class ResultStatus
    {
        internal bool isFourOfKind;
        internal bool isFullHouse;

        public string Name { get; set; }
        public bool isFlush { get; set; }
        public bool isOnePair { get; set; }
        public bool isTreeofKind { get; set; }
        public bool isHighCard { get; set; }
    }
}