using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollision : MonoBehaviour {

	void OnCollisionEnter(Collision coll){
		print (coll.collider.tag);
		if (coll.collider.tag == "Bonus") {
			print ("Yes");
			PlayScript.positionPointGenerating (gameObject);
		}
			
	}
}
