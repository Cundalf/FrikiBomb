using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioTrack : MonoBehaviour
{
    public int newTrackID;
    public bool noRepeat;
    public bool playOnStart;
    public bool destroyAfterStart;
    [Tooltip("Se reproduce una canciona aleatoria")]
    public bool Random;

    private void Start()
    {
        if (!playOnStart) return;
        
        if(Random)
        {
            AudioManager.SharedInstance.PlayRandom();
        }
        else
        {
            AudioManager.SharedInstance.PlayNewTrack(newTrackID);
        }

        if (destroyAfterStart) Destroy(gameObject);   
    }

    /*
    private void OnTriggerEnter(Collider collision)
    {        
        if(collision.gameObject.name.Equals("Player"))
        {
            AudioManager.SharedInstance.PlayNewTrack(newTrackID);
            if(noRepeat) gameObject.SetActive(false);
        }
    }
    */
}
