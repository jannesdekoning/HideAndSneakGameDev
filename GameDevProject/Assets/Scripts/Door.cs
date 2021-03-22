using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public GameObject key;
    public Door pairedDoor;
    public bool mirrorAnimation;

    bool IsInteractable = false;
    bool IsOpen = false;
    // Start is called before the first frame update

    private void Start()
    {
        if (key == null)
        {
            IsInteractable = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == key)
        {
            Destroy(key);
            IsInteractable = true;
            if (pairedDoor != null) pairedDoor.IsInteractable = true;
            print("door unlocked!");
        }
    }

    IEnumerator OpenDoor()
    {
        print("Opening the door");
        float startRot = transform.rotation.eulerAngles.y;
        float endRot;
        if (mirrorAnimation) endRot = startRot + 90f;
        else endRot = startRot - 90f;

        float t = 0f;
        IsInteractable = false;
        while (t < 1)
        {
            transform.rotation = Quaternion.Slerp(Quaternion.Euler(-90, startRot, 0), Quaternion.Euler(-90, endRot, 0), t);
            t += Time.deltaTime*2;
            yield return null;
        }
        IsInteractable = true;
        IsOpen = true;
    }
    IEnumerator CloseDoor()
    {
        print("Closing the door");
        float startRot = transform.rotation.eulerAngles.y;
        float endRot;
        if (mirrorAnimation) endRot = startRot - 90f;
        else endRot = startRot + 90f;

        float t = 0f;
        IsInteractable = false;
        while (t < 1)
        {
            transform.rotation = Quaternion.Slerp(Quaternion.Euler(-90, startRot, 0), Quaternion.Euler(-90, endRot, 0), t);
            t += Time.deltaTime * 2;
            yield return null;
        }
        IsInteractable = true;
        IsOpen = false;
    }

    public override void Interact()
    {
        if (IsInteractable)
        {
            StopAllCoroutines();
            if (IsOpen)
                StartCoroutine(CloseDoor());
            else StartCoroutine(OpenDoor());
        }
        else
        {
            print("the door is locked");
        }
    }
}
