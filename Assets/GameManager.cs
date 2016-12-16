using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager gmanager;

	// Use this for initialization
	void Start () {
		gmanager = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ResetScene(){
		SceneManager.LoadScene (0);
	}
}
