using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
	public int currentHealth;
	private GameObject gameManager;
	private bool isAlive = true;

	// Use this for initialization
	void Start () {
		gameManager = GameObject.FindGameObjectWithTag("Game Manager");
		if (!gameManager || !gameManager.GetComponent<GameManager>()) {
			Debug.LogError("Unable to initialize Health in " + gameObject.name + ": No game manager is tagged, or game manager is missing GameManager component.");
		}

		isAlive = true;
	}

	/// <summary>
	/// Adds health to the combatant. Negative values are permitted.
	/// </summary>
	/// <param name="healthChange">Health change.</param>
	public void AddHealth(int healthChange) {
		if (isAlive) {
			currentHealth += healthChange;

			if (currentHealth <= 0) {
				gameManager.SendMessage("OnCombatantDied", gameObject);
				isAlive = false;
			}
		}
	}
}
