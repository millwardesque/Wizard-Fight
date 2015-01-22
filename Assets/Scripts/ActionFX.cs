using UnityEngine;
using System.Collections;

public class ActionFX : MonoBehaviour {
	public float duration = 1.0f;
	bool hasFired = false;
	GameObject receiver;
	
	/// <summary>
	/// Launch the FX at the recipient.
	/// </summary>
	public IEnumerator Fire(GameObject receiver) {
		this.receiver = receiver;
		this.hasFired = true;
		iTween.MoveTo(gameObject, iTween.Hash("position", this.receiver.transform, "orienttopath", true, "time", duration));
		yield return new WaitForSeconds(duration);
	}

	/// <summary>
	/// Called when a collision occurs.
	/// </summary>
	/// <param name="col">Col.</param>
	void OnCollisionEnter(Collision col) {
		if (!this.hasFired) {
			return; 
		}

		if (col.collider.gameObject == receiver) {
			GameObject.Destroy(gameObject);
		}
	}
}
