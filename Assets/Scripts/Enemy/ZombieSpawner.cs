using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public float topBound;
    public float botBound;
    public float rightBound;
    public float leftBound;
    [SerializeField] GameObject[] zombiePrefabs;
    [SerializeField] LayerMask _groundLayer;
    float spawnCooldown = 1f;
    float spawnTimer = 0;
    public int maxAmountOfZombies = 140;
    public static List<GameObject> zombiesList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (Time.time >= spawnTimer && zombiesList.Count < maxAmountOfZombies)
        {
            spawnTimer = Time.time + spawnCooldown;
            RandomSpawn();
        }
    }

    void RandomSpawn()
    {
        Vector3 pos = new Vector3(Random.Range(leftBound, rightBound), 2, Random.Range(botBound, topBound));
        Ray ray = new Ray(pos, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 2, _groundLayer))
        {
            var spawnPos = new Vector3(hit.point.x, 2, hit.point.z);
            SpawnZombie(spawnPos);
        }
        else RandomSpawn();
    }

    void SpawnZombie(Vector3 spawnPos)
    {
        var randomNum = Random.Range(0, zombiePrefabs.Length);
        GameObject zombie = Instantiate(zombiePrefabs[randomNum], spawnPos, Quaternion.identity);
        zombiesList.Add(zombie);
    }
}
