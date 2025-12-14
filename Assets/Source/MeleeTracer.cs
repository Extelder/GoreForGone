using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float dmg, Vector3 hitPoint, Vector3 hitNormal, GameObject instigator);
}

public class MeleeTracer : MonoBehaviour
{
    [SerializeField] private GameObject _testHit;

    [Header("Blade points")] public Transform bladeBase;
    public Transform bladeTip;

    [Header("Trace settings")] [Range(2, 12)]
    public int segments = 6; // сколько точек вдоль клинка

    public float radius = 0.08f; // "толщина" хитбокса
    public LayerMask hitMask;
    public QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Ignore;

    [Header("Damage")] public float damage = 25f;
    public GameObject instigator; // обычно player

    bool active;
    Vector3[] prevPoints;
    readonly HashSet<Collider> hitThisSwing = new HashSet<Collider>();

    void Awake()
    {
        prevPoints = new Vector3[Mathf.Max(segments, 2)];
    }

    // Animation Event
    public void BeginSwing()
    {
        active = true;
        hitThisSwing.Clear();
        CacheCurrentPoints(prevPoints);
    }

    // Animation Event
    public void EndSwing()
    {
        active = false;
    }

    void Update()
    {
        if (!active) return;

        // Текущие точки клинка
        Vector3[] currPoints = GetCurrentPointsTemp();

        // Свип для каждой точки
        for (int i = 0; i < currPoints.Length; i++)
        {
            Vector3 from = prevPoints[i];
            Vector3 to = currPoints[i];
            Vector3 delta = to - from;
            float dist = delta.magnitude;

            if (dist > 0.0005f)
            {
                Vector3 dir = delta / dist;
                var hits = Physics.SphereCastAll(from, radius, dir, dist, hitMask, triggerInteraction);

                foreach (var h in hits)
                {
                    var col = h.collider;
                    if (col == null) continue;
                    if (hitThisSwing.Contains(col)) continue;

                    hitThisSwing.Add(col);

                    Instantiate(_testHit, h.point, Quaternion.identity);
                    var dmgable = col.GetComponentInParent<IDamageable>();
                    if (dmgable != null)
                        dmgable.TakeDamage(damage, h.point, h.normal, instigator != null ? instigator : gameObject);
                }
            }
            else
            {
                // если движения почти нет, можно OverlapSphere, чтобы не "дырявило" на паузе
                var cols = Physics.OverlapSphere(to, radius, hitMask, triggerInteraction);
                foreach (var col in cols)
                {
                    if (hitThisSwing.Contains(col)) continue;
                    hitThisSwing.Add(col);

                    Instantiate(_testHit, to, Quaternion.identity);
                    var dmgable = col.GetComponentInParent<IDamageable>();
                    if (dmgable != null)
                        dmgable.TakeDamage(damage, to, (to - transform.position).normalized,
                            instigator != null ? instigator : gameObject);
                }
            }
        }

        // Сохраняем для следующего кадра
        for (int i = 0; i < prevPoints.Length; i++)
            prevPoints[i] = currPoints[i];
    }

    Vector3[] GetCurrentPointsTemp()
    {
        // маленькая оптимизация: не аллоцировать каждый кадр — сделай поле currPoints.
        // для простоты оставлю так:
        var pts = new Vector3[Mathf.Max(segments, 2)];
        CacheCurrentPoints(pts);
        return pts;
    }

    void CacheCurrentPoints(Vector3[] buffer)
    {
        int n = buffer.Length;
        Vector3 a = bladeBase.position;
        Vector3 b = bladeTip.position;

        for (int i = 0; i < n; i++)
        {
            float t = (n == 1) ? 0f : (float) i / (n - 1);
            buffer[i] = Vector3.Lerp(a, b, t);
        }
    }
}