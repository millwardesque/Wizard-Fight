using UnityEngine;
using System.Collections;

/// <summary>
/// Base class for all combat actions.
/// </summary>
public class CombatantAction {
	public GameObject Sender { get; set; }
	public GameObject Receiver { get; set; }

	string name;
	public string Name { get { return name; } }

	/// <summary>
	/// Initializes a new instance of the <see cref="CombatantAction"/> class.
	/// </summary>
	/// <param name="name">Name.</param>
	public CombatantAction(string name, GameObject sender) {
		this.name = name;
		this.Sender = sender;
	}

	/// <summary>
	/// Virtual method for performing the method.
	/// </summary>
	public virtual void DoAction() {
		Debug.Log (this);
	}

	public override string ToString () {
		string description;
		if (Sender != null && Receiver != null) {
			description = "Performing action " + Name + " from " + Sender + " to " + Receiver;
		}
		else if (Sender != null) {
			description = "Performing action " + Name + " from " + Sender + " to no receiver";
		}
		else if (Receiver != null) {
			description = "Performing action " + Name + " from no sender to " + Receiver;
		}
		else {
			description = "Performing action " + Name + " from no sender to no receiver";
		}

		return description;
	}
}
