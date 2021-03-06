﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// State that executes all the moves.
/// </summary>
public class ExecuteMoveState : InGameState {
	bool isProcessingQueue = false;
	GameManager gameManager;
	CombatantAction currentAction;

	void Start() {
		isProcessingQueue = false;
		currentAction = null;
	}

	public override void OnEnter (GameManager gameManager) {
		gameManager.GetGUIManager().HideActions();
		gameManager.HideTargetingIndicator();

		GUIManager guiManager = gameManager.GetGUIManager();
		guiManager.ShowStatusPanel();
		guiManager.SetStatusLabelText("");

		isProcessingQueue = false;
		currentAction = null;
	}

	public override void OnUpdate(GameManager gameManager) {
		this.gameManager = gameManager;
		if (!isProcessingQueue) {
			ProcessNextQueueItem();
			isProcessingQueue = true;
		}
	}

	void ProcessNextQueueItem() {
		List<CombatantAction> queue = gameManager.GetQueuedActions();
		if (queue.Count > 0) {
			currentAction = queue[0];
			queue.RemoveAt(0);

			if (currentAction != null && currentAction.CanExecute()) {
				StartCoroutine(ExecuteAction());
			}
			else {
				ProcessNextQueueItem();
			}
		}
		else {
			gameManager.SetState(gameManager.CreateStateByName("SelectMoveState"));
		}
	}

	/// <summary>
	/// Executes the action.
	/// </summary>
	/// <returns>The action.</returns>
	IEnumerator ExecuteAction() {
		gameManager.GetGUIManager().SetStatusLabelText("Casting " + currentAction.name);
		yield return StartCoroutine(currentAction.DoAction());

		// After the action animation has completed, move to the next action in the list.
		ProcessNextQueueItem();
	}
	
	public override void OnNothingSelected(GameManager gameManager) {
		gameManager.SetPlayerTarget(null);
	}
}