using UnityEngine;
using System.Collections;

public class TouchController : MonoBehaviour
{

    public enum SwipeDirection
    {
        Null = 0,//no swipe detected
        Down = 1,//swipe down detected
        Up = 2,//swipe up detected
        Right = 3,//swipe right detected
        Left = 4//swipe left detected
    }

    //Constants
    private float fSensitivity = 25;

    //VARIABLES
    //distance calculation
    private SwipeDirection sSwipeDirection;			//string to receive swipe output (Debug Only)
    private float fInitialX;
    private float fInitialY;
    private float fFinalX;
    private float fFinalY;
    private int iTouchStateFlag;	//flag to check 

    private float inputX;				//x-coordinate
    private float inputY;				//y-coordinate
    private float slope;				//slope (m) of the the 
    private float fDistance;			//magnitude of distance between two positions

    // Use this for initialization
    void Start()
    {
        fInitialX = 0.0f;
        fInitialY = 0.0f;
        fFinalX = 0.0f;
        fFinalY = 0.0f;

        inputX = 0.0f;
        inputY = 0.0f;

        iTouchStateFlag = 0;
        sSwipeDirection = SwipeDirection.Null;
    }

    void Update()
    {
        if (iTouchStateFlag == 0 && Input.GetMouseButtonDown(0))	//state 1 of swipe control
        {
            fInitialX = Input.mousePosition.x;	//get the initial x mouse/ finger value
            fInitialY = Input.mousePosition.y;	//get the initial y mouse/ finger value

            sSwipeDirection = SwipeDirection.Null;
            iTouchStateFlag = 1;
        }
        if (iTouchStateFlag == 1)	//state 2 of swipe control
        {
            fFinalX = Input.mousePosition.x;
            fFinalY = Input.mousePosition.y;

            sSwipeDirection = swipeDirection();	//get the swipe direction
            if (sSwipeDirection != SwipeDirection.Null)
                iTouchStateFlag = 2;
        }//end of state 1		
        if (iTouchStateFlag == 2 || Input.GetMouseButtonUp(0))	//state 3 of swipe control
        {
            //sSwipeDirection = SwipeDirection.Null;
            iTouchStateFlag = 0;
        }//end of M.R. swipe control
    }

    /*
    *	FUNCTION: Calculate the swipe direction
    */
    private SwipeDirection swipeDirection()
    {
        //calculate the slope of the swipe
        inputX = fFinalX - fInitialX;
        inputY = fFinalY - fInitialY;
        slope = inputY / inputX;

        //calculate the distance of tap start and end
        fDistance = Mathf.Sqrt(Mathf.Pow((fFinalY - fInitialY), 2) + Mathf.Pow((fFinalX - fInitialX), 2));

        if (fDistance <= (Screen.width / fSensitivity))//higher the dividing factor higher the sensitivity
            return SwipeDirection.Null;

        if (inputX >= 0 && inputY > 0 && slope > 1)//first octant JUMP
        {
            return SwipeDirection.Up;
        }
        else if (inputX <= 0 && inputY > 0 && slope < -1)//eighth octant  JUMP
        {
            return SwipeDirection.Up;
        }
        else if (inputX > 0 && inputY >= 0 && slope < 1 && slope >= 0)//second octant  RIGHT
        {
            return SwipeDirection.Right;
        }
        else if (inputX > 0 && inputY <= 0 && slope > -1 && slope <= 0)//third octant  RIGHT
        {
            return SwipeDirection.Right;
        }
        else if (inputX < 0 && inputY >= 0 && slope > -1 && slope <= 0)//seventh octant  LEFT
        {
            return SwipeDirection.Left;
        }
        else if (inputX < 0 && inputY <= 0 && slope >= 0 && slope < 1)//sixth octant  LEFT
        {
            return SwipeDirection.Left;
        }
        else if (inputX >= 0 && inputY < 0 && slope < -1)//fourth octant  DUCK
        {
            return SwipeDirection.Down;
        }
        else if (inputX <= 0 && inputY < 0 && slope > 1)//fifth octant  DUCK
        {
            return SwipeDirection.Down;
        }//end of else if

        return SwipeDirection.Null;
    }//end of SwipeDirection function

    /*
    *	FUNCTION: Return swipe direction.
    *	RETURNS: Returns NULL if no swipes are detected.
    *			  Returns SwipeDirection if a swipe is detected
    */
    public SwipeDirection getSwipeDirection()
    {
        if (sSwipeDirection != SwipeDirection.Null)
        {
            var etempSwipeDirection = sSwipeDirection;
            sSwipeDirection = SwipeDirection.Null;

            return etempSwipeDirection;
        }
        else
            return SwipeDirection.Null;
    }
}
