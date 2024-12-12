using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Rigidbody rig;

    private float startTime;
    private float timeTaken;

    private int collectabledPicked;
    public int maxCollectables = 10;

    public GameObject playButton;
    public TextMeshProUGUI curTimeText;

    private bool isPlaying;

    void Awake ()
    {
        rig = GetComponent<Rigidbody>();
    }

    void Update ()
    {
        if(!isPlaying)
            return;

        // move based on keyboard inputs
        float x = Input.GetAxis("Horizontal") * speed;
        float z = Input.GetAxis("Vertical") * speed;

        rig.velocity = new Vector3(x, rig.velocity.y, z);

        // update the cur time text
        curTimeText.text = (Time.time - startTime).ToString("F2");
    }

    // called when we want to begin the game
    // called when the 'Play' button is pressed
    public void Begin ()
    {
        startTime = Time.time;
        isPlaying = true;
        playButton.SetActive(false);
    }

    // called when the time runs out
    void End ()
    {
        timeTaken = Time.time - startTime;
        isPlaying = false;
        Leaderboard.instance.SetLeaderboardEntry(-Mathf.RoundToInt(timeTaken * 1000.0f));
        playButton.SetActive(true);
    }

    void OnTriggerEnter (Collider other)
    {
        // did we collide with a collectable?
        if(other.gameObject.CompareTag("Collectable"))
        {
            collectabledPicked++;
            Destroy(other.gameObject);

            // if we've picked up all of the collectables - end the game
            if(collectabledPicked == maxCollectables)
                End();
        }
    }
}