using UnityEngine;

[CreateAssetMenu]
public class spellStats : ScriptableObject
{
    public GameObject model;
    public GameObject spellProjectile;
    [Range(1, 20)] public int shootDMG;
    [Range(0.0f, 25.0f)] public float shootRate;
    [Range(1, 1000)] public int shootDist;
    [Range(0, 200)] public int manaCost;
    [Range(0.0f, 100.0f)] public float spellRadius;

    public ParticleSystem hitEffect;
    public AudioClip[] shootSound;
    [Range(0, 1)] public float shootSoundVol;

    public bool spellCheck;
    public string spellManual;
}
