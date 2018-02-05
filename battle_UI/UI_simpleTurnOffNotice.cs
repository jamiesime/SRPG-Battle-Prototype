using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_simpleTurnOffNotice : MonoBehaviour {

    public float displayTime;
    
    void Start()
    {
        StartCoroutine(RemoveNotice());
    }
    
    void Update()
    {
        if (gameObject.activeSelf)
        {
            StartCoroutine(RemoveNotice());
        }
    }

    
    IEnumerator RemoveNotice()
    {
        yield return new WaitForSeconds(displayTime);
        gameObject.SetActive(false);
    }
    
    
}
