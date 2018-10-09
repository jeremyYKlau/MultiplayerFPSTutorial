using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillFeed : MonoBehaviour {

    [SerializeField]
    GameObject killfeedItemPrefab;

    //callbacks and methods used in them should be public as they will be accessed by someone not in the hierarchy
	void Start () {
        GameManager.instance.onPlayerKilledCallback += onKill;
	}
	
    public void onKill(string player, string source)
    {
        GameObject go = (GameObject)Instantiate(killfeedItemPrefab, this.transform);
        go.GetComponent<KillFeedItem>().setup(player, source);

        Destroy(go, 4f);
    }
}
