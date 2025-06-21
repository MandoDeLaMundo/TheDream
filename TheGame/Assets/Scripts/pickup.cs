using UnityEngine;

public class pickup : MonoBehaviour
{
	enum pickupType { weapon, item }
	[SerializeField] pickupType type;

	[SerializeField] spellStats spell;
	[SerializeField] itemStats item;

	[SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] audPick;
	[Range(0, 10)][SerializeField] float audPickVol;
    //bool isFirstTimePickedup;

    private void OnTriggerEnter(Collider other)
	{
		aud.PlayOneShot(audPick[Random.Range(0, audPick.Length)], audPickVol);
        IPickup toPickup = other.GetComponent<IPickup>();

		if (toPickup != null)
		{
			if (type == pickupType.weapon)
			{
				//Debug.Log("Picked up a weapon");
				toPickup.GetSpellStats(spell);
			}
			else if (type == pickupType.item)
			{
				//Debug.Log("Picked up an item");
				toPickup.GetItemStats(item);
			}
			
			Destroy(gameObject);
		}
	}
}
