using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class PooledCarManager : MonoBehaviour
{
    [SerializeField] Transform EntryPortal;
    [SerializeField] float SpawnRadius = 5f;
    [SerializeField] Transform ExitPortal;

    [SerializeField] GameObject CarPrefab;
    [SerializeField] float MinSpawnInterval = 0.1f;
    [SerializeField] float MaxSpawnInterval = 0.2f;

    [SerializeField] int PoolDefaultSize = 50;
    [SerializeField] int MaxPoolSize = 100;

    [SerializeField] Text InactiveCount;
    [SerializeField] Text ActiveCount;

    ObjectPool<GameObject> CarPool;

    float NextSpawnTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        CarPool = new ObjectPool<GameObject>(CreateCar, RetrieveFromPool, ReturnToPool, DestroyPooledObject, true, PoolDefaultSize, MaxPoolSize);
    }

    GameObject CreateCar()
    {
        return Instantiate(CarPrefab, EntryPortal.position, Quaternion.identity);
    }

    void RetrieveFromPool(GameObject car)
    {
        // make the car active
        car.SetActive(true);

        // position the car
        car.transform.position = GetSpawnLocation();

        // reset the car script
        var carScript = car.GetComponent<Car>();

        carScript.Reset();
        carScript.OnReachedExit.AddListener(OnCarReachedExit);
        carScript.KillX = ExitPortal.transform.position.x;        
    }

    void ReturnToPool(GameObject car)
    {
        // turn off the car
        car.SetActive(false);

        // disconnect the listener
        car.GetComponent<Car>().OnReachedExit.RemoveAllListeners();
    }

    void DestroyPooledObject(GameObject car)
    {
        Destroy(car);
    }

    // Update is called once per frame
    void Update()
    {
        ActiveCount.text = "Active: " + CarPool.CountActive + " / " + CarPool.CountAll;
        InactiveCount.text = "Inactive: " + CarPool.CountInactive + " / " + CarPool.CountAll;

        // update spawn time
        if (NextSpawnTime > 0)
            NextSpawnTime -= Time.deltaTime;

        if (NextSpawnTime <= 0)
            SpawnCar();
    }

    void SpawnCar()
    {
        CarPool.Get();

        NextSpawnTime = Random.Range(MinSpawnInterval, MaxSpawnInterval);
    }

    Vector3 GetSpawnLocation()
    {
        return EntryPortal.transform.position + Random.Range(-SpawnRadius, SpawnRadius) * Vector3.up + Random.Range(-SpawnRadius, SpawnRadius) * Vector3.forward;
    }

    void OnCarReachedExit(GameObject car)
    {
        CarPool.Release(car);
    }
}
