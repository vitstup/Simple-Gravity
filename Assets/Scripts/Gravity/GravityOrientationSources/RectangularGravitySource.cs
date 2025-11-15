using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RectangularGravitySource : GravityOrientationSource
{
    private Collider2D boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<Collider2D>();
    }

    public override GravityResult GetOrientation(Collider2D objectCollider)
    {
        var ob = objectCollider.bounds;
        var b = boxCollider.bounds;

        Vector2 direction;
        Vector2 normalOfObject;
        Vector2 normalCenter;
        float angle;

        // Проверяем вертикальные грани
        if (ob.max.x < b.min.x)
        {
            // Левая вертикальная грань
            direction = new Vector2(0, 1);
            angle = 90f;
            normalOfObject = new Vector2(b.min.x - ob.size.y * 0.51f, 0f);
            normalCenter = new Vector2(b.min.x, b.center.y); 
        }
        else if (ob.min.x > b.max.x)
        {
            // Правая вертикальная грань
            direction = new Vector2(0, -1);
            angle = -90f;
            normalOfObject = new Vector2(b.max.x + ob.size.y * 0.51f, 0f);
            normalCenter = new Vector2(b.max.x, b.center.y);
        }
        // Проверяем горизонтальные грани
        else if (ob.max.y > b.max.y)
        {
            // Верхняя грань
            direction = new Vector2(1, 0);
            angle = 0f;
            normalOfObject = new Vector2(0, b.max.y + ob.size.y * 0.51f);
            normalCenter = new Vector2(b.center.x, b.max.y);
        }
        else
        {
            // Нижняя грань
            direction = new Vector2(-1, 0);
            angle = 180f;
            normalOfObject = new Vector2(0, b.min.y - ob.size.y * 0.51f);
            normalCenter = new Vector2(b.center.x, b.min.y);
        }

        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        return new GravityResult
        {
            rotation = rotation,
            direction = direction,
            normal = new Normal { axis = normalOfObject, center = normalCenter }
        };
    }

}