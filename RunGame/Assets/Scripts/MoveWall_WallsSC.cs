using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWall_WallsSC : MonoBehaviour {

	void Update () {
        //子オブジェクトがいなくなったら自分も消す
        if(this.gameObject.transform.childCount==0){
            Destroy(this.gameObject);
        }
	}
}
