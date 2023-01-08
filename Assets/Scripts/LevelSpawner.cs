using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [Tooltip("GameObject for TopBorder")]
    [SerializeField]
    private GameObject TopBorder;
    [Tooltip("GameObject for BottomBorder")]
    [SerializeField]
    private GameObject BottomBorder;
    [Tooltip("GameObject for LeftBorder")]
    [SerializeField]
    private GameObject LeftBorder;
    [Tooltip("GameObject for RightBorder")]
    [SerializeField]
    private GameObject RightBorder;


    [Tooltip("Min duration between helium spawns (in fixed frames)")]
    [SerializeField]
    private int MinDurationBetweenHeliumSpawns = 150;
    [Tooltip("Max duration between helium spawns (in fixed frames)")]
    [SerializeField]
    private int MaxDurationBetweenHeliumSpawns = 250;

    private int HeliumSpawnCounter = 0;
    private int NextHeliumSpawn;


    [Tooltip("Min # of static platforms spawned")]
    [SerializeField]
    private int MinNumStaticPlatforms = 3;

    [Tooltip("Max # of static platforms spawned")]
    [SerializeField]
    private int MaxNumStaticPlatforms = 6;

    [Tooltip("Min length of static platforms")]
    [SerializeField]
    private float MinLengthStaticPlatforms = 1f;

    [Tooltip("Max length of static platforms")]
    [SerializeField]
    private float MaxLengthStaticPlatforms = 4f;

    [Tooltip("RampSpawner instance")]
    [SerializeField]
    private RampSpawner RampSpawner;

    [Tooltip("Canister Prefab")]
    [SerializeField]
    private GameObject CanisterPrefab;

    private float MinX, MinY, MaxX, MaxY;

    private Vector2 HeliumSize = new(0.5f, 0.8f);

    private List<HeliumCanister> HeliumCanisters = new List<HeliumCanister>();

    void Start()
    {
        MinX = LeftBorder.transform.position.x;
        MaxX = RightBorder.transform.position.x;
        MinY = TopBorder.transform.position.y;
        MaxY = BottomBorder.transform.position.y;

        NextHeliumSpawn = Random.Range(MinDurationBetweenHeliumSpawns, MaxDurationBetweenHeliumSpawns);
    }

    private void FixedUpdate()
    {
        if (GameStateManager.Instance.State != GameStateManager.GameState.Playing)
        {
            return;
        }

        HeliumSpawnCounter++;
        if (HeliumSpawnCounter >= NextHeliumSpawn)
        {
            Vector2 spawnPoint = new(Random.Range(MinX, MaxX), Random.Range(MinY, MaxY));
            HeliumCanister hc = TrySpawnHelium(spawnPoint);
            if (hc != null)
            {
                HeliumCanisters.Add(hc);
                HeliumSpawnCounter = 0;
                NextHeliumSpawn = Random.Range(MinDurationBetweenHeliumSpawns, MaxDurationBetweenHeliumSpawns);
            }
        }
    }

    public void Reset()
    {
        foreach(HeliumCanister hc in HeliumCanisters)
        {
            if (hc != null && hc.gameObject != null)
            {
                Destroy(hc.gameObject);
            }
        }
        HeliumCanisters.Clear();
    }

    private HeliumCanister TrySpawnHelium(Vector2 spawnPoint)
    {
        Vector2 cornerOne = spawnPoint - (HeliumSize / 2);
        Vector2 cornerTwo = spawnPoint + (HeliumSize / 2);
        // Early exit and don't spawn anything if canister would intersect with something
        Collider2D hit = Physics2D.OverlapArea(cornerOne, cornerTwo);
        if (hit != null)
        {
            return null;
        }
        return Instantiate(CanisterPrefab, spawnPoint, Quaternion.identity).GetComponent<HeliumCanister>();
    }

    // TODO: this doesn't seem to detect collisions with the other platforms being spawned, probably because
    // the Physics2D.Linecast used in TrySpawnRamp doesn't yet know about the ramps being spawned since a physics
    // update hasn't happened yet. Write a function to determine if to line segments would intersect and use that instead.
    //private void SpawnStaticPlatforms()
    //{
    //    int numPlatforms = Random.Range(MinNumStaticPlatforms, MaxNumStaticPlatforms + 1);
    //    StaticPlatforms = new Ramp[numPlatforms];
    //    int i = 0;
    //    while (i < numPlatforms)
    //    {
    //        print("Spawning ramp " + i);
    //        Vector2 startPoint = new(Random.Range(MinX, MaxX), Random.Range(MinY, MaxY));
    //        Vector2 directionPoint = new (Random.Range(MinX, MaxX), Random.Range(MinY, MaxY));
            
    //        Vector2 ray = (directionPoint - startPoint).normalized * Random.Range(MinLengthStaticPlatforms, MaxLengthStaticPlatforms);
    //        Vector2 endPoint = startPoint + ray;

    //        GameObject ramp = RampSpawner.TrySpawnRamp(
    //            startPoint,
    //            endPoint,
    //            Ramp.RampType.Static,
    //            LayerMask.GetMask("Harvester", "Platforms"));

    //        if (ramp != null)
    //        {
    //            StaticPlatforms[i] = ramp.GetComponent<Ramp>();
    //            i++;
    //        }
    //    }
    //}
}
