using UnityEngine;

[CreateAssetMenu]
public class ItemCount : ScriptableObject
{
    [Header("Ingredents Count")]
    public int beewaxCount;
    public int baconCount;
    public int mushroomCount;
    public int venomGlandCount;
    public int cinnamonCount;
    public int leafCount;

    [Header("Potions Count")]
    public int HealthPotion;
    public int ManaPotion;
    public int HealPlusPotion;
    public int ManaPlusPotion;
}

