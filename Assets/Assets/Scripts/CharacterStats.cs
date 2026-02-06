using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    public int health = 100;

    public int maxHealth = 100;

    public int damage = 100;

    public string characterName = "";

    [SerializeField]
    private TextMeshPro healthLabel;

    public TextMeshPro HealthLabel => healthLabel;

    void Awake()
    {
        CacheHealthLabel();
        UpdateHealthLabel();
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealthLabel()
    {
        if (healthLabel == null)
        {
            CacheHealthLabel();
            if (healthLabel == null)
            {
                Debug.LogWarning($"No TextMeshPro label assigned on {name}.");
                return;
            }
        }

        healthLabel.text = health.ToString();

        if (health == 0)
        {
            Debug.LogWarning(characterName + " has died");
            if (gameObject.CompareTag("Enemy"))
            {
                Destroy(gameObject);
            }
        }
    }

    private void CacheHealthLabel()
    {
        if (healthLabel == null)
        {
            healthLabel = GetComponentInChildren<TextMeshPro>();
        }
    }
}
