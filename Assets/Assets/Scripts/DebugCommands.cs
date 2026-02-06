using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCommands : MonoBehaviour
{

    public int TestDamage = 11;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            DamageAll();
        }
    }

    public void DamageAll()
    {
        CharacterStats[] characters = CharacterUtilities.GetAllCharacters();
        DamageUtilities.DamageTarget(characters, TestDamage);
    }

    public void DamageTarget(CharacterStats[] targets, int damageAmount)
    {
        DamageUtilities.DamageTarget(targets, damageAmount);
    }

    public void UpdateHealth(CharacterStats target)
    {
        DamageUtilities.UpdateHealth(target);
    }
}
