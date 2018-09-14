using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using NCMB;

public class DispScores : MonoBehaviour {

    public GameObject SCENE_FADE;

    public Text VIEW_MYRANK;
    public Text VIEW_TOP5;
    public Text VIEW_RIVAL;

    private string KEY_MYNAME = "myname";
    private string KEY_MY_SCORE = "myScore";
    private string KEY_HIGH_SCORE = "highscore";
    private string KEY_PLAYER_NAME = "playername";

    int myScore=0;
    public int myRank = 0;
    public string topRankers = "";
    public string rivalRankers = "";

    private bool F_View_MyRank = true;
    private bool F_View_TOP5 = true;
    private bool F_View_RivalRank = true;

	void Start()
	{
        //自分のランクを取得
        NCMBQuery<NCMBObject> myRankquery = new NCMBQuery<NCMBObject>(KEY_HIGH_SCORE);
        myScore = PlayerPrefs.GetInt(KEY_MY_SCORE, 0);
        myRankquery.WhereGreaterThan(KEY_HIGH_SCORE, myScore);
        myRankquery.CountAsync((int count, NCMBException e) =>
        {
            if (e == null)
            {
                myRank = count + 1;
            }
            else
            {
                Debug.Log("error");
            }
        });

        //TOP5を取得
        NCMBQuery<NCMBObject> topRankquery = new NCMBQuery<NCMBObject>(KEY_HIGH_SCORE);
        topRankquery.OrderByDescending(KEY_HIGH_SCORE);
        topRankquery.Limit = 5;
        topRankquery.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        {
            if(e==null){
                int numR = 0;
                foreach (NCMBObject obj in objList){
                    numR++;
                    topRankers +=numR.ToString()+". "+ obj[KEY_PLAYER_NAME] + " -- " + obj[KEY_HIGH_SCORE]+"\n";
                }
            }else{
                Debug.Log("error");
            }
        });

        topRankquery = new NCMBQuery<NCMBObject>(KEY_HIGH_SCORE);
	}

    //周囲2位のスコアを取得
    //1位と2位の時は調整
    void RivalRank(int rank){
        NCMBQuery<NCMBObject> RivalRankquery = new NCMBQuery<NCMBObject>(KEY_HIGH_SCORE);
        int numSkip = rank - 3;
        if (numSkip < 0) numSkip = 0;

        RivalRankquery.OrderByDescending(KEY_HIGH_SCORE);
        RivalRankquery.Skip = numSkip;
        RivalRankquery.Limit = 5;
        RivalRankquery.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        {
            if (e == null)
            {
                int numR = 0 + numSkip;
                foreach(NCMBObject obj in objList){
                    numR++;
                    rivalRankers += numR.ToString() + ". " +obj[KEY_PLAYER_NAME] + " -- " + obj[KEY_HIGH_SCORE] + "\n";
                }
            }else{
                Debug.Log("error");
            }
        });
    }

	void Update()
	{
        //自分のランク
        if((myRank!=0) && F_View_MyRank){
            F_View_MyRank = false;
            VIEW_MYRANK.text = myRank.ToString()+". "+PlayerPrefs.GetString(KEY_MYNAME, "NoName")+" -- "+myScore;
            RivalRank(myRank);
        }
        //TOP5
        if((topRankers!="") && F_View_TOP5){
            F_View_TOP5 = false;
            VIEW_TOP5.text = topRankers;
        }
        //周囲2位
        if((rivalRankers!="") && F_View_RivalRank){
            F_View_RivalRank = false;
            VIEW_RIVAL.text = rivalRankers;
        }
	}
    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}
