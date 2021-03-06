﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieAndFallover : MonoBehaviour
{
    [SerializeField] private float animationTime = 0.5f;
    [SerializeField] private float deathTime = 1;
    [SerializeField] private MonoBehaviour[] componentsToDisable;

    private bool playing;
    private float startTime;
    Animator anim;

    void Awake()
    {
        playing = false;
        startTime = -1;
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if(playing)
        {
            if(Time.time < startTime + animationTime)
            {
                transform.rotation *= Quaternion.AngleAxis(-90 / animationTime * Time.deltaTime, transform.forward);
            }
        }
    }

    public void Die()
    {
        startTime = Time.time;
        playing = true;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        foreach(MonoBehaviour m in componentsToDisable)
        {
            m.enabled = false;
        }
        anim.SetBool("Attack", false);
        anim.SetFloat("Speed", 0f);
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(deathTime);
        Destroy(gameObject);
    }
}
