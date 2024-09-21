using UnityEngine;
using DG.Tweening;

public class EnemyIndicator : MonoBehaviour
{
    Transform mTransform;

    [SerializeField] MeshRenderer mesh;

    Material meshMat;

    float originalScale;

    Enemy enemy;

    public static EnemyIndicator instance;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        mTransform = transform;
        meshMat = mesh.material;
        originalScale = mTransform.GetChild(0).localScale.x;
    }

    public void PlaceMe(Enemy _enemy)
    {
        if (_enemy == enemy)
        {
            MakeItRed(enemy.IsInPlayerAttackRange);
            return;
        }
        enemy = _enemy;
        mTransform.GetChild(0).localScale = Vector3.zero;
        mTransform.GetChild(0).DOScale(originalScale, 1).SetEase(Ease.OutBack);
        mTransform.SetParent(_enemy.transform);
        mTransform.localPosition = Vector3.zero;
        MakeItRed(_enemy.IsInPlayerAttackRange);
    }

    void MakeItRed(bool state)
    {
        meshMat.color = state ? Color.red : Color.gray;
    }

    public void SetVisibility(bool state)
    {
        gameObject.SetActive(state);
    }

    public void Unparent()
    {
        transform.SetParent(null);
    }
}