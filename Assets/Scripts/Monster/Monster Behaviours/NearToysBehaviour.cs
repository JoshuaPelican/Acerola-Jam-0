using UnityEngine;

[CreateAssetMenu(fileName = "Near Toys Behaviour", menuName = "Monster Behaviours/Near Toys")]
public class NearToysBehaviour : MonsterBehaviour
{
    [SerializeField] float nearRadius = 3;
    [SerializeField] float angerPerNearby = 0.01f;
    [Space]
    [SerializeField] float explosionForce = 100f;
    Collider[] nearby = new Collider[10];

    public override bool UpdateBehaviour(MonsterController monster)
    {
        int nearbyCount = Physics.OverlapSphereNonAlloc(monster.transform.position, nearRadius, nearby, LayerMask.GetMask("Interactable"));
        Debug.Log(nearbyCount);

        if (nearbyCount == 0)
        {
            if (Opposite)
            {
                monster.AddAnger(angerPerNearby * nearbyCount);
                return true;
            }

            return false;
        }

        if (Opposite)
            return false;

        monster.AddAnger(angerPerNearby * nearbyCount);
        return true;
    }

    public override void DoBehaviour(MonsterController monster)
    {
        foreach(Collider collider in nearby)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            rb.AddExplosionForce(explosionForce * monster.Anger, monster.transform.position, nearRadius);
        }
    }


}
