using UnityEngine;

[CreateAssetMenu]
public class ItemCount : ScriptableObject
{
    [Header("Ingredents Count")]
    public int beewaxCount;
    public int baconCount;
    public int mushroomCount;

    [Header("Potions Count")]
    public int HealthPotion;
    public int ManaPotion;
    public int HealPlusPotion;
    public int ManaPlusPotion;
}

