using UnityEngine;
using System.Collections;

public class LineController : MonoBehaviour {

	private int ownerIn, ownerOut;
	public Vector3 startPoint, endPoint;
	public float a,b;

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

	public void ComputeConstants() {
		this.a = (startPoint.x - endPoint.x)/(startPoint.z-endPoint.z);
		this.b = startPoint.z - startPoint.x * this.a;
	}
	
	public void Activate() {
		gameObject.GetComponent<LineRenderer> ().SetColors(new Color (0.5f, 0.5f, 0.5f, 0.7f),new Color (0.5f, 0.5f, 0.5f, 0.7f));
	}

	public void Deactivate() {
		gameObject.GetComponent<LineRenderer> ().SetColors(new Color (0.5f, 0.5f, 0.5f, 0.1f),new Color (0.5f, 0.5f, 0.5f, 0.1f));
	}
}
