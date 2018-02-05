using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellFunctions : MonoBehaviour {

    public static SpellFunctions control;
    
    public GameObject spellAnim;
    
    public GameObject HealAnimation;
    
    public bool targetSetup = true;
    public bool spellCall;
    
    public string spellToCast;
    
	// Use this for initialization
	void Start () {
		control = this;
	}
	
	// Update is called once per frame
	void Update () {
		if (spellCall == true)
        {
            Invoke(spellToCast, 0.5f);
            UI_impactDisplay.control.healCounter.transform.position = UnitActionMenuFunction.control.currentTarget.transform.position;
            UI_impactDisplay.control.otherNotice.transform.position = UnitActionMenuFunction.control.currentTarget.transform.position;
            UI_impactDisplay.control.damageCounter.transform.position = UnitActionMenuFunction.control.currentTarget.transform.position;
            spellCall = false;
        }
        
        if (targetSetup == true)
        {
            SpellTargeting();
        }
	}
    
    public void SpellTargeting()
    {
        switch (spellToCast)
        {
            case "Heal" :
                    UnitActionMenuFunction.control.targetTag = "Ally";
                    UnitActionMenuFunction.control.lengthToCheck = 2f;
                    UnitActionMenuFunction.control.ActorSurroundings();
                if(!UnitActionMenuFunction.control.targetable.Contains(UnitActionMenuFunction.control.currentActor))  {UnitActionMenuFunction.control.targetable.Add(UnitActionMenuFunction.control.currentActor);}
                targetSetup = false;
            break;
        }
    }
    
    
    public void Heal()
    {
        int smallHeal = Random.Range(10, 14);
        spellAnim = Instantiate(HealAnimation);
        spellAnim.transform.position = UnitActionMenuFunction.control.currentTarget.transform.position;
        iTween.ColorTo(UnitActionMenuFunction.control.currentTarget, Color.green, 0.3f);
        
        
        UnitActionMenuFunction.control.currentTarget.GetComponent<UnitInformation>().currentHP
        +=
        smallHeal;
        
        if (UnitActionMenuFunction.control.currentTarget.GetComponent<UnitInformation>().currentHP > UnitActionMenuFunction.control.currentTarget.GetComponent<UnitInformation>().maxHP)
        {
        UnitActionMenuFunction.control.currentTarget.GetComponent<UnitInformation>().currentHP = UnitActionMenuFunction.control.currentTarget.GetComponent<UnitInformation>().maxHP;
        }
        
        UI_impactDisplay.control.healAmount = smallHeal;
        UI_impactDisplay.control.healCall = true;
        
        Invoke("EndSpell", 2f);
    }
    
    public void EndSpell()
    {
        UnitActionMenuFunction.control.StayButton();
    }
    
}
