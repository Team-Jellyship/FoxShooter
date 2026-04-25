using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class door : MonoBehaviour
{

    public float interactionDistance;
    public GameObject intText;
    public string doorOpenAnimName, doorCloseAnimName;
    public int state = 0;
    public CharacterController charCtrl;

    void Start()
    {
        charCtrl = GetComponent<CharacterController>();
    }

    void Update()
    {

        
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        //if(Physics.OverlapSphere(transform.position, interactionDistance + 3))
        //{
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionDistance + 3);
        foreach (var hitCollider in hitColliders)
        {
            GameObject doorParent = hitCollider.GetComponent<Collider>().transform.root.gameObject;
            Animator doorAnim = doorParent.GetComponent<Animator>();
            if (hitCollider.tag == "door")
            {
                doorAnim.ResetTrigger("open");
                doorAnim.SetTrigger("close");
            }
        }
        //}
        //if (Physics.SphereCast(transform.position, 7, transform.forward, out hit, interactionDistance+3))
       // {
         //   GameObject doorParent = hit.collider.transform.root.gameObject;
          //  Animator doorAnim = doorParent.GetComponent<Animator>();
         //   if ((hit.collider.gameObject.tag == "door"))
         //   {
         //       doorAnim.ResetTrigger("open");
         //       doorAnim.SetTrigger("close");
         //   }


        //}

        //Sphere sphere = new Sphere(transform.position, interactionDistance);
        //if (Physics.CheckSphere(transform.position, interactionDistance))
        //{
        //    Debug.Log("Hello");
        //}
        if (Physics.Raycast(ray, out hit, interactionDistance))
        //if (Physics.SphereCast(transform.position, 2, transform.forward, out hit, interactionDistance))
        {
            GameObject doorParent = hit.collider.transform.root.gameObject;
            Animator doorAnim = doorParent.GetComponent<Animator>();
            if (hit.collider.gameObject.tag == "door")
            {
                doorAnim.ResetTrigger("close");
                doorAnim.SetTrigger("open");
                //if (doorAnim.GetCurrentAnimatorStateInfo(0).IsName(doorOpenAnimName))
            }
        }
        else
        {
            //doorAnim.ResetTrigger("open");
            //doorAnim.SetTrigger("close");
        }
    }

}
