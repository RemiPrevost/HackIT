using UnityEngine;
using System.Collections;

public class SelectionManager : MonoBehaviour {

	public ItemController item1 = null, item2 = null;
	private GameController gameController;
	private bool busy = false;

	void Start() {
		gameController = GameController.getGameController();
	}

	/**
	 * Called when a new item has been selected. Check if an other item
	 * is also selected and therefore warns the game controller
	 */
	public void newSelection(ItemController newItem) {
		if (busy) {
			return;
		}

		if (this.item1 == null) {
			this.item1 = newItem;
			this.item1.focus ();
		} else if (this.item2 == null) {
			this.item2 = newItem;

			if (this.item1.getOwner () != 1 && this.item2.getOwner () != 1) {
				this.item1.blur ();
				this.item1 = null;
				this.item2 = null;
			} else {
				this.item2.focus ();
				this.busy = true;
				gameController.selectionValidated(this.item1,this.item2);
				StartCoroutine (waitBeforeBlur());
			}
		} else {
			newItem.blur ();
		}
	}

	/**
	 * clear the selected items of this class and blur them
	 */
	public void deselect(ItemController item) {
		if (!this.busy) {
			item.blur ();
			this.item1 = null;
			this.item2 = null;
		}
	}

	/**
	 * Waits for 0.3s before bluring the selected items
	 */
	private IEnumerator waitBeforeBlur() {
		yield return new WaitForSeconds(0.3F);
		this.item1.blur ();
		this.item2.blur ();
		this.item1 = null;
		this.item2 = null;
		this.busy = false;
	}
}
