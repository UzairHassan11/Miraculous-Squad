using UnityEngine;
using DG.Tweening;

public class ShootingRangeVisualiser : MonoBehaviour
{
    [SerializeField] float scaleAnimDuration = 0.5f;
    [SerializeField] Transform visualiserObject;
    [SerializeField] Vector2 minMaxScale;
    [SerializeField] GameObject scalingUpIdentifier;

    Transform player;

    private void Start()
    {
        player = PlayerManager.instance.playerController.mTransform;
    }

    private void Update()
    {
        if(player)
            transform.position = player.position;
    }

    public void ChangeScale(float t)
    {
        scalingUpIdentifier.SetActive(false);
        scalingUpIdentifier.SetActive(true);
        float scale = Mathf.Lerp(minMaxScale.x, minMaxScale.y, t);
        visualiserObject.DOScale(scale, scaleAnimDuration).SetEase(Ease.Linear);
    }

    public void SetToMaxScale()
    {
        ChangeScale(minMaxScale.y);
    }

    public void SetToMinScale()
    {
        visualiserObject.localScale = Vector3.zero;
    }
}