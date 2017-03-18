using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatesManager : MonoBehaviour {
    public static MatesManager matesManager = null;

    public List<Mate> mates;
    private bool bridging = false;

    void Awake()
    {
        if (matesManager == null)
            matesManager = this;

        else if (matesManager != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void Start () {
        mates = new List<Mate>();
	}

    
    private void OnLevelWasLoaded(int level)
    {
        Start();
    }

    public void AddMate(Transform mate)
    {
        mates.Add(mate.GetComponent<Mate>());
    }

    public void RemoveMate(Transform mate)
    {
        mates.Remove(mate.GetComponent<Mate>());
    }

    public void OrganizeBridge(Transform[] places, float delay, float timeToMove, float selfDestructTime)
    {
        if (!bridging)
        {
            bridging = true;
            StartCoroutine(organizeBridge(places, delay, timeToMove, selfDestructTime));
        }
    }


    private IEnumerator organizeBridge(Transform[] places, float delay, float timeToMove, float selfDestructTime)
    {
        foreach (Transform place in places)
        {
            for (int i = 0; i < mates.Count; i++)
            {
                if (mates[i].isFree())
                {
                    mates[i].SetPointToMoveBridging(place, timeToMove, selfDestructTime);
                    mates.RemoveAt(i);
                    break;
                }
                else
                {
                    mates.RemoveAt(i);
                }
                    
            }
            
            yield return new WaitForSeconds(delay);
            if(mates.Count == 0)
                break;
        }
        bridging = false;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
