using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform cameraFollowPoint;
    [SerializeField] private float cameraLookDistance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float camY = Input.GetAxisRaw("Vertical") * cameraLookDistance;
        Vector3 camPos = new Vector3(0, camY, 0);

        cameraFollowPoint.localPosition = camPos;
    }

    
}
