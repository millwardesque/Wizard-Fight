/// <summary>
/// Base class for In-Game states.
/// </summary>
public class InGameState {
	public virtual void OnUpdate(GameManager gameManager) { }
	public virtual void OnEnter(GameManager gameManager) { }
	public virtual void OnExit(GameManager gameManager) { }
}

