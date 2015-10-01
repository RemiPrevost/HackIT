using UnityEngine;
using System.Collections;

public class LinesCollection {

	/**********************************************************/
	/*********************** ATTRIBUTES ***********************/
	
	/* The tab containing all the lines */
	private LineController[] tabLines;
	private GameController gameController = GameController.getGameController();

	/**********************************************************/
	/********************** CONSTRUCTORS **********************/

	/**
	 * Instantiates, setups and positions all the necessary lines
	 */
	public LinesCollection(ItemsCollection itemsCollection) {
		GameObject lineObject, itemGameObjectI, itemGameObjectJ;
		LineRenderer lineRenderer;
		int collectionSize;

		if (itemsCollection == null) {
			return;
		}

		collectionSize = (itemsCollection.GetCollectionSize() * (itemsCollection.GetCollectionSize() - 1)) / 2;

		this.tabLines = new LineController[collectionSize];

		for (int i = 0; i < this.tabLines.Length; i++) {
			for (int j = 0; j < this.tabLines.Length; j++) {
				if (i > j) {
					itemGameObjectI = itemsCollection.GetItemObject(i);
					itemGameObjectJ = itemsCollection.GetItemObject(j);
					lineObject = gameController.InstantiateLine();
					lineRenderer = lineObject.GetComponent<LineRenderer>();

					lineRenderer.SetPosition(0, new Vector3( itemGameObjectI.transform.position.x, 0, itemGameObjectI.transform.position.z));
					lineRenderer.SetPosition(1, new Vector3(itemGameObjectJ.transform.position.x, 0, itemGameObjectJ.transform.position.z));
					lineRenderer.SetWidth(0.1f, 0.1f);

					tabLines[i+j-1] = lineObject.GetComponent<LineController>();

					tabLines[i+j-1].Deactivate();
					tabLines[i+j-1].SetOwnerOut(j);
					tabLines[i+j-1].SetOwnerIn(i);
					tabLines[i+j-1].SetStartPoint(new Vector3(itemGameObjectI.transform.position.x, 0, itemGameObjectI.transform.position.z));
					tabLines[i+j-1].SetEndPoint(new Vector3(itemGameObjectJ.transform.position.x, 0, itemGameObjectJ.transform.position.z));
					tabLines[i+j-1].ComputeConstants();
				}
			}
		}
	}

	/**********************************************************/
	/********************* PUBLIC METHODS *********************/

	/**
     * Returns the wanted item controller
     * @param id: this id of the wanted item
     * @returns {LineController}
     */
	public LineController GetLineController(int id) {
		if (id < 0 || id >= this.tabLines.Length) {
			Debug.LogError("No line with id "+id+". Force to return null");
			return null;
		}

		return tabLines [id];
	}

	public LineController GetLineControllerBetween(int A, int B) {
		return this.tabLines[A + B - 1];
	}


	/**
     * Returns all the items controller
     * @returns {LineController[]}
     */
	public LineController[] GetAllLinesController() {
		return tabLines;
	}

	/**
     * Returns the size of the collection
     * @returns {Int}
     */
	public int GetCollectionSize() {
		return tabLines.Length;
	}

	/**********************************************************/
	/********************* PRIVATE METHODS ********************/

}
