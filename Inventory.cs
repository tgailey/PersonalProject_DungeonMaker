using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {
	private static int[] pieceAmounts; //array of the amounts of each piece

	// Use this for initialization
	void Start () {
		pieceAmounts = new int[4];
		//Cheat sheet
		/*
		 * 1- Two Door Hallway
		 * 2 - Corner Room
		 * 3 - Three Door Hallway
		 * 4 - Four Door Hallway
		 * */
	}

	public static int[] getPieceAmounts() {
		calculateAmounts (); //recalculate amounts left
		return pieceAmounts; //return whole array
	}
	public static int getPieceTotal() {
		calculateAmounts (); //recalculate amounts left
		int temp = 0;
		//transverse the list and add up total and return it
		for (int i = 0; i < pieceAmounts.Length; i++) {
			temp += PlayerPrefs.GetInt ("AmountOfPiece" + i.ToString ());
		}
		return temp;
	}
	public static int getAmountOfPiece (int pieceNumber) {
		calculateAmounts (); //Recalculate amounts left
		return pieceAmounts [pieceNumber]; //return amount of that piece
	}
	public static void usePiece (int pieceNumber) {
		//Using a piece will decrease the amount of that piece by 1
		PlayerPrefs.SetInt ("AmountOfPiece" + pieceNumber.ToString (), PlayerPrefs.GetInt ("AmountOfPiece" + pieceNumber.ToString ()) - 1);
	}
	// Update is called once per frame
	void Update () {
		//WILL BE CHANGED TO PART OF "STORY" LATER, BUT FOR NOW PRESSING P WILL GIVE YOU FIVE OF EACH PIECE
		if (Input.GetKeyDown (KeyCode.P)) {
			for (int j = 0; j < pieceAmounts.Length; j++) {
				PlayerPrefs.SetInt ("AmountOfPiece" + j.ToString (), 5);
			}
		}
	}
	private static void calculateAmounts() {
		//SETS AMOUNTS OF PIECE LEFT TO WHAT IS SAVED
		for (int i = 0; i < pieceAmounts.Length; i++) {
			pieceAmounts [i] = PlayerPrefs.GetInt ("AmountOfPiece" + i.ToString ());
		}
	}
}
