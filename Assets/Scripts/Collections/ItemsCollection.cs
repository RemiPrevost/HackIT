using UnityEngine;
using System.Collections;

public class ItemsCollection {

	/**********************************************************/
	/*********************** ATTRIBUTES ***********************/

	/* The tab containing all the items */
	private ItemController[] tabItems;

	/**********************************************************/
	/********************** CONSTRUCTORS **********************/

	/**
     * scan for Items and create ItemController Collection
     */
	public ItemsCollection() {
		GameObject[] gameObjectItems = GameObject.FindGameObjectsWithTag("Item");
		this.tabItems = new ItemController[gameObjectItems.Length];

		for (int i = 0; i < gameObjectItems.Length; i++) {
			this.tabItems[i] = gameObjectItems[i].GetComponent<ItemController>();
			this.tabItems[i].SetId(i);
		}
	}

	/**********************************************************/
	/********************* PUBLIC METHODS *********************/

	/**
     * Returns the wanted item controller
     * @param id: this id of the wanted item
     * @returns {ItemController}
     */
	public ItemController GetItemController(int id) {
		if (id < 0 || id >= this.tabItems.Length) {
			Debug.LogError("No item with id "+id+". Force to return null");
			return null;
		}

		return tabItems [id];
	}

	/**
     * Returns the wanted item game object
     * @param id: this id of the wanted item
     * @returns {GameObject}
     */
	public GameObject GetItemObject(int id) {
		if (id < 0 || id >= this.tabItems.Length) {
			Debug.LogError("No item with id "+id+". Force to return null");
			return null;
		}
		
		return tabItems [id].gameObject;
	}

	/**
     * Returns all the items controller
     * @returns {ItemController[]}
     */
	public ItemController[] GetAllItemsController() {
		return tabItems;
	}

	/**
     * Returns the size of the collection
     * @returns {Int}
     */
	public int GetCollectionSize() {
		return tabItems.Length;
	}

	/**********************************************************/
	/********************* PRIVATE METHODS ********************/
}
