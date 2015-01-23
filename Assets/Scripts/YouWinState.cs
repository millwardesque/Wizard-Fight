using UnityEngine;
using System.Collections;

public class YouWinState : InGameState {

	public override void OnEnter (GameManager gameManager) {
		gameManager.GetGUIManager().ShowYouWinPanel();
		gameManager.GetGUIManager().HidePauseButton();
	}
	
	public override void OnExit(GameManager gameManager) {
		gameManager.GetGUIManager().HideYouWinPanel();
		gameManager.GetGUIManager().ShowPauseButton();
	}
}
