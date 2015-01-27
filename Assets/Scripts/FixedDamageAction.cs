using UnityEngine;
using System.Collections;

/// <summary>
/// Fixed damage action.
/// </summary>
public class FixedDamageAction : CombatantAction {
	public int DamageDealt { get; set; }
	public GameObject actionFX;
	CameraMoves gameCamera;
	bool cameraHasMoved;

	void Start() {
		if (actionFX == null) {
			Debug.LogWarning("FixedDamagerAction '" + name + "' has no action FX attached.");
		}

		GameObject gameCameraObj = GameObject.FindGameObjectWithTag("MainCamera");
		if (!gameCameraObj || !gameCameraObj.GetComponent<CameraMoves>()) {
			Debug.LogError("Unable to intiailize FixedDamageAction: Can't find camera with tag MainCamera, or camera doesn't have CameraMoves action attached.");
		}
		gameCamera = gameCameraObj.GetComponent<CameraMoves>();
	}

	/// <summary>
	/// Initialize the specified sender, receiver and damageDealt.  Shortcut method rather than setting the properties individually.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="receiver">Receiver.</param>
	/// <param name="damageDealt">Damage dealt.</param>
	public FixedDamageAction Initialize(string name, GameObject sender, GameObject receiver, int damageDealt) {
		this.name = name;
		this.Sender = sender;
		this.Receiver = receiver;
		this.DamageDealt = damageDealt;

		return this;
	}

	/// <summary>
	/// Update method for the pre-cast setup action state.
	/// </summary>
	protected override IEnumerator PrecastSetupUpdate() {
		// Move the camera into position.
		if (Sender != null && Receiver != null && Receiver.GetComponent<Health>()) {
			cameraHasMoved = false;
			Vector3 senderPosition = Sender.transform.position;
			Vector3 receiverPosition = Receiver.transform.position;
			Vector3 cameraPosition = senderPosition - ((receiverPosition - senderPosition).normalized * 15.0f);
			float cameraSwingDuration = 1.5f;
			cameraPosition.y = gameCamera.transform.position.y;

			gameCamera.MoveAndLook(cameraPosition, receiverPosition, cameraSwingDuration, gameObject, "PrecastSetupCameraMoveDone"); // Once the camera is oriented, continue on.
			while (!cameraHasMoved) {
				yield return null;
			}
		}
		else {
			Debug.LogError ("Unable to do pre-cast setup for action FixedDamageAction: Either the sender/receiver are null, or the receiver doesn't have a Health component.");
		}

		canGoToNextState = true;
		yield return null;
	}

	/// <summary>
	/// Callback for when the precast setup camera movement has finished.
	/// </summary>
	void PrecastSetupCameraMoveDone() {
		cameraHasMoved = true;
	}

	/// <summary>
	/// Update method for the Cast action state.
	/// </summary>
	protected override IEnumerator CastUpdate() {
		if (Sender != null && Receiver != null && Receiver.GetComponent<Health>()) {
			GameObject launchPosition = Sender.GetComponent<CombatantActions>().GetLaunchPosition();
			
			// Create the special FX.
			GameObject newFX = (GameObject)GameObject.Instantiate(actionFX);
			newFX.transform.parent = launchPosition.transform;
			newFX.transform.localPosition = Vector3.zero;
			newFX.transform.localRotation = Quaternion.identity;
			
			// Shoot FX at Receiver and wait for it finish.
			yield return StartCoroutine(newFX.GetComponent<ActionFX>().Fire(Receiver));
			Receiver.GetComponent<Health>().AddHealth(-DamageDealt);
		}
		else {
			Debug.LogError ("Unable to do action " + name + ": Either the sender/receiver are null, or the receiver doesn't have a Health component.");
		}
		canGoToNextState = true;
		yield return null;
	}
}
