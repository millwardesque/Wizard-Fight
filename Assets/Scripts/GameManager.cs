using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	GUIManager guiManager;
	GameObject player;

	GameObject targetingIndicator;
	bool isCurrentlyTargeting = false;
	public GameObject targetingIndicatorPrefab;

	Queue<CombatantAction> queuedActions;
	InGameState state;

	// Use this for initialization
	void Start () {
		// Find the GUI Manager.
		GameObject guiManagerObject = GameObject.FindGameObjectWithTag("GUI Manager");
		if (!guiManagerObject || !guiManagerObject.GetComponent<GUIManager>()) {
			Debug.LogError("Error initializing GameManager: GUI Manager is not tagged or doesn't contain the GUIManager component.");
		}
		guiManager = guiManagerObject.GetComponent<GUIManager>();

		// Find the player object.
		player = GameObject.FindGameObjectWithTag("Player");
		if (!player) {
			Debug.LogError("Error initializing GameManager: Player is not tagged.");
		}

		// Create the targeting indicator.
		if (!targetingIndicatorPrefab) {
			Debug.LogError ("Unable to initialize GUI Manager: No Targeting Indicator Prefab is set.");
		}
		targetingIndicator = (GameObject)Instantiate(targetingIndicatorPrefab);

		queuedActions = new Queue<CombatantAction>();

		// Disable the targeting indicator.
		SetPlayerTarget(null);

		// Set the initial state.
		SetState(new SelectMoveState());
	}
	
	// Update is called once per frame
	void Update () {
		if (state != null) {
			state.OnUpdate(this);
		}
	}

	/// <summary>
	/// Sets the current in-game state.
	/// </summary>
	/// <param name="newState">New state.</param>
	public void SetState(InGameState newState) {
		if (state != null) {
			Debug.Log ("Leaving state '" + state + "'");
			state.OnExit(this);
		}

		if (newState != null) {
			state = newState;
			Debug.Log ("Entering state '" + state + "'");
			state.OnEnter (this);
		}
	}

	/// <summary>
	/// Queues the action.
	/// </summary>
	/// <param name="action">Action.</param>
	public void QueueAction(CombatantAction action) {
		queuedActions.Enqueue(action);
	}
	
	/// <summary>
	/// Gets the queued actions.
	/// </summary>
	/// <returns>The queued actions.</returns>
	public Queue<CombatantAction> GetQueuedActions() {
		return queuedActions;
	}

	/// <summary>
	/// Gets the GUI manager.
	/// </summary>
	/// <returns>The GUI manager.</returns>
	public GUIManager GetGUIManager() {
		return guiManager;
	}

	/// <summary>
	/// Gets the player.
	/// </summary>
	/// <returns>The player.</returns>
	public GameObject GetPlayer() { 
		return player;
	}

	/// <summary>
	/// Determines whether the player is targeting a game object.
	/// </summary>
	/// <returns><c>true</c> if this instance is player targeting game object; otherwise, <c>false</c>.</returns>
	public bool IsPlayerTargetingGameObject() {
		return isCurrentlyTargeting;
	}

	/// <summary>
	/// Called when a user selects a combatant.
	/// </summary>
	/// <param name="selected">Selected.</param>
	public void OnCombatantSelect(GameObject selected) {
		SetPlayerTarget(selected);
	}

	/// <summary>
	/// Called when the user attempts to select something (i.e. mouse click or tap) but no relevent object was chosen.
	/// </summary>
	public void OnNothingSelected() {
		if (isCurrentlyTargeting) {
			SetPlayerTarget (null);
		}
	}

	/// <summary>
	/// Event called when a combatant dies.
	/// </summary>
	public void OnCombatantDied(object combatantObject) {
		GameObject combatant = (GameObject)combatantObject;

		if (combatant != player && player.GetComponent<CombatantActions>().GetTarget() == combatant) {
			SetPlayerTarget(null);
		}

		// Final cleanup.
		Debug.Log ("Combatant " + combatant.name + " has died.");
		Destroy(combatant);
	}

	/// <summary>
	/// Sets the player's target.
	/// </summary>
	/// <param name="target">Target.</param>
	public void SetPlayerTarget(GameObject target) {
		if (target) {
			targetingIndicator.transform.parent = target.transform;
			targetingIndicator.transform.localPosition = Vector3.zero;
			targetingIndicator.transform.localRotation = Quaternion.identity;
			targetingIndicator.renderer.enabled = true;
			isCurrentlyTargeting = true;
		}
		else {
			targetingIndicator.transform.parent = null;
			targetingIndicator.renderer.enabled = false;
			isCurrentlyTargeting = false;
		}
		
		player.GetComponent<CombatantActions>().SetTarget(target);
	}
}
