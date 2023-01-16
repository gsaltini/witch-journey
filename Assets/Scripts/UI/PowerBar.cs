using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBar : MonoBehaviour
{
    //Bar Elements
    [SerializeField] public Image healthFill;
    [SerializeField] public Image powerFill;
    [SerializeField] public GameObject fire;
    [SerializeField] public GameObject water;
    [SerializeField] public GameObject earth;
    [SerializeField] public GameObject air;
    [SerializeField] public float maxHealth = 300f;
    [SerializeField] public float powerCount = 5f;

    // Start is called before the first frame update
    void Start()
    {
        powerFill.fillAmount = 0f;
        fire.SetActive(true);
        water.SetActive(false);
        earth.SetActive(false);
        air.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            fire.SetActive(true);
            water.SetActive(false);
            earth.SetActive(false);
            air.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            fire.SetActive(false);
            water.SetActive(true);
            earth.SetActive(false);
            air.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            fire.SetActive(false);
            water.SetActive(false);
            earth.SetActive(true);
            air.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            fire.SetActive(false);
            water.SetActive(false);
            earth.SetActive(false);
            air.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage();
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            FillPower();
        }
    }

    public void TakeDamage()
    {
        if (healthFill.fillAmount >= 0)
            healthFill.fillAmount -= 1f / maxHealth;
        Debug.Log("took damage");
    }

    public void Heal(int amount)
    {
        var tempFloat = (float)amount; 
        if (healthFill.fillAmount >= 0)
            healthFill.fillAmount += 1f / maxHealth;
        Debug.Log("healed by 1f");
    }

    public void FillPower()
    {
        if (powerFill.fillAmount < 1)
            powerFill.fillAmount += 1f / powerCount;
    }
}
