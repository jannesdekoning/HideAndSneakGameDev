/* Testprog serial port from Arduino threaded
 * Wim Van Weyenberg & Pieter Jorissen
 * 18/09/2018
 * In Start() wordt de comport geopend (stel juiste naam in van de Arduino Port!) en wordt ook de thread gestart om data te ontvangen en te zenden
 * Ontvangen en zenden gebeurt in dezelfde thread
 * Ontvangen moet via aparte thread omdat we sp.ReadTimeout = 20 ms lang moeten wachten om te weten of er iets ontvangen is.
 * Als er data ontvangen is wordt deze in update getoond via Debug.Log
 * Om dat te zenden gebruiken we hier de A en U toets op het keyboard
 * You have to set the File -> Build Settings -> Player Settings -> API Compatibility Level to .NET2.0 (NOT the subset).
 */


using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Threading;
using System;

public class SerialCommThreaded : MonoBehaviour
{
    public string portName = "COM3";
    public SerialPort sp;
    private bool blnPortcanopen = false; //if portcanopen is true the selected comport is open

    //statics to communicate with the serial com thread
    static private int databyte_in; //read databyte from serial port
    static private bool databyteRead = false; //becomes true if there is indeed a character received
    static private int databyte_out; //index in txChars array of possible characters to send
    static private bool databyteWrite = false; //to let the serial com thread know there is a byte to send
    //txChars contains the characters to send: we have to use the index
    private char[] txChars = { 'A', 'U' };

    //threadrelated
    private bool stopSerialThread = false; //to stop the thread
    private Thread readWriteSerialThread; //threadvariabele

    void Start()
    {
        sp = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One);
        OpenConnection(); //init COMPort
                          //define thread and start it
        readWriteSerialThread = new Thread(SerialThread);
        readWriteSerialThread.Start(); //start thread
    }

    void Update()
    {
        if (databyteRead) //if a databyte is received
        {
            databyteRead = false; //to see if a next databyte is received
            char inputChar = (char)databyte_in;
            Debug.Log(inputChar);
            if(inputChar == 'L')
            {
                PlayerControls.enableGrow = true;
            }
            else if (inputChar == 'R')
            {
                PlayerControls.enableShrink = true;
            }
            else if (inputChar == 'l')
            {
                PlayerControls.enableGrow = false;
            }
            else if (inputChar == 'r')
            {
                PlayerControls.enableShrink = false;
            }
        }
        if (PlayerControls.isTouching)
        {
            databyte_out = 0; //index in txChars
            databyteWrite = true;
        }
        if (!PlayerControls.isTouching)
        {
            databyte_out = 1; //index in txChars
            databyteWrite = true;
        }
    }


    void SerialThread() //separate thread is needed because we need to wait sp.ReadTimeout = 20 ms to see if a byte is received
    {
        while (!stopSerialThread) //close thread on exit program
        {
            if (blnPortcanopen)
            {
                if (databyteWrite)
                {
                    if (databyte_out == 0)
                    {
                        sp.Write(txChars, 0, 1); //tx 'A'
                    }
                    if (databyte_out == 1)
                    {
                        sp.Write(txChars, 1, 1); //tx 'U'
                    }
                    databyteWrite = false; //to be able to send again
                }
                try //trying something to receive takes 20 ms = sp.ReadTimeout
                {
                    databyte_in = sp.ReadChar();
                    databyteRead = true;
                }
                catch (Exception)
                {
                    //Debug.Log(e.Message);
                }
            }
        }
    }


    //Function connecting to Arduino
    public void OpenConnection()
    {
        if (sp != null)
        {
            if (sp.IsOpen)
            {
                string message = "Port is already open!";
                Debug.Log(message);
            }
            else
            {
                try
                {
                    sp.Open();  // opens the connection
                    blnPortcanopen = true;
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                    blnPortcanopen = false;
                }
                if (blnPortcanopen)
                {
                    sp.ReadTimeout = 20;  // sets the timeout value before reporting error
                    Debug.Log("Port Opened!");
                }
            }
        }
        else
        {
            Debug.Log("Port == null");
        }
    }


    void OnApplicationQuit() //proper afsluiten van de thread
    {
        if (sp != null) sp.Close();
        stopSerialThread = true;
        readWriteSerialThread.Abort();
    }
}
