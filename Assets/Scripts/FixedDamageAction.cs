using UnityEngine;
using System.Collections;

/// <summary>
/// Fixed damage action.
/// </summary>
public class FixedDamageAction : CombatantAction {
	public int DamageDealt { get; set; }
	public GameObject actionFX;
	public GameObject precastFX;
	
	CameraMoves gameCamera;

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

	public override bool CanExecute() {
		return (this.Sender != null && this.Receiver != null && Receiver.GetComponent<Health>() && Receiver.GetComponent<CharacterController>());
	}

	/// <summary>
	/// Update method for the pre-cast setup action state.
	/// </summary>
	protected override IEnumerator PrecastSetupUpdate() {
		// Move the camera into position.
		Vector3 senderPosition = Sender.transform.position;
		Vector3 receiverPosition = Receiver.transform.position;
		Vector3 cameraPosition = senderPosition - ((receiverPosition - senderPosition).normalized * 15.0f);
		float precastSetupDuration = 1.5f;
		cameraPosition.y = gameCamera.transform.position.y;

		gameCamera.MoveAndLook(cameraPosition, receiverPosition, precastSetupDuration, gameObject); // Once the camera is oriented, continue on.
		iTween.LookTo (Sender, receiverPosition, precastSetupDuration / 2);
		iTween.LookTo (Receiver, senderPosition, precastSetupDuration / 2);
		yield return new WaitForSeconds(precastSetupDuration);

		canGoToNextState = true;
		yield return null;
	}

	/// <summary>
	/// Update method for the prec-cast action state.
	/// </summary>
	/// <returns>The update.</returns>
	protected override IEnumerator PrecastUpdate() {
		float precastDuration = 1.5f;
		GameObject precastFXObj = (GameObject)GameObject.Instantiate(precastFX);
		precastFXObj.transform.SetParent(Sender.transform, false);
		yield return new WaitForSeconds(precastDuration);

		canGoToNextState = true;
		GameObject.Destroy(precastFXObj);
		yield return null;
	}
	
	/// <summary>
	/// Update method for the Cast action state.
	/// </summary>
	protected override IEnumerator CastUpdate() {
		GameObject launchPosition = Sender.GetComponent<CombatantActions>().GetLaunchPosition();
		
		// Create the special FX.
		GameObject newFX = (GameObject)GameObject.Instantiate(actionFX);
		newFX.transform.parent = launchPosition.transform;
		newFX.transform.localPosition = Vector3.zero;
		newFX.transform.localRotation = Quaternion.identity;
		
		// Shoot FX at Receiver and wait for it finish.
		yield return StartCoroutine(newFX.GetComponent<ActionFX>().Fire(Receiver, Receiver.GetComponent<CharacterController>().center));

		Receiver.GetComponent<Health>().AddHealth(-DamageDealt);
		canGoToNextState = true;
		yield return null;
	}
}
