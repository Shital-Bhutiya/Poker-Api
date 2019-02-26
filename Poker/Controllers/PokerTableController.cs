using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Poker.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        public string Get([FromUri]string pokerTable)
        {
            // deserialize the json object
            PokerTable table = JsonConvert.DeserializeObject<PokerTable>(pokerTable);
            return getPokerHands(table.PokerHands);
        }

        private string getPokerHands(ICollection<PokerHand> pokerHands)
        {
            resultStatuses = new HashSet<ResultStatus>();
            //set results
            foreach (PokerHand hand in pokerHands)
            {
                var resultStatus = new ResultStatus();
                resultStatus.Name = hand.Name;
                isPokerHand(hand.Cards, resultStatus);
                resultStatuses.Add(resultStatus);
            }
            //check winner
            checkWinner(resultStatuses);    
            return Winner;
        }

        private void isPokerHand(ICollection<Card> cards, ResultStatus resultStatus)
        {
            resultStatus.isFullHouse = checkFullHouse(cards);
            resultStatus.isFlush = checkFlush(cards);
            resultStatus.isFourOfKind = checkFourOfKind(cards);
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
                        .Where(g => g.Count() == 3)
                        .Count() == 1;
        }

        public bool checkFourOfKind(ICollection<Card> Cards)
        {
            return Cards.GroupBy(h => h.Rank)
                        .Where(g => g.Count() == 4)
                        .Count() == 1;
        }

        public bool checkFullHouse(ICollection<Card> Cards)
        {
            return checkThreeOfKind(Cards) && checkOnePair(Cards);
        }

        private void checkWinner(ICollection<ResultStatus> resultStatuses)
        {
            // check for four of kind or tie 
            if (resultStatuses.Where(g => g.isFourOfKind == true).GroupBy(g => g.isFourOfKind).Count() > 1)
            {
                var players = resultStatuses.Where(g => g.isFourOfKind == true).ToList();
                Winner = "The Game is Tie : ";
                foreach (var player in players)
                {
                    Winner += player.Name + "";
                }
            }
            else if (resultStatuses.Where(g => g.isFourOfKind == true).GroupBy(g => g.isFourOfKind).Count() == 1)
            {
                var finalresult = resultStatuses.FirstOrDefault(f => f.isFourOfKind == true);
                Winner = "The Winner is : " + finalresult.Name;
            }

            // Check for full house winner or tie
            else if (resultStatuses.Where(g => g.isFullHouse == true).GroupBy(g => g.isFullHouse).Count() > 1)
            {
                var players = resultStatuses.Where(g => g.isFullHouse == true).ToList();
                Winner = "The Game is tie with:";
                foreach (var player in players)
                {
                    Winner += player.Name + " ";
                }
            }
            else if (resultStatuses.Where(g => g.isFullHouse == true).GroupBy(g => g.isFullHouse).Count() == 1)
            {
                var finalresult = resultStatuses.Where(f => f.isFullHouse == true).FirstOrDefault();
                Winner = "The Winner is " + finalresult.Name;
            }

            // check for flush winner or tie
            else if (resultStatuses.Where(g => g.isFlush == true).GroupBy(g => g.isFlush).Count() > 1)
            {
                var players = resultStatuses.Where(g => g.isFlush == true).ToList();
                Winner = "The Game is tie with:";
                foreach (var player in players)
                {
                    Winner += player.Name + " ";
                }
            }
            else if (resultStatuses.Where(g => g.isFlush == true).GroupBy(g => g.isFlush).Count() == 1)
            {
                var finalresult = resultStatuses.Where(f => f.isFlush == true).FirstOrDefault();
                Winner = "The Winner is "+finalresult.Name;
            }

            // Check for Three of kind posibility or tie
            else if (resultStatuses.Where(g => g.isTreeofKind == true).GroupBy(g => g.isTreeofKind).Count() > 1)
            {
                var players = resultStatuses.Where(g => g.isTreeofKind == true).ToList();
                Winner = "The Game is Tie : ";
                foreach (var player in players)
                {
                    Winner += player.Name + "";
                }
            }
            else if (resultStatuses.Where(g => g.isTreeofKind == true).GroupBy(g => g.isTreeofKind).Count() == 1)
            {
                var finalresult = resultStatuses.FirstOrDefault(f => f.isTreeofKind == true);
                Winner = "The Winner is : "+finalresult.Name;
            }

            // Check for one pair or tie
            else if (resultStatuses.Where(g => g.isOnePair == true).GroupBy(g => g.isOnePair).Count() > 1)
            {
                var finalresult = resultStatuses.Where(f => f.isOnePair == true).FirstOrDefault();
                Winner = "The Winner is : " + finalresult.Name;
            }
            else if (resultStatuses.Where(g => g.isOnePair == true).GroupBy(g => g.isOnePair).Count() == 1)
            {
                var players = resultStatuses.Where(g => g.isOnePair == true).ToList();
                Winner = "The Game is tie with :";
                foreach (var player in players)
                {
                    Winner += player.Name + " ";
                }
            }
        }
    }
}
