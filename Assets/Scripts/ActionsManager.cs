using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ActionsManager : MonoBehaviour {
	public List<CombatantAction> actionSet;

	// Use this for initialization
	void Start () {
		Debug.Log (string.Format ("{0} actions loaded in the Action Manager", actionSet.Count));
	}

	/// <summary>
	/// Gets the named action.
	/// </summary>
	/// <returns>The action.</returns>
	/// <param name="name">Name of the action.</param>
	public CombatantAction GetAction(string name) {
		foreach (CombatantAction action in actionSet) {
			if (action.name == name) {
				CombatantAction newAction = (CombatantAction)GameObject.Instantiate(action);
				newAction.transform.parent = transform; // Attach to the action-manager in order to keep the Unity hierarchy editor clean.
				return newAction;
			}
		}

		throw new IndexOutOfRangeException("Unable to find combatant action with name '" + name + "'");
	}
}
