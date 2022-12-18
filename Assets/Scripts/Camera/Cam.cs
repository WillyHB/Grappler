using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public CameraEventChannel eventChannel;

    // Start is called before the first frame update
    void Awake()
    {
        eventChannel.ResetFollow += (f) =>
        {
            foreach (var vCam in Room.ActiveRoom.VirtualCameras)
            {
                vCam.Follow = f;
            }
        };
    }

    private void OnDisable()
    {
        eventChannel.ResetFollow -= (f) =>
        {
            foreach (var vCam in Room.ActiveRoom.VirtualCameras)
            {
                vCam.Follow = f;
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
