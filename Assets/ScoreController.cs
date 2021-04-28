using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ScoreController 
{
    private ScoreController() { 
    }
    private static ScoreController instance = null;
    public List<int> participateCoin;
   

    public static ScoreController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ScoreController();
            }
            return instance;
        }
    }
}
