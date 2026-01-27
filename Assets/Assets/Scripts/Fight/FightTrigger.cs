using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FightTrigger : MonoBehaviour
{
    [SerializeField] private string fightSceneName = "FightScene";
    [SerializeField] private List<FightModifier> modifiers = new List<FightModifier>();
    [SerializeField] private bool oneShot = true;

    private bool hasTriggered;

    private void Reset()
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (hasTriggered && oneShot)
        {
            return;
        }

        FightEncounterService service = FightEncounterService.EnsureExists();
        service.StartFight(fightSceneName, modifiers);

        if (oneShot)
        {
            hasTriggered = true;
        }
    }
}
