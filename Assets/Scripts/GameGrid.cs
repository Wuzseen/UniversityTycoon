using UnityEngine;
using System.Collections;

public class GridSpot 
{
	public int X { get; set; }
	public int Y { get; set; }
	public int Z { get; set; }

	public Vector3 ToVector()
	{
		return new Vector3(X, Y, Z);
	}

	public Vector3 WorldSpace()
	{
		return GameGrid.GridSpaceToWorldSpace(this);
	}

	public GridSpot()
	{
		this.X = 0;
		this.Y = 0;
		this.Z = 0;
	}

	public GridSpot(int X, int Z)
	{
		this.X = X;
		this.Z = Z;
		this.Y = 0;
	}

	public GridSpot(int X, int Y, int Z)
	{
		this.X = X;
		this.Y = Y;
		this.Z = Z;
	}
}

public class GameGrid : MonoBehaviour 
{
	[SerializeField]
	private float gameScale = 1f; // 1 unit = 1 spot on grid

	private static GameGrid instance;

	void Awake()
	{
		if(instance != null)
		{
			Destroy(this.gameObject);
		}
		instance = this;
		DontDestroyOnLoad(this.gameObject);
	}

	public static Vector3 GridSpaceToWorldSpace(GridSpot gridSpace)
	{
		return GridSpaceToWorldSpace(gridSpace.ToVector());
	}

	public static Vector3 GridSpaceToWorldSpace(Vector3 gridSpace)
	{
		return instance.gameScale * gridSpace;
	}

	public static GridSpot WorldSpaceToGridSpace(Vector3 WorldCoordinate)
	{
		int x = RoundToMultipleOfGameScale(WorldCoordinate.x);
		int y = RoundToMultipleOfGameScale(WorldCoordinate.y);
		int z = RoundToMultipleOfGameScale(WorldCoordinate.z);

		return new GridSpot(x,y,z);
	}

	static int RoundToMultipleOfGameScale(float number)
	{
		int multiple = Mathf.RoundToInt(number/instance.gameScale);

		return multiple;
	}
}
