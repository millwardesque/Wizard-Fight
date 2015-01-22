using UnityEngine;

/// <summary>
/// Base class for In-Game states.
/// </summary>
public class InGameState : MonoBehaviour {
	public virtual void OnUpdate(GameManager gameManager) { }
	public virtual void OnEnter(GameManager gameManager) { }
	public virtual void OnExit(GameManager gameManager) { }
	public virtual void OnCombatantSelect(GameManager gameManager, GameObject selected) { }
	public virtual void OnNothingSelected(GameManager gameManager) { }
}

