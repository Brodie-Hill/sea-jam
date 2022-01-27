using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager singleton;

    [SerializeField] private bool printVars = false;

    // Duplicate values from shader
    Material oceanMaterial;
    private float phase, gravity, depth;
    private Vector2 dir1, dir2, dir3, dir4;
    private Vector4 amplitudes, timeScales;

    private void Awake()
    {
        // simple singleton pattern
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
        {
            Debug.Log("WaveManager singleton: destroying duplicate instance of WaveManager");
            Destroy(this);
        }


        SetVars();

    }

    private void SetVars()
    {
        oceanMaterial = GetComponent<Renderer>().sharedMaterial;
        phase = oceanMaterial.GetFloat("_Phase");
        gravity = oceanMaterial.GetFloat("_Gravity");
        depth = oceanMaterial.GetFloat("_Depth");
        dir1 = oceanMaterial.GetVector("_Direction1");
        dir2 = oceanMaterial.GetVector("_Direction2");
        dir3 = oceanMaterial.GetVector("_Direction3");
        dir4 = oceanMaterial.GetVector("_Direction4");
        amplitudes = oceanMaterial.GetVector("_Amplitudes");
        timeScales = oceanMaterial.GetVector("_Time_Scales");

        if (printVars)
        {
            Debug.Log("Phase:   " + phase);
            Debug.Log("Gravity: " + gravity);
            Debug.Log("Depth:   " + depth);
            Debug.Log("D1:     " + dir1);
            Debug.Log("D2:     " + dir2);
            Debug.Log("D3:     " + dir3);
            Debug.Log("D4:     " + dir4);
            Debug.Log("Amps:   " + amplitudes);
            Debug.Log("TSs:    " + timeScales);
        }
    }

    public float GetWaveHeightAtPosition(Vector3 position)
    {
        Vector3 sampleGridPoint = new Vector3(position.x, transform.position.y, position.z);
        Vector3 sampleDisp = Vector3.zero;

        for (int i = 0; i < 3; i++)
        {
            sampleDisp = GerstnerSum(sampleGridPoint);
            sampleGridPoint -= new Vector3(sampleDisp.x, 0, sampleDisp.z);
        }

        return sampleDisp.y+sampleGridPoint.y;
    }


    private Vector3 GerstnerSum(Vector3 pos)
    {
        return
              Gerstner(pos, dir1, timeScales.x, amplitudes.x)
            + Gerstner(pos, dir2, timeScales.y, amplitudes.y)
            + Gerstner(pos, dir3, timeScales.z, amplitudes.z)
            + Gerstner(pos, dir4, timeScales.w, amplitudes.w);
    }

    private Vector3 Gerstner(Vector3 pos, Vector2 dir, float timeScale,float amp)
    {
        float dirLen = dir.magnitude;
        float w = Mathf.Sqrt((gravity * dirLen) * (float)System.Math.Tanh(dirLen * depth));
        float theta = dir.x * pos.x + dir.y * pos.z - w * (Time.time * timeScale) - phase;

        float x = -((dir.x / dirLen) * (amp / (float)System.Math.Tanh(dirLen * depth)) * Mathf.Sin(theta));
        float z = -((dir.y / dirLen) * (amp / (float)System.Math.Tanh(dirLen * depth)) * Mathf.Sin(theta));
        float y = amp * Mathf.Cos(theta);

        return (new Vector3(x, y, z));

    }

    public Vector3 GetWaveNormalAtPosition(Vector3 position)
    {
        return Vector3.zero;
    }
}
