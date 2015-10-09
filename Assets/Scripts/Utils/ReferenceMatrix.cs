public class ReferenceMatrix{

	/**********************************************************/
	/*********************** ATTRIBUTES ***********************/
	
	private int[,] referenceMatrix;

	/**********************************************************/
	/********************** CONSTRUCTORS **********************/
	
	public ReferenceMatrix(int size) {
		int position = 0;

		if (size <= 0) {
			return;
		}

		this.referenceMatrix = new int[size, size];

		for (int i = 0; i < size; i++) {
			for (int j = 0; j < size; j++) {
				if (i < j) {
					referenceMatrix[i,j] = position;
					position++;
				}
			}
		}
	}

	/**********************************************************/
	/********************* PUBLIC METHODS *********************/

	/**
	 * Returns the position of the element at the coordinates indexes
	 * @param indexes: the indexes of the element
	 * @return the position
	 */
	public int GetPositionFromCoord(Indexes indexes) {
		int i = indexes.i;
		int j = indexes.j;
		int temp;

		if (i < 0 || j < 0 || i == j) {
			ErrorManager.DisplayErrorMessage("ReferenceMatrix.GetPositionFromCoord: Invalid indexes ("+ i +","+ j + ")");
			return -1;
		}

		if (i > j) {
			temp = i;
			i = j;
			j = temp;
		}

		return referenceMatrix [i, j];
	}
}
