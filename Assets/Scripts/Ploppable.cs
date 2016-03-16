using UnityEngine;
using System.Collections;

public class Ploppable : MonoBehaviour, IInputReceiver
{
	[SerializeField]
	private Transform frontLeft;
	[SerializeField]
	private Transform frontRight;
	[SerializeField]
	private Transform backLeft;
	[SerializeField]
	private Transform backRight;

	public Transform FrontLeft {
		get {
			return frontLeft;
		}
		private set {
			frontLeft = value;
		}
	}

	public Transform FrontRight {
		get {
			return frontRight;
		}
		private set {
			frontRight = value;
		}
	}

	public Transform BackLeft {
		get {
			return backLeft;
		}
		private set {
			backLeft = value;
		}
	}

	public Transform BackRight {
		get {
			return backRight;
		}
		set {
			backRight = value;
		}
	}

	[SerializeField]
	private int width;
	[SerializeField]
	private int height;
	[SerializeField]
	private int depth;


	public int Width {
		get {
			return width;
		}
		private set {
			width = value;
		}
	}

	public int Height {
		get {
			return height;
		}
		private set {
			height = value;
		}
	}

	public int Depth {
		get {
			return depth;
		}
		private set {
			depth = value;
		}
	}

	public void Place(GridSpot coordinate)
	{
		this.transform.position = coordinate.WorldSpace();
	}

	private GridSpot position;

	public GridSpot Position
	{
		get
		{
			return position;
		}
		set
		{
			position = value;
			Place(position);
		}
	}

	public void HandleInput(InputObject obj)
	{
		Place(obj.NearestWorldGridLocation);
	}
}
