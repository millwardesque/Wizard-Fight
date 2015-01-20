using UnityEngine;
using System.Collections;

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
		GameObject[] actors = GameObject.FindGameObjectsWithTag("Combatant");
		foreach (GameObject actor in actors) {
			CombatantActions combatant = actor.GetComponent<CombatantActions>();

			// Make sure the combatant doesn't target itself.
			int randActor = Random.Range(0, actors.Length);
			while (actors[randActor] == actor) {
				randActor = Random.Range(0, actors.Length);
			}
			combatant.SetTarget(actors[randActor]);

			// Pick the action.
			int randAction = Random.Range(0, combatant.GetActionCount());
			CombatantAction action = combatant.GetAction(randAction);
			gameManager.QueueAction(action);
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
