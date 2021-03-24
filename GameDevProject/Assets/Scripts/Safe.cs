using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Safe : Interactable
{
    public Transform knob;
    public Transform door;
    public Transform player;
    public Text msgBox;
    public SerialCommThreaded sct;

    bool isInteracting;
    byte passcode;
    byte[] setActive = { 254, 255 };

    public byte Passcode { get { return passcode; } }

    // Start is called before the first frame update
    void Start()
    {
        passcode = (byte)Random.Range(-120,120);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isInteracting)
        {
            if (Vector3.SqrMagnitude(transform.position - player.transform.position) > 4f)
            {
                Debug.Log(Vector3.SqrMagnitude(transform.position - player.transform.position));
                Debug.Log("end it now");
                sct.sp.Write(setActive, 0, 1);
                isInteracting = false;
            }
        }
        //Debug.Log(Vector3.SqrMagnitude(transform.position - player.transform.position));
    }

    public override void Interact()
    {
        sct.sp.Write(setActive, 1, 1);
        isInteracting = true;
    }

    public IEnumerator DisplayError()
    {
        msgBox.text = "Code is incorrect!";
        msgBox.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        msgBox.gameObject.SetActive(false);
    }

    public IEnumerator Open()
    {
        for (int i = 0; i < 120; i++)
        {
            door.transform.Rotate(new Vector3(0, 0, 1));
            yield return null;
        }
    }

    private void OnApplicationQuit()
    {
        sct.sp.Write(setActive, 0, 1);
    }
}
