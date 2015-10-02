using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemController : MonoBehaviour {

	/**********************************************************/
	/*********************** ATTRIBUTES ***********************/

	private int id;
	public int nrj;
	public int nrjMax;
	public int owner;
	public TextMesh nrjText;
	public GameObject item;
	public GameObject pedestal;
	public GameObject gameObjectSpotlightTop;
	public GameObject gameObjectSpotlightFace;
	public GameObject shotPrefab;
	public Transform shotSpawn;
	public float waveWait;

	private GameController gameController;
	private bool focused = false;
	private List<int> shots = new List<int>();
	private List<int> pendingShots = new List<int>();

	/**********************************************************/
	/********************* PUBLIC METHODS *********************/

	void Start() {
		gameController = GameController.getGameController();
		UpdateColor ();
		UpdateScore ();
		blur ();
	}

	/**
	 * Warns that the button has been selected or deselected
	 */
	void OnMouseUpAsButton() {
		if (this.focused) {
			gameController.onItemDeselect(this);
		} else {
			gameController.onItemSelect(this);
		}
	}

	/**
	 * Returns true if the item is currently shooting at target
	 * param target: the potential target
	 * returns {bool}
	 */
	public bool IsShootingAt(int target) {
		return shots.Contains (target);
	}

	/**
	 * Suspends the shooting wave to the target. Returns true if succes
	 * param target: the target
	 * return {bool}
	 */
	public bool SuspendShootingAt(int target) {
		if (IsShootingAt (target) && !pendingShots.Contains(target)) {
			pendingShots.Add (target);
			return true;
		} else {
			return false;
		}
	}

	/**
	 * Resumes the shooting wave to the target. Returns true if succes
	 * param target: the target
	 * return {bool}
	 */
	public bool ResumeShootingAt(int target) {
		return pendingShots.Remove(target);
	}

	/**
	 * Stops the shooting wave to the target. Returns true if succes
	 * param target: the target
	 * return {bool}
	 */
	public bool StopShootingAt(int target) {
		return shots.Remove (target) && pendingShots.Remove(target);
	}

	/**
	 * Suspends the shooting wave to the target, in its direction with the quaternion
	 * parameter. Returns true if succes
	 * param quaternion: the quaternion for proper rotation to target
	 * param target: the target
	 * return {bool}
	 */
	public bool StartShootingAt(Quaternion quaternion, int target) {
		if (!shots.Contains (target)) {
			shots.Add(target);
			StartCoroutine (ShootingWave (quaternion,target));
			return true;
		}
		return false;
	}

	/**
	 * Loops for regular shoots to target
	 * param quaternion: the quaternion for proper rotation to target
	 * param target: the target
	 * return {IEnumerator}
	 */
	private IEnumerator ShootingWave(Quaternion quaternion, int target) {
		while (nrj > 0 && shots.Contains(target)) {
			if( !pendingShots.Contains(target)) {
				Shoot(quaternion,target);
			}
			yield return new WaitForSeconds (waveWait);
		}
	}

	public int GetNrj() {
		return this.nrj;
	}

	/**
	 * Increases or decreases the nrj by 'value' points. Warns the game controller
	 * if the nrj drops to 0, recovers from 0, reached fullness or recovers from
	 * fullness
	 * param value: the number of altering points
	 * param shooterOwnerCode: the owner of the shoot 
	 */
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
			this.UpdateColor();

			if (nrj == 0) {
				gameController.onZeroNrj(this);
				pendingShots.Clear();
				shots.Clear();
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

	/**
	 * Focuses the item by increasing the power of the spotlights
	 */
	public void focus() {
		Light spotlight_top = gameObjectSpotlightTop.GetComponent<Light> ();
		Light spotlight_face = gameObjectSpotlightFace.GetComponent<Light> ();

		spotlight_top.intensity = 8.0F;
		spotlight_face.intensity = 8.0F;
	
		this.focused = true;
	}

	/**
	 * Focuses the item by decreasing the power of the spotlights
	 */
	public void blur() {
		Light spotlight_top = gameObjectSpotlightTop.GetComponent<Light> ();
		Light spotlight_face = gameObjectSpotlightFace.GetComponent<Light> ();

		spotlight_top.intensity = 3.0F;
		spotlight_face.intensity = 3.0F;

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

	public bool IsFull() {
		return nrj == nrjMax;
	}

	/**********************************************************/
	/********************* PRIVATE METHODS ********************/

	/**
	 * Instantiate a shot in the good direction thanks to the quaternion input.
	 * Alters the nrj by -1
	 * param quaternion: the quaternion for rotation to the shot
	 */
	private void Shoot(Quaternion quaternion, int target) {
		ShotController shot = ((GameObject) Instantiate (this.shotPrefab, shotSpawn.position, quaternion)).GetComponent<ShotController>();
		ItemController targetController = gameController.GetItemController (target);

		shot.SetFromOwner(this.owner);
		shot.setToOwner (targetController.owner);
		shot.SetStartPosition (shotSpawn.position);
		shot.SetEndPosition (targetController.shotSpawn.position);
		shot.ComputeDistance ();
		this.AlterNrjBy (-1, -1);
	}

	private void UpdateScore() {
		nrjText.text = "" + nrj;
	}
	
	private void UpdateColor() {
		Color newColor = ColorController.getColorOfPlayer (owner);
		float s = Mathf.Sqrt((float)nrj / (float)nrjMax);

		newColor = ColorController.DesaturateColorBy (newColor, s);
		item.GetComponent<Renderer>().material.color = newColor;
	}
}