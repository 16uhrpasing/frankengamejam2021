using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private GameObject player;
    [SerializeField] private float windowX, windowY;
    private Vector3 spawnPos;

    private void Start()
    {
        cam = GetComponent<Camera>();
        spawnPos = transform.position;
    }

    public void Respawn()
    {
        transform.position = spawnPos;
    }

    private void Update()
    {
        if(player.transform.position.x > windowX + transform.position.x)
        {
            transform.position = new Vector3(player.transform.position.x - windowX, transform.position.y, transform.position.z);
        }else if(player.transform.position.x < transform.position.x - windowX)
        {
            transform.position = new Vector3(player.transform.position.x + windowX, transform.position.y, transform.position.z);
        }
        if(player.transform.position.y < transform.position.y - windowY)
        {
            transform.position = new Vector3(transform.position.x, player.transform.position.y + windowY, transform.position.z);
        }else if(player.transform.position.y > transform.position.y + windowY)
        {
            transform.position = new Vector3(transform.position.x, player.transform.position.y - windowY, transform.position.z);
        }
    }
}
