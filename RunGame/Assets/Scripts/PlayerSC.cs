using System;
using System.Collections;
using UnityEngine;

public class PlayerSC : MonoBehaviour
{

    private float speed = 0f;  //移動速度
    private Rigidbody2D rig = null;
    private Material mat = null;
    public GameObject exprosion;
    public GameObject gameFacilitator;
    private int layer_Wall;

    //定数
    public float SPEED_BACK_CONS = 1.3f; //後ろに進む時の係数
    public float SPEED_GENERALLY = 6.0f; //普段のスピード
    public float SPEED_QUICK = 14.0f; //早い時のスピード 
    public float TIME_QUICK = 0.1f; //早くなれる時間
    public float TIME_DELAY_TRANSPARENT = 1.0f; //透明を再発動できるまでのdelay
    public float TIME_DELAY_CHANGE_COLOR = 0.5f; //色変えを再発動できるまでのdelay

    public Color COLOR_GENERALLY = new Color(0.0f, 0.0f, 1.0f, 1.0f);
    public Color COLOR_MUTEKI = new Color(0.0f, 0.0f, 0.0f, 1.0f);
    public Color COLOR_YELLOW = new Color(1.0f, 1.0f, 0.0f, 1.0f);
    public Color COLOR_GREEN = new Color(0.0f, 1.0f, 0.0f, 1.0f);
    public Color COLOR_BLUE = new Color(0.0f, 0.0f, 1.0f, 1.0f);
    private Color[] COLOR_COLORWALLS;
    private int[] LAYER_COLORWALLS;

    private int LAYER_PLAYER;
    private int LAYER_WALL_KASOKU;

    //フラグ
    public bool F_Transparent_Now = false; //今透明か
    public bool F_Transparent_Able = true; //透明になれるか
    public bool F_Change_Color_Able = true; //色を変えられるか
    public int F_Change_Color_Now = 0; //今何色か　0:青 1:緑 2:赤


	private void Start()
	{
        layer_Wall = LayerMask.NameToLayer("MoveWall_KasokuNuke");
        rig = this.gameObject.GetComponent<Rigidbody2D>();
        mat = this.GetComponent<Renderer>().material;
        mat.color = COLOR_BLUE; //初期色

        COLOR_COLORWALLS=new Color[] {COLOR_BLUE,COLOR_YELLOW,COLOR_GREEN};
        LAYER_COLORWALLS = new int[] {LayerMask.NameToLayer("MoveWall_Blue"),
                                  LayerMask.NameToLayer("MoveWall_Red"),
                                  LayerMask.NameToLayer("MoveWall_Green")};
        LAYER_PLAYER = LayerMask.NameToLayer("Player");

        //もう一度を押した時の為に、当たり判定を初期化させる
        Physics2D.IgnoreLayerCollision(LAYER_COLORWALLS[0],
                                       LAYER_PLAYER,
                                       true); //初期レイヤー間当たり判定

        Physics2D.IgnoreLayerCollision(LAYER_COLORWALLS[1],
                                       LAYER_PLAYER,
                                       false); //初期レイヤー間当たり判定
        
        Physics2D.IgnoreLayerCollision(LAYER_COLORWALLS[2],
                                       LAYER_PLAYER,
                                       false); //初期レイヤー間当たり判定

        Physics2D.IgnoreLayerCollision(LAYER_PLAYER, layer_Wall, false);//初期レイヤー間当たり判定
        
        speed = SPEED_GENERALLY; //初期スピード
	}

	private void Update()
	{
        //動く
        Move(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //入力確認
        if(Input.GetKeyDown(KeyCode.Z) && F_Transparent_Able){
            Transparent();
        }
        if(Input.GetKeyDown(KeyCode.X) && F_Change_Color_Able && !F_Transparent_Now){
            ChangeColor();
        }
	}

	public void Move(float x, float y)
    {
        if (x < 0)
        {
            x *= SPEED_BACK_CONS;
        }

        rig.velocity = new Vector2(x * speed, y * speed);
    }

    public void Transparent(){
        F_Transparent_Now = true;
        F_Transparent_Able = false;

        //無敵andすり抜け
        Physics2D.IgnoreLayerCollision(LAYER_PLAYER, layer_Wall, true);

        //早くする
        speed = SPEED_QUICK;

        //色を変える
        if (mat.color.a > 0)
        {
            mat.color -= COLOR_MUTEKI;
        }

        //スピードと色を戻す
        StartCoroutine(DelayMethod(TIME_QUICK, () =>
        {
            speed = SPEED_GENERALLY;
            mat.color += COLOR_MUTEKI;
            Physics2D.IgnoreLayerCollision(LAYER_PLAYER, layer_Wall, false);
            F_Transparent_Now = false;
        }));

        //Ableフラグを戻す
        StartCoroutine(DelayMethod(TIME_DELAY_TRANSPARENT, () =>
        {
            F_Transparent_Able = true;
        }));
    }

    void ChangeColor(){
        F_Change_Color_Able = false;
        //連続で変えられないように
        StartCoroutine(DelayMethod(TIME_DELAY_CHANGE_COLOR, () =>
        {
            F_Change_Color_Able = true;
        }));
        //判定を戻す
        Physics2D.IgnoreLayerCollision(LAYER_COLORWALLS[F_Change_Color_Now],
                                       LAYER_PLAYER,
                                       false);
        F_Change_Color_Now++;
        if(F_Change_Color_Now>=COLOR_COLORWALLS.Length){
            F_Change_Color_Now = 0;
        }
        //色を変える
        mat.color = COLOR_COLORWALLS[F_Change_Color_Now];
        //判定を消す
        Physics2D.IgnoreLayerCollision(LAYER_COLORWALLS[F_Change_Color_Now],
                                       LAYER_PLAYER,
                                       true);
    }

	void OnCollisionEnter2D(Collision2D c)
	{
        //ダメージ壁に当たると死
        if(LayerMask.LayerToName(c.gameObject.layer)=="MoveWall_Damage"){
            Instantiate(exprosion, transform.position, transform.rotation);
            Destroy(this.gameObject);
            gameFacilitator.GetComponent<GameFacilitatorSC>().GameOver();
        }
	}

	private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}
