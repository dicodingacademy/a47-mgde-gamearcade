using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BolaManager : NetworkBehaviour
{

    public GameObject prefabBola;
    bool bolaMuncul = false;
    GameObject bola;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!isServer || bolaMuncul)
            return;

        if (NetworkServer.connections.Count == 2)
        {
                bola = (GameObject)Instantiate(prefabBola);
                NetworkServer.Spawn(bola);
                bolaMuncul = true;
        }
    }
}
