using UnityEngine;
using System.Collections;

public class GameOverState : InGameState {

	public override void OnEnter (GameManager gameManager) {
		gameManager.GetGUIManager().ShowGameOverPanel();
		gameManager.GetGUIManager().HidePauseButton();
	}
	
	public override void OnExit(GameManager gameManager) {
		gameManager.GetGUIManager().HideGameOverPanel();
		gameManager.GetGUIManager().ShowPauseButton();

	}
}