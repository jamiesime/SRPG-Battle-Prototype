using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMoveNodeOnCollision : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        iTween.ScaleTo(gameObject,iTween.Hash("scale",new Vector3(0.01f,0.01f,0.01f),"time", 1f));
        gameObject.tag = "Occupied";
    }
    
    void OnTriggerExit(Collider other)
    {
        iTween.ScaleTo(gameObject,iTween.Hash("scale",new Vector3(1f,0.1f,1f),"time", 1f));
        gameObject.tag = "MoveNode";
    }
    
    public void GetRidOf()
    {
        /*
        iTween.ScaleTo(gameObject,iTween.Hash("scale",new Vector3(0.01f,0.01f,0.01f),"time", 0.1f, "oncomplete", "DestroyThis", "oncompletetarget", this.gameObject));
        */
        Destroy(gameObject);
    }
        
    void DestroyThis()
    {
        Destroy(gameObject);
    }
    
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
