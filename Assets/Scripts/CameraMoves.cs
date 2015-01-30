using UnityEngine;
using System.Collections;

public class CameraMoves : MonoBehaviour {
	GameObject gameCamera;

	// Use this for initialization
	void Start () {
		gameCamera = GameObject.FindGameObjectWithTag("MainCamera");
	}

	/// <summary>
	/// Lines up the camera directly behind two actors
	/// </summary>
	/// <param name="closeActor">The actor that should be closest to the camera.</param>
	/// <param name="farActor">The actor that should be furthest from the camera.</param>
	/// <param name="distanceFromCloseActor">Distance the camera should be from close actor.</param>
	/// <param name="duration">Duration of the camera move.</param>
	public void LineUpActors(GameObject closeActor, GameObject farActor, float distanceFromCloseActor, float duration) {
		Vector3 closePosition = closeActor.transform.position;
		Vector3 farPosition = farActor.transform.position;
		Vector3 cameraPosition = closePosition - ((farPosition - closePosition).normalized * distanceFromCloseActor);

		// Keep the camera at its current height
		cameraPosition.y = gameCamera.transform.position.y;
		
		MoveAndLook(cameraPosition, farPosition, duration); // Once the camera is oriented, continue on.
		iTween.LookTo (closeActor, closePosition, duration / 2.0f);
		iTween.LookTo (farActor, farPosition, duration / 2.0f);
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
