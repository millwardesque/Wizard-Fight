using UnityEngine;
using System.Collections;

public enum CombatantActionState {
	Waiting,
	PrecastSetup,
	Precast,
	Cast,
	Done
};

/// <summary>
/// Base class for all combat actions.
/// </summary>
public class CombatantAction : MonoBehaviour {
	public GameObject Sender { get; set; }
	public GameObject Receiver { get; set; }
	protected bool canGoToNextState = false;
	protected delegate IEnumerator StateUpdateDelegate();
	protected StateUpdateDelegate StateUpdate;
	CombatantActionState state = CombatantActionState.Waiting;

	void Start() {
		state = CombatantActionState.Waiting;
		StateUpdate = WaitingUpdate;

	}
	/// <summary>
	/// Virtual method for performing the method.
	/// </summary>
	public virtual IEnumerator DoAction() {
		canGoToNextState = true;
		state = CombatantActionState.Waiting;
		while (state != CombatantActionState.Done) {
			if (canGoToNextState) {
				GoToNextState();
				if (state == CombatantActionState.Done) {
					break;
				}
			}
			yield return StartCoroutine(StateUpdate());
		}

		yield return null;
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
		description += " in state '" + state + "'";

		return description;
	}

	protected void GoToNextState() {
		canGoToNextState = false;

		switch(state) {
		case CombatantActionState.Waiting:
			state = CombatantActionState.PrecastSetup;
			StateUpdate = PrecastSetupUpdate;
			break;
		case CombatantActionState.PrecastSetup:
			state = CombatantActionState.Precast;
			StateUpdate = PrecastUpdate;
			break;
		case CombatantActionState.Precast:
			state = CombatantActionState.Cast;
			StateUpdate = CastUpdate;
			break;
		case CombatantActionState.Cast:
			state = CombatantActionState.Done;
			break;
		default:
			break;
		}
	}

	protected virtual IEnumerator WaitingUpdate() {
		canGoToNextState = true;
		yield return null;
	}

	protected virtual IEnumerator PrecastSetupUpdate() {
		canGoToNextState = true;
		yield return null;
	}

	protected virtual IEnumerator PrecastUpdate() {
		canGoToNextState = true;
		yield return null;
	}

	protected virtual IEnumerator CastUpdate() {
		canGoToNextState = true;
		yield return null;
	}
}
