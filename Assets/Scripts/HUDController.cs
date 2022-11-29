using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HUDController : SingletonMonoBehaviour<HUDController>
{
    public Slider healthBarSlider;
    public Slider energyBarSlider;


    protected override void Awake()
    {
        base.Awake();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /// touche T permet de mettre 10 de damage au player
        if (Input.GetKeyDown(KeyCode.T))
        { 
            PlayerController.Instance.Damage(10);
        }
    }

    /// <summary>
    /// Sets the health bar to the given number.
    /// </summary>
    /// <param name="health"></param>
    public void SetMaxHealth(float health)
    {
        healthBarSlider.maxValue = health;
        healthBarSlider.value = health;
        
    }

    public void SetHealth(float health)
    {
        healthBarSlider.value = health;

    }

    /// <summary>
    /// Sets the weapon energy bar to the given number.
    /// </summary>
    /// <param name="energy"></param>

    public void SetMaxEnergy(float energy)
    {
        energyBarSlider.maxValue = energy;
        energyBarSlider.value = energy;

    }

    public void SetEnergy(float energy)
    {
        energyBarSlider.value = energy;
    }

    /// <summary>
    /// Highlights the gun at position index.
    /// </summary>
    /// <param name="index">The position of the gun, starting at 0</param>
    public void SetEquippedGun(int index)
    {

    }
}
