using System;
using UnityEngine;

public class BoomManager : MonoBehaviour
{
    [SerializeField] float hideDelay = 5;

    private float showTime = 0;

    private void OnEnable()
    {
        showTime = hideDelay;
    }

    private void Update()
    {
        showTime -= Time.deltaTime;
        if (showTime <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    
}
