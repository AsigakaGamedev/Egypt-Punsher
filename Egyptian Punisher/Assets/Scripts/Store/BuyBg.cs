using System.Collections;
using System.Collections.Generic;
using TMPro;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuyBg : MonoBehaviour
{
    [SerializeField] private string bgPlayerPref;
    [SerializeField] private int ProductPrice= 2500;
    [SerializeField] private int bgIndex;
    [SerializeField] private Button useBtn;
    [SerializeField] private Button buyBtn;
    [SerializeField] private TextMeshProUGUI selectText;

    [Space]
    [SerializeField] private string curProductType;

    [Space]
    [SerializeField] bool isDefault;

    [Space]
    [SerializeField] private Button equiped;
    [SerializeField] private GameObject locekd;

    [SerializeField] private GameObject notEnough;

    private ScoreManager scoreManager;


   private void Awake()
   {
     //  notEnough.SetActive(true);
   
       if (isDefault)
       {
           PlayerPrefs.SetInt(bgPlayerPref, 1);
           SelectItem();
           notEnough.SetActive(false);
       }

        buyBtn.onClick.AddListener(BuyBG);

    }

    private void OnEnable() 
    {


        print("Store");
        
        useBtn.onClick.AddListener(SelectItem);
        equiped.onClick.AddListener(UnSelectItem);



        scoreManager = ScoreManager.instance;
   // 
   //  notEnough.SetActive(true);
   // 
   //  if (isDefault) 
   //  {
   //      PlayerPrefs.SetInt(bgPlayerPref, 1);
   //      SelectItem();
   //      notEnough.SetActive(false);
   //  } 
     
        int hasBG = PlayerPrefs.GetInt(bgPlayerPref, 0);

        if (hasBG == 1) 
        {
            //  buyBtn.gameObject.SetActive(false);
              useBtn.gameObject.SetActive(true);
            buyBtn.gameObject.SetActive(false);

            notEnough.SetActive(false);
            locekd.gameObject.SetActive(false);
        }
        else
        {
            buyBtn.gameObject.SetActive(true);
            locekd.gameObject.SetActive(true);
            useBtn.gameObject.SetActive(false);
            notEnough.SetActive(true);
        }


        //notEnough.SetActive(true);
        if (PlayerPrefs.GetInt("score", 0) >= ProductPrice || PlayerPrefs.GetInt(bgPlayerPref, 0) == 1)
        {
           notEnough.SetActive(false);
        }

    }


    private void Update()
    {
     // notEnough.SetActive(true);
     //
     // if (isDefault)
     // {
     //     PlayerPrefs.SetInt(bgPlayerPref, 1);
     //     SelectItem();
     //     notEnough.SetActive(false);
     // }
     //
        if (PlayerPrefs.GetInt(curProductType, 0) == bgIndex)
        {
           // selectText.text = "SELECT";s
           equiped.gameObject.SetActive(true);
           locekd.gameObject.SetActive(false);
        }
        if(PlayerPrefs.GetInt(curProductType, 0) != bgIndex)
        {
            equiped.gameObject.SetActive(false);
        }
    }

    public void SelectItem()
    {
        PlayerPrefs.SetInt(curProductType, bgIndex);

        PlayerPrefs.SetInt(curProductType, 1);

        equiped.gameObject.SetActive(true);
        useBtn.gameObject.SetActive(false);
    }

    public void UnSelectItem()
    {
        PlayerPrefs.SetInt(curProductType, bgIndex);

        PlayerPrefs.SetInt(curProductType, 0);

        equiped.gameObject.SetActive(false);
        useBtn.gameObject.SetActive(true);
    }

    public void BuyBG()
    {
     //  notEnough.SetActive(true);
     //
     //  if (isDefault)
     //  {
     //      PlayerPrefs.SetInt(bgPlayerPref, 1);
     //      SelectItem();
     //      notEnough.SetActive(false);
     //  }

      

        if (PlayerPrefs.GetInt("score", 0) >= ProductPrice)
        {
            notEnough.SetActive(false);
        }

        print("Buy");
        if (PlayerPrefs.GetInt("score", 0) >= ProductPrice)
        {
            //scoreManager.ChangeValue(-ProductPrice);
            PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score", 0) - ProductPrice);
            PlayerPrefs.Save();

            buyBtn.gameObject.SetActive(false);
            useBtn.gameObject.SetActive(true);
            locekd.gameObject.SetActive(false);

            PlayerPrefs.SetInt(bgPlayerPref, 1);
            SelectItem();
        }
    }

}
