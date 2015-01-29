using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Game state for when the the combatants can choose their moves.
/// </summary>
public class SelectMoveState : InGameState {
	public float turnDuration = 5.0f;
	public Vector3 startCameraPosition;
	float timeElapsed = 0.0f;
	GameObject gameCamera;

	void Awake() {
		gameCamera = GameObject.FindGameObjectWithTag("MainCamera");
		if (!gameCamera) {
			Debug.LogError("Unable to find camera with tag MainCamera");
		}
	}

	public override void OnEnter (GameManager gameManager) {
		gameManager.GetGUIManager().ShowActions();
		gameManager.GetGUIManager().ShowMoveSelectCountdown();
		gameManager.GetGUIManager().SetMoveSelectCountdownMinMax(0, turnDuration);
		gameManager.GetGUIManager().SetMoveSelectCountdownValue(turnDuration);
		gameManager.ShowTargetingIndicator();
		timeElapsed = turnDuration;

		// Move the camera back into place.
		gameCamera.GetComponent<CameraMoves>().MoveAndLook(startCameraPosition, gameManager.GetPlayer().transform.position, 2.0f);

		bool autoSelectPlayerTarget = (gameManager.GetPlayerTarget() == null); // Decided whether auto-select a target if the player hasn't selected any

		// Pick random actions for the non-player combatants.
		List<GameObject> actors = gameManager.GetActiveCombatants();
		if (actors.Count > 1) {
			foreach (GameObject actor in actors) {
				// We don't need to pick an action for the player automatically.
				if (actor.CompareTag("Player")) {
					continue;
				}

				// Auto-select the first available enemy as the player's target.
				if (autoSelectPlayerTarget) {
					gameManager.OnCombatantSelect(actor);
					autoSelectPlayerTarget = false;
				}
				
				CombatantActions combatant = actor.GetComponent<CombatantActions>();

				// Choose a target.

				// Make sure the combatant doesn't target itself.
				List<int> possibleTargets = new List<int>();
				for (int i = 0; i < actors.Count; i++) {
					if (actors[i] != actor) {
						possibleTargets.Add(i);
					}
				}

				int targetIndex = Random.Range(0, possibleTargets.Count);
				GameObject target = actors[possibleTargets[targetIndex]];
				combatant.SetTarget(target);

				// Pick the action.
				int randAction = Random.Range(0, combatant.GetActionCount());
				CombatantAction action = combatant.GetAction(randAction);
				gameManager.QueueAction(action);
			}
		}

		gameManager.UnsetSelectedAction();
		gameManager.GetPlayer().GetComponent<CombatantActions>().SelectedAction = null;
	}

	public override void OnExit(GameManager gameManager) {
		gameManager.GetGUIManager().HideActions();
		gameManager.GetGUIManager().HideMoveSelectCountdown();
		gameManager.UnsetSelectedAction();
	}

	public override void OnUpdate(GameManager gameManager) {
		timeElapsed -= Time.deltaTime;
		gameManager.GetGUIManager().SetMoveSelectCountdownValue(timeElapsed);
		if (timeElapsed <= 0.0f) {
			QueueSelectedAction(gameManager);
			gameManager.SetState(gameManager.CreateStateByName("ExecuteMoveState"));
		}
	}

	public override void OnCombatantSelect(GameManager gameManager, GameObject selected) {
		gameManager.SetPlayerTarget(selected);
		gameManager.GetGUIManager().EnableActionButtons();
	}

	public override void OnNothingSelected(GameManager gameManager) {
		gameManager.SetPlayerTarget (null);
		gameManager.GetGUIManager().DisableActionButtons();
		gameManager.UnsetSelectedAction();
	}

	void QueueSelectedAction(GameManager gameManager) {
		CombatantAction selectedAction = gameManager.GetPlayer().GetComponent<CombatantActions>().SelectedAction;
		if (selectedAction != null) {
			gameManager.QueueAction(selectedAction);
		}
		gameManager.UnsetSelectedAction();
	}
}
