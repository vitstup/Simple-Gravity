using System.Collections;
using UnityEngine;

public class EdgeAnimation : MonoBehaviour
{
    private CharaController charaCont;
    [SerializeField] private float animDuration = 0.18f;
    private Coroutine running;

    private void Awake()
    {
        charaCont = GetComponentInParent<CharaController>();
    }

    private void OnEnable()
    {
        charaCont.EdgeEvent += OnEdge;
    }

    private void OnDisable()
    {
        charaCont.EdgeEvent -= OnEdge;
    }

    private void OnEdge(Vector2 previousP, Quaternion previousR, Vector2 newP, Quaternion newR)
    {
        // стопаем предыдущую анимацию
        if (running != null)
            StopCoroutine(running);

        running = StartCoroutine(AnimateFromTo(previousP, previousR, newP, newR, animDuration));
    }

    private IEnumerator AnimateFromTo(Vector2 prevWorldPos, Quaternion prevWorldRot, Vector2 newWorldPos, Quaternion newWorldRot, float duration)
    {
        Transform parent = transform.parent;
        if (parent == null)
            yield break;

        Vector3 startLocalPos = parent.InverseTransformPoint(prevWorldPos);
        Quaternion startLocalRot = Quaternion.Inverse(parent.rotation) * prevWorldRot;

        Vector3 targetLocalPos = parent.InverseTransformPoint(newWorldPos);
        Quaternion targetLocalRot = Quaternion.Inverse(parent.rotation) * newWorldRot;

        Vector3 finalLocalPos = Vector3.zero;
        Quaternion finalLocalRot = Quaternion.identity;

        transform.localPosition = startLocalPos;
        transform.localRotation = startLocalRot;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / duration);

            float eased = k; 

            transform.localPosition = Vector3.Lerp(startLocalPos, targetLocalPos, eased);
            transform.localRotation = Quaternion.Slerp(startLocalRot, targetLocalRot, eased);

            yield return null;
        }

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        running = null;
    }
}