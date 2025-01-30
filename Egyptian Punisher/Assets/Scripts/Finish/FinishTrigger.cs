using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    private UIManager uiManager;

    private void Start()
    {
        uiManager = ServiceLocator.GetService<UIManager>();    
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Trigger 1");
        if (other.CompareTag("Player"))
        {
          //  if (ScoreManager.instance.Score > ScoreManager.instance.curLvl * 10)
          //  {
             //   uiManager.ChangeScreen("Win");
             other.GetComponent<Rigidbody>().velocity = Vector3.zero;
            
                ScoreManager.instance.FinishGame();
            other.GetComponent<PlayerMovement>().isWalk = false;
            other.GetComponent<Animator>().SetTrigger("IdleTr");
        }
    }
}
