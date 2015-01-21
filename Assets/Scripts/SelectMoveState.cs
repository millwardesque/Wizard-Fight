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
		timeElapsed = turnDuration;

		// Move the camera back into place.
		gameCamera.GetComponent<CameraMoves>().MoveAndLook(startCameraPosition, gameManager.GetPlayer().transform.position, 2.0f);

		// Pick random actions for the non-player combatants.
		List<GameObject> actors = gameManager.GetActiveCombatants();
		if (actors.Count > 1) {
			foreach (GameObject actor in actors) {
				// We dont' need to pick an action for the player automatically.
				if (actor.CompareTag("Player")) {
					continue;
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
	}

	public override void OnExit(GameManager gameManager) {
		gameManager.GetGUIManager().HideActions();
		gameManager.GetGUIManager().HideMoveSelectCountdown();
	}

	public override void OnUpdate(GameManager gameManager) {
		timeElapsed -= Time.deltaTime;
		gameManager.GetGUIManager().SetMoveSelectCountdownValue(timeElapsed);
		if (timeElapsed <= 0.0f) {
			gameManager.SetState(gameManager.CreateStateByName("ExecuteMoveState"));
		}
	}
}
