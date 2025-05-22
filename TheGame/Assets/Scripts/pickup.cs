using UnityEngine;

public class pickup : MonoBehaviour
{
	[SerializeField] spellStats spell;

	private void OnTriggerEnter(Collider other)
	{
		IPickup pickup = other.GetComponent<IPickup>();

		if (pickup != null)
		{
			pickup.GetSpellStats(spell);

			Destroy(GameObject);
		}
	}
}
