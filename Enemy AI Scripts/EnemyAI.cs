using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    public GameObject currentActor;
    public GameObject currentTarget;
    public GameObject enemyModel;
    public GameObject attackIcon;

    public Vector3 lastPosition;
    
    public float attackAnimLength;
    public bool moveFinished;
    public bool enemyIsAttacking;
    public bool hitOnce;
    public bool enemyHasAttacked;
    public bool gridSet;
    public int maxMov;
    
    public bool thisEnemyTurnNow;
    public string enemyType;
    public bool dead;
    
    //following block of variables searches immediate surroundings to base action on
    public RaycastHit inFront;
    public RaycastHit inBehind;
    public RaycastHit inRight;
    public RaycastHit inLeft;
    public GameObject objectUp;
    public GameObject objectDown;
    public GameObject objectRight;
    public GameObject objectLeft;
    public GameObject moveNode;
    public GameObject notMoveable;
    public float lengthToCheck;
    public GameObject targetNode;
    public bool nextMove;
    
    public List<GameObject> inRange;
    public List<GameObject> alliesInRange;
    
	// Use this for initialization
	void Start () {
        thisEnemyTurnNow = false;
        moveFinished = false;;
        hitOnce = true;
        enemyIsAttacking = false;
        FindClosestAlly();
        gridSet = false;
        nextMove = true;
        CheckSurroundings();
        lastPosition = enemyModel.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (thisEnemyTurnNow)
        {
                CheckIfWalking();
                if (!gridSet)
                {
                    CheckSurroundings();
                    MoveGridGeneration();
                }
            
                if (alliesInRange.Count == 0)
                {
                    FindClosestAlly();
                    lengthToCheck = maxMov;
                    if (!moveFinished)
                    {
                    CheckSurroundings();
                        if (alliesInRange.Count == 0)
                        {
                            EnemyMovement();
                        }
                    }
                    else
                    {
                        CheckSurroundings();
                        if (alliesInRange.Count > 0)
                        {
                            if (hitOnce)
                            {
                            moveFinished = true;
                            FindClosestAlly();
                            attackIcon.transform.position = currentActor.transform.position;
                            EnemyTargetSelect();
                            enemyIsAttacking = true;
                            attackIcon.SetActive(true);
                            AttackRotation();
                            iTween.MoveTo(attackIcon, currentTarget.transform.position, 0.5f);
                            }
                        }    
                        else
                        {
                            EndThisMove();
                            thisEnemyTurnNow = false;
                        }
                    }
                }
                else if (alliesInRange.Count > 0)
                {
                            if (hitOnce)
                            {
                            moveFinished = true;
                            FindClosestAlly();
                            attackIcon.transform.position = currentActor.transform.position;
                            EnemyTargetSelect();
                            enemyIsAttacking = true;
                            attackIcon.SetActive(true);
                            AttackRotation();
                            iTween.MoveTo(attackIcon, currentTarget.transform.position, 0.5f);
                            }
                }   

                
                        
        }
                    
    
        
        if (dead)
        {
            iTween.ScaleTo(currentActor, new Vector3(0.1f,0.1f,0.1f), 1f);
            iTween.MoveTo(currentActor, new Vector3(0f,10f,0f), 1f);
        }
		
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
            if (moveNode.transform.position.z > (currentActor.transform.position.z + maxMov))
            {
                Destroy(moveNode);
            }
            if (moveNode.transform.position.x < (currentActor.transform.position.x - maxMov))
            {
                Destroy(moveNode);
            }
            if (moveNode.transform.position.z < (currentActor.transform.position.z - maxMov))
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


    public void EnemyMovement()
    {
        if (objectUp.tag == "MoveNode" || objectDown.tag == "MoveNode" || objectRight.tag == "MoveNode" || objectLeft.tag == "MoveNode")
        {
            if (currentTarget.transform.position.x > currentActor.transform.position.x && objectRight.tag == "MoveNode")
            {
                iTween.MoveBy(currentActor,iTween.Hash("amount",new Vector3(1f,0f,0f),"time",0.3f, "easeType", "easeOutQuad"));
                iTween.RotateTo(enemyModel, new Vector3(0f,90f,0f), 1f);
                nextMove = true;
            }
            else if (currentTarget.transform.position.x < currentActor.transform.position.x && objectLeft.tag == "MoveNode")
            {
                iTween.MoveBy(currentActor,iTween.Hash("amount",new Vector3(-1f,0f,0f),"time",0.3f, "easeType", "easeOutQuad"));
                iTween.RotateTo(enemyModel, new Vector3(0f,270f,0f), 1f);
                nextMove = true;
            }
            else if (currentTarget.transform.position.z < currentActor.transform.position.z && objectDown.tag == "MoveNode")
                {
                iTween.MoveBy(currentActor,iTween.Hash("amount",new Vector3(0f,0f,-1f),"time",0.3f, "easeType", "easeOutQuad"));
                iTween.RotateTo(enemyModel, new Vector3(0f,180f,0f), 1f);
                nextMove = true;
                }
            else if (currentTarget.transform.position.z > currentActor.transform.position.z && objectUp.tag == "MoveNode")
                {
                iTween.MoveBy(currentActor,iTween.Hash("amount",new Vector3(0f,0f,1f),"time",0.3f, "easeType", "easeOutQuad"));
                iTween.RotateTo(enemyModel, new Vector3(0f,0f,0f), 1f);
                nextMove = true;
                }
            else
                {
                    moveFinished = true;
                }
            
            iTween.MoveTo(BattleManager.control.battleGuide,currentActor.transform.position, 0.3f);
        }
        else
        {
            moveFinished = true;
        }
    }
    
    public void AttackRotation()
    {
            if (currentTarget.transform.position.x > currentActor.transform.position.x)
            {
                iTween.RotateTo(enemyModel, new Vector3(0f,90f,0f), 1f);
            }
            else if (currentTarget.transform.position.x < currentActor.transform.position.x)
            {
                iTween.RotateTo(enemyModel, new Vector3(0f,270f,0f), 1f);
            }
            else if (currentTarget.transform.position.z < currentActor.transform.position.z)
                {
                iTween.RotateTo(enemyModel, new Vector3(0f,180f,0f), 1f);
                }
            else if (currentTarget.transform.position.z > currentActor.transform.position.z)
                {
                iTween.RotateTo(enemyModel, new Vector3(0f,0f,0f), 1f);
                }
    }
    
    public void CheckIfWalking()
    {
        if (lastPosition != enemyModel.transform.position)
        {
            enemyModel.GetComponent<Animator>().SetBool("Walk", true);
        }
        else
        {
            enemyModel.GetComponent<Animator>().SetBool("Walk", false);
        }
        lastPosition = enemyModel.transform.position;
        
    }
    
    public void FindClosestAlly()
        {
            GameObject[] gos;
            gos = GameObject.FindGameObjectsWithTag("Ally");
            GameObject closest = null;
            float distance = Mathf.Infinity;
            Vector3 position = transform.position;
            foreach (GameObject go in gos)
            {
                Vector3 diff = go.transform.position - position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    closest = go;
                    distance = curDistance;
                }
            }
            currentTarget = closest;
        }
    
    public void EnemyTargetSelect()
    {
        EnemyAttack();  
    }
    
    public void EnemyAttack()
    {
        if (hitOnce)
        {
            currentTarget.GetComponent<UnitInformation>().currentHP -= currentActor.GetComponent<UnitInformation>().physAtk;
            enemyModel.GetComponent<Animator>().SetTrigger("Attack");
            UI_impactDisplay.control.damageCounter.transform.position = currentTarget.transform.position;
            UI_impactDisplay.control.damageAmount = currentActor.GetComponent<UnitInformation>().physAtk;
            UI_impactDisplay.control.damageCall = true;
        }
        
        hitOnce = false;
        
        if (currentTarget.GetComponent<UnitInformation>().currentHP <= 0)
        {
        currentTarget.GetComponent<UnitInformation>().dead = true;
        }
        
        Invoke("EndThisMove", 1f);
    }
    
    public void CheckSurroundings()
    {
        
                objectDown = null;
                objectUp = null;
                objectLeft = null;
                objectRight = null;
        
        //the following sends out a raycast in four directions and returns the names of the objects found, if any (objectUp, objectRight, etc). these names can then be used to identify the objects to interact with in the scene. 
        
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
            
            objectUp = inFront.collider.gameObject;
            inRange.Add(objectUp);
            if (objectUp.tag == "Ally")
            {
                alliesInRange.Add(objectUp);
            }

        }
        else
        {
            if (objectUp == null)
            {
            objectUp = Instantiate(notMoveable, new Vector3(transform.position.x, transform.position.y,transform.position.z + 1), transform.rotation);
            }
        }
        
        if(Physics.Raycast(currentActor.transform.position,(backward), out inBehind, 1f))
        {    
           
            objectDown = inBehind.collider.gameObject;
            inRange.Add(objectDown);
            if (objectDown.tag == "Ally")
            {
                alliesInRange.Add(objectDown);
            }
        }
        else
        {
            if (objectDown == null)
            {
            objectDown = Instantiate(notMoveable, new Vector3(transform.position.x, transform.position.y,transform.position.z - 1), transform.rotation);
            }
        }
        
        if(Physics.Raycast(currentActor.transform.position,(right), out inRight, 1f))
        {    
            
            objectRight = inRight.collider.gameObject;
            inRange.Add(objectRight);
            if (objectRight.tag == "Ally")
            {
                alliesInRange.Add(objectRight);
            }
        }
        else
        {  
            if (objectRight == null)
            {
            objectRight = Instantiate(notMoveable, new Vector3(transform.position.x + 1, transform.position.y,transform.position.z), transform.rotation);
            }
        }
        
        if(Physics.Raycast(currentActor.transform.position,(left), out inLeft, 1f))
        {    
            
            objectLeft = inLeft.collider.gameObject;
            inRange.Add(objectLeft);
            if (objectLeft.tag == "Ally")
            {
                alliesInRange.Add(objectLeft);
            }

        }
        else
        {
            if (objectLeft == null)
            {
                objectLeft = Instantiate(notMoveable, new Vector3(transform.position.x - 1, transform.position.y,transform.position.z), transform.rotation);
                inRange.Add(objectUp);
            }
        }
        
    }
    

    
    public void EndThisMove()
    {
         attackIcon.SetActive(false);
         thisEnemyTurnNow = false;
         currentActor.GetComponent<UnitInformation>().currentMov = currentActor.GetComponent<UnitInformation>().maxMov;
         inRange.Clear();
         alliesInRange.Clear();
         objectUp = null;
         objectDown = null;
         objectLeft = null;
         objectRight = null;
         enemyIsAttacking = false;
         moveFinished = false;
         enemyModel.GetComponent<Animator>().SetBool("Walk", false);
         gameObject.GetComponent<EnemyAI>().enabled = false;
         TurnEnd();
    }
    
    public void TurnEnd()
    {
        BattleManager.control.EndUnitTurn();
    }

}
