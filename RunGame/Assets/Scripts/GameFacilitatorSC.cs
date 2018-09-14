using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using NCMB;

public class GameFacilitatorSC : MonoBehaviour
{
    public float TIME_DELAY_GAMEOVER = 2.0f;
    public float TIME_DELAY_GOSCENE = 0.5f;
    public float TIME_DELTA_ADD_SCORE = 0.3f;
    public float TIME_DELTA_ADD_DIFICULTY = 10;

    private string KEY_MYNAME = "myname";
    private string KEY_MY_SCORE = "myScore";
    private string KEY_HIGH_SCORE = "highscore";
    private string KEY_PLAYER_NAME = "playername";
    private string KEY_POINT = "point";
    private string KEY_NAMING = "naming";

    public GameObject SCENE_FADE;
    public Text SCORE_VIEW;
    public Text SCORE_VIEW_GAMEEND;
    public Text HIGHSCORE_VIEW_GAMEEND;
    public Text POINT_VIEW_GAMEEND;
    public Button Button_RePlay;
    public Button Button_GoMenu;

    private int score = 0;
    private float stime = 0;
    private float dtime = 0;
    public float dificulty = 0.6f;

    private bool F_Now_Game=true;
    private bool F_Already_Set_Key = false;

	void Start()
	{
        //初めてMenuに来たら名前の設定をさせる
        if(SceneManager.GetActiveScene().name == "Menu"){
            int alr = PlayerPrefs.GetInt(KEY_NAMING, 0);
            if (alr == 0)
            {
                SceneManager.LoadScene("Option");
            }
            PlayerPrefs.SetInt(KEY_NAMING, 1);
        }
	}

	void Update(){
        //シーンがGameだったらスコアを計測
        if (SceneManager.GetActiveScene().name == "Game")
        {
            stime += Time.deltaTime;
            dtime += Time.deltaTime;

            if ((stime > TIME_DELTA_ADD_SCORE) && F_Now_Game)
            {
                stime = 0;
                score++;
                SCORE_VIEW.GetComponent<Text>().text = "SCORE:" + score.ToString();
                SCORE_VIEW_GAMEEND.GetComponent<Text>().text = "SCORE:" + score.ToString();
            }

            if ((dtime > TIME_DELTA_ADD_DIFICULTY) && F_Now_Game)
            {
                dtime = 0;
                if (dificulty < 1)
                {
                    dificulty += 0.2f;
                }
            }
        }
	}

	public void GameOver(){
        F_Now_Game = false;

        //ストーリー用ポイント加算
        int point = PlayerPrefs.GetInt(KEY_POINT, 0)+(int)Math.Floor((double)score / 10);
        PlayerPrefs.SetInt(KEY_POINT, point);

        POINT_VIEW_GAMEEND.text = "POINT:"+point.ToString() + " +" + Math.Floor((double)score / 10).ToString();

        //ボタン許可
        Button_GoMenu.interactable = true;
        Button_RePlay.interactable = true;

        //ゲームオーバー画面をフェードイン
        StartCoroutine(DelayMethod(TIME_DELAY_GAMEOVER, () =>
        {
            this.GetComponent<FadeInSC>().F_isFadeIn = true;
        }));

        //ハイスコアをサーバーへ送信
        if(PlayerPrefs.GetInt(KEY_MY_SCORE, 0)<score){
            PlayerPrefs.SetInt(KEY_MY_SCORE, score);
        }

        //ハイスコアを表示
        HIGHSCORE_VIEW_GAMEEND.text = "HIGH SCORE:"+PlayerPrefs.GetInt(KEY_MY_SCORE, 0).ToString();

        //自分のkeyを取得
        string key = PlayerPrefs.GetString(KEY_HIGH_SCORE, "NoSet");

        F_Already_Set_Key = (key == "NoSet") ? false : true;

        NCMBObject NCobj = new NCMBObject(KEY_HIGH_SCORE);

        if (F_Already_Set_Key){ //キーがあったら
            NCobj.ObjectId = key;
            NCobj.FetchAsync((NCMBException e)=>{
                if(e==null){
                    //サーバーの自分のハイスコアを取得
                    int x = int.Parse(NCobj[KEY_HIGH_SCORE].ToString());
                    NCobj[KEY_PLAYER_NAME] = PlayerPrefs.GetString(KEY_MYNAME, "NoName");
                    //ハイスコアだったら更新
                    if(x<score){
                        NCobj[KEY_HIGH_SCORE] = score;
                    }
                    NCobj.SaveAsync();
                }else{
                    Debug.Log("error");
                }
            });
        }else{  //キーがなかったら
            //スコアを保存
            NCobj[KEY_PLAYER_NAME] = PlayerPrefs.GetString(KEY_MYNAME,"NoName");
            NCobj[KEY_HIGH_SCORE] = score;
            NCobj.SaveAsync((NCMBException e) =>
            {
                if (e == null)
                {
                    //keyを保存
                    PlayerPrefs.SetString(KEY_HIGH_SCORE, NCobj.ObjectId);
                }
            });
        }
    }

    /// <summary>
    /// 以下シーン遷移用
    /// 多分もっと短くできる
    /// </summary>

    public void GoMenu(){
        SCENE_FADE.GetComponent<FadeInSC>().F_isFadeIn = true;
        SCENE_FADE.transform.SetAsLastSibling();
        StartCoroutine(DelayMethod(TIME_DELAY_GOSCENE, () =>
        {
            SceneManager.LoadScene("Menu");
        }));
    }

    public void Retry(){
        SCENE_FADE.GetComponent<FadeInSC>().F_isFadeIn = true;
        SCENE_FADE.transform.SetAsLastSibling();
        StartCoroutine(DelayMethod(TIME_DELAY_GOSCENE, () =>
        {
            SceneManager.LoadScene("Game");
        }));
    }

    public void GoCollection(){
        SCENE_FADE.GetComponent<FadeInSC>().F_isFadeIn = true;
        SCENE_FADE.transform.SetAsLastSibling();
        StartCoroutine(DelayMethod(TIME_DELAY_GOSCENE, () =>
        {
            SceneManager.LoadScene("Collection");
        }));
    }

    public void GoRanking()
    {
        SCENE_FADE.GetComponent<FadeInSC>().F_isFadeIn = true;
        SCENE_FADE.transform.SetAsLastSibling();
        StartCoroutine(DelayMethod(TIME_DELAY_GOSCENE, () =>
        {
            SceneManager.LoadScene("Ranking");
        }));
    }

    public void GoOption()
    {
        SCENE_FADE.GetComponent<FadeInSC>().F_isFadeIn = true;
        SCENE_FADE.transform.SetAsLastSibling();
        StartCoroutine(DelayMethod(TIME_DELAY_GOSCENE, () =>
        {
            SceneManager.LoadScene("Option");
        }));
    }

    public void GoAsobikata()
    {
        SCENE_FADE.GetComponent<FadeInSC>().F_isFadeIn = true;
        SCENE_FADE.transform.SetAsLastSibling();
        StartCoroutine(DelayMethod(TIME_DELAY_GOSCENE, () =>
        {
            SceneManager.LoadScene("Asobikata");
        }));
    }

    public void GoEnd()
    {
        Application.Quit();
    }

    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}
