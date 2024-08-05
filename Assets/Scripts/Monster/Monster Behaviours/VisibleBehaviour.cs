using UnityEngine;

[CreateAssetMenu(fileName = "Visible Behaviour", menuName = "Monster Behaviours/Visible")]
public class VisibleBehaviour : MonsterBehaviour
{
    public float AngerPerUpdate = 0.01f;

    public override bool UpdateBehaviour(MonsterController monster)
    {
        if (!monster.Pickup.Renderer.isVisible)
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
        monster.transform.position = Vector3.zero;
    }
}
