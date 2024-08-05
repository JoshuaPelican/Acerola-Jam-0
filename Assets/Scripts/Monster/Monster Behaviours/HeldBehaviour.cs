using UnityEngine;

[CreateAssetMenu(fileName = "Held Behaviour", menuName = "Monster Behaviours/Held")]
public class HeldBehaviour : MonsterBehaviour
{
    public float AngerPerUpdate = 0.01f;

    public override bool UpdateBehaviour(MonsterController monster)
    {
        if (!monster.Pickup.IsBeingHeld)
        {
            if (Opposite)
            {
                monster.AddAnger(AngerPerUpdate);
                return true;
            }

            return false;
        }

        if (Opposite)
            return false;

        monster.AddAnger(AngerPerUpdate);
        return true;
    }

    public override void DoBehaviour(MonsterController monster)
    {
        monster.Pickup.ForceRelease();
    }
}
