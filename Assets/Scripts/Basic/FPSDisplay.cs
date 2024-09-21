/*
 * @Author: CuongNH
 * @Description: Dislay fps
 * */

using UnityEngine;
using System.Collections;

public class FPSDisplay : MonoBehaviour {

    public UnityEngine.UI.Text textui;
    float deltaTime = 0.0f;
    float fps;
    float msec;

    // Update is called once per frame
    IEnumerator Start()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

        while (true)
        {
            msec = deltaTime * 1000.0f;
            fps = 1.0f / deltaTime;
            //textui.text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            textui.text = "FPS: " + string.Format("{1:0.} fps ({0:0.0} ms)", msec, fps);
            yield return new WaitForSeconds(0.5f);
        }
    }

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }
}
