using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ElRaccoone.Tweens;

public class Grapple : MonoBehaviour
{
    private ConnectionRope connectionRope;

    public float GrappleExtensionSpeed = 2;

    public string GrappleTag = "Ground";

    public GameObject TwoPointRope;
    private GameObject hook;
    private GameObject hookInstance;

    public InputProvider InputProvider;

    public Transform GrapplePos;

    public GameObject Hand;

    public bool IsGrappling => connectionRope.enabled;

    private bool grappledThisFrame;

    void Start()
    {
        hook = new GameObject();
        hook.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        connectionRope = GetComponent<ConnectionRope>();
        connectionRope.enabled = false;

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
            connectionRope.startOffset = GrapplePos.position - transform.position;
            grappledThisFrame = false;

            float grappleVal = InputProvider.GetState().GrappleLength;

            if (grappleVal != 0)
            {
                connectionRope.SetLength(connectionRope.GetLength() - grappleVal * GrappleExtensionSpeed * Time.deltaTime);
            }

            Hand.GetComponent<Hand>().followPosition = (Vector2)connectionRope.connectedBody.transform.position + connectionRope.endOffset;
        }

    }

    private void OnGrapple()
    {
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
            if (h.collider.CompareTag(GrappleTag))
            {
                hit = h;
                grappleHit = true;
            }
        }

        if (grappleHit)
        {
            grappledThisFrame = true;

            if (hookInstance != null) Destroy(hookInstance);

            if (hit.collider.GetComponent<Rigidbody2D>() != null)
            {

                connectionRope.connectedBody = hit.transform.gameObject.GetComponent<Rigidbody2D>();

                Vector3 offset = (Vector3)hit.point - hit.transform.position;

                connectionRope.endOffset = offset;
                connectionRope.SetLength(((GrapplePos.position) - (hit.transform.position + offset)).magnitude);
            }

            else
            {
                hookInstance = Instantiate(hook);
                hookInstance.name = "Hook";

                hookInstance.transform.position = hit.point;
                connectionRope.endOffset = Vector2.zero;
                connectionRope.connectedBody = hookInstance.GetComponent<Rigidbody2D>();
                connectionRope.SetLength((transform.position - hookInstance.transform.position).magnitude);
            }

            connectionRope.enabled = true;
        }

    }

    public void ReleaseGrapple()
    {
        if (!grappledThisFrame)
        {
            Destroy(hookInstance);
            connectionRope.enabled = false;

            Hand.GetComponent<Hand>().followPosition = null;
        }
    }
}
