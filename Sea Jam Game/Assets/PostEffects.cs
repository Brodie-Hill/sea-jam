using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostEffects : MonoBehaviour
{
    [SerializeField] Volume underwaterVolume = null;
    [SerializeField] MeshRenderer waterEffect = null;
    private bool isUnderwater;

    private void Start()
    {
        isUnderwater = false;

        underwaterVolume.enabled = false;
        waterEffect.enabled = false;
    }
    private void Update()
    {
        if (!isUnderwater && WaveManager.singleton.GetWaveHeightAtPosition(transform.position) >= transform.position.y)
        {
            // is underwater
            isUnderwater = true;

            underwaterVolume.enabled = true;
            waterEffect.enabled = true;
        }
        else if (isUnderwater && WaveManager.singleton.GetWaveHeightAtPosition(transform.position) < transform.position.y)
        {
            isUnderwater = false;

            underwaterVolume.enabled = false;
            waterEffect.enabled = false;
        }
    }
}
