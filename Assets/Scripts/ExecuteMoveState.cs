using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// State that executes all the moves.
/// </summary>
public class ExecuteMoveState : InGameState {

	float countdown = 5.0f;	// Must remain for 5 seconds in this state to simulate time required to show attacks.

	public override void OnUpdate(GameManager gameManager) {
		// Process the queued items
		Queue<CombatantAction> queue = gameManager.GetQueuedActions();
		CombatantAction action;
		while (queue.Count > 0) {
			action = queue.Dequeue();
			action.DoAction();
		}
		
		countdown -= Time.deltaTime;
		if (countdown < 0) {
			gameManager.SetState(new SelectMoveState());
		}
	}
}