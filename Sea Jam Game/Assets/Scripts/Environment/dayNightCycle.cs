using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public static DayNightCycle singleton;

    [Tooltip("One second in real time equates to this many seconds in a game day")]
    [SerializeField] private float timeScale;
    [SerializeField] [TextArea] private string inspectorComment;
    [Tooltip("[in-game seconds]")]
    [SerializeField] private int timeOffset = 0;

    private int day, hour, minute, second;
    private Transform sun;

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



        sun = transform.GetChild(0); // only the dir light is child of this to it is at index 0
    }

    // Update is called once per frame
    void Update()
    {
        second = timeOffset + (int)Mathf.Floor(Time.time * timeScale);
        minute = second / 60;
        hour = minute / 60;
        day = hour / 24;

        // set sun rotation
        float hourAngle = (hour % 24) * 15; // 360/24
        float minuteAngle = (minute % 60) * 0.25f; // 360/24 /60
        float secondAngle = (second % 60) * 0.00416666f; // 360/24 /60 /60
        // enough precision already so dont need to consider seconds
        float angle = hourAngle + minuteAngle + secondAngle;

        sun.transform.localRotation = Quaternion.Euler(0, angle, 0);
        

    }


    public Vector3Int GetTime24()
    {
        return (new Vector3Int(hour % 24, minute % 60, second % 60));
    }
    public int GetDay()
    {
        return day;
    }
}
