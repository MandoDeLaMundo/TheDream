using UnityEngine;

[CreateAssetMenu]
public class spellStats : ScriptableObject
{
    public GameObject model;
    [Range(1, 20)] public int shootDMG;
    [Range(1.0f, 25.0f)] public float shootRate;
    [Range(1, 1000)] public int shootDist;
    [Range(1, 1000)] public int currMana;
    [Range(1, 1000)] public int maxMana;
    [Range(1, 200)] public int manaCost;

    public ParticleSystem hitEffect;
    public AudioClip[] shootSound;
    [Range(0, 1)] public float shootSoundVol;
}
