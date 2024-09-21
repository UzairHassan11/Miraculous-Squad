using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvents : MonoBehaviour
{
    public bool turnOffCollider, tunSelftOff;

    public List<string> _tags;

    public UnityEvent enterEvents, exitEvents;


    private void OnTriggerEnter(Collider other)
    {
        if (_tags.Contains(other.tag))
        {
            if (turnOffCollider)
                other.gameObject.SetActive(false);

            if (tunSelftOff)
                gameObject.SetActive(false);

            if (enterEvents != null)
                enterEvents.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_tags.Contains(other.tag))
        {
            if (turnOffCollider)
                other.gameObject.SetActive(false);

            if (exitEvents != null)
                exitEvents.Invoke();
        }
    }
}
