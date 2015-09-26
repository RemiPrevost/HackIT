using UnityEngine;
using System.Collections;

public class ItemController : MonoBehaviour {
	
	public int nrj;
	public int nrjMax;
	public int owner;
	public TextMesh nrjText;
	public GameObject item;
	public GameObject pedestal;
	public GameObject gameObjectSpotlightTop;
	public GameObject gameObjectSpotlightFace;
	private bool focused = false;
	public GameObject shot;
	public Transform shotSpawn;
	public GameController gameController;

	void Start() {
		gameController = GameController.getGameController();
	}

	void OnMouseUpAsButton() {
		if (this.focused) {
			gameController.onItemDeselect(this);
		} else {
			gameController.onItemSelect(this);
		}
	}

	public void Shoot(Quaternion quaternion) {
		Instantiate (shot, shotSpawn.position, quaternion);
	}

	public int GetNrj() {
		return this.nrj;
	}

	public void AlterNrjBy(int value) {

		if (value != 0) {
			this.nrj += value;

			if (this.nrj > this.nrjMax) {
				this.nrj = this.nrjMax;
			} else if (this.nrj <= 0) {
				this.nrj = 0;
			}

			this.UpdateScore ();
		}
	}

	public int getOwner() {
		return this.owner;
	}

	public void focus() {
		Light spotlight_top = gameObjectSpotlightTop.GetComponent<Light> ();
		Light spotlight_face = gameObjectSpotlightFace.GetComponent<Light> ();

		spotlight_top.intensity = 8.0F;
		spotlight_face.intensity = 8.0F;
	
		this.focused = true;
	}

	public void blur() {
		Light spotlight_top = gameObjectSpotlightTop.GetComponent<Light> ();
		Light spotlight_face = gameObjectSpotlightFace.GetComponent<Light> ();

		spotlight_top.intensity = 1.0F;
		spotlight_face.intensity = 1.0F;

		this.focused = false;
	}
	
	void Update () {
		UpdateScore ();
		UpdateColor ();
	}

	void UpdateScore() {
		nrjText.text = "" + nrj;
	}

	void UpdateColor() {
		Color new_color = GameController.getColorOfPlayer (owner);
		item.GetComponent<Renderer>().material.color = new_color;
	}


	/********************************/
	/*       PRIVATE FUNCTIONS      */
	/********************************/

}