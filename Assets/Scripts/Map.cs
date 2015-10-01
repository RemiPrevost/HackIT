using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map {

	/**********************************************************/
	/*********************** ATTRIBUTES ***********************/

	/* The map of the connections */
	private bool[,] map;

	/**********************************************************/
	/********************** CONSTRUCTORS **********************/

	/**
	 * Initialize the map with no connections
	 */
	public Map(int size) {
		if (size <= 0) {
			return;
		}

		map = new bool[size, size];
		for (int i=0; i<size; i++) {
			for (int j=0; j<size; j++) {
				map[i,j] = false;
			}
		}
	}

	/**********************************************************/
	/********************* PUBLIC METHODS *********************/

	/**
	 * Declares that A is now shooting at B
	 * param A: the shooter
	 * param B: the target
	 */
	public void AshootsAtB(int A, int B) {
		map [A, B] = true;
	}

	/**
	 * Declares that A is no more shooting at B
	 * param A: the shooter
	 * param B: the target
	 */
	public void AstopsShootingAtB(int A, int B) {
		map [A, B] = false;
	}
	
	/**
	 * Declares that A is no more shooting at anyone
	 * param A: the item id
	 */
	public void AstopsShooting(int A) {
		for (int i = 0; i < map.GetLength(0); i++) {
			if (A != i) {
				map[A,i] = false;
			}
		}
	}

	/**
	 * Returns true if A is currently shooting at B
	 * param A: the potential shooter
	 * param B: the potential target
	 * returns {bool}
	 */
	public bool IsAShootingAtB(int A, int B) {
		return map [A, B];
	}

	/**
	 * Returns a list of the id of the items currently shoot by A
	 * param A: the item id
	 * returns {List<int>}
	 */
	public List<int> AtWhoAIsShootingAt(int A) {
		List<int> list = new List<int>();

		for (int i = 0; i < map.GetLength(0); i++) {
			if ((map[A,i] == true)  && (i != A)) {
				list.Add(i);
			}
		}

		return list;
	}

	/**
	 * Returns a list of the id of the items currently shooting at A
	 * param A: the item id
	 * returns {List<int>}
	 */
	public List<int> ByWhoAIsShooting(int A) {
		List<int> list = new List<int>();
		
		for (int i = 0; i < map.GetLength(0); i++) {
			if ((map[i,A] == true) && (i != A)) {
				list.Add(i);
			}
		}
		
		return list;
	}

	/**********************************************************/
	/********************* PRIVATE METHODS ********************/
}
