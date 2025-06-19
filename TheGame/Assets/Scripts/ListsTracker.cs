using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class ListsTracker : ScriptableObject
{
    public List<spellStats> spellList = new List<spellStats>();
    public int spellListPos;
    public List<itemStats> ItemList = new List<itemStats>();
}
