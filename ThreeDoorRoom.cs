using UnityEngine;
using System.Collections;

public class ThreeDoorRoom : Piece {

	// Use this for initialization
	void Start () {
		//ends = 3;
		//rotations = 3;
		pieceName = "Three Room Hallway";
		//Set endsUsed relative to what ends are being used
		//NOTE- Exact values are not being used because sometimes they do not register exact values (DONT KNOW WHY-TRIED TO FIX FOR HOURS)
		endsUsed = new bool[4];
		if (this.gameObject.transform.eulerAngles.y >= 355 || this.gameObject.transform.eulerAngles.y <= 5) {
			endsUsed [0] = false;
			endsUsed [1] = true;
			endsUsed [2] = true;
			endsUsed [3] = true;
		} else if (this.gameObject.transform.eulerAngles.y >= 85 && this.gameObject.transform.eulerAngles.y <= 95) {
			endsUsed [0] = true;
			endsUsed [1] = false;
			endsUsed [2] = true;
			endsUsed [3] = true;
		}
		else if (this.gameObject.transform.eulerAngles.y >= 175 && transform.eulerAngles.y <= 185) {
			endsUsed [0] = true;
			endsUsed [1] = true;
			endsUsed [2] = false;
			endsUsed [3] = true;
		}
		else {
			endsUsed [0] = true;
			endsUsed [1] = true;
			endsUsed [2] = true;
			endsUsed [3] = false;
		}
		int curRow, curColumn;
		curRow = InstantiateBlocks.getCurRow (this.gameObject.transform.position);
		curColumn = InstantiateBlocks.getCurColumn (this.gameObject.transform.position);
		InstantiateBlocks.getAllBlocks () [curRow, curColumn].GetComponent<Block> ().setEnds (endsUsed[0], endsUsed[1], endsUsed[2], endsUsed[3]);
		InstantiateBlocks.getAllBlocks () [curRow, curColumn].GetComponent<Block> ().setUsed (true);
	}
}
