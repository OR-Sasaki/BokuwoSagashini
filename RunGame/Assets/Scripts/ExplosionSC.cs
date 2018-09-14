using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSC : MonoBehaviour {

    void OnAnimationFinish(){
        //アニメーションが終わったら消す
        Destroy(gameObject);
    }
}
