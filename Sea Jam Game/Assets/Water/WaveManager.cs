using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager singleton;
    [SerializeField] private float waterLevel = 0f;
    [SerializeField] private float amplitude = 1f;
    [SerializeField] private float waveLength = 3f;
    [SerializeField] private float speed = 1f;
    private float offset = 0f;

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
        {
            Debug.Log("WaveManager singleton: destroying duplicate instance of WaveManager");
            Destroy(this);
        }
    }

    private void Update()
    {
        offset = Time.time * speed;
    }

    // in future x and z will be used but atm only x is used
    public float GetWaveHeightAtPosition(Vector3 position)
    {
        return waterLevel + amplitude * Mathf.Sin(position.z / waveLength + offset);
    }
}
