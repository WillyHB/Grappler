using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ElRaccoone.Tweens;

public class Grapple : MonoBehaviour
{

    private LineRenderer lineRenderer;
    private DistanceJoint2D playerDistanceJoint;
    private GameObject player;

    public float GrappleExtensionSpeed = 2;

    public LayerMask GroundLayerMask;

    public GameObject TwoPointRope;
    public GameObject Hook;
    [HideInInspector]public GameObject grappleInstance;

    public bool IsGrappling => grappleInstance != null;


    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerDistanceJoint = player.GetComponent<DistanceJoint2D>();
        playerDistanceJoint.enabled = false;

        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.useWorldSpace = true;
        lineRenderer.SetPosition(1, new Vector3(0, 0, 0));

        GetComponentInParent<PlayerInput>().actions["Grapple"].started += MousePressed;

    }

    private void Update()
    {
        if (IsGrappling)
        {
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                FindObjectOfType<Grapple>().ReleaseGrapple();
                player.transform.TweenRotation(Vector3.zero, 0.1f);
            }

            TwoPointRope tpr = grappleInstance.GetComponent<TwoPointRope>();
            if (tpr.LineLength <= tpr.GetDistanceBetweenPoints())
            {
                Vector3 dir = player.transform.position - GameObject.Find("Hook").transform.position;

                Quaternion rot = Quaternion.LookRotation(Vector3.forward, -dir);

                player.transform.rotation = rot;
            }

            float grappleVal = player.GetComponent<PlayerInput>().actions["GrappleLength"].ReadValue<float>();

            if (grappleVal != 0)
            {
                if (tpr.LineLength - grappleVal * GrappleExtensionSpeed * Time.deltaTime > 0.25)
                {
                    playerDistanceJoint.distance -= grappleVal * GrappleExtensionSpeed * Time.deltaTime;
                    tpr.LineLength -= grappleVal * GrappleExtensionSpeed * Time.deltaTime;
                }
            }
        }
    }

    private void MousePressed(InputAction.CallbackContext cc)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        RaycastHit2D hit = Physics2D.Linecast(transform.position, mousePos, GroundLayerMask);

        if (hit)
        {
            if (grappleInstance != null)
            {
                Destroy(grappleInstance);
            }

            Vector2 point = hit.point;

            
            //var sm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateMachine>();
            //sm.Transition(sm.GrappleState);

            Hook.transform.position = point;

            grappleInstance = Instantiate(TwoPointRope, transform.parent.parent);

            grappleInstance.GetComponent<TwoPointRope>().LineLength = (transform.position - Hook.transform.position).magnitude;
            grappleInstance.GetComponent<TwoPointRope>().StartPoint = transform;
            grappleInstance.GetComponent<TwoPointRope>().EndPoint = Hook.transform;

            playerDistanceJoint.distance = (transform.position - Hook.transform.position).magnitude;
            playerDistanceJoint.enabled = true;
        }

    }

    public void ReleaseGrapple()
    {
        Destroy(grappleInstance);
        playerDistanceJoint.enabled = false;
    }
}
