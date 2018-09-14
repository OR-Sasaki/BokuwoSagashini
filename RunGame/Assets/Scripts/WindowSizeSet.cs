using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowSizeSet : MonoBehaviour {

    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        //画面サイズの固定
        Screen.SetResolution(1024,576, false, 60);
    }
}
