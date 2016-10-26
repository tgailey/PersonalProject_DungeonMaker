using UnityEngine;
using System.Collections;

public class CornerRoom : Piece {

	void Start () {
		//ends = 2;
		//rotations = 4;
		pieceName = "Corner Hallway";
		//Set endsUsed relative to what ends are being used
		//NOTE- Exact values are not being used because sometimes they do not register exact values (DONT KNOW WHY-TRIED TO FIX FOR HOURS)
		endsUsed = new bool[4];
		if (this.gameObject.transform.eulerAngles.y >= 355 || this.gameObject.transform.eulerAngles.y <= 5) {
			endsUsed [0] = true;
			endsUsed [1] = true;
			endsUsed [2] = false;
			endsUsed [3] = false;
		} else if (this.gameObject.transform.eulerAngles.y >= 85 && this.gameObject.transform.eulerAngles.y <= 95) {
			endsUsed [0] = false;
			endsUsed [1] = true;
			endsUsed [2] = true;
			endsUsed [3] = false;
		} else if (transform.eulerAngles.y >= 175 && transform.eulerAngles.y <= 185) {
			endsUsed [0] = false;
			endsUsed [1] = false;
			endsUsed [2] = true;
			endsUsed [3] = true;
		} else if (this.gameObject.transform.eulerAngles.y >= 265 && this.gameObject.transform.eulerAngles.y <= 275) {
			endsUsed [0] = true;
			endsUsed [1] = false;
			endsUsed [2] = false;
			endsUsed [3] = true;
		}
		int curRow, curColumn;
		curRow = InstantiateBlocks.getCurRow (this.gameObject.transform.position);
		curColumn = InstantiateBlocks.getCurColumn (this.gameObject.transform.position);
		if (curRow != -1 && curRow != InstantiateBlocks.getRows() && curColumn != -1 && curColumn != InstantiateBlocks.getColumns()) {
			InstantiateBlocks.getAllBlocks () [curRow, curColumn].GetComponent<Block> ().setEnds (endsUsed [0], endsUsed [1], endsUsed [2], endsUsed [3]);
			InstantiateBlocks.getAllBlocks () [curRow, curColumn].GetComponent<Block> ().setUsed (true);
		}
	}
}
