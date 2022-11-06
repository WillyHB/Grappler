using UnityEngine;

public class CameraZoomZone : MonoBehaviour
{
    public float Zoom;

    private float oldZoom;

    public CameraEventChannel CamEventChannel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            oldZoom = ResolutionManager.CameraZoom;

            CamEventChannel.PerformZoom(Zoom);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CamEventChannel.PerformZoom(oldZoom);
        }
    }
}
