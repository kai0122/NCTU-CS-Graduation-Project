using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopWindow : MonoBehaviour {



    // Singleton
    private static ShopWindow instance = null;
    public static ShopWindow Instance
    {
        get
        {
            return instance;
        }
    }

    public GameObject shopWindowHolder;
    public GameObject charactersContentHolder;
    public Text txtShopWindowCoinsTotal;

    public List<ShopWindowCharacterItem> characters;



    public void addCharacter(ShopWindowCharacterItem character)
    {

        if (characters == null)
        {
            characters = new List<ShopWindowCharacterItem>();
        }

        characters.Add(character);


    }

    public void openShopWindow()
    {

        refreshScoreWindowCoins();
        shopWindowHolder.SetActive(true);

    }

    private void refreshScoreWindowCoins()
    {
        if (txtShopWindowCoinsTotal != null)
        {
            txtShopWindowCoinsTotal.text = PlayerPrefs.GetInt("totalCoins", 0).ToString();
        }

    }


    public ShopWindowCharacterItem getSelectedCharacter()
    {
        return (getCharacterItem(PlayerPrefs.GetInt("selected_character", 0)));
    }

    public ShopWindowCharacterItem getCharacterItem(int characterID)
    {

        if (characters == null) return null;
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].characterID == characterID)
            {
                return characters[i];
            }
        }
        return null;
    }

    private void Awake()
    {

        // Singleton
        instance = this;
        shopWindowHolder.SetActive(false);

        // Loading Characters From Shop Window
        Component[] components = charactersContentHolder.transform.GetComponentsInChildren(typeof(ShopWindowCharacterItem), true);
        for (int i = 0; i < components.Length; i++)
        {

            ShopWindowCharacterItem currentCharacter = components[i] as ShopWindowCharacterItem;
            currentCharacter.characterID = i;
            if (PlayerPrefs.GetInt("character" + i.ToString() + "_unlocked", 0) == 1)
            {
                currentCharacter.unlocked = true;
            }

            addCharacter(currentCharacter);
            currentCharacter.onAdded();


        }


    }

    private ShopWindowCharacterItem selectedCharacter;
    public void buyCharacter(int characterID)
    {

        ShopWindowCharacterItem character = getCharacterItem(characterID);
        if (character == null) return;
        selectedCharacter = character;

        if (selectedCharacter.characterPrice > GameGlobals.Instance.achievements.totalCoins)
        {
            DialogWindow.Instance.showDialog(DialogWindow.DialogType.OkOnly, "No enough coins to buy this character",null);
            GameGlobals.Instance.audioController.playSound("UIError", false);
            return;
        }
        else
        {
            DialogWindow.Instance.showDialog(DialogWindow.DialogType.YesNo, "Are you sure you want to buy this character ?", buyCharacterDialogResult);
        }
       
    }

    public void buyCharacterDialogResult(DialogWindow.DialogResult result)
    {
        if (selectedCharacter == null) return;

        if (result == DialogWindow.DialogResult.Yes)
        {

        

            // Unlock Character
            PlayerPrefs.SetInt("character" + selectedCharacter.characterID.ToString() + "_unlocked", 1);

            // Pay with coins
            PlayerPrefs.SetInt("totalCoins", PlayerPrefs.GetInt("totalCoins", 0) - selectedCharacter.characterPrice);
            refreshScoreWindowCoins();
            GameGlobals.Instance.achievements.Reset();

            GameGlobals.Instance.audioController.playSound("PowerupLetterStep", false);
            selectedCharacter.unlocked = true;
            refreshAllCharacterButtonStates();



        }
        
    }


    public void selectCharacter(int characterID)
    {

        ShopWindowCharacterItem character = getCharacterItem(characterID);
        if (character == null) return;
        selectedCharacter = character;

        DialogWindow.Instance.showDialog(DialogWindow.DialogType.YesNo, "Are you sure you want to select this character ?", selectharacterDialogResult);

    }

    public void selectharacterDialogResult(DialogWindow.DialogResult result)
    {
        if (selectedCharacter == null) return;

        if (result == DialogWindow.DialogResult.Yes)
        {
            PlayerPrefs.SetInt("selected_character", selectedCharacter.characterID);
     
            selectedCharacter.playAnimation(selectedCharacter.storeSelectedAnimation);
            GameGlobals.Instance.audioController.playSound("UICharacterUnlock", false);
            refreshAllCharacterButtonStates();

            // Selecting Character In Game
            GameGlobals.Instance.controller.setCharacter(selectedCharacter.getPlayerClone());
        }

    }




    private void refreshAllCharacterButtonStates()
    {
        if (characters == null) return;
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].refreshButtonState();
        }
    }





}
