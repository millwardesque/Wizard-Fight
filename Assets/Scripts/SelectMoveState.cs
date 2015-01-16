using UnityEngine;
using System.Collections;

/// <summary>
/// Game state for when the the combatants can choose their moves.
/// </summary>
public class SelectMoveState : InGameState {
	public float turnDuration = 5.0f;
	float timeElapsed = 0.0f;

	public override void OnEnter (GameManager gameManager) {
		gameManager.GetGUIManager().ShowActions();
		timeElapsed = turnDuration;
	}

	public override void OnExit(GameManager gameManager) {
		gameManager.GetGUIManager().HideActions();
	}

	public override void OnUpdate(GameManager gameManager) {
		timeElapsed -= Time.deltaTime;
		if (timeElapsed <= 0.0f) {
			gameManager.SetState(new ExecuteMoveState());
		}
	}
}
