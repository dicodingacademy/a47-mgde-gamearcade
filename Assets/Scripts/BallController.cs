using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class BallController : NetworkBehaviour
{

	public int force;
	Rigidbody2D rigid;

	[SyncVar (hook = "OnChangeScore1")]
	public int scoreP1;
	[SyncVar (hook = "OnChangeScore2")]
	public int scoreP2;


	Text scoreUIP1;
	Text scoreUIP2;

	GameObject panelSelesai;
	Text txPemenang;

	AudioSource audio;
	public AudioClip hitSound;

	// Use this for initialization
	void Start ()
	{
		rigid = GetComponent<Rigidbody2D> ();
		Vector2 arah = new Vector2 (2, 0).normalized;
		rigid.AddForce (arah * force);

		scoreP1 = 0;
		scoreP2 = 0;

		scoreUIP1 = GameObject.Find ("Score1").GetComponent<Text> ();
		scoreUIP2 = GameObject.Find ("Score2").GetComponent<Text> ();

		panelSelesai = GameObject.Find ("PanelSelesai");

		audio = GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update ()
	{

	}

	void OnChangeScore1 (int score)
	{
		if (scoreUIP1 != null)
			scoreUIP1.GetComponent<Text> ().text = "" + score;
	}

	void OnChangeScore2 (int score)
	{
		if (scoreUIP2 != null)
			scoreUIP2.GetComponent<Text> ().text = "" + score;
	}


	private void OnCollisionEnter2D (Collision2D coll)
	{
		if (!isServer)
			return;

		audio.PlayOneShot (hitSound);

		if (coll.gameObject.name.Contains ("Pemukul")) {
			float sudut = (transform.position.y - coll.transform.position.y) * 5f;
			Vector2 arah = new Vector2 (rigid.velocity.x, sudut).normalized;
			rigid.velocity = new Vector2 (0, 0);
			rigid.AddForce (arah * force * 2);
		} else if (coll.gameObject.name == "TepiKanan") {
			scoreP1 += 1;

			if (scoreP1 == 5) {
				RpcTampilanSelesai ("Biru");
				return;
			}
			ResetBall ();
			Vector2 arah = new Vector2 (2, 0).normalized;
			rigid.AddForce (arah * force);
		} else if (coll.gameObject.name == "TepiKiri") {
			scoreP2 += 1;

			if (scoreP2 == 5) {
				RpcTampilanSelesai ("Merah");
				return;
			}
			ResetBall ();
			Vector2 arah = new Vector2 (-2, 0).normalized;
			rigid.AddForce (arah * force);
		}

		if (GameObject.FindGameObjectsWithTag ("Player").Length == 1) {
			ClientDisconnect ();
		}
	}

	[ClientRpc]
	void RpcTampilanSelesai (string warna)
	{
		panelSelesai.transform.localPosition = Vector3.zero;
		txPemenang = GameObject.Find ("Pemenang").GetComponent<Text> ();
		txPemenang.text = "Player " + warna + " Pemenang!";
		gameObject.SetActive (false);
	}

	void ResetBall ()
	{
		transform.localPosition = new Vector2 (0, 0);
		rigid.velocity = new Vector2 (0, 0);
	}

	void ClientDisconnect ()
	{
		GameObject.Find ("Main Camera").SendMessage ("KembaliKeMain");
	}
}
