using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WorldController : MonoBehaviour {
	World world;
	int[,] gameMaze; // Used in generateMaze();
	public Sprite[] MazeSprites;

	GameObject tileHolder;

	int width = 7;
	int height = 7;

	// Use this for initialization
	void Start () {
		tileHolder = new GameObject ();
		tileHolder.name = "TileHolder";
		world = CreateWorld (width, height);
		gameMaze = CreateMaze (world);
	}

	World CreateWorld(int width, int height) {
		world = new World (width, height); // If blank, dimensions are 33x33 by default
		return world;
	}

	int[,] CreateMaze(World world) {
		int[,] maze = generateMaze (world); // Create the maze; 0 = floor, 1 = wall;

		for (int x = 0; x < world.Width; x++) {
			for (int y = 0; y < world.Height; y++) {
				Tile tile_data = world.GetTileAt (x, y);
				GameObject tile_go = new GameObject ();
				tile_go.transform.SetParent (tileHolder.transform); // Put tile under WorldController's hiearchy
				tile_go.name = "Tile_" + x + "_" + y;
				tile_go.transform.position = new Vector3 (x, y, 0);
				SpriteRenderer tile_sr = tile_go.AddComponent<SpriteRenderer> ();

				// the TileTypeChanged callback updates the sprite as a tile's type is changed
				tile_data.RegisterTileTypeChangedCallBack ( (tile) => {OnTileTypeChanged(tile, tile_go);} );

				if (maze [x, y] == 0) {
					// Since the maze has an exit, if a floor tile lies on the outer edge, it's the exit
					if (x == 0 || x == world.Width-1 || y == 0 || y == world.Height-1) {
						tile_data.Type = Tile.TileType.Exit_Door;
						BoxCollider2D bc = tile_go.AddComponent <BoxCollider2D> ();
						bc.isTrigger = true; // The exit door will be a trigger to build a new world (or go to level 2)
						ExitDoorController exit = tile_go.AddComponent<ExitDoorController>();
						exit.RegisterExitDoorReached ( () => {OnExitDoorReached();} );
						tile_go.tag = "ExitDoor";

					} else {
						Destroy ( tile_go.GetComponent<BoxCollider2D>() ); 
						tile_data.Type = Tile.TileType.Floor;
					}
				} else {
					tile_go.AddComponent<BoxCollider2D>(); // Walls need to have a collider
					int element = GetWallTile (world, maze, x, y);
					tile_data.Type = (Tile.TileType)element; // set TileType using a (cast)enum_integer
				}
			}
		}
		return maze;
	}

		
	// getWallTile() uses an x and y value to find a wall tile with those coordinates
	// That wall's neighboring tiles (Up Right Down Left) are checked for a 0 (floor) or a 1 (wall);
	/* W = the wall tile in question. It begins checking with the Up direction.
	 * 	  |U|          |0|
	 *  |L|W|R|   => |0|W|0|
	 *    |D|          |1|
	*/
	// Example: 0010 means a tile has a wall which shares a wall only in its Down direction.
	// 0010 (base 2) turns into 2 (base 10)
	int GetWallTile(World world, int[,] maze, int x, int y) {
		int max_X = world.Width-1;
		int max_Y = world.Height-1;

		// Check for Corners and Outer Walls first; These tiles will always have these coordinates; Only 2 neighbors to check
		if (x == 0 || y == 0 || x == max_X || y == max_Y) { // 
			if (x == 0 && y == 0) { // Place bottom left corner
				return GetTileElement (maze [x, y + 1], maze [x + 1, y], 0, 0);
			} else if (x == max_X && y == 0) { // Place bottom right corner
				return GetTileElement (maze [x, y + 1], 0, 0, maze [x - 1, y]);
			} else if (x == 0 && y == max_Y) { // Place top left corner
				return GetTileElement (0, maze [x + 1, y], maze [x, y - 1], 0);
			} else if (x == max_X && y == max_Y) { // Place top right corner
				return GetTileElement (0, 0, maze [x, y - 1], maze [x - 1, y]);
			} // Done placing corners
			else {
				// Check for Outer Walls; these wall tiles with always fall in these coordinates; Only 3 neighbors to check
				if (y == max_Y && x > 0 && x < max_X) { // It's a wall on the top row
					return GetTileElement(0, maze[x+1, y], maze [x, y-1], maze[x-1, y]);
				} else if (x + 1 > max_X && y > 0 && y < max_Y) { // On right column
					return GetTileElement(maze [x, y+1], 0, maze [x, y-1], maze[x-1, y]);
				} else if (y == 0 && x > 0 && x < max_X) { // On the bottom row
					return GetTileElement(maze [x, y+1], maze [x+1, y], 0, maze[x-1, y]);
				}  else if (x - 1 < 0 && y > 0 && y < max_Y) { // On the left column
					return GetTileElement(maze [x, y+1], maze [x+1, y], maze[x, y-1], 0);
				} // Done placing outer walls
			}
		}

		// Must be an inner wall piece; calculate which kind of wall by checking all 4 of its URDL neighbors
		return GetTileElement (maze[x, y + 1], maze [x + 1, y], maze [x, y - 1], maze [x - 1, y]);
	} 

	// Convert the values of a wall tiles neighbors to a binary string and then an integer based on that string
	// Eg. Up = 1, Right = 0, Down = 0, Left = 0 ==> "1000"(binary) ==>  8(decimal)
	int GetTileElement(int Up, int Right, int Down, int Left) {
		return Convert.ToInt16 ((Up.ToString() + Right.ToString() + Down.ToString() + Left.ToString()), 2);
	}

	void OnTileTypeChanged(Tile tile_data, GameObject tile_go) {
		tile_go.GetComponent<SpriteRenderer>().sprite = MazeSprites[(int)tile_data.Type];
	}

	void OnExitDoorReached() {
		Destroy (tileHolder); // Deletes all children as well (all the tile game objects)
		tileHolder = new GameObject ();
		tileHolder.name = "TileHolder";
		GameObject.FindGameObjectWithTag ("Player").transform.position = new Vector2 (1, 1);
		width += 4;
		height = width;
		world = CreateWorld (width, height);
		gameMaze = CreateMaze (world);

		//generate new monsters
		GenerateMonsters genMonsters;
		genMonsters.startGeneration();
	}

	public World getWorld {
		get {
			return world;
		}
	}

	/***** THE DEPTH-FIRST SEARCH MAZE GENERATOR STARTS BELOW *****/
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


}
