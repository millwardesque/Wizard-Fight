using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (Health))]
public class CombatantActions : MonoBehaviour {
	GameObject targetedObject;
	List<CombatantAction> actions;
	public GameObject launchPoint;
	public CombatantAction SelectedAction { get; set; }

	// Use this for initialization
	void Awake () {
		if (!launchPoint) {
			Debug.LogError("Unable to initialize combatant actions for '" + gameObject.name + "': No launch point is set.");
		}

		// Load the actions.
		GameObject actionsManagerObj = GameObject.FindGameObjectWithTag("Actions Manager");
		if (!actionsManagerObj || !actionsManagerObj.GetComponent<ActionsManager>()) {
			Debug.LogError ("Unable to initialize combatant actions for '" + gameObject.name + "': No object has the Actions Manager tag, or the tagged Actions Manager is missing the Actions Manager component.");
		}
		ActionsManager actionsManager = actionsManagerObj.GetComponent<ActionsManager>();
		actions = new List<CombatantAction>();

		// @TODO Load these per-combatant from a file / player-prefs / DB.
		actions.Add(((FixedDamageAction)actionsManager.GetAction("FixedDamageAction")).Initialize("Energy Blast (7)", gameObject, null, 7, 5));
		actions.Add(((FixedDamageAction)actionsManager.GetAction("FixedDamageAction")).Initialize("Energy Wave (3)", gameObject, null, 3, 3));
		actions.Add(((FixedDamageAction)actionsManager.GetAction("FixedDamageAction")).Initialize("Slap (1)", gameObject, null, 1, 1));
		actions.Add(((FixedDamageAction)actionsManager.GetAction("FixedDamageAction")).Initialize("Burn (5)", gameObject, null, 5, 4));
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

	/// <summary>
	/// Gets the action count.
	/// </summary>
	/// <returns>The action count.</returns>
	public int GetActionCount() {
		return actions.Count;
	}

	/// <summary>
	/// Gets the launch position for an action.
	/// </summary>
	/// <returns>The launch position.</returns>
	public GameObject GetLaunchPosition() {
		return launchPoint;
	}
}
