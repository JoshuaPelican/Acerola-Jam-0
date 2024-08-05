using UnityEngine;

public abstract class MonsterBehaviour : ScriptableObject
{
    public bool IsActive = true;
    [Space]
    public bool Positive = false;
    public bool Opposite = false;

    public abstract bool UpdateBehaviour(MonsterController monster);

    public abstract void DoBehaviour(MonsterController monster);
}
