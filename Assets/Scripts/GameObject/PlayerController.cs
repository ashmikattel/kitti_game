using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace kitti
{
    public class PlayerController : MonoBehaviour
    {
        public List<CardModel> cardsInHand = new List<CardModel>();
        public List<GameObject> cardsPrefabList = new List<GameObject>();

        public List<GameObject> cardSlots = new List<GameObject>();
        public GameObject deckPrefab;

        public GameObject cardSlotPrefab;
        public GameObject cardPrefab;

        //for moving in deck
        private CardModel tempCard;
        private GameObject tempCardPrefab;

        private GameObject deck;
        public GameObject handPrefab;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        //TODO change all string to card model
        public void TakeInHand(CardModel card)
        {
            cardsInHand.Add(card);
        }

        //Create and show deck
        public void ShowDeck()
        {
            deck = Instantiate(deckPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z-1.9f), Quaternion.identity, this.transform);
            cardSlots = deck.GetComponent<DeckController>().cardSlots;
            deck.GetComponent<RectTransform>().localScale = new Vector3(20.0f, 20.0f, 20.0f);
            LeanTween.scale(deck, new Vector3(55.9f, 55.9f, 55.9f), 0.8f).setEase(LeanTweenType.easeSpring);
            foreach (var card in cardsInHand)
            {
                /*GameObject cardSlot = Instantiate(cardSlotPrefab, new Vector3(0, 0, 0), Quaternion.identity, deck.transform);
                cardSlot.name = this.name + "slot" + cardSlots.Count;
                cardSlot.GetComponent<CardSlot>().index = cardSlots.Count;
                cardSlots.Add(cardSlot);*/
                Debug.Log("Count"+ cardSlots[cardsPrefabList.Count].name);

                GameObject prefabCard = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity, cardSlots[cardsPrefabList.Count].transform);
                prefabCard.name = card.name;
                prefabCard.transform.position = Vector3.zero;
                prefabCard.transform.localPosition = Vector3.zero;
                cardsPrefabList.Add(prefabCard);
            }
        }

        public void CloseDeck()
        {
            LeanTween.scale(deck, new Vector3(0, 0, 0), 0.6f);
        }

        public void ReadyHand()
        {
            cardsPrefabList.Clear();
            cardSlots.Clear();
            LeanTween.scale(deck, new Vector3(0, 0, 0), 0.6f).setEase(LeanTweenType.easeInOutBounce).setDestroyOnComplete(true);
        }

        public HandModel ShowHand(Transform target, Vector2 handOffset)
        {
            HandModel newHand = new HandModel();
            for (int i = 0; i < 3; i++)
            {
                newHand.cards.Add(cardsInHand[0]);
                cardsInHand.RemoveAt(0);
            }
            GameObject hand = Instantiate(handPrefab,new Vector3(transform.position.x,transform.position.y,transform.position.z),Quaternion.identity,transform);

            hand.GetComponent<HandController>().CreateHand(newHand.cards);
            LeanTween.moveLocal(hand, handOffset, 0.4f);

            return newHand;
        }

        public void ShiftCardsAtLeft(int index)
        {
            Debug.Log("Shifting" + index);
            tempCard = cardsInHand[index];
            tempCardPrefab = cardsPrefabList[index];
            cardsInHand.RemoveAt(index);
            cardsPrefabList.RemoveAt(index);
            for (int i = index; i < cardSlots.Count - 1; i++)
            {
                cardSlots[i + 1].transform.GetChild(0).SetParent(cardSlots[i].transform);
                cardsPrefabList[i].transform.position = cardSlots[i].transform.position;
            }
        }

        public void ShiftCardsAtRight(int index)
        {
            cardsInHand.Insert(index, tempCard);
            cardsPrefabList.Insert(index, tempCardPrefab);
            for (int i = cardSlots.Count - 1; i > index; i--)
            {
                cardSlots[i - 1].transform.GetChild(0).SetParent(cardSlots[i].transform);
                Debug.Log("Card slot" + cardSlots[i].name);
                cardsPrefabList[i].transform.position = cardSlots[i].transform.position;
            }
        }

        public void ArrangeCards()
        {
            List<CardModel> newhands = new List<CardModel>();
            ///for trial
            var trialsGroup = cardsInHand.GroupBy(s => s.value)
                 .Where(g => g.Count() == 3);
            foreach (var trials in trialsGroup)
            {
                Debug.Log("Trial of " + trials.Key);
                foreach (CardModel cards in trials)//Each group has a inner collection  
                {
                    cardsInHand.Remove(cards);
                    newhands.Add(cards);
                }
            }

            ///A,2,3 remains
            //For Double run
            var potetialDoubleRun = cardsInHand.GroupBy(s => s.suit).Where(g => g.Count() > 2);
            foreach (var suitsGroup in potetialDoubleRun)
            {
                Debug.Log("chances of double run in " + suitsGroup.Key);
                //Sort in descending order based on value of the card.
                var sortedGroup = suitsGroup.OrderByDescending(p => p.value);
                //Create a subset of consecutive sequential cards based on value, with a minimum of 3 cards.
                var sequentialCardSet = sortedGroup.FindConsecutiveSequence(p => p.value, 3);
                foreach (var runSequence in sequentialCardSet)
                {
                    Debug.Log("Double Run sequence" + runSequence.name);
                    cardsInHand.Remove(runSequence);
                    newhands.Add(runSequence);
                }
            }

            //For run
            //Sort in descending order based on value of the card.
            cardsInHand.Sort((x, y) => y.value.CompareTo(x.value));

            //Create a subset of distinct card values.
            var distinctCardSet = cardsInHand.Distinct(new CardValueComparer());

            //Create a subset of consecutive sequential cards based on value, with a minimum of 5 cards.
            var sequentialCard = distinctCardSet.FindConsecutiveSequence(p => p.value, 3).ToList();
            foreach (var runSequence in sequentialCard)
            {
                Debug.Log("Run sequence" + runSequence.name);
                cardsInHand.Remove(runSequence);
                newhands.Add(runSequence);
            }

            //For color
            var colorGroup = cardsInHand.GroupBy(s => s.suit)
                 .Where(g => g.Count() == 3);
            foreach (var suit in colorGroup)
            {
                Debug.Log("Color of " + suit.Key);
                foreach (CardModel cards in suit)//Each group has a inner collection  
                {
                    Debug.Log("color sequnce of " + cards.name);
                    cardsInHand.Remove(cards);
                    newhands.Add(cards);
                }
            }

            //For jute
            var cardGroup = cardsInHand.GroupBy(s => s.value);
            var JuteGroup =cardGroup.Where(g => g.Count() == 2);
            foreach (var sameCard in JuteGroup)
            {
                Debug.Log("Jute of " + sameCard.Key);
                foreach (CardModel cards in sameCard)//Each group has a inner collection  
                {
                    Debug.Log("sameCard " + cards.name);
                    cardsInHand.Remove(cards);
                    newhands.Add(cards);
                }
            }
            newhands.AddRange(cardsInHand);
            cardsInHand.Clear();
            cardsInHand = newhands;
        }
    }
}

