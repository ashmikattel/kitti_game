using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace kitti
{
    public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        private CanvasGroup canvasGroup;
        Transform parentToReturnTo = null;
        private RectTransform rectTransform;

        Vector3 positionToReturnTo;

        private AudioSource cardSwapSound;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            cardSwapSound = GetComponent<AudioSource>();
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            parentToReturnTo = this.transform.parent;
            positionToReturnTo = this.transform.position;

            transform.parent.parent.parent.GetComponent<PlayerController>().ShiftCardsAtLeft(transform.parent.GetComponent<CardSlot>().index);

            rectTransform.SetParent(null);
            canvasGroup.blocksRaycasts = false;
            this.gameObject.GetComponent<RectTransform>().transform.position += new Vector3(0,0,-1.4f);
            cardSwapSound.Play();
            LeanTween.scale(this.gameObject,new Vector3(1.5f,1.5f,1.5f),0.6f).setEase(LeanTweenType.easeSpring);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Debug.Log("OnDrag");
            Vector2 localPos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, Camera.main, out localPos);
            transform.position = transform.TransformPoint(localPos);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (transform.parent == null)
            {
                Debug.Log("Drag stopped");
                this.gameObject.GetComponent<RectTransform>().transform.position -= new Vector3(0, 0, -1.4f);

                parentToReturnTo.parent.parent.GetComponent<PlayerController>().ShiftCardsAtRight(parentToReturnTo.GetComponent<CardSlot>().index);
                rectTransform.SetParent(parentToReturnTo);
                transform.position = positionToReturnTo;
                LeanTween.scale(this.gameObject, new Vector3(1.4f, 1.4f, 1.4f), 0.6f).setEase(LeanTweenType.easeSpring);
            }
            canvasGroup.blocksRaycasts = true;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("onPointerDown");
        }
    }
}

