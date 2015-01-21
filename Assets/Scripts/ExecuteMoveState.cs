﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// State that executes all the moves.
/// </summary>
public class ExecuteMoveState : InGameState {
	CameraMoves gameCamera;
	bool isProcessingQueue = false;
	GameManager gameManager;
	CombatantAction currentAction;

	void Start() {
		GameObject gameCameraObj = GameObject.FindGameObjectWithTag("MainCamera");
		if (!gameCameraObj || !gameCameraObj.GetComponent<CameraMoves>()) {
			Debug.LogError("Unable to intiailize ExecuteMoveState: Can't find camera with tag MainCamera, or camera doesn't have CameraMoves action attached.");
		}
		gameCamera = gameCameraObj.GetComponent<CameraMoves>();
		isProcessingQueue = false;
		currentAction = null;
	}

	public override void OnEnter (GameManager gameManager) {
		gameManager.GetGUIManager().HideActions();
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
		Queue<CombatantAction> queue = gameManager.GetQueuedActions();
		if (queue.Count > 0) {
			currentAction = queue.Dequeue();

			if (currentAction.Sender != null && currentAction.Receiver != null) {
				Vector3 senderPosition = currentAction.Sender.transform.position;
				Vector3 receiverPosition = currentAction.Receiver.transform.position;
				Vector3 cameraPosition = senderPosition - ((receiverPosition - senderPosition).normalized * 15.0f);
				cameraPosition.y = gameCamera.transform.position.y;
				gameCamera.MoveAndLook(cameraPosition, receiverPosition, 3.0f, gameObject, "ExecuteAction");
			}
		}
		else {
			gameManager.SetState(gameManager.CreateStateByName("SelectMoveState"));
		}
	}

	/// <summary>
	/// Executes the action.
	/// </summary>
	void ExecuteAction() {
		if (currentAction != null && currentAction.Sender != null && currentAction.Receiver != null) {
			currentAction.DoAction();
		}
		ProcessNextQueueItem();
	}
}