using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

namespace kitti
{
    public class GameController : MonoBehaviour
    {
        public Sprite[] cardFaces = new Sprite[52];

        public static string[] suits = new string[] { "Club", "Diamond", "Heart", "Spade" };
        public static string[] values = new string[] { "Ace", "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King" };

        public List<CardModel> deck;

        //player no
        public int noOfPlayer = 2;

        public List<GameObject> participants;
      //  public List<int> participantsCoin;
        public List<GameObject> participantsCoinTm;
        public int tableCoin = 0;
        public Canvas canvas;

        public GameObject placeholderPrefab;
        public GameObject cardBackPrefab;

        public static Vector2[] playersPosition = new Vector2[]
        {
            new Vector2(35,-232),
            new Vector2(35,232),
            new Vector2(-291,0),
            new Vector2(507,0)
        };

        public static Vector2[] handPosition = new Vector2[]
        {
            new Vector2(-90,164),
            new Vector2(-90,-164),
            new Vector2(-354,0),
            new Vector2(400,0)
        };

        public List<string> roundWinner = new List<string>();

        void Start()
        {
            participants = new List<GameObject>();
            SetParticipants();
            PlayCards();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetParticipants()
        {
            tableCoin = 0;
         
            if (ScoreController.Instance.participateCoin == null)
                ScoreController.Instance.participateCoin = new List<int>();

            for (int i = 0; i < noOfPlayer; i++)
            {
                GameObject participant = Instantiate(placeholderPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, canvas.transform);
                participant.name = "Player " + (i + 1);
                participant.GetComponentInChildren<NameManager>().SetName("Player " + (i + 1));
                RectTransform placeholderRect = participant.GetComponent<RectTransform>();
                placeholderRect.localPosition = playersPosition[i];
                ScoreController.Instance.participateCoin.Add (100);

                Debug.Log(ScoreController.Instance.participateCoin[0]);
               // participantsCoinTm[i].GetComponent<TextMeshPro>().text = ""+ participantsCoin[i];

                participants.Add(participant);
            }
            UpdateCoin();
        }
       
        public void PlayCards()
        {
            roundWinner = new List<string>();
            tableCoin += 10 * participants.Count;
            for(int i = 0; i< ScoreController.Instance.participateCoin.Count; i++)
            {
                ScoreController.Instance.participateCoin[i] -= 10;
            }
            UpdateCoin();
            deck = GenerateDeck();
            Shuffle(deck);
            StartCoroutine(Deal());

        }

        public static List<CardModel> GenerateDeck()
        {
            List<CardModel> newDeck = new List<CardModel>();
            foreach (string suit in suits)
            {
                foreach (string value in values)
                {
                    CardModel newCard = new CardModel(value, suit, (suit + value));
                    newDeck.Add(newCard);
                }
            }
            return newDeck;
        }

        //officiate shuffle
        void Shuffle<T>(List<T> list)
        {
            System.Random random = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                int k = random.Next(n);
                n--;
                T temp = list[k];
                list[k] = list[n];
                list[n] = temp;
            }
        }

        //Distribute card and also prefab of the card
        IEnumerator Deal()
        {
            for (int j = 0; j < 9; j++)
            {
                for (int i = 0; i < noOfPlayer; i++)
                {
                    PlayerController playerController = participants[i].GetComponent<PlayerController>();
                    CardModel card = deck.Last<CardModel>();
                    deck.RemoveAt(deck.Count - 1);
                    playerController.TakeInHand(card);

                    GameObject newCard = Instantiate(cardBackPrefab, new Vector3(canvas.transform.position.x, canvas.transform.position.y, canvas.transform.position.z - 0.2f), Quaternion.identity, canvas.transform);
                    newCard.name = card.GetName();
                    LeanTween.move(newCard,participants[i].transform.position,0.4f).setEase(LeanTweenType.pingPong).setDestroyOnComplete(true);
                    yield return new WaitForSeconds(0.3f);
                }
            }
            participants[0].GetComponent<PlayerController>().ShowDeck();
        }

        void DestroyPrefab(GameObject prefab)
        {
            Destroy(prefab);
        }

        public void OnShowButtonClicked()
        {
          //GameObject.Find("Button").SetActive(false);
            participants[0].GetComponent<PlayerController>().CloseDeck();
            StartCoroutine(ShowCard());
        }

        IEnumerator ShowCard()
        {
            //of Bot player
            participants[1].GetComponent<PlayerController>().ArrangeCards();
            //remove deck of player
            participants[0].GetComponent<PlayerController>().ReadyHand();
            for(int i =0; i<3; i++)
            {
                double[] participantsScore = new double[4] { 0,0,0,0};
                HandModel hand;
                for(int j = 0; j < noOfPlayer; j++)
                {
                    PlayerController playerController = participants[j].GetComponent<PlayerController>();
                    hand= playerController.ShowHand(canvas.transform,handPosition[j]);
                    participantsScore[j] = hand.GetTotalValue();
                }

                double maxValue = participantsScore.Max();
                int playerIndex = participantsScore.ToList().IndexOf(maxValue);

                roundWinner.Add(participants[playerIndex].name);

                for (int j = 0; j < noOfPlayer; j++)
                {
                    participants[j].GetComponentInChildren<WinStatController>().UpdateWinningStatus(roundWinner);
                }
                
                yield return new WaitForSeconds(5.6f);
            }

            SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
            if (roundWinner[0] == roundWinner[1] || roundWinner[1] == roundWinner[2])
            {
                if(roundWinner[1]=="Player 1")
                {
                    Debug.Log("Yeah winner");
                    int index = participants.FindIndex(item => item.name == "Player 1");
                    ScoreController.Instance.participateCoin[index] += tableCoin;
                    tableCoin = 0;
                    sceneLoader.LoadScene(4);
                }
                else
                {
                    int index = participants.FindIndex(item => item.name == "Player 2");
                    ScoreController.Instance.participateCoin[index] += tableCoin;
                    tableCoin = 0;
                    Debug.Log("Lose");
                    sceneLoader.LoadScene(3);
                }
            } else
            {
                Debug.Log("Kitty by" + roundWinner[2]);
               
                PlayCards();
            }
                tableCoin += 10 * participants.Count;
            Debug.Log("kitty coin" + ScoreController.Instance.participateCoin[0]);
            UpdateCoin();
        }

        public void UpdateCoin()
        {
            GameObject.Find("Score").GetComponent<TextMeshProUGUI>().text = ScoreController.Instance.participateCoin[0].ToString();
            GameObject.Find("Score_Opp").GetComponent<TextMeshProUGUI>().text = ScoreController.Instance.participateCoin[1].ToString();

        }
    }
}

