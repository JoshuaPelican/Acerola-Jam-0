using UnityEngine;

[CreateAssetMenu(fileName = "In Box Behaviour", menuName = "Monster Behaviours/In Box")]
public class InBoxBehaviour : MonsterBehaviour
{
    public float AngerPerUpdate = 0.01f;

    public override bool UpdateBehaviour(MonsterController monster)
    {
        if (!BoxController.IsInBox(monster.Pickup))
            return false;

        monster.AddAnger(AngerPerUpdate);
        return true;
    }

    public override void DoBehaviour(MonsterController monster)
    {
        monster.transform.position = Vector3.zero;
    }
}
