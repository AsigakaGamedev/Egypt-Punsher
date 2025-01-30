using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacter : MonoBehaviour
{
    [SerializeField] private Character character;
    [SerializeField] private AIBehvaiour[] allBehaviours;

    [Space]
    [ReadOnly, SerializeField] private AIBehvaiour curBehaviour;

    private void Start()
    {
        character.Init();

        foreach (var behaviour in allBehaviours)
        {
            behaviour.OnInit(character);
        }
    }

    private void Update()
    {
        foreach(var behaviour in allBehaviours)
        {
            if (behaviour.OnCheckEnter())
            {
                if (behaviour != curBehaviour)
                {
                    if (curBehaviour)
                    {
                        curBehaviour.OnExitLogic();
                    }

                    curBehaviour = behaviour;
                    curBehaviour.OnEnterLogic();
                }
                break;
            }
        }

        if (curBehaviour != null)
        {
            curBehaviour.OnUpdateLogic();
        }
    }
}
