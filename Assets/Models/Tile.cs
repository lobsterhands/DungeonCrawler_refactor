using UnityEngine;
using System.Collections;
using System;

// This is the most atomic part of the game world
public class Tile {

	// Walls are ordered Up->Right->Down->Left; Wall_L means Up(0)->Right(0)->Down(0)->Left(1) => 0001 binary => 1 decimal
	public enum TileType { Floor = 0, Wall_L = 1, Wall_D = 2, Wall_DL = 3, Wall_R = 4, Wall_RL = 5, 
		Wall_RD = 6, Wall_RDL = 7, Wall_U = 8, Wall_UL = 9, Wall_UD = 10, 
		Wall_UDL = 11, Wall_UR = 12, Wall_URL = 13, Wall_URD = 14, Wall_URDL = 15, Exit_Door = 16 };
	TileType type = TileType.Floor; // Default to floor

	/* I want to use a callback that detects a change to a TileType.
	 *  On TileTypeChange, update WorldController's view to match the new TileType.
	 *  Exampe: Once I find the location for the exit door, I will do: tile_data.Type = Tile.TileType.Exit_Door;
	 *  Inside that setter, I want to update the sprite adder inside the WC view
	 * 
	 * 
	 * */
	Action<Tile> cbTileTypeChanged;

	StaticObject staticObject;

	World world; // Store reference to the world since Tiles exist in a world; allows tile to be "self-aware"
	int x;
	int y;

	// Constructor
	public Tile(World world, int x, int y) {
		this.world = world;
		this.x = x;
		this.y = y;
	}

	// Getters and Setters
	public int X {
		get {
			return x;
		}
	}

	public int Y {
		get {
			return y;
		}
	}

	public TileType Type {
		get {
			return type;
		}
		set {
			type = value;
			if (cbTileTypeChanged != null) {
				// do the callback thing
				cbTileTypeChanged (this); // pass this particular Tile to the callback
			} else {
				Debug.LogError ("cbTileTypeChanged is NULL");
			}
		}
	}

	public void RegisterTileTypeChangedCallBack(Action<Tile> callback) {
		cbTileTypeChanged += callback;
	}

	public void UnRegisterTileTypeChangedCallBack(Action<Tile> callback) {
		cbTileTypeChanged -= callback;
	}
}
