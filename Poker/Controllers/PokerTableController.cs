using Poker.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.UI.WebControls;

namespace Poker.Controllers
{

    public class PokerTableController : ApiController
    {
        ICollection<ResultStatus> resultStatuses { get; set; }
        private string Winner { get; set; }
        [HttpGet]
        public string Get(PokerTable table)
        {
            return getPokerHands(table.PokerHands);
        }
        [HttpPost]
        public string getPokerHands(ICollection<PokerHand> pokerHands)
        {
            resultStatuses = new HashSet<ResultStatus>();

            foreach (PokerHand hand in pokerHands)
            {
                var resultStatus = new ResultStatus();
                resultStatus.Name = hand.Name;
                //hand.Cards = Sort(hand.Cards);
                isPokerHand(hand.Cards, resultStatus);
                resultStatuses.Add(resultStatus);
            }
            checkWinner(resultStatuses);
            if (Winner == "")
            {
                //checkHighCard()
            }
            return Winner;
        }

        [HttpPost]
        private void isPokerHand(ICollection<Card> cards, ResultStatus resultStatus)
        {
            resultStatus.isFlush = checkFlush(cards);
            resultStatus.isTreeofKind = checkThreeOfKind(cards);
            resultStatus.isOnePair = checkOnePair(cards);
            resultStatus.isHighCard = false;
        }
        public bool checkFlush(ICollection<Card> Cards)
        {
            return Cards.GroupBy(h => h.Suit).Count() == 1;
        }
        public bool checkOnePair(ICollection<Card> Cards)
        {
            return Cards.GroupBy(h => h.Rank)
                       .Where(g => g.Count() >= 2)
                       .Count() == 1;
        }
        public bool checkThreeOfKind(ICollection<Card> Cards)
        {
            return Cards.GroupBy(h => h.Rank)
                        .Where(g => g.Count() >= 3)
                        .Count() == 3;
        }

        private void checkWinner(ICollection<ResultStatus> resultStatuses)
        {
            // Check for flush winner or tie for flush
            if (resultStatuses.Where(p => p.isFlush == true).Count() == 1)
            {
                var finalresult = resultStatuses.FirstOrDefault(f => f.isFlush == true);
                Winner = finalresult.Name;
            }
            else if (resultStatuses.Where(g => g.isFlush == true).Count() > 1)
            {
                var players = resultStatuses.Where(g => g.isFlush == true);
                Winner = "The Game is tie with:";
                foreach (var player in players)
                {
                    Winner += player.Name + " ";
                }
            }

            // Check for Three of kind posibility or tie for that
            else if (resultStatuses.Where(p => p.isTreeofKind == true).Count() == 1)
            {
                var finalresult = resultStatuses.FirstOrDefault(f => f.isTreeofKind == true);
                Winner = finalresult.Name;
            }
            else if (resultStatuses.GroupBy(g => g.isTreeofKind).Count() > 1)
            {
                var players = resultStatuses.Where(g => g.isTreeofKind == true);
                Winner = "";
                foreach (var player in players)
                {
                    Winner += player.Name + "";
                }
            }

            // Check for one pair or tie for that.
            else if (resultStatuses.Where(p => p.isOnePair == true).Count() == 1)
            {
                var finalresult = resultStatuses.FirstOrDefault(f => f.isOnePair == true);
                Winner = finalresult.Name;
            }
            else if (resultStatuses.GroupBy(g => g.isOnePair).Count() > 1)
            {
                var players = resultStatuses.Where(g => g.isOnePair == true);
                Winner = "";
                foreach (var player in players)
                {
                    Winner += player.Name + "";
                }
            }

            // Check for high card if none of above is successed
            if (resultStatuses.Where(p => p.isFlush == true).Count() == 0 && resultStatuses.Where(p => p.isOnePair == true).Count() == 0 && resultStatuses.Where(p => p.isTreeofKind == true).Count() == 0)
            {

            }

        }

        // sort by enum
        private ICollection<Card> Sort(ICollection<Card> cards)
        {
            cards = cards.OrderBy(e => (int)e.Rank).ToList();
            return cards;
        }

    }
}
