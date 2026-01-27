using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Keeps track of which modifiers should be applied when the next fight scene loads.
/// Persisted via DontDestroyOnLoad so overworld triggers can pass data into fights.
/// </summary>
public class FightEncounterService : MonoBehaviour
{
    public static FightEncounterService Instance { get; private set; }

    [SerializeField] private string defaultFightSceneName = "FightScene";

    private readonly List<FightModifier> queuedModifiers = new List<FightModifier>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static FightEncounterService EnsureExists()
    {
        if (Instance != null)
        {
            return Instance;
        }

        GameObject serviceHost = new GameObject(nameof(FightEncounterService));
        return serviceHost.AddComponent<FightEncounterService>();
    }

    public void StartFight(string sceneName, IEnumerable<FightModifier> modifiers)
    {
        if (string.IsNullOrWhiteSpace(sceneName))
        {
            sceneName = defaultFightSceneName;
        }

        QueueModifiers(modifiers);
        SceneManager.LoadScene(sceneName);
    }

    public void StartFight(IEnumerable<FightModifier> modifiers)
    {
        StartFight(defaultFightSceneName, modifiers);
    }

    public IReadOnlyList<FightModifier> ConsumeModifiers()
    {
        FightModifier[] snapshot = queuedModifiers.ToArray();
        queuedModifiers.Clear();
        return snapshot;
    }

    private void QueueModifiers(IEnumerable<FightModifier> modifiers)
    {
        queuedModifiers.Clear();
        if (modifiers == null)
        {
            return;
        }

        foreach (FightModifier modifier in modifiers)
        {
            if (modifier == null)
            {
                continue;
            }

            queuedModifiers.Add(ScriptableObject.Instantiate(modifier));
        }
    }
}
