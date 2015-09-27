using UnityEngine;
using System.Collections;

public class ItemController : MonoBehaviour {

	private int id;
	public int nrj;
	public int nrjMax;
	public int owner;
	public TextMesh nrjText;
	public GameObject item;
	public GameObject pedestal;
	public GameObject gameObjectSpotlightTop;
	public GameObject gameObjectSpotlightFace;
	public GameObject shot;
	public Transform shotSpawn;
	public float waveWait;

	private GameController gameController;
	private bool focused = false;

	void Start() {
		gameController = GameController.getGameController();
		UpdateColor ();
	}

	void OnMouseUpAsButton() {
		if (this.focused) {
			gameController.onItemDeselect(this);
		} else {
			gameController.onItemSelect(this);
		}
	}

	public void StartShootingWave(Quaternion quaternion) {
		StartCoroutine (ShootingWave (quaternion));
	}

	private IEnumerator ShootingWave(Quaternion quaternion) {
		while (nrj > 0) {
			Shoot(quaternion);
			yield return new WaitForSeconds (waveWait);
		}
	}

	private void Shoot(Quaternion quaternion) {
		GameObject shot = (GameObject) Instantiate (this.shot, shotSpawn.position, quaternion);
		shot.SendMessage ("SetFromOwner", this.owner);
		this.AlterNrjBy (-1);
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

			if (nrj == 0) {
				gameController.onZeroNrj(this);
			}
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

	public void SetId(int id) {
		this.id = id;
	}

	public int GetId() {
		return this.id;
	}

	public void SetOwner(int newOwner) {
		this.owner = newOwner;
		this.UpdateColor ();
	}
	
	/********************************/
	/*       PRIVATE FUNCTIONS      */
	/********************************/

	private void UpdateScore() {
		nrjText.text = "" + nrj;
	}
	
	private void UpdateColor() {
		Color new_color = GameController.getColorOfPlayer (owner);
		item.GetComponent<Renderer>().material.color = new_color;
	}
}