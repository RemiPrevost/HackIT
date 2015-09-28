using UnityEngine;
using System.Collections;

public class DragDropManager : MonoBehaviour {

	private Vector3 downPosition;
	private Vector3 upPosition;
	private GameObject[,] lines;
	private GameController gameController;

	void Start() {
		gameController = GameController.getGameController ();
	}

	void SetLines(GameObject[,] lines) {
		this.lines = lines;
	}

	void Update() {

		if (Input.GetMouseButtonDown(0)) {
			SetEventPosition(out downPosition);
			print ("down");
		}

		if (Input.GetMouseButtonUp(0)) {
			SetEventPosition(out upPosition);
			if ((downPosition == null) || (upPosition == null)) {
				return;
			}
			ComputeCutLines();
			print("up");
		}
	}

	private void SetEventPosition(out Vector3 position) {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		Collider collider = gameObject.GetComponent<Collider>();
		if (collider.Raycast (ray, out hit, 100.0F)) {
			position = hit.point;
		} else {
			position = new Vector3();
		}
	}

	private void ComputeCutLines() {
		LineController lineController;
		float aCut = (upPosition.z - downPosition.z) / (upPosition.x - downPosition.x);
		float bCut = upPosition.z - aCut * upPosition.x;
		float xCommon, zCommon;
		
		foreach (GameObject line in lines) {
			if (line != null) {
				lineController = line.GetComponent<LineController>();
				lineController.Deactivate();
				if (aCut != lineController.a) {
					xCommon = (lineController.b - bCut)/(aCut - lineController.a);
					zCommon = aCut * xCommon + bCut;
					if ((Mathf.Abs(lineController.startPoint.x - lineController.endPoint.x) >= Mathf.Abs(xCommon)) 
					    &&
					    (Mathf.Abs(lineController.startPoint.z - lineController.endPoint.z) >= Mathf.Abs(zCommon))) {
						print("On the line");
						if ((Mathf.Abs(upPosition.x - downPosition.x) >= Mathf.Abs(xCommon)) 
						    &&
						    (Mathf.Abs(upPosition.z - downPosition.z) >= Mathf.Abs(zCommon))) {
							print ("cut");
							lineController.Activate();
						}
					}
				}
			}
		}
	}

}
