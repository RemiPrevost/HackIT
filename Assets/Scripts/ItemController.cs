using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	private List<int> shots = new List<int>();
	private List<int> pendingShots = new List<int>();

	void Start() {
		gameController = GameController.getGameController();
		UpdateColor ();
		UpdateScore ();
	}

	void OnMouseUpAsButton() {
		if (this.focused) {
			gameController.onItemDeselect(this);
		} else {
			gameController.onItemSelect(this);
		}
	}

	public bool IsShootingAt(int target) {
		return shots.Contains (target);
	}
	
	public bool SuspendShootingAt(int target) {
		if (IsShootingAt (target) && !pendingShots.Contains(target)) {
			pendingShots.Add (target);
			return true;
		} else {
			return false;
		}
	}

	public bool ResumeShootingAt(int target) {
		return pendingShots.Remove(target);
	}

	public bool StopShootingAt(int target) {
		return shots.Remove (target) && pendingShots.Remove(target);
	}

	public bool StartShootingAt(Quaternion quaternion, int target) {
		if (!shots.Contains (target)) {
			shots.Add(target);
			StartCoroutine (ShootingWave (quaternion,target));
			return true;
		}
		return false;
	}

	private IEnumerator ShootingWave(Quaternion quaternion, int target) {
		while (nrj > 0 && shots.Contains(target)) {
			if( !pendingShots.Contains(target)) {
				Shoot(quaternion);
			}
			yield return new WaitForSeconds (waveWait);
		}
	}

	private void Shoot(Quaternion quaternion) {
		GameObject shot = (GameObject) Instantiate (this.shot, shotSpawn.position, quaternion);
		shot.SendMessage ("SetFromOwner", this.owner);
		this.AlterNrjBy (-1, -1);
	}

	public int GetNrj() {
		return this.nrj;
	}

	public void AlterNrjBy(int value, int shooterOwnerCode) {
		int before = nrj;

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
			else if (nrj == nrjMax) {
				gameController.onFullNrj(this);
			}
			else if (before == nrjMax) {
				gameController.onNoMoreFullNrj(this);
			}
			if (before == 0) {
				gameController.OnItemAcquiredBy(this,shooterOwnerCode);
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