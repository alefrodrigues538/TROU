using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadScene : MonoBehaviour {
	proceduralGrid pGrid;
	public Vector2 gridSize;
	public GameObject chunkObject;
	GameObject Chunks;
	void Awake () {
		pGrid = GameObject.FindGameObjectWithTag ("SceneManager").GetComponent<proceduralGrid> ();
		gridSize = new Vector2 (pGrid.gridSizeX/100,pGrid.gridSizeY/100);

		Chunks = new GameObject("Chunks");

		for (int x = 0; x < gridSize.x; x++) {
			for (int y = 0; y < gridSize.y; y++) {
				GameObject currentChunk = Instantiate (chunkObject,new Vector2(0,0),Quaternion.identity);
				currentChunk.GetComponent<chunkControl> ().chunkX = x * 100;
				currentChunk.GetComponent<chunkControl> ().chunkY = y * 100;
				currentChunk.name = "x:" + x*100 + ",y:" + y*100;

				currentChunk.transform.SetParent (Chunks.transform);
				pGrid.chunksObjects.Add (currentChunk);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
