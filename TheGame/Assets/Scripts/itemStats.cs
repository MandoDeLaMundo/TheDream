using UnityEngine;

[CreateAssetMenu]
public class itemStats : ScriptableObject
{
	public GameObject model;
	public string itemName;
	public string itemDescription;
	[Range(0, 10)] public int healFactor;
}
