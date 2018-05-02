using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayScript : MonoBehaviour {


	public RuntimeAnimatorController left;
	public RuntimeAnimatorController leftClose;
	public RuntimeAnimatorController right;
	public RuntimeAnimatorController rightClose;

	public GameObject animBackground;
	public GameObject animLeft;
	public GameObject animRight;

	public Camera mainCamera;

	void Start () {
		animBackground.SetActive (true);
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				switch (hit.collider.name) {
				case "Back":
					animLeft.GetComponent<Animator> ().runtimeAnimatorController = leftClose;
					animRight.GetComponent<Animator> ().runtimeAnimatorController = rightClose;
					StartCoroutine (loadMainMenu ());
					break;
				}
			}
		}
	}

	IEnumerator loadMainMenu(){
		yield return new WaitForSeconds (1f);
		SceneManager.LoadScene ("Main menu");
	}
}
