using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMP_Waveheight_Seeer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, WaveManager.singleton.GetWaveHeightAtPosition(transform.position), transform.position.z);
    }
}
