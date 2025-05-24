using UnityEngine;

public class pickup : MonoBehaviour
{
	enum pickupType { weapon, item }
	[SerializeField] pickupType type;

	[SerializeField] spellStats spell;
	[SerializeField] itemStats item;

	bool isFirstTimePickedup;

	private void OnTriggerEnter(Collider other)
	{
		IPickup toPickup = other.GetComponent<IPickup>();

		if (toPickup != null)
		{
			if (type == pickupType.weapon)
				toPickup.GetSpellStats(spell);
			else if (type == pickupType.item)
				toPickup.GetItemStats(item);
			
			Destroy(gameObject);
		}
	}
}
