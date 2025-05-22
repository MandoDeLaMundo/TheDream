using UnityEngine;

public class pickup : MonoBehaviour
{
	[SerializeField] spellStats spell;

	private void OnTriggerEnter(Collider other)
	{
		IPickup toPickup = other.GetComponent<IPickup>();

		if (toPickup != null)
		{
			Debug.Log("Pickup is not null");
			toPickup.GetSpellStats(spell);

			Destroy(gameObject);
		}
	}
}
