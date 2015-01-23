using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	GUIManager guiManager;
	GameObject player;

	GameObject targetingIndicator;
	bool isCurrentlyTargeting = false;
	public GameObject targetingIndicatorPrefab;
	public InGameState[] stateTypes;

	Queue<CombatantAction> queuedActions;
	InGameState state;

	/// <summary>
	/// Restarts the scene.
	/// </summary>
	public void RestartScene() {
		Application.LoadLevel(Application.loadedLevel);
	}

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
		OnNothingSelected();
		state = null;
	}
	
	// Update is called once per frame
	void Update () {
		if (state != null) {
			state.OnUpdate(this);
		}
		else {
			// Set the initial state.
			SetState(CreateStateByName("SelectMoveState"));
		}
	}

	/// <summary>
	/// Sets the current in-game state.
	/// </summary>
	/// <param name="newState">New state.</param>
	public void SetState(InGameState newState) {
		if (state != null) {
			state.OnExit(this);
			GameObject.Destroy(state.gameObject);
		}

		if (newState != null) {
			state = newState;
			state.OnEnter (this);
		}
	}

	/// <summary>
	/// Creates a state by by name.
	/// </summary>
	/// <returns>The state object</returns>
	/// <param name="name">Name of the state</param>
	public InGameState CreateStateByName(string name) {
		foreach (InGameState state in stateTypes) {
			if (state.name == name) {
				InGameState newState = (InGameState)GameObject.Instantiate(state);
				return newState;
			}
		}

		throw new UnityException("Couldn't find state named '" + name + "'");
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
	/// Gets the player's current target. Doesn't check whether a target is currently set, use IsPlayerTargetingGameObject first for that.
	/// </summary>
	/// <returns>The player target.</returns>
	public GameObject GetPlayerTarget() {
		return player.GetComponent<CombatantActions>().GetTarget();
	}

	/// <summary>
	/// Called when a user selects a combatant.
	/// </summary>
	/// <param name="selected">Selected.</param>
	public void OnCombatantSelect(GameObject selected) {
		if (state != null) {
			state.OnCombatantSelect(this, selected);
		}
	}

	/// <summary>
	/// Called when the user attempts to select something (i.e. mouse click or tap) but no relevent object was chosen.
	/// </summary>
	public void OnNothingSelected() {
		if (state != null) {
			state.OnNothingSelected(this);
		}
	}

	/// <summary>
	/// Event called when a combatant dies.
	/// </summary>
	public void OnCombatantDied(object combatantObject) {
		GameObject combatant = (GameObject)combatantObject;

		if (combatant != player && player.GetComponent<CombatantActions>().GetTarget() == combatant) {
			OnNothingSelected();
		}

		if (combatant == GetPlayer()) {
			SetState(CreateStateByName("GameOverState"));
		}
		else if (GetActiveCombatants().Count == 1) {
			SetState(CreateStateByName("YouWinState"));
		}

		// Final cleanup.
		Debug.Log ("Combatant " + combatant.name + " has died.");
	}

	/// <summary>
	/// Sets the player's target.
	/// </summary>
	/// <param name="target">Target.</param>
	public void SetPlayerTarget(GameObject target) {
		if (target != null) {
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

	/// <summary>
	/// Gets the active combatants.
	/// </summary>
	/// <returns>The active combatants.</returns>
	public List<GameObject> GetActiveCombatants() {
		List<GameObject> combatants = new List<GameObject>();

		GameObject[] actors = GameObject.FindGameObjectsWithTag("Combatant");
		foreach (GameObject actor in actors) {
			if (actor != null && actor.GetComponent<Health>().IsAlive()) {
				combatants.Add(actor);
			}
		}

		if (GetPlayer() != null) {
			combatants.Add (GetPlayer());
		}

		return combatants;
	}

	public void PauseGame() {
		GetGUIManager().ShowPausePanel();
		GetGUIManager().HidePauseButton();
		Time.timeScale = 0;
	}

	public void UnpauseGame() {
		GetGUIManager().HidePausePanel();
		GetGUIManager().ShowPauseButton();
		Time.timeScale = 1.0f;
	}
}
