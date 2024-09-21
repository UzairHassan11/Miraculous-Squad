using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField] bool isPlayer;
    ChasingEnemy chasingEnemy;

    public void PlayFootStep()
    {
        if (isPlayer)
            SoundManager.Instance.PlaySound(ClipName.FootStep);
    }

    public void GiveDamage()
    {
        if (!chasingEnemy)
            chasingEnemy = GetComponentInParent<ChasingEnemy>();

        chasingEnemy.GiveDamage();
    }
}