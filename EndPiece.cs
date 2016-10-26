using UnityEngine;
using System.Collections;

public class EndPiece : MonoBehaviour {
	private int rows,columns;
	private int curRow, curColumn;
	private GameObject[,] blocks;
	// Use this for initialization
	void Start () {
		blocks = InstantiateBlocks.getAllBlocks ();
		rows = InstantiateBlocks.getRows ();
		columns = InstantiateBlocks.getColumns ();
		curRow = InstantiateBlocks.getCurRow (GetComponentInParent<Block> ().gameObject.transform.position);
		curColumn = InstantiateBlocks.getCurColumn (GetComponentInParent<Block> ().gameObject.transform.position);
	}

	void Update () {
		//Set all end pieces on the edges facing the outside to false
		//If the corresponding pieces are in use (SO A NORTH PIECE AND SOUTH PIECE ARE BOTH USED) turn it off
		//Disable mesh if block isnt used
		if (curRow != -1 && curRow != rows && curColumn != -1 && curColumn != columns) {
			if (name.Equals ("NorthEnd")) {
				if (curRow == 0) {
					this.gameObject.SetActive (false);
				} else if ((blocks [curRow, curColumn].GetComponent<Block> ().ends [0] && blocks [curRow - 1, curColumn].GetComponent<Block> ().ends [2]) && blocks [curRow - 1, curColumn].GetComponent<Block> ().getUsed ()) {
					this.gameObject.SetActive (false);
				} else {
					this.gameObject.SetActive (true);
					if (blocks [curRow, curColumn].GetComponent<Block> ().isUsed && blocks [curRow, curColumn].GetComponent<Block> ().ends [0]) {
						this.gameObject.GetComponent<MeshRenderer> ().enabled = true;
					} else {
						this.gameObject.GetComponent<MeshRenderer> ().enabled = false;
					}
				}
			} else if (name.Equals ("EastEnd")) {
				if (curColumn == columns - 1) {
					this.gameObject.SetActive (false);
				} else if ((blocks [curRow, curColumn].GetComponent<Block> ().ends [1] && blocks [curRow, curColumn + 1].GetComponent<Block> ().ends [3]) && blocks [curRow, curColumn + 1].GetComponent<Block> ().getUsed ()) {
					this.gameObject.SetActive (false);
				} else {
					this.gameObject.SetActive (true);
					if (blocks [curRow, curColumn].GetComponent<Block> ().isUsed && blocks [curRow, curColumn].GetComponent<Block> ().ends [1]) {
						this.gameObject.GetComponent<MeshRenderer> ().enabled = true;
					} else {
						this.gameObject.GetComponent<MeshRenderer> ().enabled = false;
					}
				}
			} else if (name.Equals ("SouthEnd")) {
				if (curRow == rows - 1) {
					this.gameObject.SetActive (false);
				} else if ((blocks [curRow, curColumn].GetComponent<Block> ().ends [2] && blocks [curRow + 1, curColumn].GetComponent<Block> ().ends [0]) && blocks [curRow + 1, curColumn].GetComponent<Block> ().getUsed ()) {
					this.gameObject.SetActive (false);
				} else {
					this.gameObject.SetActive (true);
					if (blocks [curRow, curColumn].GetComponent<Block> ().isUsed && blocks [curRow, curColumn].GetComponent<Block> ().ends [2]) {
						this.gameObject.GetComponent<MeshRenderer> ().enabled = true;
					} else {
						this.gameObject.GetComponent<MeshRenderer> ().enabled = false;
					}
				}
			} else if (name.Equals ("WestEnd")) {
				if (curColumn == 0) {
					this.gameObject.SetActive (false);
				} else if ((blocks [curRow, curColumn].GetComponent<Block> ().ends [3] && blocks [curRow, curColumn - 1].GetComponent<Block> ().ends [1]) && blocks [curRow, curColumn - 1].GetComponent<Block> ().getUsed ()) {
					this.gameObject.SetActive (false);
				} else {
					this.gameObject.SetActive (true);
					if (blocks [curRow, curColumn].GetComponent<Block> ().isUsed && blocks [curRow, curColumn].GetComponent<Block> ().ends [3]) {
						this.gameObject.GetComponent<MeshRenderer> ().enabled = true;
					} else {
						this.gameObject.GetComponent<MeshRenderer> ().enabled = false;
					}
				}
			}
		} else {
			this.gameObject.SetActive (false);
		}
	}
}
