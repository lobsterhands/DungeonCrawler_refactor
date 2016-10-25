using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WorldController : MonoBehaviour {

	World world;
	// 2-dimensional arrays to hold maze information
	int[,] gameMaze; // Used in generateMaze(); @LYLE: Can these be condensed into 1 array?

	public Sprite[] MazeSprites;

	// Use this for initialization
	void Start () {
		world = new World ();
		gameMaze = generateMaze (world); // Create the maze; 0 = floor, 1 = wall; (pass world by reference)

		for (int x = 0; x < world.Width; x++) {
			for (int y = 0; y < world.Height; y++) {
				Tile tile_data = world.GetTileAt (x, y);
				GameObject tile_go = new GameObject ();
				tile_go.name = "Tile_" + x + "_" + y;
				tile_go.transform.position = new Vector3 (x, y, 0);
				SpriteRenderer tile_sr = tile_go.AddComponent<SpriteRenderer> ();

				if (gameMaze [x, y] == 0) {
					tile_data.Type = Tile.TileType.Floor;
					tile_sr.sprite = MazeSprites[0];
				} else {
					int element = getWallTile (world, x, y);
					tile_data.Type = (Tile.TileType)element; // set TileType using a (cast)enum_integer
					tile_go.AddComponent<BoxCollider2D>(); // Walls need to have a collider
					if (element != -1) {
						tile_sr.sprite = MazeSprites[element];
					}
				}
			}
		}
	}
		
	public int[,] generateMaze(World world) {
		int dimSquare = world.Width * world.Height;
		int[,] mazeArray = new int[dimSquare, dimSquare];

		for (int i = 0; i < dimSquare; i++) {
			for (int j = 0; j < dimSquare; j++) {
				mazeArray [i, j] = 1;
			}
		}

		// Choose maze starting row and column
		int r = UnityEngine.Random.Range (0, dimSquare);
		while (r % 2 == 0) {
			r = UnityEngine.Random.Range (0, dimSquare);
		}

		int c = UnityEngine.Random.Range (0, dimSquare);
		while (c % 2 == 0) {
			c = UnityEngine.Random.Range (0, dimSquare);
		}

		mazeArray [r, c] = 0; // Set starting maze position to 0 (floor tile)
		recursion (world, mazeArray, r, c);
		return mazeArray;
	}

	void recursion(World world, int[,] mazeArray, int r, int c) {
		// Get [1,2,3,4] in randomized order
		int[] randDirs = generateRandomDirections ();

		for (int i = 0; i < randDirs.Length; i++) {
			switch (randDirs [i]) {
			case 1: // Up
				//Whether 2 cells up is out or not
				if (r - 2 <= 0) {
					continue;
				}
				if (mazeArray [r - 2, c] != 0) {
					mazeArray [r - 2, c] = 0;
					mazeArray [r - 1, c] = 0;
					recursion (world, mazeArray, r - 2, c);
				}
				break;
			case 2: // Right
				// Whether 2 cells to the right is out or not
				if (c + 2 >= world.Width - 1) {
					continue;
				}
				if (mazeArray [r, c + 2] != 0) {
					mazeArray [r, c + 2] = 0;
					mazeArray [r, c + 1] = 0;
					recursion (world, mazeArray, r, c + 2);
				}
				break;
			case 3: // Down
				// Whether 2 cells down is out or not
				if (r + 2 >= world.Height - 1) {
					continue;
				}
				if (mazeArray [r + 2, c] != 0) {
					mazeArray [r + 2, c] = 0;
					mazeArray [r + 1, c] = 0;
					recursion (world, mazeArray, r + 2, c);
				}
				break;
			case 4: // Left
				// Whether 2 cells to the left is out or not
				if (c - 2 <= 0){
					continue;
				}
				if (mazeArray [r, c - 2] != 0) {
					mazeArray [r, c - 2] = 0;
					mazeArray [r, c - 1] = 0;
					recursion (world, mazeArray, r, c - 2);
				}
				break;
			}
		}
	}

	int[] generateRandomDirections() {
		List<int> randoms = new List<int> ();
		for (int i = 0; i < 4; i++) {
			randoms.Add (i + 1);
		}

		// Shuffle the randoms list
		for (int j = 0; j < 4; j++) {
			int temp = randoms [j];
			int randomIndex = UnityEngine.Random.Range (j, randoms.Count);
			randoms [j] = randoms [randomIndex];
			randoms [randomIndex] = temp;
		}

		return randoms.ToArray ();
	}

	// getWallTile() accepts an x and y values and returns
	// an integer based on the binary sequence of a wall with those coordinates Up Right Down Left neighbors;
	// 0 represents a floor; 1, a wall. Example: 0010 means a tile has a wall which connects to its Down direction.
	// 0010 (base 2) turns into 2 (base 10)
	int getWallTile(World world, int x, int y) {
		int max_X = world.Width-1;
		int max_Y = world.Height-1;

		string binary;
		// Check for Corners and Outer Walls first; These tiles will always have these coordinates
		if ((x == 0 || y == 0) || x == max_X || y == max_Y) { // 
			if (x == 0 && y == 0) { // Place bottom left corner
				binary = gameMaze [ x, y+1].ToString() + gameMaze [x+1, y].ToString() + "00";
				return Convert.ToInt16 (binary, 2); 
			} else if (x == max_X && y == 0) { // Place bottom right corner
				binary = gameMaze [ x, y+1].ToString() + "00" + gameMaze [x-1, y].ToString();
				return Convert.ToInt16 (binary, 2); 
			} else if (x == 0 && y == max_Y) { // Place top left corner
				binary = "0" + gameMaze [ x+1, y].ToString() + gameMaze [x, y-1].ToString() + 0;
				return Convert.ToInt16 (binary, 2); } 
			else if (x == max_X && y == max_Y) { // Place top right corner
				binary = "00" + gameMaze [ x, y-1].ToString() + gameMaze [x-1, y].ToString();
				return Convert.ToInt16 (binary, 2); 
			} // Done placing corners

			// Check for Outer Walls; these wall tiles with always fall in these coordinates
			if (y == max_Y && x > 0 && x < max_X) { // It's a wall on the top row
				binary = "0" + gameMaze [ x+1, y].ToString() + gameMaze [x, y-1].ToString() + gameMaze[x-1, y].ToString();
				return Convert.ToInt16 (binary, 2);
			} else if (x + 1 > max_X && y > 0 && y < max_Y) { // On right column
				binary =  gameMaze [ x, y+1].ToString() + "0" + gameMaze [x, y-1].ToString() + gameMaze[x-1, y].ToString();
				return Convert.ToInt16 (binary, 2); 
			} else if (y == 0 && x > 0 && x < max_X) { // On the bottom row
				binary =  gameMaze [ x, y+1].ToString() + gameMaze [x+1, y].ToString() + "0" + gameMaze[x-1, y].ToString();
				return Convert.ToInt16 (binary, 2); 
			}  else if (x - 1 < 0 && y > 0 && y < max_Y) { // On the left column
				binary =  gameMaze [ x, y+1].ToString() + gameMaze [x+1, y].ToString() + gameMaze[x, y-1].ToString() + "0";
				return Convert.ToInt16 (binary, 2); 			
			}
		} // Done placing outer walls
			
		// Must be an inner wall piece; calculate which kind of wall by checking its URDL neighbors
		binary = gameMaze [x, y + 1].ToString () + gameMaze [x + 1, y].ToString () +
			gameMaze [x, y - 1].ToString () + gameMaze [x - 1, y].ToString ();
		int elementNum = Convert.ToInt16 (binary, 2);
		return elementNum;
	}
}
