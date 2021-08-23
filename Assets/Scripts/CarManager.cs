using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    [SerializeField] Transform EntryPortal;
    [SerializeField] float SpawnRadius = 5f;
    [SerializeField] Transform ExitPortal;

    [SerializeField] GameObject CarPrefab;
    [SerializeField] float MinSpawnInterval = 0.1f;
    [SerializeField] float MaxSpawnInterval = 0.2f;

    float NextSpawnTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // update spawn time
        if (NextSpawnTime > 0)
            NextSpawnTime -= Time.deltaTime;

        if (NextSpawnTime <= 0)
            SpawnCar();
    }

    void SpawnCar()
    {
        var newCar = Instantiate(CarPrefab, GetSpawnLocation(), Quaternion.identity);
        var carScript = newCar.GetComponent<Car>();

        carScript.OnReachedExit.AddListener(OnCarReachedExit);
        carScript.KillX = ExitPortal.transform.position.x;

        NextSpawnTime = Random.Range(MinSpawnInterval, MaxSpawnInterval);
    }

    Vector3 GetSpawnLocation()
    {
        return EntryPortal.transform.position + Random.Range(-SpawnRadius, SpawnRadius) * Vector3.up + Random.Range(-SpawnRadius, SpawnRadius) * Vector3.forward;
    }

    void OnCarReachedExit(GameObject car)
    {
        Destroy(car);
    }
}
