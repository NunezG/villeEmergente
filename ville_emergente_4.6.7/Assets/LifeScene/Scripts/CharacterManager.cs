using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;
using System;

public class CharacterManager : SingletonScriptableObject<CharacterManager>
{
    public GameObject[] CharacterNames;
    public List<GameObject> Cnames = new List<GameObject>();
    private int ancNumC = -1;

    public void UpdateC()
    {
        GameObject[] existingCharacters = GameObject.FindGameObjectsWithTag("Character");
        if (CharacterNames != null) // && ancNumC != existingCharacters.Length)
        {
            Array.Sort(existingCharacters, CompareObNames);

            Cnames.Clear();
            for (int i = 0; i < existingCharacters.Length; i++)
                Cnames.Add(existingCharacters[i]);
            CharacterNames = Cnames.ToArray();
            ancNumC = existingCharacters.Length;
        }
    }

    public int CompareObNames(GameObject x, GameObject y)
    {
        int result = 0;
        Regex re = new Regex(@"(.*)(\d+)$");
        Match m1 = re.Match(x.name);
        Match m2 = re.Match(y.name);
        if (m1.Success && m2.Success)
        {
            result = m1.Groups[1].Value.CompareTo(m2.Groups[1].Value);
            if (result == 0)
                result = int.Parse(m1.Groups[2].Value).CompareTo(int.Parse(m2.Groups[2].Value));
        }
        else
        {
            result = x.name.CompareTo(y.name);
        }
        return result;
    }

}
