using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCamControl : MonoBehaviour {

    public static BattleCamControl control;
    
    public GameObject Camera;
    
    public GameObject currentFocus;
    public GameObject battleGuide;
    
    public float xOffset;
    public float yOffset;
    public float zOffset;
    
    public bool camShift;
    public float camFOV;
    
    public float rotSpeed;
    
    public int camPos;
    
	// Use this for initialization
	void Start () {
        camFOV = Camera.GetComponent<Camera>().fieldOfView;
        control = this;
		currentFocus = battleGuide;
        camPos = 1;
        camShift = false;
	}
	
	// Update is called once per frame
	void Update () {
		

        /*Camera.transform.position = new Vector3((currentFocus.transform.position.x + xOffset), (currentFocus.transform.position.y + yOffset), (currentFocus.transform.position.z + zOffset));*/
        if (!camShift)
        {
        iTween.MoveUpdate(Camera,iTween.Hash("position",new Vector3((currentFocus.transform.position.x + xOffset), (currentFocus.transform.position.y + yOffset), (currentFocus.transform.position.z + zOffset)), "time",1f, "easeType", "easeInOutQuad"));
        }
        else
        {
        iTween.MoveTo(Camera,iTween.Hash("position",new Vector3((currentFocus.transform.position.x + xOffset), (currentFocus.transform.position.y + yOffset), (currentFocus.transform.position.z + zOffset)), "time",0.6f, "easeType", "easeInOutQuad", "onComplete", "CamShiftDone", "onCompleteTarget", this.gameObject));
        }
        
        switch (camPos)
        {
            case 1:
                xOffset = 0;
                yOffset = 3;
                zOffset = -5;
                iTween.RotateTo(Camera,iTween.Hash("rotation", new Vector3(25f,0f,0f),"time",rotSpeed,"easeType", "easeInOutQuad"));
            break;
            case 2:
                xOffset = -5;
                yOffset = 3;
                zOffset = 0;
                iTween.RotateTo(Camera,iTween.Hash("rotation", new Vector3(25f,90f,0f),"time",rotSpeed,"easeType", "easeInOutQuad"));
            break;
            case 3:
                xOffset = 0;
                yOffset = 3;
                zOffset = 5;
                iTween.RotateTo(Camera,iTween.Hash("rotation", new Vector3(25f,180f,0f),"time",rotSpeed,"easeType", "easeInOutQuad"));
            break;
            case 4:
                xOffset = 5;
                yOffset = 3;
                zOffset = 0;
                iTween.RotateTo(Camera,iTween.Hash("rotation", new Vector3(25f,270f,0f),"time",rotSpeed,"easeType", "easeInOutQuad"));
            break;
        }
            
        
        if (Input.GetAxis("Triggers") > -0.5f)
        {
            if (camFOV < 90f)
            {
                camFOV += 1f;
            }
            Camera.GetComponent<Camera>().fieldOfView = camFOV;
        }
        
        
        if (Input.GetAxis("Triggers") < 0.5f)
        {
            if (camFOV > 40f)
            {
                camFOV -= 1f;
            }
            Camera.GetComponent<Camera>().fieldOfView = camFOV;
        }
        
        
        if (Input.GetAxis("Triggers") > -0.5f && Input.GetAxis("Triggers") < 0.5f)
        {
            if (camFOV > 60f)
            {
                camFOV -= 1f;
            }
            else if (camFOV < 60f)
            {
                camFOV += 1f;
            }
            Camera.GetComponent<Camera>().fieldOfView = camFOV;
            
        }
        
        
        
             /*iTween.MoveTo(Camera,iTween.Hash("position",new Vector3((currentFocus.transform.position.x + xOffset), (currentFocus.transform.position.y + yOffset), (currentFocus.transform.position.z + zOffset)), "time",0.5f, "easeType", "easeInOutQuad"));*/
        
        if (Input.GetButtonUp("LButton"))
        {
            camShift = true;
            if (camPos < 4)
            {
                camPos += 1;
            }
            else
            {
                camPos = 1;
            }
        }
        
        if (Input.GetButtonUp("RButton"))
        {
            camShift = true;
            if (camPos > 1)
            {
                camPos -= 1;
            }
            else
            {
                camPos = 4;
            }
        }
            
	}
    
     public void CamShiftDone()
        {
            camShift = false;
        }
}
