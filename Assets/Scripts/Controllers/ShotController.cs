using UnityEngine;
using System.Collections;

public class ShotController : MonoBehaviour {

	/**********************************************************/
	/*********************** ATTRIBUTES ***********************/

	public float speed;

	private int fromOwner, toOwner;
	private Vector3 startPosition, endPosition;
	private float fullDistance;
	private int count = 0;
	private GameController gameController;

	/**********************************************************/
	/********************* PUBLIC METHODS *********************/

	void Start () {
		gameController = GameController.getGameController ();
		GetComponent<Rigidbody>().velocity = transform.forward * speed;
	}

	void Update() {
		float distanceFromOrigin = Vector3.Distance (startPosition,transform.position);
		float factor = distanceFromOrigin / fullDistance;

		UpdateColor (factor);
	}

	/**
	 * On trigger enter: if the item is not an other shot, i.e. is an item
	 * and is the second item collided with, wue destroy the shot and warn
	 * the game controller;
	 * param other: the touched collider
	 */
	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "Shot") {
			return;
		}
		if (count == 0)
		{
			count++;
			return;
		}
		gameController.onShotItem (other.gameObject.GetComponent<ItemController>(),
		                           gameObject.GetComponent<ShotController>().fromOwner);
		Destroy(gameObject);
	}

	public void setToOwner(int owner) {
		this.toOwner = owner;
	}

	public void SetFromOwner(int owner) {
		this.fromOwner = owner;
	}

	public void SetStartPosition(Vector3 position) {
		this.startPosition = position;
	}

	public void SetEndPosition(Vector3 position) {
		this.endPosition = position;
	}

	public void ComputeDistance() {
		fullDistance = Vector3.Distance (startPosition, endPosition);
	}
	
	/**********************************************************/
	/********************* PRIVATE METHODS ********************/

	/**
	 * Setups the color according to the current owner
	 */
	private void UpdateColor(float factor) {
		Color originColor = ColorController.getColorOfPlayer (fromOwner);
		Color destinationColor = toOwner == 0 ? originColor : ColorController.getColorOfPlayer (toOwner);
		Color newColor = Color.Lerp (originColor, destinationColor, factor);
		gameObject.GetComponent<Renderer>().material.color = newColor;
	}
}
