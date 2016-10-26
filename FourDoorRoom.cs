using UnityEngine;
using System.Collections;

public class FourDoorRoom : Piece {

	// Use this for initialization
	void Start () {
		//ends = 4;
		//rotations = 0;
		pieceName = "Four Door Hallway";
		//Set endsUsed relative to what ends are being used
		//NOTE- Exact values are not being used because sometimes they do not register exact values (DONT KNOW WHY-TRIED TO FIX FOR HOURS)
		endsUsed = new bool[4];
		endsUsed [0] = true;
		endsUsed [1] = true;
		endsUsed [2] = true;
		endsUsed [3] = true;
		int curRow, curColumn;
		curRow = InstantiateBlocks.getCurRow (this.gameObject.transform.position);
		curColumn = InstantiateBlocks.getCurColumn (this.gameObject.transform.position);
		InstantiateBlocks.getAllBlocks () [curRow, curColumn].GetComponent<Block> ().setEnds (endsUsed[0], endsUsed[1], endsUsed[2], endsUsed[3]);
		InstantiateBlocks.getAllBlocks () [curRow, curColumn].GetComponent<Block> ().setUsed (true);
	}
}
