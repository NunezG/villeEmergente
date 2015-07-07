using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;
using System;

public class LifeSceneManager : SingletonScriptableObject<LifeSceneManager>
{
    public GameObject[] LifeSceneNames;
    public List<GameObject> LSnames = new List<GameObject>();
    private int ancNumLS = -1;

    public void UpdateLS()
    {
        GameObject[] existingLifescenes = GameObject.FindGameObjectsWithTag("LifeScene");
        if (LifeSceneNames != null) // && ancNumLS != existingLifescenes.Length)
        {
            Array.Sort(existingLifescenes, CompareObNames);

            LSnames.Clear();
            for (int i = 0; i < existingLifescenes.Length; i++)
                if (!existingLifescenes[i].name.Contains("_"))
                    LSnames.Add(existingLifescenes[i]);
            LifeSceneNames = LSnames.ToArray();
            ancNumLS = existingLifescenes.Length;
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
