using UnityEngine;
using System.Collections;
using DG.Tweening;


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
    
    private Vector3 startMainRotateEuler;

    void Start()
    {
        startMainRotateEuler = MainCameraControl.rotation.eulerAngles;
        StartCoroutine(CameraController());

    }

    bool rotationResetting = false;
    IEnumerator ResetRotation()
    {
        rotationResetting = true;
        float resetTime = .5f;
        Tween orbitReset = RigRoot.DORotate(Vector3.zero, resetTime);
        Tween rotateReset = MainCameraControl.DORotate(startMainRotateEuler, resetTime);
        while(Input.GetButton("ResetRotation"))
        {
            if(orbitReset.IsComplete())
            {
                break;
            }
            yield return null;
        }
        orbitReset.Kill();
        rotateReset.Kill();
        rotationResetting = false;
    }

    private bool goingHome = false;
    IEnumerator CamHome()
    {
        goingHome = true;
        float resetTime = .5f;
        Tween homeReset = RigRoot.DOMove(Vector3.zero, resetTime);
        while(Input.GetButton("Home"))
        {
            if(homeReset.IsComplete())
            {
                break;
            }
            yield return null;
        }
        homeReset.Kill();
        goingHome = false;
    }

    IEnumerator CameraController()
    {
        while(true)
        {
            if(Input.GetButtonDown("ResetRotation"))
            {
                StartCoroutine(ResetRotation());
            }
            if(Input.GetButtonDown("Home"))
            {
                StartCoroutine(CamHome());
            }

            bool camModifier = Input.GetButton("CameraModifier");

            if(!rotationResetting)
            {
                float orbit = Input.GetAxis("Orbit");
                Transform orbitTarget = RigRoot;
                Space s = Space.Self;
                if (camModifier)
                {
                    orbit *= -1f;
                    orbitTarget = MainCameraControl;
                    s = Space.World;
                }
                float orbitValue = orbit * rotateSpeed * Time.deltaTime;
                orbitTarget.Rotate(Vector3.up * orbitValue, s);
            }

            if(goingHome == false)
            {
                float horizontal = Input.GetAxis("Horizontal");
                float forward = Input.GetAxis("Forward");
                Vector3 forwardDirection = MainCameraControl.TransformDirection(Vector3.forward);
                forwardDirection.y = 0f;
                Vector3 forwardTranslate = forwardDirection * forward * Time.deltaTime * panSpeed;

                Vector3 horizontalDirection = MainCameraControl.TransformDirection(Vector3.right);
                Vector3 horizontalTranslate = horizontalDirection * horizontal * Time.deltaTime * panSpeed;

                Vector3 translate = horizontalTranslate + forwardTranslate;

                //Vector3 translate = new Vector3(horizontal, 0f, forward) * Time.deltaTime * panSpeed;
                RigRoot.position = RigRoot.position + translate;
            }

            float zoom = Input.GetAxis("Mouse ScrollWheel");
            Vector3 dir = MainCameraControl.forward;
            Vector3 targetPosition = MainCameraControl.position + dir * zoom * zoomSpeed * Time.deltaTime;
            float camDist = Vector3.Distance(RigRoot.position, targetPosition);
            if (camDist < maxZoomDistance && camDist > minZoomDistance)
            {
                MainCameraControl.position = targetPosition;
            }
            yield return null;
        }
    }
}
