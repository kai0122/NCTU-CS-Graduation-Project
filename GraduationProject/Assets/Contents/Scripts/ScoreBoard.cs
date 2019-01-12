using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour {


    public GameObject[] numbers;
    public GameObject letters;

    private GameObject scoreRoot;
    private GameObject letterRoot;


    void Awake()
    {

        scoreRoot = this.transform.Find("score").gameObject;
        letterRoot = this.transform.Find("letters").gameObject;

        foreach (GameObject number in numbers)
        {
            number.transform.position = new Vector3(0, 0, -5000);
        }

    }

	// Use this for initialization
	void Start () {

        writeScore();

	}
	
	// Update is called once per frame
	void Update () {
	
	}


    private void writeScore()
    {

        string scoreText = "SCORE"; // SCORE

 
        float curX = 0;


        for (int i = 0; i < scoreText.ToString().Length; i++)
        {

            string currentLetter = scoreText.ToString().Substring(i, 1).ToLower();

            GameObject newLetter = null;

            foreach (Transform letter in letters.transform)
            {

                if (letter.gameObject.name.ToLower().Equals(currentLetter))
                {
                    newLetter = (GameObject)GameObject.Instantiate(letter.gameObject, letter.transform.position, Quaternion.identity); ;
                }
                
            }

            if (newLetter != null)
            {

                newLetter.transform.parent = letterRoot.transform;
                newLetter.transform.position = new Vector3(0, 0, 0);
                newLetter.transform.localPosition = new Vector3(curX, 0, 0);

                curX -= 0.42f;
            }


        }


    }

    public void RefreshScore()
    {

        foreach (Transform number in scoreRoot.transform)
        {
            Destroy(number.gameObject);
        }


       int totalScore = PlayerPrefs.GetInt("totalScore", 0);
       float curX = 0;

       for (int i = 0; i < totalScore.ToString().Length; i++)
       {

           string currentNumber = totalScore.ToString().Substring(i, 1);

           GameObject newNumber = getNumber(currentNumber);
           if (newNumber != null)
           {

               newNumber.transform.parent = scoreRoot.transform;
               newNumber.transform.position = new Vector3(0, 0, 0);
               newNumber.transform.localPosition = new Vector3(curX, 0, 0);

               curX -= 0.25f;
           }


       }


    }

    private GameObject getNumber(string number)
    {

        foreach (GameObject nmrObj in numbers)
        {
            if (nmrObj.name.ToLower().Equals(number.ToLower()))
            {
                return (GameObject)GameObject.Instantiate(nmrObj,nmrObj.transform.position,Quaternion.identity);
            }
        }

        return null;
    }


}
