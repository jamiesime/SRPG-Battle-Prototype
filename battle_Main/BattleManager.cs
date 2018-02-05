using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour {

    public static BattleManager control;
    
    //variables relating to turn flow
    public string turnPhase;
    public int maxTurns; //just set the fucking thing in the inspector
    public GameObject currentUnit;
    public bool newTurnSet;
    public Vector3 initialPosition;
    public GameObject unitActionMenu;
    public bool moveEnabled;
    public GameObject currentEnemy;
    public GameObject currentActorTarget;
    public int currentActorMov;
    public int highestAgility;
    public int turnCount;
    public int currentTurn;
    public GameObject currentActor;
    
    //all active units and enemies stored here, assigned in inspector depending on battle. used to determine turn order in WhoIsNext() by putting objects in array and comparing agility int variable then picking the highest remaining number.
    public GameObject[] participantsFull;
    public GameObject[] partcipantsWorking;
    public List<GameObject> participantList;
    public int numberOfParticipants;
    
    
    
    //variables relating to UI
    public GameObject battleGuide;
    public GameObject turnDisplay;
    public GameObject unitName;
    public GameObject unitHP;
    public GameObject unitMP;
    public GameObject targetInfo;
    public GameObject targetNameUI;
    public GameObject targetHP;
    public GameObject targetMP;
    public GameObject target;
    public string targetName;
    public int targetCurrentHP;
    public int targetMaxHP;
    public int targetCurrentMP;
    public int targetMaxMP;
    
    //problem solving variables
    public Vector3 checkMoveComplete; //checks object did not move in the last frame before allowing new move input
    public bool notSubMenu; //to stop movement being enabled when cancelling out of a submenu
    
    
	// Use this for initialization
	void Start () {
        
        control = this;

        turnPhase = "player";
        initialPosition = currentActor.transform.position;
        moveEnabled = true;
        notSubMenu = true;
        unitActionMenu.SetActive(false);
        turnDisplay.GetComponent<Text>().text = turnPhase;
        
        currentTurn = -1;
        //this will sort everything in the participantFull array into a list of gameobjects going from highest agility to lowest, allowing this list to be used for the turn order
        foreach (GameObject participant in participantsFull)
        {
            participantList.Add (participant);
        }
        if (participantList.Count > 0)
        {
            participantList.Sort(delegate(GameObject a, GameObject b) {return(b.GetComponent<UnitInformation>().agility).CompareTo(a.GetComponent<UnitInformation>().agility);});
        }
        
        MovementGrid.control.BuildMovementGrid();
        WhoIsNext();
	}
	
	// Update is called once per frame
	void Update () {
        
        if (currentActor != null)
        {
        
            
            if (currentActor.tag == "Ally")
            { 
                if (currentActor.GetComponent<AllyMoveControl>().moveEnabled)
                {
                    battleGuide.transform.position = currentActor.transform.position;
                }
                PlayerTurn();
            }
            else if (currentActor.tag == "Enemy")
            {
                battleGuide.transform.position = currentActor.transform.position;
                EnemyTurn();
            }   
        }
        
        
        if (currentActor != null)
        {
            
        unitName.GetComponent<Text>().text = currentActor.GetComponent<UnitInformation>().displayName;
        unitHP.GetComponent<Text>().text = " HP:" + currentActor.GetComponent<UnitInformation>().currentHP + " / " + currentActor.GetComponent<UnitInformation>().maxHP;
        unitMP.GetComponent<Text>().text = " MP:" + currentActor.GetComponent<UnitInformation>().currentMP + " / " + currentActor.GetComponent<UnitInformation>().maxMP;
            
        battleHUD();
            
        }
        
        
	}
    
    public void battleHUD()
    {
    if (currentActor != null)
    {
        if (currentActor.tag == "Ally")
        {
            if (GetComponent<UnitActionMenuFunction>().selectTarget && GetComponent<UnitActionMenuFunction>().currentTarget != null)
            {
                if (GetComponent<UnitActionMenuFunction>().currentTarget.tag == "Enemy")
                {
                
                targetNameUI.GetComponent<Text>().text = GetComponent<UnitActionMenuFunction>().currentTarget.GetComponent<UnitInformation>().displayName;
                    
                targetHP.GetComponent<Text>().text = "HP: " + GetComponent<UnitActionMenuFunction>().currentTarget.GetComponent<UnitInformation>().currentHP + " / " + GetComponent<UnitActionMenuFunction>().currentTarget.GetComponent<UnitInformation>().maxHP;
                    
                targetMP.GetComponent<Text>().text = "MP: " + GetComponent<UnitActionMenuFunction>().currentTarget.GetComponent<UnitInformation>().currentMP + " / " + GetComponent<UnitActionMenuFunction>().currentTarget.GetComponent<UnitInformation>().maxMP;
                    
                    
                targetInfo.GetComponent<CanvasGroup>().alpha = 1f;
                }
                if (GetComponent<UnitActionMenuFunction>().currentTarget.tag == "Ally")
                {
                
                targetNameUI.GetComponent<Text>().text = GetComponent<UnitActionMenuFunction>().currentTarget.GetComponent<UnitInformation>().displayName;
                    
                targetHP.GetComponent<Text>().text = "HP: " + GetComponent<UnitActionMenuFunction>().currentTarget.GetComponent<UnitInformation>().currentHP + " / " + GetComponent<UnitActionMenuFunction>().currentTarget.GetComponent<UnitInformation>().maxHP;
                    
                targetMP.GetComponent<Text>().text = "MP: " + GetComponent<UnitActionMenuFunction>().currentTarget.GetComponent<UnitInformation>().currentMP + " / " + GetComponent<UnitActionMenuFunction>().currentTarget.GetComponent<UnitInformation>().maxMP;
                    
                    
                targetInfo.GetComponent<CanvasGroup>().alpha = 1f;
                }
            }
            else
            {
                targetInfo.GetComponent<CanvasGroup>().alpha = 0f;
            }
        }
    
        
        if (currentActor.tag == "Enemy")
        {   
            if (currentActor.GetComponent<EnemyAI>().enemyIsAttacking)
            {
                target = currentActor.GetComponent<EnemyAI>().currentTarget;
                targetName = target.GetComponent<UnitInformation>().displayName;
                targetCurrentHP = target.GetComponent<UnitInformation>().currentHP;
                targetMaxHP = target.GetComponent<UnitInformation>().maxHP;
                targetCurrentMP = target.GetComponent<UnitInformation>().currentMP;
                targetMaxMP = target.GetComponent<UnitInformation>().maxMP;
                
                targetNameUI.GetComponent<Text>().text = targetName;
                targetHP.GetComponent<Text>().text = "HP: " + targetCurrentHP + " / " + targetMaxHP;
                targetMP.GetComponent<Text>().text = "MP: " + targetCurrentMP + " / " + targetMaxMP;
                    
                    
                targetInfo.GetComponent<CanvasGroup>().alpha = 1f;
            }
            else
            {
                targetInfo.GetComponent<CanvasGroup>().alpha = 0f;
            }
        }
        
    }
        
}
    
    public void PlayerTurn()
    {
        currentActor.GetComponent<AllyMoveControl>().enabled = true;
    }
    
    public void EnemyTurn()
    {
        currentActor.GetComponent<EnemyAI>().enabled = true;
    }
    
    public void EndUnitTurn()
    {
        
     if (currentActor != null)
     {
        MovementGrid.control.ClearMovementGrid();
        DestroyNotMoveable();
        
        iTween.ScaleTo(BattleManager.control.battleGuide,new Vector3(0.1f,0.1f,0.1f), 1f);
        
        if (currentActor.tag == "Ally")
        {
            currentActor.GetComponent<UnitInformation>().currentMov = currentActor.GetComponent<UnitInformation>().maxMov;
            currentActor.GetComponent<AllyMoveControl>().thisUnitActive = false;
            currentActor.GetComponent<AllyMoveControl>().moveEnabled = true;
            currentActor.GetComponent<AllyMoveControl>().gridSet = false;
            currentActor.GetComponent<AllyMoveControl>().enabled = false;
            
        }
        else if (currentActor.tag == "Enemy")
        {
            currentActor.GetComponent<EnemyAI>().enabled = false;
            currentActor.GetComponent<EnemyAI>().thisEnemyTurnNow = false;
            currentActor.GetComponent<EnemyAI>().gridSet = false;
            currentActor.GetComponent<EnemyAI>().enemyHasAttacked = false;
            currentActor.GetComponent<UnitInformation>().currentMov = currentActor.GetComponent<UnitInformation>().maxMov;
        }
        
        Invoke("WhoIsNext", 0.3f);
        Debug.Log("end unit turned has occurred");
     }
    }
    
    public void SkipUnitTurn()
    {
        
     if (currentActor != null)
     {
        
        if (currentActor.tag == "Ally")
        {
            currentActor.GetComponent<UnitInformation>().currentMov = currentActor.GetComponent<UnitInformation>().maxMov;
            currentActor.GetComponent<AllyMoveControl>().thisUnitActive = false;
            currentActor.GetComponent<AllyMoveControl>().moveEnabled = true;
            currentActor.GetComponent<AllyMoveControl>().gridSet = false;
            currentActor.GetComponent<AllyMoveControl>().enabled = false;
            currentActor = null;
            
        }
        else if (currentActor.tag == "Enemy")
        {
            currentActor.GetComponent<EnemyAI>().enabled = false;
            currentActor.GetComponent<EnemyAI>().thisEnemyTurnNow = false;
            currentActor.GetComponent<EnemyAI>().gridSet = false;
            currentActor.GetComponent<EnemyAI>().hitOnce = true;
            currentActor.GetComponent<UnitInformation>().currentMov = currentActor.GetComponent<UnitInformation>().maxMov;
            currentActor = null;
        }
        
        WhoIsNextNoWait();
         
     }
    }
    
    public void WhoIsNext()
    {
            //currentTurn = participantList.IndexOf(participantsFull[currentTurn]);
            Debug.Log("current turn = " + currentTurn);
            if (!newTurnSet)
            {
                if (currentTurn == maxTurns)
                {
                    currentTurn = 0;
                }
                else
                {
                currentTurn += 1;
                }
                currentActor = participantList[currentTurn];
                newTurnSet = true;
            }
        
            if (newTurnSet)
            {
                if (currentActor.GetComponent<UnitInformation>().dead)
                {
                    SkipUnitTurn();
                }
                else
                {
                StartCoroutine(FollowActiveUnit());
                }
            }
    }
    
    public void WhoIsNextNoWait()
    {
            //currentTurn = participantList.IndexOf(participantsFull[currentTurn]);
            Debug.Log("current turn = " + currentTurn);
            
            if (currentTurn == maxTurns)
            {
                currentTurn = 0;
            }
            else
            {
                currentTurn += 1;
            }
            
            currentActor = participantList[currentTurn];
        
            if (currentActor.GetComponent<UnitInformation>().dead)
            {
                SkipUnitTurn();
            }
            else
            {
            StartCoroutine(FollowActiveUnit());
            }
    }
    
    IEnumerator FollowActiveUnit()
    {
        iTween.MoveTo(battleGuide,currentActor.transform.position, 0.3f);
        yield return new WaitForSeconds(0.5f);
        if (currentActor != null)
        {
            newTurnSet = false;
            if (currentActor.tag == "Ally")
            {
                MovementGrid.control.BuildMovementGrid();
                currentActor.GetComponent<AllyMoveControl>().thisUnitActive = true;
                if (UI_magicMenu.control != null)
                {
                    UI_magicMenu.control.menuTurnOff = false;
                }
                currentActor.GetComponent<AllyMoveControl>().initialPosition = currentActor.transform.position;
                currentActor.GetComponent<AllyMoveControl>().enabled = true;
                
            }
            if (currentActor.tag == "Enemy")
            {
                MovementGrid.control.BuildMovementGrid();
                currentActor.GetComponent<EnemyAI>().thisEnemyTurnNow = true;
                currentActor.GetComponent<EnemyAI>().enabled = true;
                currentActor.GetComponent<EnemyAI>().hitOnce = true;
            }
        }
    }
    
    public void DestroyNotMoveable()
    {
        GameObject[] nms; 
        nms = GameObject.FindGameObjectsWithTag ("NotMoveable");
     
        foreach(GameObject nm in nms)
        {
            Destroy(nm);
        }
    }
}
