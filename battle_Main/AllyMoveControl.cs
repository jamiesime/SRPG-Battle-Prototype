using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyMoveControl : MonoBehaviour {

    //variables relating to turn flow
    public bool thisUnitActive;
    public Vector3 initialPosition;
    public GameObject unitActionMenu;
    public bool moveEnabled;
    public GameObject currentActor;
    public GameObject moveNode;
    public bool gridSet;
    
    //variables relating to currentActor
    public GameObject unitModel;
    public int maxMov;
    public int currentMov;
    public float rotSpeed1;
    
    public bool nextInput;
    
    //following block of variables searches immediate surroundings to base action on
    public float lengthToCheck;
    public RaycastHit inFront;
    public RaycastHit inBehind;
    public RaycastHit inRight;
    public RaycastHit inLeft;
    public GameObject objectNorth;
    public GameObject objectSouth;
    public GameObject objectEast;
    public GameObject objectWest;
    
    public List<GameObject> inRange;
    public List<GameObject> alliesInRange;
    
    
    //problem solving variables
    public Vector3 checkMoveComplete; //checks object did not move in the last frame before allowing new move input
    public bool notSubMenu; //to stop movement being enabled when cancelling out of a submenu
    
	// Use this for initialization
	void Start () {
        rotSpeed1 = 0.5f;
		initialPosition = currentActor.transform.position;
        moveEnabled = true;
        gridSet = false;
        unitActionMenu.SetActive(false);
        nextInput = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (thisUnitActive)
        {

            if (!gridSet)
            {
                MoveGridGeneration();
                CheckSurroundings();
            }
            PlayerTurn();
            checkMoveComplete = currentActor.transform.position;
            if (moveEnabled)
            {
            BattleManager.control.battleGuide.transform.position = currentActor.transform.position;
            }
        }
	}
    
    public void PlayerTurn()
    {
        if (currentActor.transform.position == checkMoveComplete && moveEnabled)
        {
            CheckSurroundings();
            UnitMoveInput();
            inRange.Clear();
        }
        
        if (Input.GetButtonUp("Cancel") && moveEnabled == true && currentActor.transform.position == checkMoveComplete)
        {
            currentActor.transform.position = initialPosition;
            currentActor.GetComponent<UnitInformation>().currentMov = currentActor.GetComponent<UnitInformation>().maxMov;
        }
        
        if (Input.GetButtonUp("Cancel") && BattleManager.control.notSubMenu && moveEnabled == false)
        {
            moveEnabled = true;
            UnitActionMenuFunction.control.mainMenu = false;
            unitActionMenu.SetActive(false);
            iTween.ScaleTo(BattleManager.control.battleGuide,new Vector3(0.1f,0.1f,0.1f), 1f);
        }
            
        if (Input.GetButtonUp("Submit") && moveEnabled == true)
        {
            //put in a coroutine with 0.1 delay to separate button inputs
            StartCoroutine(EnableMenu());
        }
    }
    
    IEnumerator EnableMenu()
    {
        yield return new WaitForSeconds(0.1f);
        UnitActionMenuFunction.control.mainMenu = true;
        moveEnabled = false;
        unitActionMenu.SetActive(true);
        CheckSurroundings();
        UnitActionMenuFunction.control.mainMenu = true;
        iTween.ScaleTo(BattleManager.control.battleGuide,new Vector3(1f,0.05f,1f), 1f);
        iTween.MoveBy(BattleManager.control.battleGuide,new Vector3(0f,-0.85f,0f), 1f);
    }
    
    public void MoveGridGeneration()
    {
        maxMov = currentActor.GetComponent<UnitInformation>().maxMov;
        GameObject[] moveNodes;
        moveNodes = GameObject.FindGameObjectsWithTag("MoveNode");
        foreach(GameObject moveNode in moveNodes)
        {
            if (moveNode.transform.position.x > (currentActor.transform.position.x + maxMov))
            {
                Destroy(moveNode);
            }
            else if (moveNode.transform.position.z > (currentActor.transform.position.z + maxMov))
            {
                Destroy(moveNode);
            }
            else if (moveNode.transform.position.x < (currentActor.transform.position.x - maxMov))
            {
                Destroy(moveNode);
            }
            else if (moveNode.transform.position.z < (currentActor.transform.position.z - maxMov))
            {
                Destroy(moveNode);
            }
            else
            {
                moveNode.GetComponent<Renderer>().enabled = true;
            }
            
            }

            gridSet = true;
        }
        
    
    public void UnitMoveInput()
    {
        
        
        if (Input.GetAxis("Vertical") != 0f || (Input.GetAxis("Horizontal") != 0f))
        {
            unitModel.GetComponent<Animator>().SetBool("Walk", true);
        }
        else
        {
            unitModel.GetComponent<Animator>().SetBool("Walk", false);
        }
        

            if (Input.GetAxis("Vertical") > 0.1f)
            {
                    switch (BattleCamControl.control.camPos)
                    {
                        case 1:
                        if (objectNorth != null)
                        {
                            if (objectNorth.tag == "MoveNode")
                            {
                            iTween.MoveBy(currentActor,iTween.Hash("amount",new Vector3(0f,0f,1f),"time",0.3f, "easeType", "easeOutQuad", "oncomplete", "ReduceCurrentMov", "oncompletetarget", this.gameObject));
                            iTween.RotateTo(unitModel,new Vector3(0f,0f,0f), rotSpeed1);
                            nextInput = false;
                            }
                        }
                        
                        break;
                        case 2:
                        if (objectEast != null)
                        {
                            if (objectEast.tag == "MoveNode")
                            {
                            iTween.MoveBy(currentActor,iTween.Hash("amount",new Vector3(1f,0f,0f),"time",0.3f, "easeType", "easeOutQuad", "oncomplete", "ReduceCurrentMov", "oncompletetarget", this.gameObject));
                            iTween.RotateTo(unitModel,new Vector3(0f,90f,0f), rotSpeed1);
                            nextInput = false;
                            }
                        }
                        break;
                        case 3:
                        if (objectSouth != null)
                        {
                            if (objectSouth.tag == "MoveNode")
                            {
                            iTween.MoveBy(currentActor,iTween.Hash("amount",new Vector3(0f,0f,-1f),"time",0.3f, "easeType", "easeOutQuad", "oncomplete", "ReduceCurrentMov", "oncompletetarget", this.gameObject));
                            iTween.RotateTo(unitModel,new Vector3(0f,180f,0f), rotSpeed1);
                            nextInput = false;
                            }
                        }
                        break;
                        case 4:
                        if (objectWest != null)
                        {
                            if (objectWest.tag == "MoveNode")
                            {
                            iTween.MoveBy(currentActor,iTween.Hash("amount",new Vector3(-1f,0f,0f),"time",0.3f, "easeType", "easeOutQuad", "oncomplete", "ReduceCurrentMov", "oncompletetarget", this.gameObject));
                            iTween.RotateTo(unitModel,new Vector3(0f,270f,0f), rotSpeed1);
                            nextInput = false;
                            }
                        }
                        break;
                    }
            }
        if (Input.GetAxis("Vertical") < -0.1f )
            {
                switch (BattleCamControl.control.camPos)
                    {
                        case 1:
                        if (objectSouth != null)
                        {
                            if (objectSouth.tag == "MoveNode")
                            {
                            iTween.MoveBy(currentActor,iTween.Hash("amount",new Vector3(0f,0f,-1f),"time",0.3f, "easeType", "easeOutQuad", "oncomplete", "ReduceCurrentMov", "oncompletetarget", this.gameObject));
                            iTween.RotateTo(unitModel,new Vector3(0f,180f,0f), rotSpeed1);
                            }
                        }
                        
                        break;
                        case 2:
                        if (objectWest != null)
                        {
                            if (objectWest.tag == "MoveNode")
                            {
                            iTween.MoveBy(currentActor,iTween.Hash("amount",new Vector3(-1f,0f,0f),"time",0.3f, "easeType", "easeOutQuad", "oncomplete", "ReduceCurrentMov", "oncompletetarget", this.gameObject));
                            iTween.RotateTo(unitModel,new Vector3(0f,270f,0f), rotSpeed1);
                            }
                        }
                        break;
                        case 3:
                        if (objectNorth != null)
                        {
                            if (objectNorth.tag == "MoveNode")
                            {
                            iTween.MoveBy(currentActor,iTween.Hash("amount",new Vector3(0f,0f,1f),"time",0.3f, "easeType", "easeOutQuad", "oncomplete", "ReduceCurrentMov", "oncompletetarget", this.gameObject));
                            iTween.RotateTo(unitModel,new Vector3(0f,0f,0f), rotSpeed1);
                            }
                        }
                        break;
                        case 4:
                        if (objectEast != null)
                        {
                            if (objectEast.tag == "MoveNode")
                            {
                            iTween.MoveBy(currentActor,iTween.Hash("amount",new Vector3(1f,0f,0f),"time",0.3f, "easeType", "easeOutQuad", "oncomplete", "ReduceCurrentMov", "oncompletetarget", this.gameObject));
                            iTween.RotateTo(unitModel,new Vector3(0f,90f,0f), rotSpeed1);
                            }
                        }
                        break;
                    }
            }
        if (Input.GetAxis("Horizontal") > 0.1f )
            {
              
                switch (BattleCamControl.control.camPos)
                    {
                        case 1:
                        if (objectEast != null)
                        {
                            if (objectEast.tag == "MoveNode")
                            {
                            iTween.MoveBy(currentActor,iTween.Hash("amount",new Vector3(1f,0f,0f),"time",0.3f, "easeType", "easeOutQuad", "oncomplete", "ReduceCurrentMov", "oncompletetarget", this.gameObject));
                            iTween.RotateTo(unitModel,new Vector3(0f,90f,0f), rotSpeed1);
                            }
                        }
                        
                        break;
                        case 2:
                        if (objectSouth != null)
                        {
                            if (objectSouth.tag == "MoveNode")
                            {
                            iTween.MoveBy(currentActor,iTween.Hash("amount",new Vector3(0f,0f,-1f),"time",0.3f, "easeType", "easeOutQuad", "oncomplete", "ReduceCurrentMov", "oncompletetarget", this.gameObject));
                            iTween.RotateTo(unitModel,new Vector3(0f,180f,0f), rotSpeed1);
                            }
                        }
                        break;
                        case 3:
                        if (objectWest != null)
                        {
                            if (objectWest.tag == "MoveNode")
                            {
                            iTween.MoveBy(currentActor,iTween.Hash("amount",new Vector3(-1f,0f,0f),"time",0.3f, "easeType", "easeOutQuad", "oncomplete", "ReduceCurrentMov", "oncompletetarget", this.gameObject));
                            iTween.RotateTo(unitModel,new Vector3(0f,270f,0f), rotSpeed1);
                            }
                        }
                        break;
                        case 4:
                        if (objectNorth != null)
                        {
                            if (objectNorth.tag == "MoveNode")
                            {
                            iTween.MoveBy(currentActor,iTween.Hash("amount",new Vector3(0f,0f,1f),"time",0.3f, "easeType", "easeOutQuad", "oncomplete", "ReduceCurrentMov", "oncompletetarget", this.gameObject));
                            iTween.RotateTo(unitModel,new Vector3(0f,0f,0f), rotSpeed1);
                            }
                        }
                        break;
                    }
            }
        if (Input.GetAxis("Horizontal") < -0.1f )
            {
               
                switch (BattleCamControl.control.camPos)
                    {
                        case 1:
                        if (objectWest != null)
                        {
                            if (objectWest.tag == "MoveNode")
                            {
                            iTween.MoveBy(currentActor,iTween.Hash("amount",new Vector3(-1f,0f,0f),"time",0.3f, "easeType", "easeOutQuad", "oncomplete", "ReduceCurrentMov", "oncompletetarget", this.gameObject));
                            iTween.RotateTo(unitModel,new Vector3(0f,270f,0f), rotSpeed1);
                            }
                        }
                        
                        break;
                        case 2:
                        if (objectNorth != null)
                        {
                            if (objectNorth.tag == "MoveNode")
                            {
                            iTween.MoveBy(currentActor,iTween.Hash("amount",new Vector3(0f,0f,1f),"time",0.3f, "easeType", "easeOutQuad", "oncomplete", "ReduceCurrentMov", "oncompletetarget", this.gameObject));
                            iTween.RotateTo(unitModel,new Vector3(0f,0f,0f), rotSpeed1);
                            }
                        }
                        break;
                        case 3:
                        if (objectEast != null)
                        {
                            if (objectEast.tag == "MoveNode")
                            {
                            iTween.MoveBy(currentActor,iTween.Hash("amount",new Vector3(1f,0f,0f),"time",0.3f, "easeType", "easeOutQuad", "oncomplete", "ReduceCurrentMov", "oncompletetarget", this.gameObject));
                            iTween.RotateTo(unitModel,new Vector3(0f,90f,0f), rotSpeed1);
                            }
                        }
                        break;
                        case 4:
                        if (objectSouth != null)
                        {
                            if (objectSouth.tag == "MoveNode")
                            {
                            iTween.MoveBy(currentActor,iTween.Hash("amount",new Vector3(0f,0f,-1f),"time",0.3f, "easeType", "easeOutQuad", "oncomplete", "ReduceCurrentMov", "oncompletetarget", this.gameObject));
                            iTween.RotateTo(unitModel,new Vector3(0f,180f,0f), rotSpeed1);
                            }
                        }
                        break;
                    }
            }
        
    }
    
    public void ReduceCurrentMov() //V. IMPORTANT - on the completion of a move nextInput is set to true, allowing the next move. otherwise, some drift occurs away from co-ords, meaning raycasts won't reach them and will mess up the movement
    {
        float roundX = Mathf.Round(currentActor.transform.position.x);
        float roundZ = Mathf.Round(currentActor.transform.position.z);
        currentActor.transform.position = new Vector3 (roundX, currentActor.transform.position.y, roundZ);
        CheckSurroundings();
        UnitActionMenuFunction.control.ActorSurroundings();
    }
    
   
    
    public void CheckSurroundings()
    {
       
        //the following sends out a raycast in four directions and returns the names of the objects found, if any (objectNorth, objectEast, etc). these names can then be used to identify the objects to interact with in the scene. 
        
		Vector3 forward = currentActor.transform.TransformDirection(Vector3.forward) * lengthToCheck;
        Vector3 backward = currentActor.transform.TransformDirection(-Vector3.forward) * lengthToCheck;
        Vector3 right = currentActor.transform.TransformDirection(Vector3.right) * lengthToCheck;
        Vector3 left = currentActor.transform.TransformDirection(Vector3.left) * lengthToCheck;
        //Debug Raycast in the scene view - so it's position can be seen
        Debug.DrawRay(currentActor.transform.position, forward, Color.green);
        Debug.DrawRay(currentActor.transform.position, backward, Color.green);
        Debug.DrawRay(currentActor.transform.position, right, Color.green);
        Debug.DrawRay(currentActor.transform.position, left, Color.green);
    
        if(Physics.Raycast(currentActor.transform.position,(forward), out inFront, 1f))
        {    
            
            objectNorth = inFront.collider.gameObject;
            inRange.Add(objectNorth);
            if (objectNorth.tag == "Ally")
            {
                alliesInRange.Add(objectNorth);
            }
        }
        else
        {
            objectNorth = null;
        }
        
        if(Physics.Raycast(currentActor.transform.position,(backward), out inBehind, 1f))
        {    
           
            objectSouth = inBehind.collider.gameObject;
            inRange.Add(objectSouth);
            if (objectSouth.tag == "Ally")
            {
                alliesInRange.Add(objectSouth);
            }
        }
        else
        {
            objectSouth = null;
        }
        
        if(Physics.Raycast(currentActor.transform.position,(right), out inRight, 1f))
        {    
            
            objectEast = inRight.collider.gameObject;
            inRange.Add(objectEast);
            if (objectEast.tag == "Ally")
            {
                alliesInRange.Add(objectEast);
            }
        }
        else
        {
            objectEast = null;
        }
        
        if(Physics.Raycast(currentActor.transform.position,(left), out inLeft, 1f))
        {    
            
            objectWest = inLeft.collider.gameObject;
            inRange.Add(objectWest);
            if (objectWest.tag == "Ally")
            {
                alliesInRange.Add(objectWest);
            }
        }
        else
        {
            objectWest = null;
        }
    }
}
