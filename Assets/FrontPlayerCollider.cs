using UnityEngine;
using System.Collections;

public class FrontPlayerCollider : MonoBehaviour {


	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Obstacle") {
			PlayerController.player.Death ();
		}

	}
}
