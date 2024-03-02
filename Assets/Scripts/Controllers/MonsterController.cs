using System.Threading;
using UnityEngine;

[RequireComponent (typeof(Pickup), typeof(Collider), typeof(Rigidbody))]
public class MonsterController : MonoBehaviour
{
    [SerializeField] float MaxAnger = 1;
    [Space]
    [SerializeField] MonsterBehaviour[] Behaviours;

    public Pickup Pickup { get; private set; }
    public float Anger { get; private set; } = 0;

    public Renderer Renderer { get; private set; }

    private void Awake()
    {
        Pickup = GetComponent<Pickup>();
        Renderer = Pickup.GetComponent<Renderer>();
    }

    private void Update()
    {
        foreach (var behaviour in Behaviours)
        {
            if (!behaviour.IsActive)
                continue;

            behaviour.UpdateBehaviour(this);
            if (Anger >= MaxAnger)
            {
                DoMonsterThing(behaviour);
                return;
            }
        }
    }

    public void AddAnger(float anger)
    {
        Anger = Mathf.Clamp(Anger + anger, 0, MaxAnger);
    }

    public void ResetAnger()
    {
        Anger = 0;
    }

    void DoMonsterThing(MonsterBehaviour lastBehaviour)
    {
        lastBehaviour.DoMonsterThing(this);
    }
}
