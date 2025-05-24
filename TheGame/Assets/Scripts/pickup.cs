using UnityEngine;

public class pickup : MonoBehaviour
{


	[SerializeField] spellStats spell;
	//[SerializeField] itemStats item;

	private void OnTriggerEnter(Collider other)
	{
		IPickup toPickup = other.GetComponent<IPickup>();

		if (toPickup != null)
		{
			toPickup.GetSpellStats(spell);

			Destroy(gameObject);
		}
	}
}
