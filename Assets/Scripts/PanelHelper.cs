using UnityEngine;
using System.Collections;

public class PanelHelper : MonoBehaviour {
	public Vector2 inset;

	// Use this for initialization
	void Start () {
		SetToDefaultPosition();
		gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetToDefaultPosition() {
		RectTransform panelRect = (RectTransform)transform;
		panelRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, inset.x, panelRect.rect.width);
		panelRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, inset.y, panelRect.rect.height);
	}
}
