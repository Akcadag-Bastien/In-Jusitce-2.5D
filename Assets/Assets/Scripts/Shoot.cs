using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{

    [SerializeField] KeyCode shoot = KeyCode.Space;

    public GameObject prefab;

    public TurnManager TurnManager; // !!!!!!! THIS LINE HAS TO CHANGE !!!!!!!

    public GameObject player;

    public transform newposition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(shoot))
        {

            if (TurnManager.HasShot == false)
            {

                Instantiate(prefab, player.transform.position, Quaternion.identity);
                TurnManager.HasShot = true;
            }

            else 
            {

                Debug.Log("Already Shot this turn");
            }
        }
    }
}
