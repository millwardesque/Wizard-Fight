using UnityEngine;
using System.Collections;

/// <summary>
/// Fixed damage action.
/// </summary>
public class FixedDamageAction : CombatantAction {
	public int DamageDealt { get; set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="FixedDamageAction"/> class.
	/// </summary>
	/// <param name="name">Name.</param>
	public FixedDamageAction(string name, GameObject sender, int damage) : base(name, sender) {
		DamageDealt = damage;
	}

	/// <summary>
	/// Virtual method for performing the action.
	/// </summary>
	public override void DoAction ()
	{
		if (Sender != null && Receiver != null && Receiver.GetComponent<Health>()) {
			Receiver.GetComponent<Health>().AddHealth(-DamageDealt);
		}
		else {
			Debug.Log ("Unable to do action " + Name + ": Either the sender/receiver are null, or the receiver doesn't have a Health component.");
		}
		base.DoAction ();
	}
}
