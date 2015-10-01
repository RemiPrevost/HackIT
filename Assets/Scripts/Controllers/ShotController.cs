using UnityEngine;
using System.Collections;

public class ShotController : MonoBehaviour {

	/**********************************************************/
	/*********************** ATTRIBUTES ***********************/

	public float speed;
	public int fromOwner;

	private int count = 0;
	private GameController gameController;

	/**********************************************************/
	/********************* PUBLIC METHODS *********************/

	void Start () {
		gameController = GameController.getGameController ();
		GetComponent<Rigidbody>().velocity = transform.forward * speed;
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

	public void SetFromOwner(int owner) {
		this.fromOwner = owner;
		this.UpdateColor ();
	}
	
	/**********************************************************/
	/********************* PRIVATE METHODS ********************/

	/**
	 * Setups the color according to the current owner
	 */
	private void UpdateColor() {
		Color new_color = GameController.getColorOfPlayer (fromOwner);
		gameObject.GetComponent<Renderer>().material.color = new_color;
	}
}
