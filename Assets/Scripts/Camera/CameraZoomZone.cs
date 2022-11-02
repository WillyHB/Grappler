using UnityEngine;

public class CameraZoomZone : MonoBehaviour
{
    public float Zoom;
    public float Time;

    private float oldZoom;

    public CameraEventChannel CamEventChannel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            oldZoom = ResolutionManager.CameraZoom;

            CamEventChannel.PerformZoom(Zoom, Time);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CamEventChannel.PerformZoom(oldZoom, Time);
        }
    }
}
