using UnityEngine;
using System.Collections;

public class InputObject 
{
	public Vector2 MousePosition { get; set; }

	public GridSpot NearestWorldGridLocation { get; set; }

	public InputObject()
	{
		NearestWorldGridLocation = new GridSpot();
		MousePosition = Vector2.zero;
	}
}
