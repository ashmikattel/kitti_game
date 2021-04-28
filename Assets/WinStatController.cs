using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinStatController : MonoBehaviour
{
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void UpdateWinningStatus(List<string> roundWinner)
    {
        foreach (Transform child in rectTransform.transform)
        {
            Destroy(child.gameObject);
        }
        Debug.Log("Parent name================================"+rectTransform.parent.name);

        foreach (string winner in roundWinner)
        {
            GameObject circle = new GameObject("square");
            circle.AddComponent<RectTransform>();
            circle.AddComponent<CanvasRenderer>();

            circle.GetComponent<RectTransform>().SetParent(rectTransform);
            circle.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 10);
            circle.GetComponent<RectTransform>().position = new Vector3(0, 0, 0);
            circle.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            circle.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            if (this.rectTransform.parent.name == winner)
            {
                Debug.Log("Winner");
                Image i = circle.AddComponent<Image>();
                i.color = Color.green;
            }
            else
            {
                Debug.Log("Loser");
                Image i = circle.AddComponent<Image>();
                i.color = Color.red;
            }
        }
    }
}
