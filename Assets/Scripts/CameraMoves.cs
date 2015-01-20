using UnityEngine;
using System.Collections;

public class CameraMoves : MonoBehaviour {
	GameObject gameCamera;

	// Use this for initialization
	void Start () {
		gameCamera = GameObject.FindGameObjectWithTag("MainCamera");
	}
	
	/// <summary>
	/// Moves the camera and looks at an object
	/// </summary>
	/// <param name="position">Position.</param>
	/// <param name="lookAt">Look at.</param>
	/// <param name="duration">Duration.</param>
	/// <param name="onCompleteTarget">On complete target.</param>
	/// <param name="onCompleteAction">On complete action.</param>
	public void MoveAndLook(Vector3 position, Vector3 lookAt, float duration, GameObject onCompleteTarget = null, string onCompleteAction = "") {
		if (onCompleteAction != "" && onCompleteTarget != null) {
			iTween.MoveTo(gameCamera, iTween.Hash("position", position, "looktarget", lookAt, "time", duration, "onComplete", onCompleteAction, "onCompleteTarget", onCompleteTarget));
		}
		else {
			iTween.MoveTo(gameCamera, iTween.Hash("position", position, "looktarget", lookAt, "time", duration));
		}
	}

	/// <summary>
	/// Moves the camera near an object and looks at that object.
	/// </summary>
	/// <param name="position">Position.</param>
	/// <param name="duration">Duration.</param>
	/// <param name="onCompleteTarget">On complete target.</param>
	/// <param name="onCompleteAction">On complete action.</param>
	public void WatchPosition(Vector3 position, float duration, float watchDistance, GameObject onCompleteTarget = null, string onCompleteAction = "") {
		Vector3 direction = position - gameCamera.transform.position;
		Vector3 actualTarget = gameCamera.transform.position + direction.normalized * (direction.magnitude - watchDistance);

		iTween.LookTo(gameCamera, iTween.Hash("looktarget", position, "time", duration));

		if (onCompleteAction != "" && onCompleteTarget != null) {
			iTween.MoveTo(gameCamera, iTween.Hash("position", actualTarget, "time", duration, "onComplete", onCompleteAction, "onCompleteTarget", onCompleteTarget));
		}
		else {
			iTween.MoveTo(gameCamera, iTween.Hash("position", actualTarget, "time", duration));
		}
	}

	/// <summary>
	/// Called by the test camera click event.
	/// </summary>
	public void OnTestCameraClick() {
		GameManager gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
		GameObject toTrack = gameManager.GetPlayer();

		if (gameManager.IsPlayerTargetingGameObject()) {
			toTrack = gameManager.GetPlayerTarget();
		}
		WatchPosition (toTrack.transform.position, 2.0f, 15.0f);
	}
}
