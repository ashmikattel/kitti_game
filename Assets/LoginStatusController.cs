using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoginStatusController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectOfType<FaceBookUtils>().isLoggedIn)
        {
            GetComponent<TextMeshProUGUI>().text = "Facebook logged in";
        }
        else
        {
            GetComponent<TextMeshProUGUI>().text = "";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
