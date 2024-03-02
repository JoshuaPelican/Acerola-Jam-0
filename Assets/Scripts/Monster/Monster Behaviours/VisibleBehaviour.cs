using UnityEngine;

[CreateAssetMenu(fileName = "Visible Behaviour", menuName = "Monster Behaviours/Visible")]
public class VisibleBehaviour : MonsterBehaviour
{
    public float AngerPerSecond = 0.01f;

    public override void UpdateBehaviour(MonsterController monster)
    {
        if (!monster.Renderer.isVisible)
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
