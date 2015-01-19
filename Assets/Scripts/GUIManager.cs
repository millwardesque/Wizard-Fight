using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class GUIManager : MonoBehaviour {
	GameManager gameManager;
	GameObject actionsPanel;
	EventSystem eventSystem;
	bool refreshActionButtons = false;	// Flag to indicate whether to refresh the action buttons.

	// Use this for initialization
	void Start () {
		// Find and store the Actions panel.
		actionsPanel = GameObject.FindGameObjectWithTag("UI Actions Panel");
		if (!actionsPanel) {
			Debug.LogError("Unable to initialize GUI Manager: No Action Panel is tagged with the 'UI Actions Panel' tag.");
		}
		// Find and store the Game Manager.
		GameObject gameManagerObject = GameObject.FindGameObjectWithTag("Game Manager");
		if (gameManagerObject && gameManagerObject.GetComponent<GameManager>()) {
			gameManager = gameManagerObject.GetComponent<GameManager>();
		}
		else {
			Debug.LogError("Error initializing GUIManager: Game Manager is not tagged or doesn't contain the GameManager component.");
		}
		refreshActionButtons = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (refreshActionButtons) {
			RefreshActionButtons();
			refreshActionButtons = false;
		}

		if (Input.GetMouseButtonDown(0)) {
			RaycastHit clickHit;
			GameObject activeObject = EventSystem.current.currentSelectedGameObject;
			if (activeObject && LayerMask.NameToLayer("UI") == activeObject.layer) {
				// User clicked a GUI component. Do nothing.
			}
			else if (ProcessClick (out clickHit, LayerMask.NameToLayer("Combatant"))) {
				gameManager.OnCombatantSelect(clickHit.collider.gameObject);
			}
			else if (gameManager.GetComponent<GameManager>().IsPlayerTargetingGameObject()) {
				gameManager.OnNothingSelected();
			}
		}
	}

	/// <summary>
	/// Processes a click on game objects.
	/// </summary>
	/// <returns>True if the click intersected an object, else false.</returns>
	/// <param name="layerID">Optional layer mask ID.</param>
	bool ProcessClick(out RaycastHit clickHit, int layerID = 0) {
		float distance = 100.0f;

		int layerMask = layerID > 0 ? (1 << layerID) : 0;	// Shift the Layer ID into place
		Ray clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		return Physics.Raycast(clickRay, out clickHit, distance, layerMask);
	}

	/// <summary>
	/// Shows the Actions panel.
	/// </summary>
	public void ShowActions() {
		actionsPanel.SetActive(true);
	}

	/// <summary>
	/// Hides the Actions panel.
	/// </summary>
	public void HideActions() {
		actionsPanel.SetActive(false);
	}

	/// <summary>
	/// Enable clicking on the action-buttons 
	/// </summary>
	public void EnableActionButtons() {
		Button[] buttons = actionsPanel.GetComponentsInChildren<Button>();
		foreach (Button button in buttons) {
			button.interactable = true;
		}
	}

	/// <summary>
	/// Disable clicking on the action-buttons 
	/// </summary>
	public void DisableActionButtons() {
		Button[] buttons = actionsPanel.GetComponentsInChildren<Button>();
		foreach (Button button in buttons) {
			button.interactable = false;
		}
	}

	/// <summary>
	/// Processes a click on the Action 1 button.
	/// </summary>
	public void OnAction1Click() {
		if (gameManager.IsPlayerTargetingGameObject()) {
			CombatantAction action = gameManager.GetPlayer().GetComponent<CombatantActions>().GetAction(0);
			gameManager.QueueAction(action);
		}
	}

	/// <summary>
	/// Processes a click on the Action 2 button.
	/// </summary>
	public void OnAction2Click() {
		if (gameManager.IsPlayerTargetingGameObject()) {
			CombatantAction action = gameManager.GetPlayer().GetComponent<CombatantActions>().GetAction(1);
			gameManager.QueueAction(action);
		}
	}

	/// <summary>
	/// Processes a click on the Action 3 button.
	/// </summary>
	public void OnAction3Click() {
		if (gameManager.IsPlayerTargetingGameObject()) {
			CombatantAction action = gameManager.GetPlayer().GetComponent<CombatantActions>().GetAction(2);
			gameManager.QueueAction(action);
		}
	}

	/// <summary>
	/// Processes a click on the Action 4 button.
	/// </summary>
	public void OnAction4Click() {
		if (gameManager.IsPlayerTargetingGameObject()) {
			CombatantAction action = gameManager.GetPlayer().GetComponent<CombatantActions>().GetAction(3);
			gameManager.QueueAction(action);
		}
	}

	/// <summary>
	/// Refreshes the action buttons.
	/// </summary>
	public void RefreshActionButtons() {
		Button[] buttons = actionsPanel.GetComponentsInChildren<Button>();
		for (int i = 0; i < buttons.Length; ++i) {
			buttons[i].GetComponentInChildren<Text>().text = gameManager.GetPlayer().GetComponent<CombatantActions>().GetAction(i).name;
		}
	}
}
