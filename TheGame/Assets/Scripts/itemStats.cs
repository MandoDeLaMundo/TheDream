using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class itemStats : ScriptableObject
{
	public GameObject model;
	public string itemName;
	public string itemDescription;
	[Range(0, 10)] public int healFactor;
	[Range(0, 10)] public int ManaFactor;
	public bool firstTime;

	public GameObject pickup;
    public Sprite sprite;
}
