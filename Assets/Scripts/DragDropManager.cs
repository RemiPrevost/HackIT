using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
		}

		if (Input.GetMouseButtonUp(0)) {
			SetEventPosition(out upPosition);
			if (upPosition == downPosition) {
				return;
			}
			ComputeCutLines();
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
		List<Indexes> cutLines = new List<Indexes>();
		LineController lineController;
		float aCut = (upPosition.z - downPosition.z) / (upPosition.x - downPosition.x);
		float bCut = upPosition.z - aCut * upPosition.x;
		float xCommon;
		
		for (int i = 0; i < lines.GetLength(0); i++) {
			for (int j = 0; j < lines.GetLength(0); j++) {
				if (i < j) {
					lineController = lines[i,j].GetComponent<LineController>();
					if (lineController.a == Mathf.Infinity || aCut == Mathf.Infinity || aCut != lineController.a) {
						xCommon = (lineController.b - bCut)/(aCut - lineController.a);
						if (xCommon < Mathf.Max(lineController.startPoint.x, lineController.endPoint.x)
						    &&
						    xCommon > Mathf.Min(lineController.startPoint.x, lineController.endPoint.x)
						    &&
						    xCommon < Mathf.Max(upPosition.x, downPosition.x)
						    &&
						    xCommon > Mathf.Min(upPosition.x, downPosition.x)) {
							cutLines.Add(new Indexes(i,j));
						}
					}
				}
			}
		}
		gameController.onCutLines (cutLines);
	}

}
