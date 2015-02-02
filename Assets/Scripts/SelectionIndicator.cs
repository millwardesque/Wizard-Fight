using UnityEngine;
using System.Collections;

public class SelectionIndicator : MonoBehaviour {
	public float bounceHeight = 1.0f;
	public float bounceDuration = 0.5f;

	void Start() {
		iTween.MoveBy(gameObject, iTween.Hash (
			"amount", new Vector3(0.0f, bounceHeight, 0.0f),
			"time", bounceDuration,
			"looptype", iTween.LoopType.pingPong,
			"easetype", iTween.EaseType.linear
		));
	}

	void LateUpdate() {
		// Billboard the indicator.
		transform.LookAt (Camera.main.transform.position, Vector3.up);
	}
}
