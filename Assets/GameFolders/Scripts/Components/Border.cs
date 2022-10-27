using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    private EventData _eventData;

    private void Awake()
    {
        _eventData = Resources.Load("EventData") as EventData;
    }

    void Start()
    {
        _eventData.OnSetBorderPosition?.Invoke(transform.position);
    }
}
