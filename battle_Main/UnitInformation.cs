using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInformation : MonoBehaviour {

    public GameObject currentUnit;
    public string displayName;
    public string unitName;
    public bool isLeader;
    public int maxHP;
    public int currentHP;
    public int maxMP;
    public int currentMP;
    public int maxMov;
    public int currentMov;
    public int agility;
    public int physAtk;
    public List<GameObject> spells;
    
    public bool dead = true;
    
    public RaycastHit inFront;
    public RaycastHit inBehind;
    public RaycastHit inRight;
    public RaycastHit inLeft;
    
    public string objectFront;
    public string objectBehind;
    public string objectRight;
    public string objectLeft;
    
    public GameObject loseNotice;
    public GameObject winNotice;
    
	// Use this for initialization
	void Start () {
		currentHP = maxHP;
        currentMP = maxMP;
        currentMov = maxMov;
        dead = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (dead)
        {
            iTween.ScaleTo(currentUnit, new Vector3(0.1f,0.1f,0.1f),1f);
            iTween.MoveTo(currentUnit, new Vector3(0f,10f,0f), 1f);
            if (isLeader)
            {
                if (gameObject.tag == "Ally")
                {
                    loseNotice.SetActive(true);
                }
                
                if (gameObject.tag == "Enemy")
                {
                    winNotice.SetActive(true);
                }
                
            }
        }
    
	}
}
