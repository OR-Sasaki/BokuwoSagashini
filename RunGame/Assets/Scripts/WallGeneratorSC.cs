using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WallGeneratorSC : MonoBehaviour {

    public float DISTANCE_DELAY_GENELATE_WALL = -10.0f;//壁が生成されるまでのdelay
    public float TIME_DELAY_GENELATE_OBSTACLE = 3.0f; //障害物が生成されるまでのdelay
    private float time_O = 0.0f;
    public GameObject[] WALLS = { };
    public GameObject[] OBSRACLES = { };
    private System.Random rand =null;
    private GameObject wall;
    private GameObject obstacle;

	void Start () {
        //初期の壁を生成
        rand = new System.Random();
        wall=Instantiate(WALLS[rand.Next(WALLS.Length)]);
        obstacle=Instantiate(OBSRACLES[rand.Next(OBSRACLES.Length)]);
	}
	
	void Update () {
        time_O += Time.deltaTime;

        //ある程度壁が進んだら新しい壁を生成
        if(wall.transform.position.x < (DISTANCE_DELAY_GENELATE_WALL)){
            wall=Instantiate(WALLS[rand.Next(WALLS.Length)]);
        }

        //時間がたったら障害物を生成
        if (time_O > TIME_DELAY_GENELATE_OBSTACLE)
        {
            time_O = 0;
            Instantiate(OBSRACLES[rand.Next(OBSRACLES.Length)]);
        }
	}
}