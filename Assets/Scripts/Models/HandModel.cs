using System.Collections.Generic;

namespace kitti
{
    public class HandModel
    {
        public enum Combination
        {
            Trail,
            DoubleRun,
            Run,
            Color,
            Jute,
            HighCard
        }

        double totalValue;
        public List<CardModel> cards=new List<CardModel>();
        public Combination cardCombination;

        public double GetTotalValue()
        {
            if((cards[0].value) == cards[1].value
                && cards[1].value == cards[2].value)
            {
                this.totalValue = 1000000 + cards[0].value + cards[1].value + cards[2].value;
            }
            else if ((cards[0].suit==cards[1].suit && cards[1].suit == cards[2].suit)&&((cards[0].value + 1) == cards[1].value
                && cards[1].value == cards[2].value - 1))
            {
                this.totalValue = 100000 + cards[0].value + cards[1].value + cards[2].value;
            }
            else if ((cards[0].value + 1) == cards[1].value
                && cards[1].value == cards[2].value - 1)
            {
                this.totalValue = 10000 + cards[0].value + cards[1].value + cards[2].value;
            } else if (cards[0].suit == cards[1].suit && cards[1].suit == cards[2].suit)
            {
                this.totalValue = 1000 + cards[0].value + cards[1].value + cards[2].value;
            }
            else if (cards[0].value == cards[1].value || cards[1].value==cards[2].value)
            {
                this.totalValue = 100 + cards[0].value + cards[1].value + cards[2].value;
            }
            else
            {
                this.totalValue = cards[0].value + cards[1].value + cards[2].value;
            }

            return this.totalValue;
        }
    }
}