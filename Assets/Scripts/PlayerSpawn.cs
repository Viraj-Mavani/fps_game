using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public GameObject player;
    public GameObject platform; 
    public Transform spawnPoint;

    void Start()
    {
        Instantiate(player, spawnPoint.position, spawnPoint.rotation);
        Destroy(platform, 3f);
    }
}
