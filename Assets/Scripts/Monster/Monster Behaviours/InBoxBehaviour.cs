using UnityEngine;

[CreateAssetMenu(fileName = "In Box Behaviour", menuName = "Monster Behaviours/In Box")]
public class InBoxBehaviour : MonsterBehaviour
{
    public float AngerPerSecond = 0.01f;

    public override void UpdateBehaviour(MonsterController monster)
    {
        if (!BoxController.IsInBox(monster.Pickup))
            return;

        monster.AddAnger(AngerPerSecond * Time.deltaTime);

        Debug.Log($"{monster.name} {name}...");
    }

    public override void DoMonsterThing(MonsterController monster)
    {
        monster.ResetAnger();
        monster.transform.position = Vector3.zero;
    }
}
