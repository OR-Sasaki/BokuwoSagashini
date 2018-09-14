using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionSC : MonoBehaviour {

    public GameObject SCENE_FADE;
    public InputField INPUTFIELD;

    private string KEY_MYNAME = "myname";
    private string name = "NoName";

	//inputfield用
	public void InputValue(){
        name = (INPUTFIELD.text=="")?"NoName":INPUTFIELD.text;
        PlayerPrefs.SetString(KEY_MYNAME, name);
    }

    //確定ボタンを押したら、名前を保存してメニューへ
    public void OnClick(){
        Debug.Log(name);
        SCENE_FADE.GetComponent<FadeInSC>().F_isFadeIn = true;
        SCENE_FADE.transform.SetAsLastSibling();
        StartCoroutine(DelayMethod(0.5f, () =>
        {
            SceneManager.LoadScene("Menu");
        }));
    }
    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}
