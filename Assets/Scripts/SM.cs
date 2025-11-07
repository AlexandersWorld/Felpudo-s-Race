using System.Collections.Generic;
using UnityEngine;

public class SM : MonoBehaviour
{
    [Header("General Settings")]
    [Tooltip("Prefabs to spawn.")]
    public List<GameObject> prefabs = new List<GameObject>();

    [Tooltip("Time interval between spawns (seconds).")]
    public float spawnInterval = 4f;

    [Tooltip("Random horizontal and vertical offset range.")]
    public Vector2 randomOffset = new Vector2(1f, 0.2f);

    [Header("Height Offsets (adjust freely in the Inspector)")]
    [Tooltip("FlyingSlang spawn height range.")]
    public Vector2 flyingSlangHeight = new Vector2(1.5f, 2.5f);

    [Tooltip("Normal Slang spawn height range.")]
    public Vector2 slangHeight = new Vector2(-0.5f, 0.1f);

    [Tooltip("Rock spawn height range.")]
    public Vector2 rockHeight = new Vector2(0.8f, 1.4f);

    [Tooltip("Banana spawn height range.")]
    public Vector2 bananaHeight = new Vector2(0.7f, 1.2f);

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnPrefab();
            timer = 0f;
        }
    }

    void SpawnPrefab()
    {
        if (prefabs.Count == 0) return;

        GameObject prefab = prefabs[Random.Range(0, prefabs.Count)];
        string name = prefab.name.ToLower();

        // Base spawn position at the SpawnManager
        Vector3 spawnPos = transform.position;

        // Add small random horizontal offset
        spawnPos.x += Random.Range(-randomOffset.x, randomOffset.x);

        // Adjust vertical position depending on prefab name
        if (name.Contains("flyingslang"))
        {
            spawnPos.y += Random.Range(flyingSlangHeight.x, flyingSlangHeight.y);
        }
        else if (name.Contains("slang"))
        {
            spawnPos.y += Random.Range(slangHeight.x, slangHeight.y);
        }
        else if (name.Contains("rock"))
        {
            spawnPos.y += Random.Range(rockHeight.x, rockHeight.y);
        }
        else if (name.Contains("banana"))
        {
            spawnPos.y += Random.Range(bananaHeight.x, bananaHeight.y);
        }

        Instantiate(prefab, spawnPos, Quaternion.identity);
        Debug.Log($"Spawned {prefab.name} at {spawnPos}");
    }
}
