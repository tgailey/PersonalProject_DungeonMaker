using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerScript : MonoBehaviour {
	//TODO Still errors in availability on the sides with corner pieces. Both sides I think
	//	   ADD COOL STUFF
	//		Inventory/Saving Amount of Pieces


	GameObject selectedTarget; //target that is in player raycasting
	static string selectedTargetName; //Name of the selected target
	GameObject player; //The player itself
	public bool isPlacingPiece; //A boolean to tell if the player is currently placing a piece
	bool[] possibilities; //An array of booleans that tell what pieces are possible to place
	GameObject refBlock; //The block that is referenced when we are placing a piece. 
	//^^FOR EXAMPLE^^ if placing block on north side, refBlock will be the block one north of the one player is currently in
	Texture2D[] corRoomTextures; //An array of textures for the corner room
	int roomNum; //The room number *NOTE* this is to know what texture is to display when placing a piece
	Texture2D[] threeRoomTextures; //An array of textures for the three door room

	FirstPersonController FPC; //First Person Controller script found on player
	Inventory inventory; //Inventory script found on player

	void Start () {
		//Set player to the player and all the scripts to the player's scripts
		player = GameObject.FindGameObjectWithTag ("Player");
		FPC = player.GetComponent<FirstPersonController> ();
		inventory = player.GetComponent<Inventory> ();

		//Instantiate so that we do not get NullReference
		selectedTarget = this.gameObject;
		refBlock = this.gameObject;


		possibilities = new bool[11]; 


		//Set corRoomTextures to the corner textures repeated once
			//They look like this { "|_", ":-", "-:", "_|", "|_", ":-", "-:", "_|" }
		corRoomTextures = new Texture2D[8];
		threeRoomTextures = new Texture2D[8];
		for (int i = 0; i < 8; i++) {
			if (i < 4) {
				corRoomTextures [i] = InstantiateBlocks.getTextures () [i + 2];
			} else {
				corRoomTextures [i] = InstantiateBlocks.getTextures () [i - 2];
			}
		}
		//Set threeRoomTextures to the three room textures repeated once
			//They look like this { "-:-", "-|", "_|_", "|-","-:-", "-|", "_|_", "|-" }
		for (int i = 0; i < 8; i++) {
			if (i < 4) {
				threeRoomTextures [i] = InstantiateBlocks.getTextures () [i + 6];
			} else {
				threeRoomTextures [i] = InstantiateBlocks.getTextures () [i + 2];
			}
		}
		roomNum = 0;
	}

	void Update () {
		//If the raycast finds an endpiece and the player is close and presses E, set isPlacingPiece to true and begin placing piece
		if (selectedTarget.tag == "EndPiece" && Vector3.Distance(selectedTarget.transform.position, player.transform.position) < 25) {
			if (Input.GetKeyDown (KeyCode.E)) {
				isPlacingPiece = true;
				checkAvailability ();
				Debug.Log (selectedTarget.name);
				selectedTargetName = selectedTarget.name;
			}
		}

		//Raycast in front of player and store object that is hit
		RaycastHit hit;
		Ray ray = this.gameObject.GetComponentInChildren<Camera> ().ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out hit, 25)) {
			selectedTarget = hit.transform.gameObject;
		}
		if (isPlacingPiece) {
			//STOP Player movement and display mouse
			FPC.enabled = false;
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			Time.timeScale = 0;
		} else {
			FPC.enabled = true;
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			Time.timeScale = 1;
		}

		//DEBUG PURPOSES-WILL ERROR-C WILL CLEAR MAP SO THAT IT IS NO LONGER SAVED
		if (Input.GetKeyDown (KeyCode.C)) {
			for (int x = 0; x < InstantiateBlocks.getRows (); x++) {
				for (int z = 0; z < InstantiateBlocks.getColumns (); z++) {
					PlayerPrefs.SetInt (x.ToString () + z.ToString () + "isUsed", 0);
					PlayerPrefs.SetInt (x.ToString () + z.ToString () + "pieceType", 0);
				}
			}
		}
	}
	void checkAvailability () {
		int refRow, refColumn; //reference row and column of reference block
		refRow = InstantiateBlocks.getCurRow (); //refRow to currentRow
		refColumn = InstantiateBlocks.getCurColumn (); //refColumn to currentColumn
		GameObject[,] blocks = InstantiateBlocks.getAllBlocks();
		//If we are at northend, refrow is one up
		if (selectedTarget.name.Equals ("NorthEnd")) {
			refRow--;
			roomNum = 0;
		} else if (selectedTarget.name.Equals ("EastEnd")) {
			//If we are at northend, refrow is one up
			refColumn++;
			roomNum = 3;
		} else if (selectedTarget.name.Equals ("SouthEnd")) {
			//If we are at southend, refrow is one down
			refRow++;
			roomNum = 2;
		} else if (selectedTarget.name.Equals ("WestEnd")) {
			//If we are at westend, refcol is one left
			refColumn--;
			roomNum = 1;
		}
		refBlock = blocks [refRow, refColumn];
		possibilities = InstantiateBlocks.checkAvailability (refRow, refColumn); //Set possibilities to available possibilities

		for (int i = 0; i < possibilities.Length; i++) {
			Debug.Log (possibilities [i]);
		}
	}
	void OnGUI() {
		//SCALE GUI TO SCREEN RESOLUTION
		float rX, rY;
		float scale_width, scale_height;
		scale_width = 1296;
		scale_height = 729;
		rX = Screen.width / scale_width;
		rY = Screen.height / scale_height;
		GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(rX , rY , 1));

		#region PlacingAPiece
		//IF PLACING A PIECE IS AVAILABLE AND WE HAVE ENOUGH OF THAT PIECE IN INVENTORY, ALLOW PLAYER TO PLACE PIECE
		//DISPLAY AMOUNT OF PIECE LEFT UNDERNEATH
		if (isPlacingPiece) {
			int x = 50;
			if (possibilities [0]) {
				if (GUI.Button (new Rect (x, 550, 100, 50), InstantiateBlocks.getTextures()[1])) {
					if (Inventory.getAmountOfPiece(0) > 0) {
						refBlock.GetComponent<Block> ().addPiece (Resources.Load ("TwoDoorRoom") as GameObject, new Vector3 (0, 0, 0));
						for (int i = 0; i < possibilities.Length; i++) {
							possibilities [i] = false;
						}
						isPlacingPiece = false;
					}
				}
				GUI.Label (new Rect(x,600,100,50), "Amount Left: " + Inventory.getAmountOfPiece(0));
				x += 100;
			}  if (possibilities [1]) {
				if (GUI.Button (new Rect (x, 550, 100, 50), InstantiateBlocks.getTextures()[1])) {
					if (Inventory.getAmountOfPiece(0) > 0) {
						refBlock.GetComponent<Block> ().addPiece (Resources.Load ("TwoDoorRoom") as GameObject, new Vector3 (0, 90, 0));
						for (int i = 0; i < possibilities.Length; i++) {
							possibilities [i] = false;
						}
						isPlacingPiece = false;
					}
				}
				GUI.Label (new Rect(x,600,100,50), "Amount Left: " + Inventory.getAmountOfPiece(0));
				x += 100;
			}
			if (possibilities [2]) {
				if (GUI.Button (new Rect (x, 550, 100, 50), corRoomTextures[roomNum])) {
					if (Inventory.getAmountOfPiece(1) > 0) {
						refBlock.GetComponent<Block> ().addPiece (Resources.Load ("CornerRoom") as GameObject, new Vector3 (0, 0, 0));
						for (int i = 0; i < possibilities.Length; i++) {
							possibilities [i] = false;
						}
						isPlacingPiece = false;
					}
				}
				GUI.Label (new Rect(x,600,100,50), "Amount Left: " + Inventory.getAmountOfPiece(1));
				x += 100;
			} 
			if (possibilities [3]) {
				if (GUI.Button (new Rect (x, 550, 100, 50), corRoomTextures[1+roomNum])) {
					if (Inventory.getAmountOfPiece(1) > 0) {
						refBlock.GetComponent<Block> ().addPiece (Resources.Load ("CornerRoom") as GameObject, new Vector3 (0, 90, 0));
						for (int i = 0; i < possibilities.Length; i++) {
							possibilities [i] = false;
						}
						isPlacingPiece = false;
					}
				}
				GUI.Label (new Rect(x,600,100,50), "Amount Left: " + Inventory.getAmountOfPiece(1));
				x += 100;
			}
			if (possibilities [4]) {
				if (GUI.Button (new Rect (x, 550, 100, 50), corRoomTextures[2+roomNum])) {
					if (Inventory.getAmountOfPiece(1) > 0) {
						refBlock.GetComponent<Block> ().addPiece (Resources.Load ("CornerRoom") as GameObject, new Vector3 (0, 180, 0));
						for (int i = 0; i < possibilities.Length; i++) {
							possibilities [i] = false;
						}
						isPlacingPiece = false;
					}
				}
				GUI.Label (new Rect(x,600,100,50), "Amount Left: " + Inventory.getAmountOfPiece(1));
				x += 100;
			} 
			if (possibilities [5]) {
				if (GUI.Button (new Rect (x, 550, 100, 50), corRoomTextures[3+roomNum])) {
					if (Inventory.getAmountOfPiece(1) > 0) {
						refBlock.GetComponent<Block> ().addPiece (Resources.Load ("CornerRoom") as GameObject, new Vector3 (0, 270, 0));
						for (int i = 0; i < possibilities.Length; i++) {
							possibilities [i] = false;
						}
						isPlacingPiece = false;
					}
				}
				GUI.Label (new Rect(x,600,100,50), "Amount Left: " + Inventory.getAmountOfPiece(1));
				x += 100;
			} if (possibilities [6]) {
				if (GUI.Button (new Rect (x, 550, 100, 50), threeRoomTextures[roomNum])) {
					if (Inventory.getAmountOfPiece(2) > 0) {
						refBlock.GetComponent<Block> ().addPiece (Resources.Load ("ThreeDoorRoom") as GameObject, new Vector3 (0, 0, 0));
						for (int i = 0; i < possibilities.Length; i++) {
							possibilities [i] = false;
						}
						isPlacingPiece = false;
					}
				}
				GUI.Label (new Rect(x,600,100,50), "Amount Left: " + Inventory.getAmountOfPiece(2));
				x += 100;
			}  if (possibilities [7]) {
				if (GUI.Button (new Rect (x, 550, 100, 50), threeRoomTextures[1+roomNum])) {
					if (Inventory.getAmountOfPiece(2) > 0) {	
						refBlock.GetComponent<Block> ().addPiece (Resources.Load ("ThreeDoorRoom") as GameObject, new Vector3 (0, 90, 0));
						for (int i = 0; i < possibilities.Length; i++) {
							possibilities [i] = false;
						}
						isPlacingPiece = false;
					}
				}
				GUI.Label (new Rect(x,600,100,50), "Amount Left: " + Inventory.getAmountOfPiece(2));
				x += 100;
			}  if (possibilities [8]) {
				if (GUI.Button (new Rect (x, 550, 100, 50), threeRoomTextures[2+roomNum])) {
					if (Inventory.getAmountOfPiece(2) > 0) {
						refBlock.GetComponent<Block> ().addPiece (Resources.Load ("ThreeDoorRoom") as GameObject, new Vector3 (0, 180, 0));
						for (int i = 0; i < possibilities.Length; i++) {
							possibilities [i] = false;
						}
						isPlacingPiece = false;
					}
				}
				GUI.Label (new Rect(x,600,100,50), "Amount Left: " + Inventory.getAmountOfPiece(2));
				x += 100;
			}  if (possibilities [9]) {
				if (GUI.Button (new Rect (x, 550, 100, 50), threeRoomTextures[3+roomNum])) {
					if (Inventory.getAmountOfPiece(2) > 0) {
						refBlock.GetComponent<Block> ().addPiece (Resources.Load ("ThreeDoorRoom") as GameObject, new Vector3 (0, 270, 0));
						for (int i = 0; i < possibilities.Length; i++) {
							possibilities [i] = false;
						}
						isPlacingPiece = false;
					}
				}
				GUI.Label (new Rect(x,600,100,50), "Amount Left: " + Inventory.getAmountOfPiece(2));
				x += 100;
			}  if (possibilities [10]) {
				if (GUI.Button (new Rect (x, 550, 100, 50),InstantiateBlocks.getTextures()[10])) {
					if (Inventory.getAmountOfPiece(3) > 0) {
						refBlock.GetComponent<Block> ().addPiece (Resources.Load ("FourDoorRoom") as GameObject, new Vector3 (0, 0, 0));
						for (int i = 0; i < possibilities.Length; i++) {
							possibilities [i] = false;
						}
						isPlacingPiece = false;
					}
				}
				GUI.Label (new Rect(x,600,100,50), "Amount Left: " + Inventory.getAmountOfPiece(3));
				x += 100;
			}
			x = 0;
		}
		#endregion
	}
	public static string getPieceName() {
		return selectedTargetName;
	}
}
