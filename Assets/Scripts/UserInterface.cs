﻿using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour {
    public static UserInterface instance;
    public GameObject inventoryButton;
    public GameObject inventoryMenu;
    public GameObject backgroundPanel;
    public RectTransform slotPrefab;
    public Text estimationText;
    public Text playerMessage;
    public Text goalText;
    private float totalPoints;
    
    public void Awake() {
        if (instance == null) instance = this;
        goalText.text = "Goal: "+GameManager.getInstance().goal+"$";
        updateInventory();
    }

    public void updateInventory() {
        Data data = Data.instance;

        foreach (Transform child in inventoryMenu.transform) {
            Destroy(child.gameObject);
        }
        
        Inventory inventory = data.getPlayerInventory();
        float totalWorth = 0;
        for (int i = 0; i < inventory.items.Length; i++) {
            RectTransform inventorySlot = Instantiate(slotPrefab);
            inventorySlot.name = "Slot "+i;
            inventorySlot.transform.SetParent(inventoryMenu.transform);
            inventorySlot.localScale = new Vector3(1,1,1);
            Item item = inventory.items[i];
            if(item!=null){
                totalWorth += item.worth;
                inventorySlot.GetChild(0).GetComponent<Image>().sprite = inventory.items[i].sprite;
                inventorySlot.GetComponent<Button>().onClick.AddListener(()=>item.use());
            }
        }

        totalPoints = totalWorth;
        estimationText.text = "Earned\n"+totalWorth + "$";
    }


    public void toggleInventory(GameObject inventoryMenu) {
        if (inventoryMenu.active) {
            inventoryMenu.SetActive(false);
        } else if (!inventoryMenu.active) {
            inventoryMenu.SetActive(true);
        }
        
        AudioManager.instance.Play("bag");
    }

    public float getTotalPoints() {
        return totalPoints;
    }
    
    public void endScreen(bool completed) {
        if (completed) {
            playerMessage.text = "Level Completed";
            playerMessage.color = Color.green;
        } else {
            playerMessage.text = "Level Failed";
            playerMessage.color = Color.red;
        }
        inventoryMenu.SetActive(false);
        inventoryButton.SetActive(false);
        backgroundPanel.SetActive(true);

    }
}
