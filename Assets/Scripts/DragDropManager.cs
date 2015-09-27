using UnityEngine;
using System.Collections;

public class DragDropManager : MonoBehaviour {

	private Vector3 downPosition;
	private GameObject[,] lines;
	private GameController gameController;

	void Start() {
		gameController = GameController.getGameController ();
	}

	void SetLines(GameObject[,] lines) {
		this.lines = lines;
	}

	void OnMouseDown() {
		downPosition = Input.mousePosition;
	}
	
	void OnMouseUp() {
		Vector3 upPosition = Input.mousePosition;
		LineController lineController;
		float aCut = (upPosition.x - downPosition.x) / (upPosition.y - downPosition.y);
		float bCut = upPosition.y - aCut * upPosition.x;
		float xCommon, zCommon;
	
		foreach (GameObject line in lines) {
			if (line != null) {
				print ("Not null");
				lineController = line.GetComponent<LineController>();
				if (aCut != lineController.a) {
					print ("Not parallel");
					xCommon = (lineController.b - bCut)/(lineController.a - aCut);
					zCommon = aCut * xCommon + bCut;
					if ((xCommon < lineController.startPoint.x 
					     && xCommon > lineController.endPoint.x)
					    || (xCommon < lineController.endPoint.x 
					    && xCommon > lineController.startPoint.x)) {
						print ("x in scope");
						if ((zCommon < lineController.startPoint.z 
						     && zCommon > lineController.endPoint.z)
						    || (zCommon < lineController.endPoint.z 
						    && zCommon > lineController.startPoint.z)) {
							print ("Cut line");
						}
					}
				}
			}
		}
	}
}
