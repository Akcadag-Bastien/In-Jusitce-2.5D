using UnityEngine;

/// <summary>
/// Base class for defining reusable fight modifiers that can mutate the fight scene
/// as soon as it loads (e.g., altering grid size, tweaking turn rules, etc.).
/// </summary>
public abstract class FightModifier : ScriptableObject
{
    public abstract void Apply(FightContext context);
}
