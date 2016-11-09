using UnityEngine;
using System.Collections;

public class World {

	Tile[ , ] tiles;
	int width;
	int height;

	public World(int width, int height) {

		if (width == 0 || height == 0) {
			Debug.LogError ("No width or no height provided.");
		}

		this.width = width;
		this.height = height;

		tiles = new Tile[width, height];

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				tiles [x, y] = new Tile (this, x, y);
			}
		}

	}

	public Tile GetTileAt(int x, int y) {
		if (x < 0 || x > width || y < 0 || y > height) {
			Debug.Log ("Tile " + x + ", " + y + " is out of range.");
			return null;
		}

		return tiles [x, y];
	}

	public int Width {
		get {
			return width;
		}
	}

	public int Height {
		get {
			return height;
		}
	}
}