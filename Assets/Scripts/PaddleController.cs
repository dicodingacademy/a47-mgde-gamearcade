using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PaddleController : NetworkBehaviour
{

    public float batasAtas;
    public float batasBawah;

    public float kecepatan;
    public string axis;

    private void Awake()
    {
        if (transform.position.x > 0) transform.GetComponent<SpriteRenderer>().color = Color.red;
        else transform.GetComponent<SpriteRenderer>().color = Color.blue;

    }

    public override void OnStartLocalPlayer()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;

        float gerak = GetInputPC();

        float nextPos = transform.position.y + gerak;
        if (nextPos > batasAtas)
        {
            gerak = 0;
        }
        if (nextPos < batasBawah)
        {
            gerak = 0;
        }

        transform.Translate(0, gerak, 0);
    }

    float GetInputPC()
    {
        return Input.GetAxis(axis) * kecepatan * Time.deltaTime;
    }
}
