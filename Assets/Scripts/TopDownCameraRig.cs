using UnityEngine;
using System.Collections;

public class TopDownCameraRig : MonoBehaviour
{
    [SerializeField]
    private Transform rigRoot;

    [SerializeField]
    private Transform mainCameraControl;

    [SerializeField]
    private float rotateSpeed = 20f;

    [SerializeField]
    private float panSpeed = 60f;

    [SerializeField]
    private float panAcceleration = 5f;

    [SerializeField]
    private float maxZoomDistance = 300f;

    [SerializeField]
    private float minZoomDistance = 0f;

    [SerializeField]
    private float zoomSpeed = 20f;


    public Transform RigRoot
    {
        get
        {
            return rigRoot;
        }
        set
        {
            rigRoot = value;
        }
    }

    public Transform MainCameraControl
    {
        get
        {
            return mainCameraControl;
        }

        set
        {
            mainCameraControl = value;
        }
    }

    // Use this for initialization
    void Start ()
    {

    }

    // Update is called once per frame
    void Update ()
    {
        float orbit = Input.GetAxis("Orbit");
        bool camModifier = Input.GetButton("CameraModifier");

        float orbitValue = orbit * rotateSpeed * Time.deltaTime;
        Transform orbitTarget = RigRoot;
        Space s = Space.Self;
        if(camModifier)
        {
            orbitTarget = MainCameraControl;
            s = Space.World;
        }
        orbitTarget.Rotate(Vector3.up * orbitValue, s);

        float horizontal = Input.GetAxis("Horizontal");
        float forward = Input.GetAxis("Forward");
        Vector3 translate = new Vector3(horizontal, 0f, forward) * Time.deltaTime * panSpeed;
        RigRoot.position = RigRoot.position + translate;

        float zoom = Input.GetAxis("Mouse ScrollWheel");
        Vector3 dir = MainCameraControl.forward;
        Vector3 targetPosition = MainCameraControl.position + dir * zoom * zoomSpeed * Time.deltaTime;
        float camDist = Vector3.Distance(RigRoot.position, targetPosition);
        if (camDist < maxZoomDistance && camDist > minZoomDistance)
        {
            MainCameraControl.position = targetPosition;
        }
    }
}
