using UnityEngine;

[CreateAssetMenu(fileName = "Held Behaviour", menuName = "Monster Behaviours/Held")]
public class HeldBehaviour : MonsterBehaviour
{
    public float AngerPerSecond = 0.01f;

    public override void UpdateBehaviour(MonsterController monster)
    {
        if (!monster.Pickup.IsBeingHeld)
            return;

        monster.AddAnger(AngerPerSecond * Time.deltaTime);

        Debug.Log($"{monster.name} {name}...");
    }

    public override void DoMonsterThing(MonsterController monster)
    {
        monster.ResetAnger();
        monster.Pickup.IsInteractable = false;
        monster.Pickup.ForceRelease();
    }
}
