using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWallSC : MonoBehaviour {

    public float SPEED = 0.7f;
    public float ACCELERATION = 1.0f;

    private Rigidbody2D rig = null;

    private GameObject FACI;

	void Start () {
        rig = this.GetComponent<Rigidbody2D>();
        FACI = GameObject.Find("GameFacilitator");
    }
	
	void Update () {
        //難易度を取得
        float dif = FACI.GetComponent<GameFacilitatorSC>().dificulty;
        rig.velocity = new Vector2(-1 * SPEED * dif, 0);
        SPEED *= ACCELERATION;
	}
}
