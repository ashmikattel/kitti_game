using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameManager : MonoBehaviour
{
    void Start()
    {
    }
    
    public void SetName(string name)
    {
        this.GetComponent<TextMeshProUGUI>().text = name;
    }
}
