using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Car : MonoBehaviour
{
    [SerializeField] MeshRenderer[] MeshesToRandomise;
    [SerializeField] float MinMoveSpeed = 2f;
    [SerializeField] float MaxMoveSpeed = 5f;
    [SerializeField] Vector3 MoveDirection = new Vector3(-1f, 0f, 0f);

    public UnityEvent<GameObject> OnReachedExit = new UnityEvent<GameObject>();
    public float KillX = float.MinValue;

    float MoveSpeed;
    bool CanUpdate = true;

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (!CanUpdate)
            return;

        transform.Translate(MoveDirection * MoveSpeed * Time.deltaTime);

        if (transform.position.x < KillX)
        {
            OnReachedExit.Invoke(gameObject);
            CanUpdate = false;
        }
    }

    public void Reset()
    {
        // allow car to update
        CanUpdate = true;

        // randomise the speed
        MoveSpeed = Random.Range(MinMoveSpeed, MaxMoveSpeed);

        // randomise the mesh colours
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        foreach (var mesh in MeshesToRandomise)
        {
            propertyBlock.SetColor("_Color", Color.HSVToRGB(Random.Range(0f, 1f), 0.75f, 0.75f));
            mesh.SetPropertyBlock(propertyBlock);
        }
    }
}
