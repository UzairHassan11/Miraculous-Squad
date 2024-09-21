using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepsManager : MonoBehaviour
{
    public GameObject leftFoot, rightFoot;

    public Transform leftFootPos, rightFootPos, container;

    public int initPoolSize = 10;

    List<GameObject> leftFeet = new List<GameObject>()
        , rightFeet = new List<GameObject>();
    int leftIndex = 0, rightIndex = 0;

    public bool enemy;

    // Start is called before the first frame update
    void Start()
    {
        PopulateLists();
    }


    void PopulateLists()
    {
        for (int i = 0; i < initPoolSize; i++)
        {
            GameObject obj = Instantiate(leftFoot, container);
            leftFeet.Add(obj);
            obj.SetActive(false);
            obj = Instantiate(rightFoot, container);
            rightFeet.Add(obj);
            obj.SetActive(false);
        }
    }

    public void LeftFoot()
    {
        //print("LeftFoot");
        GameObject obj = leftFeet[leftIndex++];
        obj.SetActive(true);
        obj.transform.position = leftFootPos.position;
        obj.transform.rotation = leftFootPos.rotation;
        if (leftIndex == initPoolSize)
            leftIndex = 0;
    }

    public void RightFoot()
    {
        //print("RightFoot");
        GameObject obj = rightFeet[rightIndex++];
        obj.SetActive(true);
        obj.transform.position = rightFootPos.position;
        obj.transform.rotation = rightFootPos.rotation;
        //obj.transform.eulerAngles = new Vector3(obj.transform.eulerAngles.x, transform.eul ;
        if (rightIndex == initPoolSize)
            rightIndex = 0;
    }
}
