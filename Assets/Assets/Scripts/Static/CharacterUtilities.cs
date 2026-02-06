using System.Collections.Generic;
using UnityEngine;

public static class CharacterUtilities
{
    public static CharacterStats[] GetAllCharacters()
    {
        return Object.FindObjectsOfType<CharacterStats>();
    }

    public static CharacterStats[] GetPlayers()
    {
        return FilterByTag("Player");
    }

    public static CharacterStats[] GetEnemies()
    {
        return FilterByTag("Enemy");
    }

    private static CharacterStats[] FilterByTag(string tag)
    {
        CharacterStats[] allCharacters = GetAllCharacters();
        List<CharacterStats> filtered = new List<CharacterStats>();

        foreach (CharacterStats character in allCharacters)
        {
            if (character != null && character.CompareTag(tag))
            {
                filtered.Add(character);
            }
        }

        return filtered.ToArray();
    }
}
