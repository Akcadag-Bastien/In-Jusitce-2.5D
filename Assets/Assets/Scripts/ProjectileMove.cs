using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMove : MonoBehaviour
{

    [SerializeField] private TurnManager TurnManager;

    // Start is called before the first frame update
    void Start()
    {
        TurnManager = FindObjectOfType<TurnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
