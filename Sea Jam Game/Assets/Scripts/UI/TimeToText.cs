using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeToText : MonoBehaviour
{
    [SerializeField] private bool showDay = false;
    [SerializeField] private bool showTime = true;
    [SerializeField] private bool showSeconds = false;
    [SerializeField] private bool use12hour = false;
    [SerializeField] private TMP_Text text = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 time = DayNightCycle.singleton.GetTime24();
        int day = DayNightCycle.singleton.GetDay();

        string secs = (showSeconds ? " : " + time.z.ToString() : "");
        string timeSt = time.x + ":" + time.y + secs;
        if (use12hour)
        {
            if (time.x > 12)
            {
                timeSt = (time.x - 12) + " : " + time.y + secs + " pm";
            }
            else
            {
                timeSt = time.x + " : " + time.y + secs + " am";
            }
        }

        text.text = showDay ? day.ToString() + (showTime ? " / " + timeSt : "") : (showTime ? timeSt : "");
    }
}
