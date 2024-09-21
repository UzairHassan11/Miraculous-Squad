using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator _animator;

    public void SetBool(string str, bool val)
    {
        _animator.SetBool(str, val);
    }
    
    public void SetTrigger(string str)
    {
        _animator.SetTrigger(str);
    }
    
    public void SetFloat(string str, float val)
    {
        _animator.SetFloat(str, val);
    }
}
