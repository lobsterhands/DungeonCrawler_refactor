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
		gameMaze = generateMaze (world); // Create the maze; 0 = floor, 1 = wall;

		for (int x = 0; x < world.Width; x++) {
			for (int y = 0; y < world.Height; y++) {
				Tile tile_data = world.GetTileAt (x, y);
				GameObject tile_go = new GameObject ();
				tile_go.name = "Tile_" + x + "_" + y;
				tile_go.transform.position = new Vector3 (x, y, 0);
				SpriteRenderer tile_sr = tile_go.AddComponent<SpriteRenderer> ();

				if (gameMaze [x, y] == 0) {
					if (tile_data.Type != Tile.TileType.Exit_Door) {
						tile_data.Type = Tile.TileType.Floor;
						tile_sr.sprite = MazeSprites [0];
					} else {
						tile_go.AddComponent<BoxCollider2D>(); // Exit Door needs collider for trigger
						tile_sr.sprite = MazeSprites [16];
					}
				} else {
					int element = GetWallTile (world, x, y);
					tile_data.Type = (Tile.TileType)element; // set TileType using a (cast)enum_integer
					tile_go.AddComponent<BoxCollider2D>(); // Walls need to have a collider
					tile_sr.sprite = MazeSprites[element];
				}
			}
		}
	}

	// generateMaze() and recursion() were adapted from the following site: www.migapro.com/depth-first-search
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
	int GetWallTile(World world, int x, int y) {
		int max_X = world.Width-1;
		int max_Y = world.Height-1;

		// Check for Corners and Outer Walls first; These tiles will always have these coordinates
		if (x == 0 || y == 0 || x == max_X || y == max_Y) { // 
			if (x == 0 && y == 0) { // Place bottom left corner
				return GetTileElement (gameMaze [x, y + 1], gameMaze [x + 1, y], 0, 0);
			} else if (x == max_X && y == 0) { // Place bottom right corner
				return GetTileElement (gameMaze [x, y + 1], 0, 0, gameMaze [x - 1, y]);
			} else if (x == 0 && y == max_Y) { // Place top left corner
				return GetTileElement (0, gameMaze [x + 1, y], gameMaze [x, y - 1], 0);
			} else if (x == max_X && y == max_Y) { // Place top right corner
				return GetTileElement (0, 0, gameMaze [x, y - 1], gameMaze [x - 1, y]);
			} // Done placing corners
			else {
				// Check for Outer Walls; these wall tiles with always fall in these coordinates
				if (y == max_Y && x > 0 && x < max_X) { // It's a wall on the top row
					if (gameMaze [x + 1, y] == 0) { // If a top wall has floor to the right or left, the floor is an exit
						PlaceExitDoorAt (x + 1, y);
					} else if (gameMaze [x - 1, y] == 0) {
						PlaceExitDoorAt (x - 1, y);
					}
					return GetTileElement(0, gameMaze[x+1, y], gameMaze [x, y-1], gameMaze[x-1, y]);
				} else if (x + 1 > max_X && y > 0 && y < max_Y) { // On right column
					if (gameMaze [x, y + 1] == 0) {
						PlaceExitDoorAt (x, y + 1);
					} else if (gameMaze [x, y - 1] == 0) {
						PlaceExitDoorAt (x, y - 1);
					}
					return GetTileElement(gameMaze [x, y+1], 0, gameMaze [x, y-1], gameMaze[x-1, y]);
				} else if (y == 0 && x > 0 && x < max_X) { // On the bottom row
					if (gameMaze [x + 1, y] == 0) {
						PlaceExitDoorAt (x + 1, y);
					} else if (gameMaze [x - 1, y] == 0) {
						PlaceExitDoorAt (x - 1, y);
					}
					return GetTileElement(gameMaze [x, y+1], gameMaze [x+1, y], 0, gameMaze[x-1, y]);
				}  else if (x - 1 < 0 && y > 0 && y < max_Y) { // On the left column
					if (gameMaze [x, y + 1] == 0) {
						PlaceExitDoorAt (x, y + 1);
					} else if (gameMaze [x, y - 1] == 0) {
						PlaceExitDoorAt (x, y - 1);
					}
					return GetTileElement(gameMaze [x, y+1], gameMaze [x+1, y], gameMaze[x, y-1], 0);
				} // Done placing outer walls
			}
		}

		// Must be an inner wall piece; calculate which kind of wall by checking its URDL neighbors
		return GetTileElement (gameMaze[x, y + 1], gameMaze [x + 1, y], gameMaze [x, y - 1], gameMaze [x - 1, y]);
	} 

	// Convert the values of a wall tiles neighbors to a binary string and then an integer based on that string
	// Eg. Up = 1, Right = 0, Down = 0, Left = 0 ==> "1000"(binary) ==>  8(decimal)
	int GetTileElement(int Up, int Right, int Down, int Left) {
		return Convert.ToInt16 ((Up.ToString() + Right.ToString() + Down.ToString() + Left.ToString()), 2);
	}

	void PlaceExitDoorAt(int x, int y) {
		Tile tile_data = world.GetTileAt (x, y);
		tile_data.Type = Tile.TileType.Exit_Door;
	}
}
