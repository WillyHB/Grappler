using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ElRaccoone.Tweens;

public class Grapple : MonoBehaviour
{
    private ConnectionRope connectionRope;

    private GameObject player;

    public float GrappleExtensionSpeed = 2;

    public LayerMask GroundLayerMask;

    public GameObject TwoPointRope;
    private GameObject hook;
    private GameObject hookInstance;

    public bool IsGrappling => connectionRope.enabled;


    void Start()
    {
        hook = new GameObject();
        hook.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        player = GameObject.FindWithTag("Player");

        connectionRope = GetComponent<ConnectionRope>();
        connectionRope.enabled = false;

        GetComponentInParent<PlayerInput>().actions["Grapple"].started += MousePressed;

    }

    private void Update()
    {
        if (IsGrappling)
        {
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                FindObjectOfType<Grapple>().ReleaseGrapple();
            }

            float grappleVal = player.GetComponent<PlayerInput>().actions["GrappleLength"].ReadValue<float>();

            if (grappleVal != 0)
            {
                connectionRope.SetLength(connectionRope.GetLength() - grappleVal * GrappleExtensionSpeed * Time.deltaTime);
               // connectionRope.LineLength -= grappleVal * GrappleExtensionSpeed * Time.deltaTime;
            }
        }
    }

    private void MousePressed(InputAction.CallbackContext cc)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        RaycastHit2D hit = Physics2D.Linecast(transform.position, mousePos, GroundLayerMask);

        if (hit)
        {
            if (hookInstance != null) Destroy(hookInstance);

            if (hit.collider.GetComponent<Rigidbody2D>() != null)
            {

                connectionRope.connectedBody = hit.transform.gameObject.GetComponent<Rigidbody2D>();

                Vector3 offset = (Vector3)hit.point - hit.transform.position;
                connectionRope.endOffset = offset;
                connectionRope.SetLength((transform.position - (hit.transform.position + offset)).magnitude);
               // connectionRope.LineLength = (transform.position - (hit.transform.position + offset)).magnitude;
            }

            else
            {
                hookInstance = Instantiate(hook);
                hookInstance.name = "Hook";

                hookInstance.transform.position = hit.point;
                connectionRope.endOffset = Vector2.zero;
                connectionRope.connectedBody = hookInstance.GetComponent<Rigidbody2D>();
                connectionRope.SetLength((transform.position - hookInstance.transform.position).magnitude);
                //connectionRope.LineLength = (transform.position - hookInstance.transform.position).magnitude;
            }

            connectionRope.enabled = true;
        }

    }

    public void ReleaseGrapple()
    {
        connectionRope.enabled = false;
    }
}
