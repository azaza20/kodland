using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.ComponentModel.Design.Serialization;
public class Shop : MonoBehaviour
{
    [SerializeField] List<GameObject> canvas;
    bool isOpen;
    //отображение стоимости скина
    [SerializeField] List<TMP_Text> costText = new List<TMP_Text>();
    //текущий скин
    [SerializeField] GameObject currentSkin;
    //массив с данными для наших скинов
    public Skin[] skin;
    // Start is called before the first frame update
    PlayerController playerController;
    void Start()
    {
        playerController = PlayerController.instance;
        Debug.Log(playerController.name);
        for (int i = 0; i < skin.Length; i++)
        {
            costText[i].text = skin[i].price.ToString() + " gold";
        }
        gameObject.SetActive(false);
    }
    public void BuySkin(int count)
    {
        if (count > skin.Length)
        {
            return;
        }
        if (skin[count].price <= playerController.coins && skin[count].isBuy == false)
        {
            //Покупаем
            costText[count].text = "Sold";
            skin[count].isBuy = true;
            playerController.AddCoin(-skin[count].price);
        }
        if (skin[count].isBuy == true)
        {
            //Переключаемся
            currentSkin.SetActive(false);
            currentSkin = skin[count].skinToBuy;
            currentSkin.SetActive(true);
        }
    }
    public void BuyAmmo(int count)
    {
        if(playerController.coins >= count*2)
        {
            playerController.AddCoin(-count*2);
            playerController.AddAmmo(count);
        }
    }
    [System.Serializable]
    public class Skin
    {
        //скин, который мы хотим купить
        public GameObject skinToBuy;
        //стоимость скина
        public int price;
        //куплен ли скин?
        public bool isBuy;
    }
    public void OpenShop()
    {
        if (!isOpen)
        {
            canvas[0].SetActive(false);
            canvas[1].SetActive(true);
            canvas[2].SetActive(false);
            isOpen = true;
        }
        else
        {
            canvas[2].SetActive (false);
            canvas[1].SetActive(false);
            canvas[0].SetActive(true);
            isOpen = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
