using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStats : MonoBehaviour
{
    public Slider healthSlider;
    int health;

    public int Health
    {
        get { return health; }
        set 
        {
            if (value <= 0)
            {
                health = 0;
                GameManager.Instance.GameOver();
            }
            else health = value;
            healthSlider.value = health;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        health = 20;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
