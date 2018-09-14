using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAreaSC : MonoBehaviour {

    //触ったら消す
	private void OnTriggerEnter2D(Collider2D c)
	{
        Destroy(c.gameObject);
	}
}
