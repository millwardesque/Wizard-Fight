using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActionButton : MonoBehaviour {
	GameManager gameManager;
	public int ActionIndex { get; set; }

	// Use this for initialization
	void Start () {
		GameObject gameManagerObj = GameObject.FindGameObjectWithTag("Game Manager");
		if (!gameManagerObj || !gameManagerObj.GetComponent<GameManager>()) {
			Debug.LogError("Unable to initialize action button: Game Manager isn't tagged or is missing the GameManager component.");
		}
		gameManager = gameManagerObj.GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RefreshLabel() {
		CombatantAction action = gameManager.GetPlayer().GetComponent<CombatantActions>().GetAction(ActionIndex);
		GetComponentInChildren<Text>().text = action.Name;
	}
}
