using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    public Audio GrappleSound;
    public Audio GrappleRetractSound;

    public bool IsGrappling => ConnectionRope.enabled;

    private float timeGrappled;
    private float timeReleased;
    private bool canGrapple = true;

    private Vector2 grapplePointOffset;
    public float GrappleTime = 1;
    public float GrappleDelay = 0.5f;
    public float GrappleStopDistance = 1;

    private AudioMaster.PlayingClip? playingRetract = null;

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
            ConnectionRope.endOffset = Vector2.Lerp(ConnectionRope.connectedBody.position + (Vector2)GrapplePos.position, grapplePointOffset, (Time.time - timeGrappled) / GrappleTime);

            float grappleVal = InputProvider.GetState().GrappleLength;

            if (grappleVal == 0)
            {
                if (playingRetract.HasValue)
                AudioMaster.Instance.Stop(playingRetract.Value);
                playingRetract = null;
            }
            
            else 
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, ConnectionRope.GetCalculatedEndPoint() - (Vector2)transform.position, 1);
                bool hit = false;
    
                foreach (var h in hits)
                {
                    if (h.collider.CompareTag(GrappleBlockTag)) break;

                    if (h.collider.CompareTag(GrappleTag))
                    {
                        hit = true;
                        break;
                    }
                }

                if (hit) grappleVal = Mathf.Clamp(grappleVal, -1, 0);   
                else playingRetract ??= AudioMaster.Instance.Play(GrappleRetractSound, MixerGroup.Player);
            
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
        if (Time.time - timeReleased < GrappleDelay || !canGrapple || IsGrappling) return;

        RaycastHit2D[] hits = default;

        if (InputDeviceManager.CurrentDeviceType == InputDevices.MnK)
        {
            Vector2 mousePos = ResolutionManager.ScreenToWorld(Mouse.current.position.ReadValue());

            hits = Physics2D.LinecastAll(transform.position, mousePos);
        }

        else if (InputDeviceManager.CurrentDeviceType == InputDevices.Controller)
        {
            hits = Physics2D.RaycastAll(transform.position, Gamepad.current.rightStick.ReadValue(), 50);
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
            AudioMaster.Instance.Play(GrappleSound, MixerGroup.Player);
            timeGrappled = Time.time;

            if (hookInstance != null) Destroy(hookInstance);

            if (hit.collider.GetComponent<Rigidbody2D>() != null)
            {
                ConnectionRope.connectedBody = hit.transform.gameObject.GetComponent<Rigidbody2D>();

                Vector3 offset = (Vector3)hit.point - hit.transform.position;

                offset = new Vector3(
                    offset.x / ConnectionRope.connectedBody.transform.localScale.x,
                    offset.y / ConnectionRope.connectedBody.transform.localScale.y,
                    0);

                grapplePointOffset = offset;

                ConnectionRope.endOffset = (GrapplePos.position - transform.position) - ConnectionRope.connectedBody.transform.position;
                ConnectionRope.SetLength(((GrapplePos.position) - (hit.transform.position + offset)).magnitude);
            }

            ConnectionRope.enabled = true;
        }
    }

    public void ReleaseGrapple()
    {
        if (timeGrappled != Time.time && IsGrappling)
        {
            timeReleased = Time.time;
            Destroy(hookInstance);
            ConnectionRope.enabled = false;

            Hand.GetComponent<Hand>().followPosition = null;
            if (playingRetract.HasValue)
            AudioMaster.Instance.Stop(playingRetract.Value);
            playingRetract = null;
        }
    }
}
