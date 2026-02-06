using UnityEngine;

public static class DamageUtilities
{
    public static void DamageTarget(CharacterStats[] targets, int damageAmount)
    {
        if (targets == null || targets.Length == 0)
        {
            Debug.LogWarning("DamageTarget called with no targets.");
            return;
        }

        foreach (CharacterStats stats in targets)
        {
            if (stats == null)
            {
                continue;
            }

            stats.health = Mathf.Max(0, stats.health - damageAmount);
            UpdateHealth(stats);
        }
    }

    public static void UpdateHealth(CharacterStats target)
    {
        if (target == null)
        {
            Debug.LogWarning("UpdateHealth called with a null target.");
            return;
        }

        target.UpdateHealthLabel();
    }
}
