using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionMenuFunction : MonoBehaviour {

    public static UnitActionMenuFunction control;
    
    public GameObject currentActor;
    public GameObject attackIcon;
    public GameObject offenseInd;
    public GameObject supportInd;
    public GameObject unitActionMenu;
    public bool selectTarget; //when this is on, the control is of the attackIcon
    public GameObject currentTarget; //this allows scrolling through available enemies
    
    //menu options
    public GameObject menuAttack;
    public GameObject menuStay;
    public GameObject menuItem;
    public GameObject menuMagic;
    public GameObject menuNotInRange;
    
    public int inputCooldown;
    public GameObject magicSubMenu;
    public GameObject magicEmptySlot;
    public GameObject currentSpell;
    public int magicMenuRow;
    public int magicMenuColumn;
    
    public RaycastHit hit;
    public RaycastHit inFront;
    public RaycastHit inBehind;
    public RaycastHit inRight;
    public RaycastHit inLeft;
    public float lengthToCheck;
    public string targetTag;
    public string actionType;
    
    public List<GameObject> inRange;
    
    public GameObject objectNorth;
    public GameObject objectSouth;
    public GameObject objectEast;
    public GameObject objectWest;
    public GameObject objectNorthWest;
    public GameObject objectNorthEast;
    public GameObject objectSouthEast;
    public GameObject objectSouthWest;
    
    public bool notHoldingKey;
    public bool mainMenu;
    public bool hitOnce; //to stop attack button from triggering again
    public bool waitOver; //used to separate button inputs with short delay
    public int menuSelect;

    public List<GameObject> targetable;
    public GameObject closestTarget;
    public bool newTargetSeek;
    public GameObject targetMenu;
    public GameObject thisTarget;
    public GameObject targetInfo; //prefab
    public GameObject targetName;
    public GameObject targetStat;
    
	// Use this for initialization
	void Start () {
		control = this;
        selectTarget = false;
        menuSelect = 1;
        magicMenuRow = 1;
        waitOver = false;
        hitOnce = false;
        inputCooldown = 60;
	}
	
	// Update is called once per frame
	void Update () {
            
        if (inputCooldown > 0)
        {
        inputCooldown -= 1;
        }
        
        
        currentActor = BattleManager.control.currentActor;
        
        if (mainMenu)
        {
            MainMenuSelect();
        }
            
        
		if (selectTarget)
        {
            TargetSelect();
            BattleCamControl.control.camPos = 1; 
        }
        
        if (magicSubMenu.activeSelf)
        {
            if(UI_magicMenu.control != null && !selectTarget)
            {
            MagicButton();
            }
        }
        
	}

    public void MainMenuSelect()
    {
        
        switch (menuSelect)
            {
                case 0:
                    menuAttack.GetComponent<CanvasGroup>().alpha = 0.4f;
                    menuStay.GetComponent<CanvasGroup>().alpha = 0.4f;
                    menuMagic.GetComponent<CanvasGroup>().alpha = 0.4f;
                    menuItem.GetComponent<CanvasGroup>().alpha = 0.4f;
                break;
                case 1:
                    menuAttack.GetComponent<CanvasGroup>().alpha = 1f;
                    menuStay.GetComponent<CanvasGroup>().alpha = 0.4f;
                    menuMagic.GetComponent<CanvasGroup>().alpha = 0.4f;
                    menuItem.GetComponent<CanvasGroup>().alpha = 0.4f;
                break;
                case 2:
                    menuStay.GetComponent<CanvasGroup>().alpha = 1f;
                    menuAttack.GetComponent<CanvasGroup>().alpha = 0.4f;
                    menuMagic.GetComponent<CanvasGroup>().alpha = 0.4f;
                    menuItem.GetComponent<CanvasGroup>().alpha = 0.4f;
                break;
                case 3:
                    menuAttack.GetComponent<CanvasGroup>().alpha = 0.4f;
                    menuStay.GetComponent<CanvasGroup>().alpha = 0.4f;
                    menuMagic.GetComponent<CanvasGroup>().alpha = 1f;
                    menuItem.GetComponent<CanvasGroup>().alpha = 0.4f;
                break;
                case 4:
                    menuAttack.GetComponent<CanvasGroup>().alpha = 0.4f;
                    menuStay.GetComponent<CanvasGroup>().alpha = 0.4f;
                    menuMagic.GetComponent<CanvasGroup>().alpha = 0.4f;
                    menuItem.GetComponent<CanvasGroup>().alpha = 1f;
                break;
                
            }
        
            if (Input.GetButtonUp("Submit"))
            {
                if (mainMenu == true)
                {
                    switch (menuSelect)
                    {
                        case 1:
                            lengthToCheck = 1f;
                            targetTag = "Enemy";
                            actionType = "Attack";
                            inputCooldown = 30;
                            AttackButton();
                            mainMenu = false;
                        break;
                        case 2:
                            StayButton();
                            mainMenu = false;
                        break;
                        case 3:
                            magicSubMenu.SetActive(true);
                            actionType = "Magic";
                            mainMenu = false;
                            inputCooldown = 30;
                            magicSubMenu.GetComponent<UI_magicMenu>().menuTurnOn = true;
                        break;
                    }
                }
            }
        
            if (Input.GetButtonUp("Cancel") && mainMenu == true && inputCooldown == 0)
            {
                mainMenu = false;
                targetable.Clear();
            }
            
            if (Input.GetAxis("Vertical") > 0.1f)
            {
                    menuSelect = 1;
            }
            else if (Input.GetAxis("Vertical") < -0.1f)
            {
                    menuSelect = 2;
            }
            else if (Input.GetAxis("Horizontal") > 0.1f)
            {
                menuSelect = 3;
            }
            else if (Input.GetAxis("Horizontal") < -0.1f)
            {
                menuSelect = 4;
            }
            else
            {
            }
        
            
    }
    
    IEnumerator AutoEnableMenu()
    {
        
        yield return new WaitForSeconds(0.2f);
        mainMenu = true;
    }
    
    public void AttackButton()
    {
        StartCoroutine(EnterTargeting());
    }
    
    public void MagicButton()
    {
        
        magicMenuNavRow();
        
        if (currentSpell != null)
        {
            SpellFunctions.control.spellToCast = currentSpell.name;
        }
        
        if(Input.GetButtonDown("Submit") && inputCooldown == 0)
        {
            if (currentSpell != null)
            {
            //this calls a function in spellfunctions that will populate the target list depending on the spell currently highlighted
            SpellFunctions.control.targetSetup = true;
            //variables in this script are set in spell functions to begin the targeting controls
            StartCoroutine(EnterTargeting());
            }
        }
        
        if(Input.GetButtonUp("Cancel"))
        {
            mainMenu = true;
            UI_magicMenu.control.menuTurnOff = true;
            inputCooldown = 30;
        }
        
        StartCoroutine(spellSelect());
    }
    
    public void magicMenuNavRow()
    {
        
        if (UI_magicMenu.control.currentSpells.Count > 0)
        {
            if(Input.GetAxis("Horizontal") > 0.2f && inputCooldown == 0)
            {
                if (magicMenuRow < 3)
                {
                magicMenuRow += 1;
                }
                inputCooldown = 30;
            }
            if(Input.GetAxis("Horizontal") < -0.2f && inputCooldown == 0)
            {
                if (magicMenuRow > 1)
                {
                magicMenuRow -= 1;
                }
                inputCooldown = 30;
            }
        }
    }
    
    IEnumerator spellSelect()
    {
        yield return new WaitForSeconds(0.1f);
        
        if (UI_magicMenu.control.spellSpawn != null)
        {
         switch (magicMenuRow)
                {
                    case 1:
                    if (UI_magicMenu.control.currentSpells[0] != magicEmptySlot)
                    {
                    currentSpell = UI_magicMenu.control.currentSpells[0];
                    UI_magicMenu.control.currentSpells[0].GetComponent<CanvasGroup>().alpha = 1f;
                    UI_magicMenu.control.currentSpells[1].GetComponent<CanvasGroup>().alpha = 0.4f;
                    UI_magicMenu.control.currentSpells[2].GetComponent<CanvasGroup>().alpha = 0.4f;
                    }
                    break;
                    case 2:
                    if (UI_magicMenu.control.currentSpells[1] != magicEmptySlot)
                    {
                    currentSpell = UI_magicMenu.control.currentSpells[1];
                    UI_magicMenu.control.currentSpells[0].GetComponent<CanvasGroup>().alpha = 0.4f;
                    UI_magicMenu.control.currentSpells[1].GetComponent<CanvasGroup>().alpha = 1f;
                    UI_magicMenu.control.currentSpells[2].GetComponent<CanvasGroup>().alpha = 0.4f;
                    }
                    break;
                    case 3:
                    if (UI_magicMenu.control.currentSpells[2] != magicEmptySlot)
                    {
                    currentSpell = UI_magicMenu.control.currentSpells[2];
                    UI_magicMenu.control.currentSpells[0].GetComponent<CanvasGroup>().alpha = 0.4f;
                    UI_magicMenu.control.currentSpells[1].GetComponent<CanvasGroup>().alpha = 0.4f;
                    UI_magicMenu.control.currentSpells[2].GetComponent<CanvasGroup>().alpha = 1f;
                    }
                    break;
                }
        }
    }
    
    
    IEnumerator EnterTargeting()
    {
        unitActionMenu.SetActive(false);
        BattleManager.control.notSubMenu = false;
        ActorSurroundings();
        yield return new WaitForSeconds(0.3f);
        //the following autoselects a target first
        if (targetable.Count > 0)
        {
            attackIcon.SetActive(true);
            currentTarget = targetable[0];
            IconSurroundings();
            attackIcon.transform.position = targetable[0].transform.position;
            yield return new WaitForSeconds(0.1f);
            selectTarget = true;
        }
        else
        {
            Debug.Log("No enemy in range");
            menuNotInRange.SetActive(true);
            unitActionMenu.SetActive(true);
            magicSubMenu.SetActive(false);
            attackIcon.SetActive(false);
            selectTarget = false;
            BattleManager.control.notSubMenu = true;
            mainMenu = true;
            StartCoroutine(AutoEnableMenu());
        }
        
    }
    
    public void FeedTargetMenu()
    {
        foreach (GameObject lastTarget in GameObject.FindGameObjectsWithTag("ui_targetListItem"))
        {
            Destroy(lastTarget);
        }
        foreach (GameObject posTar in inRange)
        {
            if (posTar != null)
            {
            if (posTar.tag == targetTag)
            {
            targetable.Add(posTar);
            }
            }
        }
        for (int i = 0; i < targetable.Count; i++)
        {
            thisTarget = Instantiate(targetInfo);
            thisTarget.gameObject.transform.SetParent(targetMenu.transform, false);
            targetName = thisTarget.transform.Find("TargetName").gameObject;
            if (targetName != null)
            {
                targetName.GetComponent<Text>().text = targetable[i].GetComponent<UnitInformation>().displayName;
            }
        }
        inRange.Clear();
        
    }
    
    IEnumerator FindTargetByClosest()
    {
            GameObject closest = null;
            float distanceX = Mathf.Infinity;
            float distanceZ = Mathf.Infinity;
            Vector3 IconPosition = attackIcon.transform.position;
            foreach (GameObject targ in targetable)
            {
                float diffX = targ.transform.position.x - IconPosition.x;
                float diffZ = targ.transform.position.z - IconPosition.z;
                
                float curDistanceX = diffX;
                float curDistanceZ = diffZ;
                
                if (curDistanceX < distanceX)
                {
                    if(Input.GetAxis("Horizontal") > 0.1f)
                    {
                        if (targ.transform.position.x > IconPosition.x)
                        {
                        closest = targ;
                        distanceX = curDistanceX;
                        }
                    }
                    if(Input.GetAxis("Horizontal") < -0.1f)
                    {
                        if (targ.transform.position.x < IconPosition.x)
                        {
                        closest = targ;
                        distanceX = curDistanceX;
                        }
                    }
                }
                
                if (curDistanceZ < distanceZ)
                {
                    if(Input.GetAxis("Vertical") > 0.1f)
                    {
                        if (targ.transform.position.z > IconPosition.z)
                        {
                        closest = targ;
                        distanceZ = curDistanceZ;
                        }
                    }
                    if(Input.GetAxis("Vertical") < -0.1f)
                    {
                        if (targ.transform.position.z < IconPosition.z)
                        {
                        closest = targ;
                        distanceX = curDistanceZ;
                        }
                    }
                }
            }
            if (closest != closestTarget)
            {
            closestTarget = closest;
            }
            yield return new WaitForSeconds(0.1f);
            
    }
    
    public void TargetSelect()
    {
        
            if (targetTag == "Enemy")
            {
                 offenseInd.SetActive(true);
                 supportInd.SetActive(false);
            }
            if (targetTag == "Ally")
            {
                 offenseInd.SetActive(false);
                 supportInd.SetActive(true);
            }

            if(Input.GetAxis("Vertical") != 0f || Input.GetAxis("Horizontal") != 0f)
            {
                StartCoroutine(FindTargetByClosest());
                if (closestTarget != null)
                {
                currentTarget = closestTarget;
                }
                if (currentTarget != null)
                {
                iTween.MoveTo(attackIcon, currentTarget.transform.position, 0.3f);
                }
            }
            
            if (Input.GetButtonUp("Cancel"))
            {
                foreach (GameObject lastTarget in GameObject.FindGameObjectsWithTag("ui_targetListItem"))
                {
                Destroy(lastTarget);
                }
                unitActionMenu.SetActive(true);
                attackIcon.SetActive(false);
                selectTarget = false;
                mainMenu = true;
                currentTarget = null;
                BattleManager.control.notSubMenu = true;
            }  
        
            
                if(Input.GetButtonUp("Submit") && inputCooldown == 0)
                {
                    if (actionType == "Attack")
                    {
                            if (!hitOnce)
                            {
                            StartCoroutine(NormalAttack());
                            UI_impactDisplay.control.damageCounter.transform.position = UnitActionMenuFunction.control.currentTarget.transform.position;
                            currentActor.GetComponent<AllyMoveControl>().unitModel.GetComponent<Animator>().SetTrigger("Attack");
                            }
                    }
                    
                    if (actionType == "Magic")
                    {
                        if (!hitOnce && currentTarget != null)
                        {
                        SpellFunctions.control.spellCall = true;
                        attackIcon.SetActive(false);
                        hitOnce = true;
                        }
                    }
                    
                }
                
    }
    
    public void IconMovement()
    {
         if (Input.GetAxis("Vertical") > 0.1f)
            {
                    switch (BattleCamControl.control.camPos)
                    {
                        case 1:
                        if (objectNorth != null)
                        {
                            if (objectNorth.tag == targetTag)
                            {
                            iTween.MoveBy(attackIcon,iTween.Hash("amount",new Vector3(0f,0f,1f),"time",0.3f, "easeType", "easeOutQuad", "oncomplete", "ReduceCurrentMov", "oncompletetarget", this.gameObject));
                            
                            }
                        }
                        
                        break;
                        case 2:
                        if (objectEast != null)
                        {
                            if (targetable.Contains(objectEast))
                            {
                            iTween.MoveBy(attackIcon,iTween.Hash("amount",new Vector3(1f,0f,0f),"time",0.3f, "easeType", "easeOutQuad", "oncomplete", "ReduceCurrentMov", "oncompletetarget", this.gameObject));
                            
                            }
                        }
                        break;
                        case 3:
                        if (objectSouth != null)
                        {
                            if (targetable.Contains(objectSouth))
                            {
                            iTween.MoveBy(attackIcon,iTween.Hash("amount",new Vector3(0f,0f,-1f),"time",0.3f, "easeType", "easeOutQuad", "oncomplete", "ReduceCurrentMov", "oncompletetarget", this.gameObject));
                            
                            }
                        }
                        break;
                        case 4:
                        if (objectWest != null)
                        {
                            if (targetable.Contains(objectWest))
                            {
                            iTween.MoveBy(attackIcon,iTween.Hash("amount",new Vector3(-1f,0f,0f),"time",0.3f, "easeType", "easeOutQuad", "oncomplete", "ReduceCurrentMov", "oncompletetarget", this.gameObject));
                            
                           
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
                            if (objectSouth.tag == targetTag)
                            {
                            iTween.MoveBy(attackIcon,iTween.Hash("amount",new Vector3(0f,0f,-1f),"time",0.3f, "easeType", "easeOutQuad", "oncomplete", "ReduceCurrentMov", "oncompletetarget", this.gameObject));
                            
                            }
                        }
                        
                        break;
                        case 2:
                        if (objectWest != null)
                        {
                            if (objectWest.tag == "MoveNode")
                            {
                            iTween.MoveBy(attackIcon,iTween.Hash("amount",new Vector3(-1f,0f,0f),"time",0.3f, "easeType", "easeOutQuad", "oncomplete", "ReduceCurrentMov", "oncompletetarget", this.gameObject));
                            
                            }
                        }
                        break;
                        case 3:
                        if (objectNorth != null)
                        {
                            if (targetable.Contains(objectNorth))
                            {
                            iTween.MoveBy(attackIcon,iTween.Hash("amount",new Vector3(0f,0f,1f),"time",0.3f, "easeType", "easeOutQuad", "oncomplete", "ReduceCurrentMov", "oncompletetarget", this.gameObject));
                            
                            }
                        }
                        break;
                        case 4:
                        if (objectEast != null)
                        {
                            if (objectEast.tag == "MoveNode")
                            {
                            iTween.MoveBy(attackIcon,iTween.Hash("amount",new Vector3(1f,0f,0f),"time",0.3f, "easeType", "easeOutQuad", "oncomplete", "ReduceCurrentMov", "oncompletetarget", this.gameObject));
                            
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
                            if (objectEast.tag == targetTag)
                            {
                            iTween.MoveBy(attackIcon,iTween.Hash("amount",new Vector3(1f,0f,0f),"time",0.3f, "easeType", "easeOutQuad", "oncomplete", "ReduceCurrentMov", "oncompletetarget", this.gameObject));
                            
                            }
                        }
                        
                        break;
                        case 2:
                        if (objectSouth != null)
                        {
                            if (objectSouth.tag == "MoveNode")
                            {
                            iTween.MoveBy(attackIcon,iTween.Hash("amount",new Vector3(0f,0f,-1f),"time",0.3f, "easeType", "easeOutQuad", "oncomplete", "ReduceCurrentMov", "oncompletetarget", this.gameObject));
                            
                            }
                        }
                        break;
                        case 3:
                        if (objectWest != null)
                        {
                            if (objectWest.tag == "MoveNode")
                            {
                            iTween.MoveBy(attackIcon,iTween.Hash("amount",new Vector3(-1f,0f,0f),"time",0.3f, "easeType", "easeOutQuad", "oncomplete", "ReduceCurrentMov", "oncompletetarget", this.gameObject));
                            
                            }
                        }
                        break;
                        case 4:
                        if (objectNorth != null)
                        {
                            if (targetable.Contains(objectNorth))
                            {
                            iTween.MoveBy(attackIcon,iTween.Hash("amount",new Vector3(0f,0f,1f),"time",0.3f, "easeType", "easeOutQuad", "oncomplete", "ReduceCurrentMov", "oncompletetarget", this.gameObject));
                            
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
                            if (objectWest.tag == targetTag)
                            {
                            iTween.MoveBy(attackIcon,iTween.Hash("amount",new Vector3(-1f,0f,0f),"time",0.3f, "easeType", "easeOutQuad", "oncomplete", "ReduceCurrentMov", "oncompletetarget", this.gameObject));
                            
                            }
                        }
                        
                        break;
                        case 2:
                        if (objectNorth != null)
                        {
                            if (targetable.Contains(objectNorth))
                            {
                            iTween.MoveBy(attackIcon,iTween.Hash("amount",new Vector3(0f,0f,1f),"time",0.3f, "easeType", "easeOutQuad", "oncomplete", "ReduceCurrentMov", "oncompletetarget", this.gameObject));
                            
                            }
                        }
                        break;
                        case 3:
                        if (objectEast != null)
                        {
                            if (objectEast.tag == "MoveNode")
                            {
                            iTween.MoveBy(attackIcon,iTween.Hash("amount",new Vector3(1f,0f,0f),"time",0.3f, "easeType", "easeOutQuad", "oncomplete", "ReduceCurrentMov", "oncompletetarget", this.gameObject));
                            
                            }
                        }
                        break;
                        case 4:
                        if (objectSouth != null)
                        {
                            if (objectSouth.tag == "MoveNode")
                            {
                            iTween.MoveBy(attackIcon,iTween.Hash("amount",new Vector3(0f,0f,-1f),"time",0.3f, "easeType", "easeOutQuad", "oncomplete", "ReduceCurrentMov", "oncompletetarget", this.gameObject));
                            
                            }
                        }
                        break;
                    }
            }
    }
    
    IEnumerator NormalAttack()
    {
        hitOnce = true;
        if (currentTarget != null)
        {
            if (currentTarget.tag == targetTag)
            {
            currentTarget.GetComponent<UnitInformation>().currentHP -= currentActor.GetComponent<UnitInformation>().physAtk;
            UI_impactDisplay.control.damageAmount = currentActor.GetComponent<UnitInformation>().physAtk;
            UI_impactDisplay.control.damageCall = true;
            if (currentTarget.GetComponent<UnitInformation>().currentHP <= 0)
            {
                currentTarget.GetComponent<UnitInformation>().dead = true;
            }
            }
            else
            {
                Debug.Log("Can't attack that!");
            }
        }
        attackIcon.SetActive(false);
        yield return new WaitForSeconds(1);
        selectTarget = false;
        BattleManager.control.notSubMenu = true;
        currentTarget = null;
        objectNorth = null;
        objectSouth = null;
        objectEast = null;
        objectWest = null;
        hitOnce = false;
        StayButton(); 
    }
    
    public void StayButton()
    {
        foreach (GameObject lastTarget in GameObject.FindGameObjectsWithTag("ui_targetListItem"))
        {
        Destroy(lastTarget);
        }
        mainMenu = false;
        currentTarget = null;
        hitOnce = false;
        targetable.Clear();
        inRange.Clear();
        if (UI_magicMenu.control != null)
        {
            UI_magicMenu.control.menuTurnOff = true;
        }
        BattleManager.control.moveEnabled = true;
        BattleManager.control.unitActionMenu.SetActive(false);
        BattleManager.control.currentActor.GetComponent<UnitInformation>().currentMov = BattleManager.control.currentActor.GetComponent<UnitInformation>().maxMov;
        BattleManager.control.EndUnitTurn();
    }
    
     public void ActorSurroundings()
    {
        targetable.Clear();
        inRange.Clear();
        
        Vector3 north = currentActor.transform.TransformDirection(Vector3.forward);
        Vector3 south = currentActor.transform.TransformDirection(-Vector3.forward);
        Vector3 east = currentActor.transform.TransformDirection(Vector3.right);
        Vector3 west = currentActor.transform.TransformDirection(Vector3.left);
        Vector3 northEast = currentActor.transform.TransformDirection(1f, 0f, 1f) * (lengthToCheck - 1f);
        Vector3 northWest = currentActor.transform.TransformDirection(-1f, 0f, 1f) * (lengthToCheck - 1f);
        Vector3 southEast = currentActor.transform.TransformDirection(1f, 0f, -1f) * (lengthToCheck - 1f);
        Vector3 southWest = currentActor.transform.TransformDirection(-1f, 0f, -1f) * (lengthToCheck - 1f);
        
        Debug.DrawRay(currentActor.transform.position, north, Color.green);
        Debug.DrawRay(currentActor.transform.position, south, Color.green);
        Debug.DrawRay(currentActor.transform.position, east, Color.green);
        Debug.DrawRay(currentActor.transform.position, west, Color.green);
        Debug.DrawRay(currentActor.transform.position, northEast, Color.green);
        Debug.DrawRay(currentActor.transform.position, northWest, Color.green);
        Debug.DrawRay(currentActor.transform.position, southEast, Color.green);
        Debug.DrawRay(currentActor.transform.position, southWest, Color.green);
        
        if(Physics.Raycast(currentActor.transform.position,(north), out hit, lengthToCheck))
        {    
            inRange.Add(hit.collider.gameObject);
        }
        if(Physics.Raycast(currentActor.transform.position,(south), out hit, lengthToCheck))
        {    
            inRange.Add(hit.collider.gameObject);
        }
        if(Physics.Raycast(currentActor.transform.position,(east), out hit, lengthToCheck))
        {    
            inRange.Add(hit.collider.gameObject);
        }
        if(Physics.Raycast(currentActor.transform.position,(west), out hit, lengthToCheck))
        {    
            inRange.Add(hit.collider.gameObject);
        }
        if(Physics.Raycast(currentActor.transform.position,(northWest), out hit, lengthToCheck))
        {    
            inRange.Add(hit.collider.gameObject);
        }
        if(Physics.Raycast(currentActor.transform.position,(northEast), out hit, lengthToCheck))
        {    
            inRange.Add(hit.collider.gameObject);
        }
        if(Physics.Raycast(currentActor.transform.position,(southEast), out hit, lengthToCheck))
        {    
            inRange.Add(hit.collider.gameObject);
        }
        if(Physics.Raycast(currentActor.transform.position,(southWest), out hit, lengthToCheck))
        {    
            inRange.Add(hit.collider.gameObject);
        }
        
        if (targetTag == "Ally")
        {
            inRange.Add(this.gameObject);
        }
            
        FeedTargetMenu();
        
    }

    public void IconSurroundings()
    {
        Vector3 north = attackIcon.transform.TransformDirection(Vector3.forward);
        Vector3 south = attackIcon.transform.TransformDirection(-Vector3.forward);
        Vector3 east = attackIcon.transform.TransformDirection(Vector3.right);
        Vector3 west = attackIcon.transform.TransformDirection(Vector3.left);
        Vector3 northEast = attackIcon.transform.TransformDirection(1f, 0f, 1f) * (lengthToCheck - 1f);
        Vector3 northWest = attackIcon.transform.TransformDirection(-1f, 0f, 1f) * (lengthToCheck - 1f);
        Vector3 southEast = attackIcon.transform.TransformDirection(1f, 0f, -1f) * (lengthToCheck - 1f);
        Vector3 southWest = attackIcon.transform.TransformDirection(-1f, 0f, -1f) * (lengthToCheck - 1f);
        
        Debug.DrawRay(attackIcon.transform.position, north, Color.green);
        Debug.DrawRay(attackIcon.transform.position, south, Color.green);
        Debug.DrawRay(attackIcon.transform.position, east, Color.green);
        Debug.DrawRay(attackIcon.transform.position, west, Color.green);
        Debug.DrawRay(attackIcon.transform.position, northEast, Color.green);
        Debug.DrawRay(attackIcon.transform.position, northWest, Color.green);
        Debug.DrawRay(attackIcon.transform.position, southEast, Color.green);
        Debug.DrawRay(attackIcon.transform.position, southWest, Color.green);
        
        if(Physics.Raycast(attackIcon.transform.position,(north), out hit, lengthToCheck))
        {    
            inRange.Add(hit.collider.gameObject);
        }
        if(Physics.Raycast(attackIcon.transform.position,(south), out hit, lengthToCheck))
        {    
            inRange.Add(hit.collider.gameObject);
        }
        if(Physics.Raycast(attackIcon.transform.position,(east), out hit, lengthToCheck))
        {    
            inRange.Add(hit.collider.gameObject);
        }
        if(Physics.Raycast(attackIcon.transform.position,(west), out hit, lengthToCheck))
        {    
            inRange.Add(hit.collider.gameObject);
        }
        if(Physics.Raycast(attackIcon.transform.position,(northWest), out hit, lengthToCheck))
        {    
            inRange.Add(hit.collider.gameObject);
        }
        if(Physics.Raycast(attackIcon.transform.position,(northEast), out hit, lengthToCheck))
        {    
            inRange.Add(hit.collider.gameObject);
        }
        if(Physics.Raycast(attackIcon.transform.position,(southEast), out hit, lengthToCheck))
        {    
            inRange.Add(hit.collider.gameObject);
        }
        if(Physics.Raycast(attackIcon.transform.position,(southWest), out hit, lengthToCheck))
        {    
            inRange.Add(hit.collider.gameObject);
        }
        
    }
    
}
