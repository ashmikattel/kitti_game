using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kitti
{
    // Match card sprite to name
    public class UpdateSprite : MonoBehaviour
    {
        //for swap
        public Sprite cardFace;
        public Sprite cardBack;

        private Selectable selectable;
        private SpriteRenderer spriteRenderer;
        private GameController gameController;

        public CardModel cardData;

        void Start()
        {
            List<CardModel> deck = GameController.GenerateDeck();
            gameController = FindObjectOfType<GameController>();

            int i = 0;
            foreach (CardModel card in deck)
            {
                if (this.name == card.GetName())
                {
                    cardFace = gameController.cardFaces[i];
                    break;
                }
                i++;
            }
            spriteRenderer = GetComponent<SpriteRenderer>();
            selectable = GetComponent<Selectable>();
        }

        // Update is called once per frame
        void Update()
        {
            if (selectable.faceUp == true)
            {
                spriteRenderer.sprite = cardFace;
            }
            else
            {
                spriteRenderer.sprite = cardBack;
            }
        }
    }
}
