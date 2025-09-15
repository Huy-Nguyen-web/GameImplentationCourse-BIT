using UnityEngine;

public class AliveTime : MonoBehaviour
{
    public float aliveTime;

    private void Update()
    {
        aliveTime -= 1f * Time.deltaTime;

        if(aliveTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
