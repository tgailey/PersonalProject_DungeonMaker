using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {
	public bool isUsed; //Whether or not a piece has been created on this block
	public bool[] ends; //What ends are open. If a corner-piece is created, two ends will be true and two others will be false
	int curColumn, curRow; //Current row and column of this block
	int pieceType; //What the piece type is. Each int stands for a different piece

	void Start () {
		//Get the current row and column and store it
		curColumn = InstantiateBlocks.getCurColumn (this.gameObject.transform.position);
		curRow = InstantiateBlocks.getCurRow (this.gameObject.transform.position);

		//Instantiate ends and set all to true
		ends = new bool[4]; 
		setEnds (true, true, true, true);

		//Check whether or not the piece has been created in another game
		isUsed = PlayerPrefs.GetInt (curRow.ToString() + curColumn.ToString() + "isUsed") == 1;

		#region LoadingAlreadyCreatedBlocks
		if (isUsed) {
			pieceType = PlayerPrefs.GetInt (curRow.ToString() + curColumn.ToString() + "pieceType");
			if (pieceType == 1) {
				this.loadPiece (Resources.Load ("TwoDoorRoom") as GameObject, new Vector3 (0, 0, 0));
			} else if (pieceType == 2) {
				this.loadPiece (Resources.Load ("TwoDoorRoom") as GameObject, new Vector3 (0, 90, 0));
			} else if (pieceType == 3) {
				this.loadPiece (Resources.Load ("CornerRoom") as GameObject, new Vector3 (0, 0, 0));
			} else if (pieceType == 4) {
				this.loadPiece (Resources.Load ("CornerRoom") as GameObject, new Vector3 (0, 90, 0));
			} else if (pieceType == 5) {
				this.loadPiece (Resources.Load ("CornerRoom") as GameObject, new Vector3 (0, 180, 0));
			} else if (pieceType == 6) {
				this.loadPiece (Resources.Load ("CornerRoom") as GameObject, new Vector3 (0, 270, 0));
			} else if (pieceType == 7) {
				this.loadPiece (Resources.Load ("ThreeDoorRoom") as GameObject, new Vector3 (0, 0, 0));
			} else if (pieceType == 8) {
				this.loadPiece (Resources.Load ("ThreeDoorRoom") as GameObject, new Vector3 (0, 90, 0));
			} else if (pieceType == 9) {
				this.loadPiece (Resources.Load ("ThreeDoorRoom") as GameObject, new Vector3 (0, 180, 0));
			} else if (pieceType == 10) {
				this.loadPiece (Resources.Load ("ThreeDoorRoom") as GameObject, new Vector3 (0, 270, 0));
			} else if (pieceType == 11) {
				this.loadPiece (Resources.Load ("FourDoorRoom") as GameObject, new Vector3 (0, 0, 0));
			}
			#endregion
		}
	}
	void Update () {
		//If the block is in use, turn off outside collider so that player can travel inside it
		if (isUsed) {
			GetComponent<BoxCollider> ().enabled = false;
		} else {
			GetComponent<BoxCollider> ().enabled = true;
		}
	}
	public void addPiece(GameObject p, Vector3 rot) {
		//isUsed set to true when piece is added
		this.isUsed = true;

		//Create a new piece with the GameObject and rotation provided
		GameObject tempPiece; 
		tempPiece = (GameObject)Instantiate (p, Vector3.zero, Quaternion.Euler (rot));
		tempPiece.transform.parent = this.gameObject.transform;
		tempPiece.transform.localPosition = Vector3.zero;
		tempPiece.transform.localScale = Vector3.one;

		//Set piece type to appropriate pieceType based on the GameObject and Rotation provided and use the piece from inv
		#region SettingPieceType
		if (p.Equals (Resources.Load ("TwoDoorRoom") as GameObject)) {
			if (rot.y == 0) {
				pieceType = 1;
			} else {
				pieceType = 2;
			}
			Inventory.usePiece (0);
		}
		else if (p.Equals(Resources.Load("CornerRoom") as GameObject)) {
			if (rot.y == 0) {
				pieceType = 3;
			} else if (rot.y == 90) {
				pieceType = 4;
			} else if (rot.y == 180) {
				pieceType = 5;
			} else {
				pieceType = 6;
			}
			Inventory.usePiece (1);
		}
		else if (p.Equals(Resources.Load("ThreeDoorRoom") as GameObject)) {
			if (rot.y == 0) {
				pieceType = 7;
			} else if (rot.y == 90) {
				pieceType = 8;
			} else if (rot.y == 180) {
				pieceType = 9;
			} else {
				pieceType = 10;
			}
			Inventory.usePiece (2);
		}
		else if (p.Equals(Resources.Load ("FourDoorRoom") as GameObject)) {
			pieceType = 11;
			Inventory.usePiece (3);
		}
		#endregion

		//Save the piece type and the fact that the piece is in use
		PlayerPrefs.SetInt (curRow.ToString() + curColumn.ToString() + "pieceType", pieceType);
		PlayerPrefs.SetInt (curRow.ToString() + curColumn.ToString() + "isUsed", 1);
	}
	public void deletePiece() {
		//*****NOTE***** Not tested yet

		//set isUsed to false
		isUsed = false;

		//Destroy the piece
		GameObject.Destroy (GetComponentInChildren<Piece> ().gameObject);

		//Set all the ends to true
		for (int i = 0; i < ends.Length; i++) {
			ends [i] = true;
		}

		//Save piece type as 0 and the fact the piece is no longer is use
		PlayerPrefs.SetInt (curRow.ToString() + curColumn.ToString() + "pieceType", 0);
		PlayerPrefs.SetInt (curRow.ToString() + curColumn.ToString() + "isUsed", 0);
	}
	private void loadPiece(GameObject p, Vector3 rot) {
		//SAME THING AS ADD PIECE EXCEPT DONT HAVE TO FIND PIECE TYPE AND USE PIECE FROM INVENTORY

		//create piece with specified GameObject and rot
		GameObject tempPiece;
		tempPiece = (GameObject)Instantiate (p, Vector3.zero, Quaternion.Euler (rot));
		tempPiece.transform.parent = this.gameObject.transform;
		tempPiece.transform.localPosition = Vector3.zero;
		tempPiece.transform.localScale = Vector3.one;
	}
	public void setEnds(bool north, bool east, bool south, bool west) {
		ends [0] = north;
		ends [1] = east;
		ends [2] = south;
		ends [3] = west;
	}
	public bool[] Ends {
		get {
			return ends;
		}
		set {
			ends = (bool[])value.Clone ();
		}
	}
	public void setUsed(bool used) {
		isUsed = used;
	}
	public bool getUsed() {
		return isUsed;
	}
}
