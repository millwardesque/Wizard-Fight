using UnityEngine;
using System.Collections;

/// <summary>
/// Fixed damage action.
/// </summary>
public class FixedDamageAction : CombatantAction {
	public int DamageDealt { get; set; }
	public GameObject actionFX;

	void Start() {
		if (actionFX == null) {
			Debug.LogWarning("FixedDamagerAction '" + name + "' has no action FX attached.");
		}
	}

	/// <summary>
	/// Initialize the specified sender, receiver and damageDealt.  Shortcut method rather than setting the properties individually.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="receiver">Receiver.</param>
	/// <param name="damageDealt">Damage dealt.</param>
	public FixedDamageAction Initialize(string name, GameObject sender, GameObject receiver, int damageDealt) {
		this.name = name;
		this.Sender = sender;
		this.Receiver = receiver;
		this.DamageDealt = damageDealt;

		return this;
	}

	/// <summary>
	/// Virtual method for performing the action.
	/// </summary>
	public override IEnumerator DoAction () {
		if (Sender != null && Receiver != null && Receiver.GetComponent<Health>()) {
			GameObject launchPosition = Sender.GetComponent<CombatantActions>().GetLaunchPosition();

			// Create the special FX.
			GameObject newFX = (GameObject)GameObject.Instantiate(actionFX);
			newFX.transform.parent = launchPosition.transform;
			newFX.transform.localPosition = Vector3.zero;
			newFX.transform.localRotation = Quaternion.identity;

			// Shoot FX at Receiver and wait for it finish.
			yield return StartCoroutine(newFX.GetComponent<ActionFX>().Fire(Receiver));
			Receiver.GetComponent<Health>().AddHealth(-DamageDealt);
		}
		else {
			Debug.Log ("Unable to do action " + name + ": Either the sender/receiver are null, or the receiver doesn't have a Health component.");
		}
		yield return base.DoAction ();
	}
}
