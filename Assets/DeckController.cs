using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    public GameObject cardSlot0;
    public GameObject cardSlot1;
    public GameObject cardSlot2;
    public GameObject cardSlot3;
    public GameObject cardSlot4;
    public GameObject cardSlot5;
    public GameObject cardSlot6;
    public GameObject cardSlot7;
    public GameObject cardSlot8;

    public List<GameObject> cardSlots;
    // Start is called before the first frame update
   
    void Awake()
    {
        cardSlots = new List<GameObject>()
    {
        cardSlot0,
        cardSlot1,
        cardSlot2,
        cardSlot3,
        cardSlot4,
        cardSlot5,
        cardSlot6,
        cardSlot7,
        cardSlot8
    };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
