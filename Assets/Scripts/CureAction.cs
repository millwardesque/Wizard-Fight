using UnityEngine;
using System.Collections;

public class CureAction : CombatantAction {
	public int CureAmount { get; set; }
	public GameObject actionFX;
	public GameObject precastFX;
	
	CameraMoves gameCamera;

	void Start() {
		if (actionFX == null) {
			Debug.LogWarning("CureAction '" + name + "' has no action FX attached.");
		}
		
		GameObject gameCameraObj = GameObject.FindGameObjectWithTag("MainCamera");
		if (!gameCameraObj || !gameCameraObj.GetComponent<CameraMoves>()) {
			Debug.LogError("Unable to intiailize CureAction: Can't find camera with tag MainCamera, or camera doesn't have CameraMoves action attached.");
		}
		gameCamera = gameCameraObj.GetComponent<CameraMoves>();
	}

	/// <summary>
	/// Initialize the specified sender, receiver and damageDealt.  Shortcut method rather than setting the properties individually.
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="sender">Sender.</param>
	/// <param name="receiver">Receiver.</param>
	/// <param name="cureAmounts">Cure amounts.</param>
	/// <param name="castTime">Cast time.</param>
	public CureAction Initialize(string name, GameObject sender, GameObject receiver, int cureAmount, float castTime) {
		this.name = name;
		this.Sender = sender;
		this.Receiver = receiver;
		this.CureAmount = cureAmount;
		this.CastTime = castTime;
		
		return this;
	}
	
	public override bool CanExecute() {
		return (this.Sender != null && this.Receiver != null && 
		        Sender.GetComponent<Health>() && Sender.GetComponent<Health>().IsAlive() && Sender.GetComponent<CharacterController>() &&
		        Receiver.GetComponent<Health>() && Receiver.GetComponent<Health>().IsAlive() && Receiver.GetComponent<CharacterController>());
	}

	/// <summary>
	/// Update method for the pre-cast setup action state.
	/// </summary>
	protected override IEnumerator PrecastSetupUpdate() {
		if (!CanExecute()) {
			yield return null;
		}

		float precastSetupDuration = 1.5f;
		
		// Move the camera into position.
		float cameraDistance = 15.0f;
		gameCamera.LineUpActors(Sender, Receiver, cameraDistance, precastSetupDuration);
		yield return new WaitForSeconds(precastSetupDuration);
		
		canGoToNextState = true;
		yield return null;
	}
	
	/// <summary>
	/// Update method for the pre-cast action state.
	/// </summary>
	/// <returns>The update.</returns>
	protected override IEnumerator PrecastUpdate() {
		if (!CanExecute()) {
			yield return null;
		}
		
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
		if (!CanExecute()) {
			yield return null;
		}
		
		GameObject launchPosition = Sender.GetComponent<CombatantActions>().GetLaunchPosition();
		
		// Create the special FX.
		GameObject newFX = (GameObject)GameObject.Instantiate(actionFX);
		newFX.transform.parent = launchPosition.transform;
		newFX.transform.localPosition = Vector3.zero;
		newFX.transform.localRotation = Quaternion.identity;
		
		// Shoot FX at Receiver and wait for it finish.
		yield return StartCoroutine(newFX.GetComponent<ActionFX>().Fire(Receiver, Receiver.GetComponent<CharacterController>().center));
		
		Receiver.GetComponent<Health>().AddHealth(CureAmount);
		canGoToNextState = true;
		yield return null;
	}
}
