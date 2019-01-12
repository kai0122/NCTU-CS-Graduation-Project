using UnityEngine;
using System.Collections;

public class EffetcController : MonoBehaviour
{
    private GameObject effetcsRoot;
    public string ID;
    public float customDestroyTime;
	void OnEnable()
	{

       
        if (ID.Equals("point"))
        {
            effetcsRoot = GameObject.Find("Player");
        }
        else
        {
            effetcsRoot = GameObject.Find("EffetcsRoot");
        }

        this.transform.parent = effetcsRoot.transform;
           
		StartCoroutine("CheckIfAlive");
	}
	
	public IEnumerator CheckIfAlive ()
	{
		while(true)
		{
            if (customDestroyTime > 0)
            {
                yield return new WaitForSeconds(customDestroyTime);
            }
            else 
            {
                yield return new WaitForSeconds(0.5f);
            }
		
			GameObject.Destroy(this.gameObject);
			
		}
	}

}
