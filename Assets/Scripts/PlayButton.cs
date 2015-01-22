using UnityEngine;
using System.Collections;

public class PlayButton : MonoBehaviour {
	public string levelName;
	
	public void OnPlayClick() {
		Application.LoadLevel(levelName);
	}
}
