using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Health : MonoBehaviour {
	public int currentHealth;
	private Slider healthBar;
	private GameObject gameManager;
	public GameObject combatantMesh;
	private bool isAlive = true;

	// Use this for initialization
	void Start () {
		gameManager = GameObject.FindGameObjectWithTag("Game Manager");
		if (!gameManager || !gameManager.GetComponent<GameManager>()) {
			Debug.LogError("Unable to initialize Health in " + gameObject.name + ": No game manager is tagged, or game manager is missing GameManager component.");
		}


		GameObject healthBarObject = (GameObject)GameObject.Instantiate (GameObject.FindGameObjectWithTag("GUI Manager").GetComponent<GUIManager>().healthBarPrefab);
		healthBar = healthBarObject.GetComponent<Slider>();
		healthBar.maxValue = (float)currentHealth;
		healthBar.value = (float)currentHealth;
		healthBar.transform.SetParent(GameObject.Find("Canvas").transform, false);
		healthBarObject.transform.SetAsFirstSibling();

		isAlive = true;
	}

	void Update() {
		if (!isAlive) {
			StartCoroutine("DyingFade");
		}
	}

	void LateUpdate() {
		if (healthBar) {
			healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(0, 30.0f, 0);
		}
	}

	void OnDestroy() {
		if (healthBar) {
			GameObject.Destroy (healthBar.gameObject);
		}
	}

	public bool IsAlive() {
		return isAlive;
	}

	/// <summary>
	/// Adds health to the combatant. Negative values are permitted.
	/// </summary>
	/// <param name="healthChange">Health change.</param>
	public void AddHealth(int healthChange) {
		if (isAlive) {
			currentHealth += healthChange;
			healthBar.value = (float)currentHealth;

			if (currentHealth <= 0) {
				isAlive = false;
				gameManager.SendMessage("OnCombatantDied", gameObject);
			}
		}
	}

	/// <summary>
	/// Basic animation for combatant death.
	/// </summary>
	/// <returns>The fade.</returns>
	IEnumerator DyingFade() {
		for (float f = 1.0f; f >= 0; f -= 0.02f) {
			Color c = combatantMesh.renderer.material.color;
			c.a = f;
			combatantMesh.renderer.material.color = c;
			yield return null;
		}

		GameObject.Destroy (gameObject);
	}
}
