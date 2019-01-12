using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThunderEvents;

public class GameState : MonoBehaviour {

    
    public ThunderEventList Events;

    public bool executeOnStart;
    public float executeDelay;
    public GameState returnToState;
 
    void Start()
    {
        if (executeOnStart == true)
        {
            ThunderEvent.Execute(Events.List);
        }
    }

    public void ExecuteAll()
    {
        if (Events == null) return;

        if (executeDelay <= 0)
        {
            Execute();
        }
        else
        {
            Invoke("Execute", executeDelay);
        }
      
    }

    private void Execute()
    {
        ThunderEvent.Execute(Events.List);

        if (returnToState == null)
        {
            GameGlobals.Instance.currentGameState = this.name;
        }
        else
        {
            GameGlobals.Instance.currentGameState = returnToState.gameObject.name;
        }
    }

}
