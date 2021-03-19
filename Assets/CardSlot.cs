using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace kitti
{
    public class CardSlot : MonoBehaviour, IDropHandler
    {
        public int index;
        private AudioSource onCardDrop;

        public void Start()
        {
            onCardDrop = GetComponent<AudioSource>();
        }
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                onCardDrop.Play();
                transform.parent.parent.GetComponent<PlayerController>().ShiftCardsAtRight(index);
                eventData.pointerDrag.GetComponent<RectTransform>().SetParent(transform);
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                eventData.pointerDrag.GetComponent<RectTransform>().localPosition = Vector3.zero;
                LeanTween.scale(eventData.pointerDrag, new Vector3(1.4f, 1.4f, 1.4f), 0.6f).setEase(LeanTweenType.easeSpring);
            }
        }
    }
}

