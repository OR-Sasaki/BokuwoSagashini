using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class TitleSC : BaseMeshEffect{

    private float times = 0;

    public float DELAY=0.5f;
    public float LENGTH = 0.1f;//どのくらい動かすか

    //文字をプルプルさせる
	public override void ModifyMesh(VertexHelper vh)
	{
        //三角形の頂点取得
        List<UIVertex> verts = new List<UIVertex>();
        vh.GetUIVertexStream(verts);

        //あえて全ての頂点をバラバラの方向に動かす
        for (int i = 0; i < verts.Count; i++){
            float rad = Random.Range(0, 360) * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(LENGTH * Mathf.Cos(rad), LENGTH * Mathf.Sin(rad));
            var buf = verts[i];
            buf.position += (Vector3)dir;
            verts[i]=buf;
        }

        vh.Clear();
        vh.AddUIVertexTriangleStream(verts);
	}

	void Update()
	{
        times += Time.deltaTime;
        if(times>DELAY){
            times = 0;
            base.GetComponent<Graphic>().SetVerticesDirty();
        }
	}
}
