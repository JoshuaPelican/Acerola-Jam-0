using UnityEngine;

public abstract class MonsterBehaviour : ScriptableObject
{
    public bool IsActive = true;

    public abstract void UpdateBehaviour(MonsterController monster);

    public abstract void DoMonsterThing(MonsterController monster);
}
