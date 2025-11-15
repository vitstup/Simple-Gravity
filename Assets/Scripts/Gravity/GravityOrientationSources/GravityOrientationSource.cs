using UnityEngine;

public abstract class GravityOrientationSource : MonoBehaviour
{
    public abstract GravityResult GetOrientation(Collider2D objectCollider);
}

public struct GravityResult
{
    public Quaternion rotation;
    public Vector2 direction;
    public Vector2 normal;
}