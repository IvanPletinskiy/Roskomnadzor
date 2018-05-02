using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScriot : MonoBehaviour {

	public Camera mainCamera;

	void Start () {
		
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				switch (hit.collider.name) {
				case "Play":
					SceneManager.LoadScene ("Play");
					break;
				}
			}
		}
	}
}
