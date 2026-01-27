using UnityEngine;

[CreateAssetMenu(menuName = "Fight/Fight Modifiers/Grid Size Modifier")]
public class GridSizeModifier : FightModifier
{
    [SerializeField] private int widthDelta = 0;
    [SerializeField] private int lengthDelta = 0;

    public override void Apply(FightContext context)
    {
        if (context == null)
        {
            Debug.LogWarning("Cannot apply a grid size modifier without a valid FightContext.");
            return;
        }

        context.AdjustGridSize(widthDelta, lengthDelta);
    }
}
