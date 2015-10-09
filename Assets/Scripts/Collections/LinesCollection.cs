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
		int collectionSize, position = 0;

		if (itemsCollection == null) {
			return;
		}

		collectionSize = (itemsCollection.GetCollectionSize() * (itemsCollection.GetCollectionSize() - 1)) / 2;

		this.tabLines = new LineController[collectionSize];

		for (int i = 0; i < itemsCollection.GetCollectionSize(); i++) {
			for (int j = 0; j < itemsCollection.GetCollectionSize(); j++) {
				if (j > i) {
					itemGameObjectI = itemsCollection.GetItemObject(i);
					itemGameObjectJ = itemsCollection.GetItemObject(j);
					lineObject = gameController.InstantiateLine();
					lineRenderer = lineObject.GetComponent<LineRenderer>();

					lineRenderer.SetPosition(0, new Vector3( itemGameObjectI.transform.position.x, 0, itemGameObjectI.transform.position.z));
					lineRenderer.SetPosition(1, new Vector3(itemGameObjectJ.transform.position.x, 0, itemGameObjectJ.transform.position.z));
					lineRenderer.SetWidth(0.1f, 0.1f);

					tabLines[position] = lineObject.GetComponent<LineController>();

					tabLines[position].Deactivate();
					tabLines[position].SetOwnerOut(j);
					tabLines[position].SetOwnerIn(i);
					tabLines[position].SetStartPoint(new Vector3(itemGameObjectI.transform.position.x, 0, itemGameObjectI.transform.position.z));
					tabLines[position].SetEndPoint(new Vector3(itemGameObjectJ.transform.position.x, 0, itemGameObjectJ.transform.position.z));
					tabLines[position].ComputeConstants();

					position++;
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
