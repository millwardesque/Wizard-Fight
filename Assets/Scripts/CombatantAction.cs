using UnityEngine;
using System.Collections;

/// <summary>
/// Base class for all combat actions.
/// </summary>
public class CombatantAction : MonoBehaviour {
	public GameObject Sender { get; set; }
	public GameObject Receiver { get; set; }

	/// <summary>
	/// Virtual method for performing the method.
	/// </summary>
	public virtual void DoAction() {
	
	}

	public override string ToString () {
		string description;
		if (Sender != null && Receiver != null) {
			description = "Performing action " + name + " from " + Sender + " to " + Receiver;
		}
		else if (Sender != null) {
			description = "Performing action " + name + " from " + Sender + " to no receiver";
		}
		else if (Receiver != null) {
			description = "Performing action " + name + " from no sender to " + Receiver;
		}
		else {
			description = "Performing action " + name + " from no sender to no receiver";
		}

		return description;
	}
}
