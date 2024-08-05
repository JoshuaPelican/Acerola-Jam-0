using System.Threading;
using UnityEngine;

[RequireComponent (typeof(Pickup), typeof(Collider), typeof(Rigidbody))]
public class MonsterController : MonoBehaviour
{
    [SerializeField] float BehaviourInterval = 5f;
    [SerializeField] float MaxAnger = 1f;
    [Range(0, 20)]
    [SerializeField] int AngerMultiplier = 1;
    [Space]
    [SerializeField] MonsterBehaviour[] Behaviours;

    public float Anger { get; private set; } = 0;
    bool IsInterval => interval >= BehaviourInterval;
    float interval = 0;
    public bool IsMonster { get; private set; } = false;

    public Pickup Pickup { get; private set; }
    public Monster Monster { get; private set; }

    private void Awake()
    {
        Pickup = GetComponent<Pickup>();
    }

    private void Update()
    {
        if (IsMonster)
            return;

        AddInterval(Time.deltaTime);

        if (!IsInterval)
            return;

        DoBehaviours();
    }

    public void AddInterval(float amount)
    {
        interval += amount;
    }

    void DoBehaviours()
    {
        interval = 0;

        foreach (var behaviour in Behaviours)
        {
            if (!behaviour.IsActive)
                continue;

            if(behaviour.UpdateBehaviour(this))
                Debug.Log($"{name} {behaviour.name}...");

            if (behaviour.Positive)
                continue;

            float rand = Random.Range(0f, 1f);

            if (Anger >= MaxAnger)
            {
                BecomeMonster();
            }
            else if (rand < Anger)
            {
                DoBehaviour(behaviour);
            }
        }
    }

    public void AddAnger(float amount)
    {
        Anger = Mathf.Clamp(Anger + amount * AngerMultiplier, 0, MaxAnger);
        
        (Pickup.Renderer as SpriteRenderer).color = new Color(1, 1 - Anger / MaxAnger, 1 - Anger / MaxAnger);
    }

    public void ResetAnger()
    {
        Anger = 0;
    }

    void DoBehaviour(MonsterBehaviour lastBehaviour)
    {
        lastBehaviour.DoBehaviour(this);
    }

    void BecomeMonster()
    {
        Debug.Log($"{this.name} has become a Monster!");

        IsMonster = true;
        Pickup.IsInteractable = false;
        ResetAnger();
        Pickup.gameObject.SetActive(false);
        Monster.gameObject.SetActive(true);
    }
}
