using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HandTut : MonoBehaviour
{
    public GameObject hand;
    private Image handImage;
    private bool filter = false;
    // Start is called before the first frame update
    void Start()
    {
        handImage = hand.GetComponent<Image>();
        hand.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            hand.transform.position = Input.mousePosition;
            hand.SetActive(true);
        }
        else
        {
            hand.transform.position = Input.mousePosition;
            // if (!filter)
            //     StartCoroutine(DisableHand());
            // MMVibrationManager.Haptic(HapticTypes.Selection);
        }
        //else if (Input.GetMouseButtonUp(0))
        //{
        //    StartCoroutine(DisableHand());
        //}
    }

    private IEnumerator DisableHand()
    {
        filter = true;
        yield return new WaitForSeconds(0f);
        hand.SetActive(false);
        filter = false;
    }
}