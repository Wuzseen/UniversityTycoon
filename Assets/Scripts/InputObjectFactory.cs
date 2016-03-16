using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputObjectFactory : MonoBehaviour {
	[SerializeField]
	private List<GameObject> defaultInputReceiverObjects;

	private List<IInputReceiver> InputReceivers { get; set; }

	private InputObject inputObject;

	public LayerMask groundMask;

	void Awake()
	{
		InputReceivers = new List<IInputReceiver>();
		foreach(GameObject go in defaultInputReceiverObjects)
		{
			IInputReceiver receiver = go.GetComponent<IInputReceiver>();
			if(receiver != null)
			{
				InputReceivers.Add(receiver);
			}
		}
		inputObject = new InputObject();
	}

	public void AddInputReceiver(IInputReceiver receiver)
	{
		InputReceivers.Add(receiver);
	}

	public void RemoveInputReceiver(IInputReceiver receiver)
	{
		InputReceivers.Remove(receiver);
	}
	
	// Update is called once per frame
	void Update () 
	{
		inputObject.MousePosition = Input.mousePosition;
		Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(r, out hit, 300000000f, groundMask))
		{
			GridSpot gs = GameGrid.WorldSpaceToGridSpace(hit.point);
			inputObject.NearestWorldGridLocation = gs;
		}

		foreach(IInputReceiver receiver in InputReceivers)
		{
			receiver.HandleInput(inputObject);
		}
	}
}
