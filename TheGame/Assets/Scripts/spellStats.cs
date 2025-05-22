using UnityEngine;

[CreateAssetMenu]
public class spellStats : ScriptableObject
{
    public GameObject model;
    [Range(1, 20)] public int shootDMG;
    [Range(0.0f, 25.0f)] public float shootRate;
    [Range(1, 1000)] public int shootDist;
    [Range(1, 200)] public int manaCost;

    public ParticleSystem hitEffect;
    public AudioClip[] shootSound;
    [Range(0, 1)] public float shootSoundVol;
}
