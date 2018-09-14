using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CollectionDispSC : MonoBehaviour {

    public GameObject BTNPREF;
    public GameObject CONPREF;
    public GameObject SENTVIEW;
    public GameObject VIEWCONT;
    public GameObject SCENE_FADE;

    private string KEY_POINT = "point";

    private List<string[]> loadData = new List<string[]>();
    private string viewtext = "";

	void Start () {
        loadfile("SentenceList",true);

        RectTransform content = CONPREF.GetComponent<RectTransform>();

        foreach(string[] x in loadData){
            //ボタン生成
            GameObject btn = (GameObject)Instantiate(BTNPREF);
            //親登録
            btn.transform.SetParent(content, false);
            //ボタンの設定
            if(Int32.Parse(x[0])<=PlayerPrefs.GetInt(KEY_POINT,0)){//ポイント確認
                btn.transform.GetComponentInChildren<Text>().text = x[1];
                btn.transform.GetComponent<Button>().onClick.AddListener(() => OnClick(x[2]));
            }else{
                btn.transform.GetComponentInChildren<Text>().text = x[0]+"ポイント必要";
            }
        }
	}
	
    private void OnClick(string st){
        loadfile(st,false);
        VIEWCONT.GetComponent<Text>().text=viewtext;
        SENTVIEW.SetActive(true);
    }

    public void Modoru(){
        //文表示中なら消す表示されてなかったらメニューへ

        if(SENTVIEW.activeSelf){
            SENTVIEW.SetActive(false);
        }else{
            SCENE_FADE.GetComponent<FadeInSC>().F_isFadeIn = true;
            SCENE_FADE.transform.SetAsLastSibling();
            StartCoroutine(DelayMethod(0.5f, () =>
            {
                SceneManager.LoadScene("Menu");
            }));
        }
    }

    public void loadfile(string x,bool isList){

        //CSVを読み込む
        TextAsset textFile = Resources.Load("Sentences/"+x) as TextAsset;
        StringReader reader=new StringReader(textFile.text);

        if (isList)
        {
            while (reader.Peek() > -1)
            {
                loadData.Add(reader.ReadLine().Split(','));
            }
        }else{
            viewtext = "\n";
            while (reader.Peek() > -1)
            {
                viewtext += reader.ReadLine()+"\n";
            }
        }
    }
    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}
