using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWall_NoDamage_Box_RotateSC : MonoBehaviour {

    public float SPEED_ROTATE = 0.0f;
    private int forb = 1;

    private GameObject FACI;

	void Start()
	{
        FACI = GameObject.Find("GameFacilitator");
        if(Random.Range(0,1)>0.5){
            forb = -1;
        }
	}

	void Update () {
        //難易度取得
        float dif = FACI.GetComponent<GameFacilitatorSC>().dificulty;
        transform.Rotate(new Vector3(0, 0,forb*SPEED_ROTATE*dif));
	}
}
