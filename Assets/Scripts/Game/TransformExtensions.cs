using UnityEngine;

public static class TransformExtensions 
{
    public static void LookTowards(this Transform t, Transform target)
    {
        t.LookAt(target);
        Vector3 localEulerAngles = t.localEulerAngles;
        t.localEulerAngles = new Vector3(0, localEulerAngles.y, 0);
    }

    public static void AddTo_LocalEulerY(this Transform t, float angle)
    {
        Vector3 localEulerAngles = t.localEulerAngles;
        t.localEulerAngles = new Vector3(0, localEulerAngles.y + angle, 0);
    }

    public static void Set_LocalEulerY(this Transform t, float angle)
    {
        //Vector3 localEulerAngles = t.localEulerAngles;
        t.localEulerAngles = new Vector3(0, angle, 0);
    }

    public static void MultiplyLocalScale(this Transform t, float multiple)
    {
        Vector3 localScale = t.localScale;
        localScale.x *= multiple;
        localScale.y *= multiple;
        localScale.z *= multiple;
        t.localScale = localScale;
    }

    public static void SetLocalScale(this Transform t, float scale)
    {
        t.localScale = new Vector3(scale,scale, scale);
    }
}