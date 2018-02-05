using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementGrid : MonoBehaviour {

    public static MovementGrid control;
    
    public GameObject Field;
    public Vector3 initialPos;
    public Vector3 addXRow;
    public Vector3 addZRow;
    public Vector3 resetXRow;
    
    public bool gridSet;
    
    public GameObject moveNode;
    
    public float xRows;
    public float zRows;
    
	// Use this for initialization
	void Awake () {
        control = this;
        gridSet = false;
        xRows = Field.GetComponent<MeshRenderer>().bounds.size.x + 1f;
		zRows = Field.GetComponent<MeshRenderer>().bounds.size.z + 1f;
        initialPos = new Vector3 (0f,-0.25f,0f);
        addXRow = new Vector3(1f,0f,0f);
        resetXRow = new Vector3(xRows,0f,0f);
        addZRow = new Vector3(0f,0f,1f);
	}
	
	// Update is called once per frame
	void Update () {

	}
    
    public void BuildMovementGrid()
    {
        if (!gridSet)
        {
            for (int i = 0; i < zRows; i++)
            {
                    for (int i2 = 0; i2 < xRows; i2++)
                        {
                        Instantiate(moveNode,initialPos,transform.rotation);
                        initialPos += addXRow;
                        }
                initialPos -= resetXRow;
                initialPos += addZRow;
            }
        }
        gridSet = true;
        initialPos = new Vector3 (0f,0f,0f);
    }
    
    public void ClearMovementGrid()
    {
        gridSet = false;
        GameObject[] moveNodes;
        moveNodes = GameObject.FindGameObjectsWithTag("MoveNode");
        initialPos = new Vector3 (0f,0f,0f);
        foreach (GameObject moveNode in moveNodes)
        {
                moveNode.GetComponent<DestroyMoveNodeOnCollision>().GetRidOf();
        }
    }
}
