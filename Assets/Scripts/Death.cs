using UnityEngine;

public class Death : MonoBehaviour
{
    public PlayerEventChannel PlayerEventChannel;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerEventChannel.Kill();
        }
    }
}
