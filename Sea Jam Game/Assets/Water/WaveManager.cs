using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager singleton;
    [SerializeField] private float waterLevel = 0f;
    [SerializeField] private float xAmplitude = 1f;
    [SerializeField] private float xWaveLength = 3f;
    [SerializeField] private float xSpeed = 1f;

    private float xOffset = 0f;

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
        xOffset = Time.time * xSpeed;
    }

    // in future x and z will be used but atm only x is used
    public float GetWaveHeightAtPosition(Vector3 position)
    {
        float xSine = xAmplitude * Mathf.Sin(position.x / xWaveLength + xOffset);
        return waterLevel + xSine;
    }
}
