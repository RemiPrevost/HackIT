using UnityEngine;
using System.Collections;

public class LineController : MonoBehaviour {

	/**********************************************************/
	/*********************** ATTRIBUTES ***********************/

	private int id;
	private int ownerIn, ownerOut;

	public Vector3 startPoint, endPoint;
	public float a,b;

	/**********************************************************/
	/********************* PUBLIC METHODS *********************/

	void Start() {
		Deactivate();
	}
	
	public void SetId(int id) {
		this.id = id;
	}

	public int GetId() {
		return this.id;
	}

	public int GetOwnerIn() {
		return this.ownerIn;
	}

	public int GetOwnerOut() {
		return this.ownerOut;
	}

	public void SetOwnerIn(int ownerIn) {
		this.ownerIn = ownerIn;
	}

	public void SetOwnerOut(int ownerOut) {
		this.ownerOut = ownerOut;
	}

	public void SetStartPoint(Vector3 startPoint) {
		this.startPoint = startPoint;
	}

	public void SetEndPoint(Vector3 endPoint) {
		this.endPoint = endPoint;
	}

	/**
	 * Computes the constants of the equation of this line
	 */
	public void ComputeConstants() {
		this.a = (startPoint.z-endPoint.z)/(startPoint.x - endPoint.x);
		this.b = startPoint.z - startPoint.x * this.a;
	}

	/**
	 * Set the line active by changing its color
	 */
	public void Activate() {
		gameObject.GetComponent<LineRenderer> ().SetColors(new Color (0.5f, 0.5f, 0.5f, 0.8f),new Color (0.5f, 0.5f, 0.5f, 0.8f));
	}
	
	/**
	 * Set the line inactive by changing its color
	 */
	public void Deactivate() {
		gameObject.GetComponent<LineRenderer> ().SetColors(new Color (0.5f, 0.5f, 0.5f, 0.3f),new Color (0.5f, 0.5f, 0.5f, 0.3f));
	}

	/**********************************************************/
	/********************* PRIVATE METHODS ********************/
}
