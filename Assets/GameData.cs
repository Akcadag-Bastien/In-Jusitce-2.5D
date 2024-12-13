using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public int playerMove = 0;
    public int playerMaxMove = 1;



    void Update()
    {
        Debug.Log(playerMove);
        Debug.Log(playerMaxMove);
    }

}