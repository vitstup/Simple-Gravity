using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class GravityConstraint : MonoBehaviour
{
    public Action<Quaternion, Quaternion> GravityFaceChangedEvent;

    [SerializeField] private GravityOrientationSource gravitySource;

    [SerializeField] private float gravityStrength;

    private Rigidbody2D rb;

    private Collider2D oc;

    private Quaternion previousFrameRotation;

    public GravityResult gravityResult { get; private set; }

    private float additionalRelativeGravityForce;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        oc = GetComponent<Collider2D>();

        rb.gravityScale = 0;

        gravityResult = gravitySource.GetOrientation(oc);

        transform.rotation = gravityResult.rotation;

        previousFrameRotation = gravityResult.rotation;
    }

    private void FixedUpdate()
    {
        gravityResult = gravitySource.GetOrientation(oc);

        transform.rotation = gravityResult.rotation;

        rb.AddRelativeForceY(-gravityStrength + additionalRelativeGravityForce);

        if (!gravityResult.rotation.Equals(previousFrameRotation))
            FaceChanged();

        previousFrameRotation = gravityResult.rotation;

        additionalRelativeGravityForce = 0;
    }

    private void FaceChanged()
    {
        GravityFaceChangedEvent?.Invoke(previousFrameRotation, transform.rotation);

        // выключаем все примененные силы
        rb.angularVelocity = 0f;
        rb.linearVelocity = Vector3.zero;
    }

    public void AddForceRelativeToGravity(float force)
    {
        additionalRelativeGravityForce += force;
    }
}