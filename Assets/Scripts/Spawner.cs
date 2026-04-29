using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject prefabToSpawn;     // Drag your prefab here in the Inspector
    public float spawnInterval = 2f;     // Time between spawns (seconds)
    public int maxSpawns = 20;           // Optional: limit total spawns

    [Header("Spawn Position")]
    public bool spawnAtCenter = true;    // If false, spawns will be random inside the cube
    public Vector3 spawnOffset = Vector3.zero;

    private int spawnCount = 0;
    private float nextSpawnTime = 0f;

    void Start()
    {
        // Optional: Spawn one immediately when the game starts
        // Spawn();
    }

    void Update()
    {
        if (prefabToSpawn == null) return;
        if (spawnCount >= maxSpawns) return;

        if (Time.time >= nextSpawnTime)
        {
            Spawn();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    public void Spawn()
{
    Vector3 spawnPosition;

    if (spawnAtCenter)
    {
        spawnPosition = transform.position + spawnOffset;
    }
    else
    {
        Bounds bounds = GetComponent<Renderer>().bounds;
        spawnPosition = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    // FIX 1: Pass position and rotation directly into Instantiate
    GameObject obj = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
    
    // FIX 2: Explicitly wake up the Rigidbody
    Rigidbody rb = obj.GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.WakeUp();
    }

    spawnCount++;
}
}
