using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinPanelBtn : MonoBehaviour
{
    [SerializeField] private List<GameObject> objects;
    [SerializeField] private bool turn;
    [SerializeField] private Button setbtn;

    [Space]
    [SerializeField] private float delay;
    [SerializeField] private Animator anim;

    private void Start()
    {
        setbtn.onClick.AddListener(() =>{

            if (!turn)
            {
                if (anim) anim.SetTrigger("exit");
                Invoke("Turn", delay);
            }
            else Turn();

        });
    }

    public void Turn()
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(turn);
        }
    }

}
