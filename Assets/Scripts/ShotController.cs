using UnityEngine;
using System.Collections;

public class ShotController : MonoBehaviour {

	public float speed;

	public int fromOwner;

	void Start () {
		GetComponent<Rigidbody>().velocity = transform.forward * speed;
	}

	void SetFromOwner(int owner) {
		this.fromOwner = owner;
		this.UpdateColor ();
	}

	private void UpdateColor() {
		Color new_color = GameController.getColorOfPlayer (fromOwner);
		gameObject.GetComponent<Renderer>().material.color = new_color;
	}
}
