using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Corpse : MonoBehaviour
{
    public GameObject corpse;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //GameObject parNet = transform.parent.gameObject;
        Animator parAnim = corpse.GetComponent<Animator>();
        //parAnim.SetTrigger("IsDead");
        //GetComponentInParent<Animator>().Play("A_Walk");
        parAnim.Play("A_Walk");
        Debug.Log("Help");
    }
}
