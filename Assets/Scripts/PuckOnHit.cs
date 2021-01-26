using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckOnHit : MonoBehaviour
{
    public AudioClip Puck1;
    public AudioClip Puck2;
    public AudioClip Puck3;

    private AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision other)
    {
        string tag = other.transform.tag;
        if (tag == "Wall" || tag == "Player"
            || tag == "Player2")
        {
            int rand = Random.Range(1, 3);

            switch (rand)
            {
                case 1:
                    audio.clip = Puck1;
                    audio.Play();
                    break;
                case 2:
                    audio.clip = Puck2;
                    audio.Play();
                    break;
                case 3:
                    audio.clip = Puck3;
                    audio.Play();
                    break;
            }
            
        }
    }

}
