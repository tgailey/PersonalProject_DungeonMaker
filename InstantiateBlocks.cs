using UnityEngine;
using System.Collections;

public class InstantiateBlocks : MonoBehaviour {

	#region initializingVariables
	private static int rows = 9, columns = 9; //Default rows and columns, will be changed later on based on level
	private static GameObject[,] allBlocks; //MultiDimensional Array of every block created
	private static Texture2D[] block2DRep; //An array of textures that compose of all the pieces in texture format
	private Transform playerTransform; //The transform of the player

	private float deltaRotation; //Amount to rotate the minimap by
	private Vector2 mapCenter; //Center of map to rotate around 
	private const int MINIMAP_WIDTH = 300; //X size of MiniMap
	private const int MINIMAP_LENGTH = 300; //Y size of MiniMap

	private GUISkin mapSkin; //GUISkin that has properties ideal for the map
	#endregion
	// Use this for initialization
	void Awake() {
		//Instantiate the textures so that they can be accessed before class is instantiated
		block2DRep = new Texture2D[11];
		for (int i = 0; i < block2DRep.Length; i++) {
			block2DRep[i] = Resources.Load ("GUI/Textures/Piece" + (i+1).ToString ()) as Texture2D;
		}
		mapSkin = Resources.Load ("GUI/Skins/mapSkin") as GUISkin;
	}
	void Start () {
		mapCenter = new Vector2 (10 + MINIMAP_WIDTH/2, 10 + MINIMAP_LENGTH/2); //Set pivot point to center of MiniMap

		allBlocks = new GameObject[rows,columns]; //Instantiate multidimensional array
		//Two Four Loops that go one before and one after the MD array. The ones before and after the array are "edge pieces" that
		//trap the player in. The values inside the array are added to the array, while those outside are saved as certain pieces
		for (int x = -1; x <= rows; x += 1) {
			for (int z = -1; z <= columns; z += 1) {
				//Create a block at every position, and add it to the array if needed
				string temp = "";
				temp += System.Convert.ToChar(x + 64);
				temp += System.Convert.ToChar (z + 48);
				GameObject cube = (GameObject)Instantiate (Resources.Load("Block") as GameObject, new Vector3 (x*50, 0, z*50), Quaternion.Euler(Vector3.zero));
				cube.name = temp;
				cube.transform.parent = this.gameObject.transform;
				if (x != -1 && z != -1 && x != rows && z != columns) {
					allBlocks [x, z] = cube;
				}  
				#region EdgePieces
				//This region will set the outside pieces to their appropriate piecetypes to effectively lock the player in
				//Top Left
				if (x == -1 && z == -1) {
					PlayerPrefs.SetInt (x.ToString () + z.ToString () + "isUsed", 1);
					PlayerPrefs.SetInt (x.ToString () + z.ToString () + "pieceType", 4);
				}
				//Top Right 
				else if (x == -1 && z == columns) {
					PlayerPrefs.SetInt (x.ToString () + z.ToString () + "isUsed", 1);
					PlayerPrefs.SetInt (x.ToString () + z.ToString () + "pieceType", 5);
				}
				//Bottom Left
				else if (x == rows && z == -1) {
					PlayerPrefs.SetInt (x.ToString () + z.ToString () + "isUsed", 1);
					PlayerPrefs.SetInt (x.ToString () + z.ToString () + "pieceType", 3);
				}
				//Bottom Right
				else if (x == rows && z == columns) {
					PlayerPrefs.SetInt (x.ToString () + z.ToString () + "isUsed", 1);
					PlayerPrefs.SetInt (x.ToString () + z.ToString () + "pieceType", 6);
				}
				//Top Row
				else if (x == -1) {
					PlayerPrefs.SetInt (x.ToString () + z.ToString () + "isUsed", 1);
					PlayerPrefs.SetInt (x.ToString () + z.ToString () + "pieceType", 1);
				}
				//Left Row
				else if (z == -1) {
					PlayerPrefs.SetInt (x.ToString () + z.ToString () + "isUsed", 1);
					PlayerPrefs.SetInt (x.ToString () + z.ToString () + "pieceType", 2);
				} 

				//Right Row
				else if (z == columns) {
					PlayerPrefs.SetInt (x.ToString () + z.ToString () + "isUsed", 1);
					PlayerPrefs.SetInt (x.ToString () + z.ToString () + "pieceType", 2);
				}
				//Bottom Row
				else if (x == rows) {
					PlayerPrefs.SetInt (x.ToString () + z.ToString () + "isUsed", 1);
					PlayerPrefs.SetInt (x.ToString () + z.ToString () + "pieceType", 1);
				}
				#endregion
			}
		}

		//Set starting point to a four-Door room
		PlayerPrefs.SetInt (4.ToString () + 4.ToString () + "isUsed", 1);
		PlayerPrefs.SetInt (4.ToString () + 4.ToString () + "pieceType", 11);
	}

	void Update () {
		//Set the playerTransform the the transform of the player
		playerTransform = GameObject.FindGameObjectWithTag ("Player").transform;
	}
	public static GameObject[,] getAllBlocks() 
	{
		//Return the multidimensional array of allBlocks
		return allBlocks;
	}
	public static GameObject getBlock(int x, int z) 
	{
		//Return the specific block given a row and column
		return allBlocks [x,z];
	}
	public static GameObject getBlock(Vector3 pos) {
		//get the specific block in the array based on the position specified
		GameObject block = allBlocks [0, 0];
		for (int x = -1; x <= rows; x += 1) {
			for (int z = -1; z <= columns; z += 1) {
				if ((pos.x >= x * 50 && pos.x < (x*50)+50) && (pos.z >= z * 50 && pos.z < (z*50)+50)) {
					block = allBlocks [x, z];
				}
			}
		}
		return block;
	}
	public static Texture2D[] getTextures() {
		//return all the textures of the pieces (done so I don't have to instantiate the textures multiple times)
		return block2DRep;
	}
	public static int getRows() {
		//return the total amount of rows - (DONE SO I CAN CHANGE ROWS ON OTHER LEVELS AND STILL USE CODE)
		return rows;
	}
	public static int getCurRow(Vector3 pos) {
		//Given a position, find the row of the object at said position
		int rw = 0;

		//If the position is within the bounds, return the one in the rows
		for (int x = -1; x <= rows; x += 1) {
			for (int z = -1; z <= columns; z += 1) {
				//NEED TO TEST- DO I NEED TO RUN THE COLUMNS
				if ((pos.x >= x * 50 && pos.x < (x*50)+50)) {
					rw = x;
				}
			}
		}
		return rw;
	}
	public static int getCurRow () {
		//Return the current row that the player is in
		int rw = 0;
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		for (int x = -1; x <= rows; x += 1) {
			for (int z = -1; z <= columns; z += 1) {
				//NEED TO TEST- DO I NEED TO RUN THE COLUMNS
				if ((player.transform.position.x < (x * 50) + 25 && player.transform.position.x > (x*50) - 25)
					&& (player.transform.position.z < (z * 50) + 25 && player.transform.position.z > (z * 50) - 25)) {
					rw = x;
				}
			}
		}
		return rw;
	}
	public static int getColumns() 
	{
		//return the total amount of columns - (DONE SO I CAN CHANGE COLUMNS ON OTHER LEVELS AND STILL USE CODE)
		return columns;
	}
	public static int getCurColumn(Vector3 pos) 
	{
		//Given a position, find the column of the object at said position
		int col = 0;

		//If the position is within the bounds, return the one in the columns
		for (int x = -1; x <= rows; x += 1) {
			//NEED TO TEST- DO I NEED TO RUN THE ROWS
			for (int z = -1; z <= columns; z += 1) {
				if ((pos.z >= z * 50 && pos.z < (z*50)+50)) {
					col = z;
				}
			}
		}
		return col;
	}
	public static int getCurColumn () {
		//Return the current row that the player is in
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		int col = 0;
		for (int x = -1; x <= rows; x += 1) {
			for (int z = -1; z <= columns; z += 1) {
				if ((player.transform.position.x < (x * 50) + 25 && player.transform.position.x > (x*50) - 25)
					&& (player.transform.position.z < (z * 50) + 25 && player.transform.position.z > (z * 50) - 25)) {
					col = z;
				}
			}
		}
		return col;
	}
	#region checkAvailability Code
	//To be added to when more blocks are available
	public static bool[] checkAvailability(int refRow, int refColumn) {
		bool[] possibilities = new bool[11];

		//1 - two door room - 0 degrees
		//East - West
		//2 - two door room - 90 degrees
		//North - South
		//3 - corner room - 0 degrees
		//North - East
		//4 - corner room - 90 degrees
		//East - South
		//5 - corner room - 180 degrees
		//South - West
		//6 - corner room - 270 degrees
		//North - West
		//7 - three door room - 0 degrees
		//East - South - West
		//8 - three door room - 90 degrees
		//North - South - West
		//9 - three door room - 180 degrees
		//North - East - West
		//10 - three door room - 270 degrees
		//North - East - South
		//11 - four door room
		//All Directions

		//Calculate if able to place a certain block heres
		#region TwoDoorHallway EastWest
		if (refColumn != InstantiateBlocks.getColumns ()-1 && refColumn != 0) {
			if (getAllBlocks() [refRow, refColumn + 1].GetComponent<Block> ().Ends [3] &&getAllBlocks() [refRow, refColumn - 1].GetComponent<Block> ().Ends [1]) {
				if (refRow != InstantiateBlocks.getRows () - 1 && refRow != 0) {
					if (getAllBlocks() [refRow + 1, refColumn].GetComponent<Block> ().getUsed () ||getAllBlocks() [refRow - 1, refColumn].GetComponent<Block> ().getUsed ()) {
						if (getAllBlocks() [refRow + 1, refColumn].GetComponent<Block> ().getUsed () &&getAllBlocks() [refRow - 1, refColumn].GetComponent<Block> ().getUsed ()) {
							if ((!getAllBlocks() [refRow + 1, refColumn].GetComponent<Block> ().Ends [0] && !getAllBlocks() [refRow - 1, refColumn].GetComponent<Block> ().Ends [2])) {
								possibilities [0] = true;
							}
						} else if (getAllBlocks() [refRow + 1, refColumn].GetComponent<Block> ().getUsed ()) {
							if ((!getAllBlocks() [refRow + 1, refColumn].GetComponent<Block> ().Ends [0])) {
								possibilities [0] = true;
							}
						} else if (getAllBlocks() [refRow - 1, refColumn].GetComponent<Block> ().getUsed ()) {
							if ((!getAllBlocks() [refRow - 1, refColumn].GetComponent<Block> ().Ends [2])) {
								possibilities [0] = true;
							}
						}
					} else {
						possibilities [0] = true;
					}
				} else if (refRow == InstantiateBlocks.getRows() -1) {
					if(getAllBlocks() [refRow - 1, refColumn].GetComponent<Block> ().getUsed ()) {
						if (!getAllBlocks() [refRow - 1, refColumn].GetComponent<Block> ().Ends [2]) {
							possibilities [0] = true;
						}
					} else {
						possibilities [0] = true;
					}
				} else if (refRow == 0) {
					if(getAllBlocks() [refRow +1, refColumn ].GetComponent<Block> ().getUsed ()) {
						if (!getAllBlocks() [refRow+1, refColumn].GetComponent<Block> ().Ends [0]) {
							possibilities [0] = true;
						}
					} else {
						possibilities [0] = true;
					}
				}
			}
		}
		#endregion
		#region TwoDoorHallway NorthSouth
		if (refRow != InstantiateBlocks.getRows () - 1 && refRow != 0) {
			if(getAllBlocks() [refRow + 1, refColumn].GetComponent<Block> ().Ends [0] &&getAllBlocks() [refRow - 1, refColumn].GetComponent<Block> ().Ends [2]) {
				if (refColumn != InstantiateBlocks.getColumns ()-1 && refColumn != 0) {
					if(getAllBlocks() [refRow, refColumn + 1].GetComponent<Block> ().getUsed () ||getAllBlocks() [refRow, refColumn - 1].GetComponent<Block> ().getUsed ()) {
						if(getAllBlocks() [refRow, refColumn + 1].GetComponent<Block> ().getUsed () &&getAllBlocks() [refRow, refColumn - 1].GetComponent<Block> ().getUsed ()) {
							if ((!getAllBlocks() [refRow, refColumn + 1].GetComponent<Block> ().Ends [3] && !getAllBlocks() [refRow, refColumn - 1].GetComponent<Block> ().Ends [1])) {
								possibilities [1] = true;
							}
						} else if(getAllBlocks() [refRow, refColumn + 1].GetComponent<Block> ().getUsed ()) {
							if ((!getAllBlocks() [refRow, refColumn + 1].GetComponent<Block> ().Ends [3])) {
								possibilities [1] = true;
							}
						} else if(getAllBlocks() [refRow, refColumn - 1].GetComponent<Block> ().getUsed ()) {
							if ((!getAllBlocks() [refRow, refColumn - 1].GetComponent<Block> ().Ends [1])) {
								possibilities [1] = true;
							}
						}
					} else {
						possibilities [1] = true;
					}
				} else if (refColumn == InstantiateBlocks.getColumns() -1) {
					if(getAllBlocks() [refRow, refColumn - 1].GetComponent<Block> ().getUsed ()) {
						if (!getAllBlocks() [refRow, refColumn - 1].GetComponent<Block> ().Ends [1]) {
							possibilities [1] = true;
						}
					} else {
						possibilities [1] = true;
					}
				} else if (refColumn == 0) {
					if(getAllBlocks() [refRow, refColumn + 1].GetComponent<Block> ().getUsed ()) {
						if (!getAllBlocks() [refRow, refColumn + 1].GetComponent<Block> ().Ends [3]) {
							possibilities [1] = true;
						}
					} else {
						possibilities [1] = true;
					}
				}
			}
		}
		#endregion
		#region CornerRoom NorthEast
		if (refRow != 0 && refColumn != InstantiateBlocks.getColumns ()-1) {
			if(getAllBlocks() [refRow - 1, refColumn].GetComponent<Block> ().Ends [2] &&getAllBlocks() [refRow, refColumn + 1].GetComponent<Block> ().Ends [3]) {
				if (refRow != InstantiateBlocks.getRows () - 1 && refColumn != 0) {
					if (getAllBlocks () [refRow + 1, refColumn].GetComponent<Block> ().getUsed () || getAllBlocks () [refRow, refColumn - 1].GetComponent<Block> ().getUsed ()) {
						if (getAllBlocks () [refRow + 1, refColumn].GetComponent<Block> ().getUsed () && getAllBlocks () [refRow, refColumn - 1].GetComponent<Block> ().getUsed ()) {
							if (!getAllBlocks () [refRow + 1, refColumn].GetComponent<Block> ().Ends [0] && !getAllBlocks () [refRow, refColumn - 1].GetComponent<Block> ().Ends [1]) {
								possibilities [2] = true;
							}
						} else if (getAllBlocks () [refRow + 1, refColumn].GetComponent<Block> ().getUsed ()) {
							if (!getAllBlocks () [refRow + 1, refColumn].GetComponent<Block> ().Ends [0]) {
								possibilities [2] = true;
							}
						} else if (getAllBlocks () [refRow, refColumn - 1].GetComponent<Block> ().getUsed ()) {
							if (!getAllBlocks () [refRow, refColumn - 1].GetComponent<Block> ().Ends [1]) {
								possibilities [2] = true;
							}
						}
					} else {
						possibilities [2] = true;
					}
				} else if (refRow == InstantiateBlocks.getRows () - 1 && refColumn != 0) {
					if (getAllBlocks () [refRow, refColumn - 1].GetComponent<Block> ().getUsed ()) {
						if (!getAllBlocks () [refRow, refColumn - 1].GetComponent<Block> ().Ends [1]) {
							possibilities [2] = true;
						}
					}
					else {
						possibilities [2] = true;
					}
				} else if (refRow != InstantiateBlocks.getRows () - 1 && refColumn == 0) {
					if (getAllBlocks () [refRow + 1, refColumn].GetComponent<Block> ().getUsed ()) {
						if (!getAllBlocks () [refRow + 1, refColumn].GetComponent<Block> ().Ends [0]) {
							possibilities [2] = true;
						}
					}
					else {
						possibilities [2] = true;
					}
				} else {
					possibilities [2] = true;
				}
			}
		}
		#endregion
		#region CornerRoom SouthEast
		if (refColumn != InstantiateBlocks.getColumns () -1 && refRow != InstantiateBlocks.getRows () -1) {
			if(getAllBlocks() [refRow + 1, refColumn].GetComponent<Block> ().Ends [0] &&getAllBlocks() [refRow, refColumn + 1].GetComponent<Block> ().Ends [3]) {
				if (refColumn != 0 && refRow != 0) {
					if (getAllBlocks () [refRow - 1, refColumn].GetComponent<Block> ().getUsed () || getAllBlocks () [refRow, refColumn - 1].GetComponent<Block> ().getUsed ()) {
						if (getAllBlocks () [refRow - 1, refColumn].GetComponent<Block> ().getUsed () && getAllBlocks () [refRow, refColumn - 1].GetComponent<Block> ().getUsed ()) {
							if (!getAllBlocks () [refRow - 1, refColumn].GetComponent<Block> ().Ends [2] && !getAllBlocks () [refRow, refColumn - 1].GetComponent<Block> ().Ends [1]) {
								possibilities [3] = true;
							}
						} else if (getAllBlocks () [refRow - 1, refColumn].GetComponent<Block> ().getUsed ()) {
							if (!getAllBlocks () [refRow - 1, refColumn].GetComponent<Block> ().Ends [2]) {
								possibilities [3] = true;
							}
						} else if (getAllBlocks () [refRow, refColumn - 1].GetComponent<Block> ().getUsed ()) {
							if (!getAllBlocks () [refRow, refColumn - 1].GetComponent<Block> ().Ends [1]) {
								possibilities [3] = true;
							}
						}
					} else {
						possibilities [3] = true;
					}
				} else if (refColumn == 0 && refRow != 0) {
					if (getAllBlocks () [refRow - 1, refColumn].GetComponent<Block> ().getUsed ()) {
						if (!getAllBlocks () [refRow - 1, refColumn].GetComponent<Block> ().Ends [2]) {
							possibilities [3] = true;
						}
					}
					else {
						possibilities [3] = true;
					}
				} else if (refRow == 0 && refColumn != 0) {
					if (getAllBlocks () [refRow, refColumn - 1].GetComponent<Block> ().getUsed ()) {
						if (!getAllBlocks () [refRow, refColumn - 1].GetComponent<Block> ().Ends [1]) {
							possibilities [3] = true;
						}
					}
					else {
						possibilities [3] = true;
					}
				} else {
					possibilities [3] = true;
				}
			}
		}
		#endregion
		#region CornerRoom SouthWest
		if (refRow != InstantiateBlocks.getRows () -1 && refColumn != 0) {
			if(getAllBlocks() [refRow + 1, refColumn].GetComponent<Block> ().Ends [0] &&getAllBlocks() [refRow, refColumn - 1].GetComponent<Block> ().Ends [1]) {
				if (refRow != 0 && refColumn != InstantiateBlocks.getColumns() - 1) {
					if(getAllBlocks() [refRow - 1, refColumn].GetComponent<Block> ().getUsed () ||getAllBlocks() [refRow, refColumn + 1].GetComponent<Block> ().getUsed ()) {
						if(getAllBlocks() [refRow - 1, refColumn].GetComponent<Block> ().getUsed () &&getAllBlocks() [refRow, refColumn + 1].GetComponent<Block> ().getUsed ()) {
							if (!getAllBlocks() [refRow - 1, refColumn].GetComponent<Block> ().Ends [2] && !getAllBlocks() [refRow, refColumn + 1].GetComponent<Block> ().Ends [3]) {
								possibilities [4] = true;
							}
						} else if(getAllBlocks() [refRow - 1, refColumn].GetComponent<Block> ().getUsed ()) {
							if (!getAllBlocks() [refRow - 1, refColumn].GetComponent<Block> ().Ends [2]) {
								possibilities [4] = true;
							}
						} else if(getAllBlocks() [refRow, refColumn + 1].GetComponent<Block> ().getUsed ()) {
							if (!getAllBlocks() [refRow, refColumn + 1].GetComponent<Block> ().Ends [3]) {
								possibilities [4] = true;
							}
						}
					} else {
						possibilities [4] = true;
					}
				} else if (refRow == 0 && refColumn != InstantiateBlocks.getColumns() - 1) {
					if(getAllBlocks() [refRow, refColumn + 1].GetComponent<Block> ().getUsed ()) {
						if (!getAllBlocks() [refRow, refColumn + 1].GetComponent<Block> ().Ends [3]) {
							possibilities [4] = true;
						}
					}
					else {
						possibilities [4] = true;
					}
				} else if (refRow != 0 && refColumn == InstantiateBlocks.getColumns() - 1) {
					if(getAllBlocks() [refRow - 1, refColumn].GetComponent<Block> ().getUsed ()) {
						if (!getAllBlocks() [refRow - 1, refColumn].GetComponent<Block> ().Ends [2]) {
							possibilities [4] = true;
						}
					}
					else {
						possibilities [4] = true;
					}
				} else {
					possibilities [4] = true;
				}
			}
		}
		#endregion
		#region CornerRoom NorthWest
		if (refColumn != 0 && refRow != 0) {
			if(getAllBlocks() [refRow - 1, refColumn].GetComponent<Block> ().Ends [2] &&getAllBlocks() [refRow, refColumn - 1].GetComponent<Block> ().Ends [1]) {
				if (refColumn != InstantiateBlocks.getColumns()-1 && refRow != InstantiateBlocks.getRows()-1) {
					if(getAllBlocks() [refRow + 1, refColumn].GetComponent<Block> ().getUsed () ||getAllBlocks() [refRow, refColumn + 1].GetComponent<Block> ().getUsed ()) {
						if(getAllBlocks() [refRow + 1, refColumn].GetComponent<Block> ().getUsed () &&getAllBlocks() [refRow, refColumn + 1].GetComponent<Block> ().getUsed ()) {
							if (!getAllBlocks() [refRow + 1, refColumn].GetComponent<Block> ().Ends [0] && !getAllBlocks() [refRow, refColumn + 1].GetComponent<Block> ().Ends [3]) {
								possibilities [5] = true;
							}
						} else if(getAllBlocks() [refRow + 1, refColumn].GetComponent<Block> ().getUsed ()) {
							if (!getAllBlocks() [refRow + 1, refColumn].GetComponent<Block> ().Ends [0]) {
								possibilities [5] = true;
							}
						} else if(getAllBlocks() [refRow, refColumn + 1].GetComponent<Block> ().getUsed ()) {
							if (!getAllBlocks() [refRow, refColumn + 1].GetComponent<Block> ().Ends [3]) {
								possibilities [5] = true;
							}
						}
					} else {
						possibilities [5] = true;
					}
				} else if (refColumn == InstantiateBlocks.getColumns()-1 && refRow != InstantiateBlocks.getRows()-1) {
					if(getAllBlocks() [refRow + 1, refColumn].GetComponent<Block> ().getUsed ()) {
						if (!getAllBlocks() [refRow + 1, refColumn].GetComponent<Block> ().Ends [0]) {
							possibilities [5] = true;
						}
					}
					else {
						possibilities [5] = true;
					}
				} else if (refColumn != InstantiateBlocks.getColumns()-1 && refRow == InstantiateBlocks.getRows()-1) {
					if(getAllBlocks() [refRow, refColumn + 1].GetComponent<Block> ().getUsed ()) {
						if (!getAllBlocks() [refRow, refColumn + 1].GetComponent<Block> ().Ends [3]) {
							possibilities [5] = true;
						}
					}
					else {
						possibilities [5] = true;
					}
				} else {
					possibilities [5] = true;
				}
			}
		}
		#endregion
		#region ThreeDoorHallway EastSouthWest
		if (refColumn != 0 && refColumn != InstantiateBlocks.getColumns () -1 && refRow != InstantiateBlocks.getRows () -1) {
			if(getAllBlocks() [refRow + 1, refColumn].GetComponent<Block> ().Ends [0] &&getAllBlocks() [refRow, refColumn - 1].GetComponent<Block> ().Ends [1] &&getAllBlocks() [refRow, refColumn + 1].GetComponent<Block> ().Ends [3]) {
				if (refRow != 0) {
					if(getAllBlocks() [refRow - 1, refColumn].GetComponent<Block> ().getUsed () && !getAllBlocks() [refRow - 1, refColumn].GetComponent<Block> ().Ends [2]) {
						possibilities [6] = true;
					} else if (!getAllBlocks() [refRow - 1, refColumn].GetComponent<Block> ().getUsed ()) {
						possibilities [6] = true;
					}
				} else {
					possibilities [6] = true;
				}
			}
		}
		#endregion
		#region ThreeDoorHallway NorthSouthWest
		if (refRow != 0 && refRow != InstantiateBlocks.getRows () -1 && refColumn != 0) {
			if(getAllBlocks() [refRow + 1, refColumn].GetComponent<Block> ().Ends [0] &&getAllBlocks() [refRow, refColumn - 1].GetComponent<Block> ().Ends [1] &&getAllBlocks() [refRow - 1, refColumn].GetComponent<Block> ().Ends [2]) {
				if (refColumn != InstantiateBlocks.getColumns () -1) {
					if(getAllBlocks() [refRow, refColumn + 1].GetComponent<Block> ().getUsed () && !getAllBlocks() [refRow, refColumn + 1].GetComponent<Block> ().Ends [3]) {
						possibilities [7] = true;
					} else if (!getAllBlocks() [refRow, refColumn + 1].GetComponent<Block> ().getUsed ()) {
						possibilities [7] = true;
					}
				} else {
					possibilities [7] = true;
				}
			}
		}
		#endregion
		#region ThreeDoorHallway NorthEastWest
		if (refRow != 0 && refColumn != 0 && refColumn != InstantiateBlocks.getColumns ()-1) {
			if(getAllBlocks() [refRow, refColumn + 1].GetComponent<Block> ().Ends [3] &&getAllBlocks() [refRow, refColumn - 1].GetComponent<Block> ().Ends [1] &&getAllBlocks() [refRow - 1, refColumn].GetComponent<Block> ().Ends [2]) {
				if (refRow != InstantiateBlocks.getRows () - 1) {
					if(getAllBlocks() [refRow + 1, refColumn].GetComponent<Block> ().getUsed () && !getAllBlocks() [refRow + 1, refColumn].GetComponent<Block> ().Ends [0]) {
						possibilities [8] = true;
					} else if (!getAllBlocks() [refRow + 1, refColumn].GetComponent<Block> ().getUsed ()) {
						possibilities [8] = true;
					}
				} else {
					possibilities [8] = true;
				}
			}
		}
		#endregion
		#region ThreeDoorHallway NorthEastSouth
		if (refRow != 0 && refRow != InstantiateBlocks.getRows ()-1 && refColumn != InstantiateBlocks.getColumns()-1) {
			if(getAllBlocks() [refRow, refColumn + 1].GetComponent<Block> ().Ends [3] &&getAllBlocks() [refRow + 1, refColumn].GetComponent<Block> ().Ends [0] &&getAllBlocks() [refRow - 1, refColumn].GetComponent<Block> ().Ends [2]) {
				if (refColumn != 0) {
					if(getAllBlocks() [refRow, refColumn - 1].GetComponent<Block> ().getUsed () && !getAllBlocks() [refRow, refColumn - 1].GetComponent<Block> ().Ends [1]) {
						possibilities [9] = true;
					} else if (!getAllBlocks() [refRow, refColumn - 1].GetComponent<Block> ().getUsed ()) {
						possibilities [9] = true;
					}
				} else {
					possibilities [9] = true;
				}
			}
		}
		#endregion
		#region FourDoorHallway
		if (refRow != 0 && refColumn != InstantiateBlocks.getColumns ()-1 && refRow != InstantiateBlocks.getRows ()-1 && refColumn != 0) {
			if(getAllBlocks() [refRow, refColumn + 1].GetComponent<Block> ().Ends [3] &&getAllBlocks() [refRow + 1, refColumn].GetComponent<Block> ().Ends [0] &&getAllBlocks() [refRow - 1, refColumn].GetComponent<Block> ().Ends [2] &&getAllBlocks() [refRow, refColumn - 1].GetComponent<Block> ().Ends [1]) {
				possibilities [10] = true;
			}
		}
		#endregion
		return possibilities;
	}
	#endregion
	void OnGUI() {
		//The OnGUI() function will handle the mini map of the Area and rotate based upon the players rotation and
		//move the map components based on the players position

		GUI.skin = mapSkin;

		//Based on player direction, set the rotation of the accordingly
		if (Quaternion.LookRotation (playerTransform.forward).eulerAngles.y >= 180) {
			deltaRotation = -(Quaternion.LookRotation (playerTransform.forward).eulerAngles.y - 360) - 90;
		} else {
			deltaRotation = -Quaternion.LookRotation (playerTransform.forward).eulerAngles.y - 90;
		}

		//Rotate the GUI with the rotation made around the center of the GUI map
		GUIUtility.RotateAroundPivot (deltaRotation, mapCenter);

		//Find the player and store it's position
		Vector3 temp;
		temp = GameObject.FindGameObjectWithTag ("Player").transform.position;
		//Scale the x/z components down to the GRID level
		temp.x = (temp.x + 25)/ 50;
		temp.z = (temp.z + 25) / 50;
		temp.x = 5 + temp.x * (500 / rows);	
		temp.z = 5 + temp.z * (500 / columns);	

		#region MiniMap Code
		//Creating the MiniMap
		for (int x = -1; x <= rows; x++) {
			for (int z = -1; z <= columns; z++) {
				//If x and z isn't on the ends, run the code (DONE TO AVOID NULL REFERENCE WITH THE BLOCKS MULTI-D ARRAY)
				if (x != -1 && x != rows && z != -1 && z != columns) {
					//If the block is used
					if (getAllBlocks () [x, z].GetComponent<Block> ().getUsed ()) {
						//Create the piece on the map given the piece type
						float xTemp, zTemp;
						zTemp = temp.z - (10 + MINIMAP_WIDTH/2);
						xTemp = temp.x - (10 + MINIMAP_LENGTH/2);
						if (10 + z * (500 / columns) - zTemp > 10 && 10 + z * (500 / columns) - zTemp < (10+MINIMAP_WIDTH) - (500 / columns)) {
							if (10 + x * (500 / rows) - xTemp > 10 && 10 + x * (500 / rows) - xTemp < (10+MINIMAP_LENGTH) - (500 / rows)) {
								GUI.Box (new Rect (10 + z * (500 / columns) - zTemp, 10 + x * (500 / rows) - xTemp, 500 / columns, 500 / rows), block2DRep [PlayerPrefs.GetInt (x.ToString () + z.ToString () + "pieceType") - 1]); 
							}
						}
					}
				} else {
					//If the row and column is not in the array, create the piece specified (as it is an outside peice)
					float xTemp, zTemp;
					zTemp = temp.z - (10+MINIMAP_WIDTH/2);
					xTemp = temp.x - (10+MINIMAP_LENGTH/2);
					if (10 + z * (500 / columns) - zTemp > 10 && 10 + z * (500 / columns) - zTemp < (10+MINIMAP_WIDTH) - (500 / columns)) {
						if (10 + x * (500 / rows) - xTemp > 10 && 10 + x * (500 / rows) - xTemp < (10+MINIMAP_LENGTH) - (500 / rows)) {
							GUI.Box (new Rect (10 + z * (500 / columns) - zTemp, 10 + x * (500 / rows) - xTemp, 500 / columns, 500 / rows), block2DRep [PlayerPrefs.GetInt (x.ToString () + z.ToString () + "pieceType") - 1]); 
						}
					}
				}
			}
		}
		#endregion

		//A box for all the cardinal directions
		GUI.Box (new Rect (160, 20, 30, 40), "N");
		GUI.Box (new Rect (270, 160, 30, 40), "E");
		GUI.Box (new Rect (160, 280, 30, 40), "S");
		GUI.Box (new Rect (20, 160, 30, 40), "W");

		//Reset the matrix (this finalizes the rotation)
		GUI.matrix = Matrix4x4.identity;

		//Carrot in the center of the map, symbolizing the player
		GUI.Box (new Rect (155, 155, 20, 20), "^");
	}
}
