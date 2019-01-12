using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopWindowCharacterItem : MonoBehaviour {

    public Text characterNameTextField;
    public Text characterPriceTextField;
    public Image buySelectButtonImage;
    public Text buySelectButtonText;
    public GameObject characterMesh;

    [HideInInspector]
    public int characterID;

    [HideInInspector]
    public bool unlocked;

    public string characterName;
    public int characterPrice;

    public string storeIdleAnimation;
    public string storeSelectedAnimation;

    public bool freeCharacter;

    void Awake()
    {

     

    }

    void Start()
    {

        // Assign Character Name
        if (characterNameTextField != null)
        {
            characterNameTextField.text = characterName;
        }

        // Assign Character Price
        if (characterPriceTextField != null)
        {
            if (freeCharacter == true)
            {
                characterPriceTextField.text = "FREE";
                unlocked = true;
            }
            else
            {
                characterPriceTextField.text = characterPrice.ToString();
            }
     
        }

        // Playing Character Animation
        if (characterMesh != null)
        {
            Animator charAnimator = characterMesh.GetComponent<Animator>();
            if (charAnimator != null)
            {
                charAnimator.Play(storeIdleAnimation);
            }
        }

    }

    public void onAdded()
    {

        refreshButtonState();

    }

    public void refreshButtonState()
    {

        if (freeCharacter == true)
        {
            if (PlayerPrefs.GetInt("selected_character", 0) == characterID)
            {
                buySelectButtonImage.color = new Color32(14, 188, 22, 255); // Green
                buySelectButtonText.text = "SELECTED";
            }
            else
            {
                buySelectButtonImage.color = new Color32(192, 193, 193, 255); // Gray
                buySelectButtonText.text = "SELECT";
            }
        }
        else
        {
            if (unlocked == true)
            {

                if (PlayerPrefs.GetInt("selected_character", 0) == characterID)
                {
                    buySelectButtonImage.color = new Color32(14, 188, 22, 255); // Green
                    buySelectButtonText.text = "SELECTED";
                }
                else
                {
                    buySelectButtonImage.color = new Color32(192, 193, 193, 255); // Gray
                    buySelectButtonText.text = "SELECT";
                }
            }
        }


   
    }

    public void playAnimation(string animationName)
    {

        Animator charAnimator = characterMesh.GetComponent<Animator>();
        if (charAnimator != null)
        {
            charAnimator.Play(animationName);
        }
    }

    public void doBuy()
    {
        if (unlocked == false)
        {
            ShopWindow.Instance.buyCharacter(characterID);
        }
        else
        {
            ShopWindow.Instance.selectCharacter(characterID);
        }
       
    }

    public GameObject getPlayerClone()
    {
        GameObject newPlayerCharacter = GameObject.Instantiate(characterMesh);
        newPlayerCharacter.transform.localScale = new Vector3(1, 1, 1);
        SetLayerRecursively(newPlayerCharacter, 0);

        return newPlayerCharacter;

    }

    public void SetLayerRecursively(GameObject go, int layerNumber)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }


}
