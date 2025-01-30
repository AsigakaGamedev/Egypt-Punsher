using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIBehvaiour : MonoBehaviour
{
    protected Character character;

    public virtual void OnInit(Character character) { this.character = character; }
    public virtual bool OnCheckEnter() { return true; }
    public virtual void OnEnterLogic() { }
    public virtual void OnExitLogic() { }
    public virtual void OnUpdateLogic() { }
}
