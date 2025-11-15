using System;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.UI.Image;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(GravityConstraint))]
public class CharaController : MonoBehaviour
{
    [SerializeField] private InputHandler inputHandler;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;

    [SerializeField] private float groundCheckDistance = 0.05f;
    [SerializeField] private LayerMask groundMask;

    private Rigidbody2D rb;

    private Collider2D oc;

    private GravityConstraint constraint;

    private bool IsGrounded
    {
        get
        {
            // получаем локальные точки нижних углов коллайдера
            Vector3 localBottomLeft = new Vector3(-oc.bounds.extents.x, -oc.bounds.extents.y, 0) * 1.05f;
            Vector3 localBottomRight = new Vector3(oc.bounds.extents.x, -oc.bounds.extents.y, 0) * 1.05f;

            // преобразуем в мировые координаты с учётом поворота объекта
            Vector3 worldBottomLeft = oc.transform.TransformPoint(localBottomLeft);
            Vector3 worldBottomRight = oc.transform.TransformPoint(localBottomRight);

            return GroundCast(worldBottomLeft).collider != null || GroundCast(worldBottomRight).collider != null;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        oc = GetComponent<Collider2D>();

        constraint = GetComponent<GravityConstraint>();
    }

    private void OnEnable()
    {
        inputHandler.MoveActionEvent += (v) => Move(v);
        inputHandler.JumpActionEvent += () => Jump();
        constraint.GravityFaceChangedEvent += (p, n) => OnFaceChanged();
    }

    private void Move(float moveVector)
    {
        if (IsGrounded)
            transform.position += (Vector3)(constraint.gravityResult.direction * movementSpeed * moveVector * Time.deltaTime);
    }

    private void Jump()
    {
        if (IsGrounded)
            constraint.AddForceRelativeToGravity(jumpForce);
    }

    private void OnFaceChanged()
    {
        if (Mathf.Abs(constraint.gravityResult.normal.axis.x) > 0.01f)
        {
            //Debug.Log($"X {constraint.gravityResult.normal.x}");
            transform.position = new Vector2(constraint.gravityResult.normal.axis.x, transform.position.y);
            float sign = Mathf.Sign(constraint.gravityResult.normal.center.y - transform.position.y);
            transform.position = new Vector2(transform.position.x, transform.position.y + oc.bounds.size.x * sign);
        }
        if (Mathf.Abs(constraint.gravityResult.normal.axis.y) > 0.01f)
        {
            //Debug.Log($"Y {constraint.gravityResult.normal.y}");
            transform.position = new Vector2(transform.position.x, constraint.gravityResult.normal.axis.y);
            float sign = Mathf.Sign(constraint.gravityResult.normal.center.x - transform.position.x);
            transform.position = new Vector2(transform.position.x + oc.bounds.size.x * sign, transform.position.y);
        }
    }

    private RaycastHit2D GroundCast(Vector2 origin)
    {
        return Physics2D.Raycast(
            origin,
            transform.up * -1f,
            groundCheckDistance,
            groundMask
        );
    }

    private void OnDisable()
    {
        inputHandler.MoveActionEvent -= (v) => Move(v);
        inputHandler.JumpActionEvent -= () => Jump();
        constraint.GravityFaceChangedEvent -= (p, n) => OnFaceChanged();
    }
}