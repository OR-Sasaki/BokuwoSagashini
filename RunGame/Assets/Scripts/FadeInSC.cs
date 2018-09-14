using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInSC : MonoBehaviour {

    public float SPEED_FADEIN = 0.01f;
    public bool F_isFadeIn = false;
    public bool F_isFadeOut = false;
    public Text[] TEXTS;
    public Image[] IMAGES;

    public float alfa = 0.0f;

	void Update () {

        //TEXTSとIMAGESに入れたオブジェクトをフェードしてくれる便利なやつ
        //alfaが一緒のものしか出来ないから注意！

        if(F_isFadeIn || F_isFadeOut){
            
            if ((alfa < 1.0f)&&F_isFadeIn){
                alfa += SPEED_FADEIN;
            }else{
                F_isFadeIn = false;
            }

            if((alfa>0.0f)&&F_isFadeOut){
                alfa -= SPEED_FADEIN;
            }else{
                if(F_isFadeOut)transform.SetAsFirstSibling();
                F_isFadeOut = false;
            }

            //反映
            foreach(Text x in TEXTS){
                x.color = new Color(x.color.r, x.color.g, x.color.b, alfa);
            }
            foreach(Image x in IMAGES){
                x.color = new Color(x.color.r, x.color.g, x.color.b, alfa);
            }
        }
	}
}
