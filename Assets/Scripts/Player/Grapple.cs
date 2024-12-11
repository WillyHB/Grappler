using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grapple : MonoBehaviour
{
    public ConnectionRope ConnectionRope { get; private set; }

    public float GrappleExtensionSpeed = 2;

    public string GrappleTag = "Ground";
    public string GrappleBlockTag = "BlockGrapple";

    private GameObject hook;
    private GameObject hookInstance;

    public InputProvider InputProvider;

    public Transform GrapplePos;

    public GameObject Hand;

    public bool IsGrappling => ConnectionRope.enabled;

    private bool grappledThisFrame;
    private bool canGrapple = true;

    void Start()
    {
        hook = new GameObject();
        hook.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        ConnectionRope = GetComponent<ConnectionRope>();
        ConnectionRope.enabled = false;

        InputProvider.GrappleCanceled += ReleaseGrapple;
        InputProvider.Grappled += OnGrapple;

    }

    private void OnDisable()
    {
        InputProvider.GrappleCanceled -= ReleaseGrapple;
        InputProvider.Grappled -= OnGrapple;
    }

    private void Update()
    {
        if (IsGrappling)
        {
            ConnectionRope.startOffset = GrapplePos.position - transform.position;
            grappledThisFrame = false;

            float grappleVal = InputProvider.GetState().GrappleLength;

            if (grappleVal != 0)
            {
                ConnectionRope.SetLength(ConnectionRope.GetLength() - grappleVal * GrappleExtensionSpeed * Time.deltaTime);
            }

            Hand.GetComponent<Hand>().followPosition = (Vector2)ConnectionRope.connectedBody.transform.position + ConnectionRope.endOffset;
        }

    }

    public void EnableGrapple()
    {
        canGrapple = true;
        Hand.SetActive(true);
    }

    public void DisableGrapple()
    {
        canGrapple = false;
        ReleaseGrapple();
        Hand.SetActive(false);
    }

    private void OnGrapple()
    {
        if (!canGrapple) return;

        if (IsGrappling) return;

        RaycastHit2D[] hits = default;

        if (InputDeviceManager.CurrentDeviceType == InputDevices.MnK)
        {
            Vector2 mousePos = ResolutionManager.ScreenToWorld(Mouse.current.position.ReadValue());

            hits = Physics2D.LinecastAll(transform.position, mousePos);
        }

        else if (InputDeviceManager.CurrentDeviceType == InputDevices.Controller)
        {
            hits = Physics2D.RaycastAll(transform.position, Gamepad.current.rightStick.ReadValue(), 1000);
        }

        bool grappleHit = false;
        RaycastHit2D hit = new();

        foreach (var h in hits)
        {
            if (h.collider.CompareTag(GrappleBlockTag)) break;

            if (h.collider.CompareTag(GrappleTag))
            {
                hit = h;
                grappleHit = true;
                break;
            }
        }

        if (grappleHit)
        {
            grappledThisFrame = true;

            if (hookInstance != null) Destroy(hookInstance);

            if (hit.collider.GetComponent<Rigidbody2D>() != null)
            {
                ConnectionRope.connectedBody = hit.transform.gameObject.GetComponent<Rigidbody2D>();

                Vector3 offset = (Vector3)hit.point - hit.transform.position;

                offset = new Vector3(
                    offset.x / ConnectionRope.connectedBody.transform.localScale.x,
                    offset.y / ConnectionRope.connectedBody.transform.localScale.y,
                    0);

                ConnectionRope.endOffset = offset;
                ConnectionRope.SetLength(((GrapplePos.position) - (hit.transform.position + offset)).magnitude);
            }
            /*
            else
            {
                hookInstance = Instantiate(hook);
                hookInstance.name = "Hook";

                hookInstance.transform.position = hit.point;
                ConnectionRope.endOffset = Vector2.zero;
                ConnectionRope.connectedBody = hookInstance.GetComponent<Rigidbody2D>();
                ConnectionRope.SetLength((transform.position - hookInstance.transform.position).magnitude);
            }
            */
            ConnectionRope.enabled = true;
        }
    }

    public void ReleaseGrapple()
    {
        if (!grappledThisFrame)
        {
            Destroy(hookInstance);
            ConnectionRope.enabled = false;

            Hand.GetComponent<Hand>().followPosition = null;
        }
    }
}
