using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DragDropManager : MonoBehaviour {

	private Vector3 downPosition;
	private Vector3 upPosition;
	private LinesCollection linesCollection;
	private GameController gameController;

	void Start() {
		gameController = GameController.getGameController ();
	}

	void SetLines(LinesCollection linesCollection) {
		this.linesCollection = linesCollection;
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

	/**
	 * Thanks to Raycast on the background, determines the position of the
	 * mouse event
	 */ 
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

	/**
	 * When the mouse click went up and down, we search for the lines located in the
	 * path between the up and down position and warns the game controller with all
	 * the cut lines
	 */
	private void ComputeCutLines() {
		List<Indexes> cutLines = new List<Indexes>();
		LineController lineController;
		float aCut = (upPosition.z - downPosition.z) / (upPosition.x - downPosition.x);
		float bCut = upPosition.z - aCut * upPosition.x;
		float xCommon;

		for (int i = 0; i < linesCollection.GetCollectionSize(); i++) {
			lineController = linesCollection.GetLineController(i);
			if (lineController.a == Mathf.Infinity || aCut == Mathf.Infinity || aCut != lineController.a) {
				xCommon = (lineController.b - bCut)/(aCut - lineController.a);
				if (xCommon < Mathf.Max(lineController.startPoint.x, lineController.endPoint.x)
				    &&
				    xCommon > Mathf.Min(lineController.startPoint.x, lineController.endPoint.x)
				    &&
				    xCommon < Mathf.Max(upPosition.x, downPosition.x)
				    &&
				    xCommon > Mathf.Min(upPosition.x, downPosition.x)) {
					cutLines.Add(new Indexes(lineController.GetOwnerIn(),lineController.GetOwnerOut()));
				}
			}
		}

		gameController.onCutLines (cutLines);
	}

}
