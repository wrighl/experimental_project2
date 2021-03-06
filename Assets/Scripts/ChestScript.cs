﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChestScript : MonoBehaviour
{
    [SerializeField] private float openTime;
    [SerializeField] private float turn;
    [SerializeField] private Color activeColor;
    [SerializeField] private string[] rewardText;
    [SerializeField] private GameObject instantiate;

    private new MeshRenderer renderer;
    private new Collider collider;
    private Color startColor;
    private float triggerTime;
    private bool triggered;
    private bool active;
    private TextScroll textScroller;
    private Quaternion frameTurn;
    private PlayerControl playerControl;
    private GameObject spawned;
    private new AudioSource audio;

    void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
        triggered = false;
        active = false;
        frameTurn = Quaternion.AngleAxis(turn * Time.fixedDeltaTime / openTime, Vector3.forward);
        collider = GetComponent<Collider>();
        textScroller = FindObjectOfType<TextScroll>();
        audio = GetComponent<AudioSource>();
    }

    void Start()
    {
        startColor = renderer.material.color;
    }

    void FixedUpdate()
    {
        if(active && Time.time - triggerTime <= openTime)
        {
            transform.rotation *= frameTurn;
        }
        else if(active)
        {
            active = false;
            if(textScroller != null)
            {
                playerControl.setCanMove(false);
                spawned = Instantiate(instantiate, Camera.main.transform.position + Camera.main.transform.forward * 1.5f, Camera.main.transform.rotation);
                spawned.transform.parent = Camera.main.transform;
                textScroller.ScrollText(rewardText, endText);
            }
        }
    }
    void OnTriggerEnter(Collider col)
    {
        PlayerControl pc = col.gameObject.GetComponent<PlayerControl>(); 
        if(pc == null) return;
        renderer.material.color = activeColor;
        playerControl = pc;
    }

    void OnTriggerExit(Collider col)
    {
        if(col.gameObject.GetComponent<PlayerControl>() == null) return;
        renderer.material.color = startColor;
    }

    void OnTriggerStay(Collider col)
    {
        PlayerControl player = col.gameObject.GetComponent<PlayerControl>();
        if(player == null) return;

        if(player.actionDown && !triggered)
        {
            triggered = true;
            active = true;
            triggerTime = Time.time;
            collider.enabled = false;
            renderer.material.color = startColor;
            audio.Play();
        }
    }

    void endText()
    {
        Destroy(spawned);
        SceneManager.LoadScene("Survey");
        playerControl.setCanMove(true);
    }
}