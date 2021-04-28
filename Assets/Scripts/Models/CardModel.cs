
namespace kitti
{
    public class CardModel
    {
        public string name;
        public int value;
        public Suits suit;

        public enum Suits
        {
            Club,
            Diamond,
            Heart,
            Spade
        }

        public CardModel(string value, string suit, string name)
        {
            this.name = suit + value;
            if (suit == "Club")
            {
                this.suit = Suits.Club;
            }
            if (suit == "Diamond")
            {
                this.suit = Suits.Diamond;
            }
            if (suit == "Heart")
            {
                this.suit = Suits.Heart;
            }
            if (suit == "Spade")
            {
                this.suit = Suits.Spade;
            }
            if (value == "Ace")
            {
                this.value = 14;
            }
            else if (value == "Jack")
            {
                this.value = 11;
            }
            else if (value == "Queen")
            {
                this.value = 12;
            }
            else if (value == "King")
            {
                this.value = 13;
            }
            else
            {
                this.value = int.Parse(value);
            }
        }

        public int GetValue()
        {
            return this.value;
        }

        public string GetSuits()
        {
            return this.suit.ToString();
        }

        public string GetName()
        {
            return this.name;
        }
    }
}

