using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (Health))]
public class CombatantActions : MonoBehaviour {
	GameObject targetedObject;
	List<CombatantAction> actions;

	// Use this for initialization
	void Awake () {
		// Load the actions.
		actions = new List<CombatantAction>();

		// @TODO Load these per-combatant from a file / player-prefs / DB.
		actions.Add (new FixedDamageAction("Energy blast", gameObject, 7));
		actions.Add (new FixedDamageAction("Energy wave", gameObject, 3));
		actions.Add (new FixedDamageAction("Slap", gameObject, 1));
		actions.Add (new FixedDamageAction("Burn", gameObject, 5));
	}

	/// <summary>
	/// Sets the player's current target.
	/// </summary>
	/// <param name="target">Target.</param>
	public void SetTarget(GameObject target) {
		targetedObject = target;
	}

	/// <summary>
	/// Gets the combatant's current target.
	/// </summary>
	/// <returns>The target.</returns>
	public GameObject GetTarget() {
		return targetedObject;
	}

	/// <summary>
	/// Gets the requested action.
	/// </summary>
	/// <returns>The action.</returns>
	/// <param name="index">Index.</param>
	public CombatantAction GetAction(int index) {
		if (index < 0 || index >= actions.Count) {
			throw new IndexOutOfRangeException("Unable to get combatant action '" + index + "': Index is out of range.");
		}

		actions[index].Receiver = targetedObject;
		return actions[index];
	}
}
