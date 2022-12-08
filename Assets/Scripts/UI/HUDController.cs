using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class HUDController : SingletonMonoBehaviour<HUDController>
{
    public Slider healthBarSlider;
    public Slider energyBarSlider;

    public Image hitFlashOverlay;
    public Image invulnerabilityOverlay;


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
        if (Input.GetKeyDown(KeyCode.H))
        {
            PlayerController.Instance.Heal(10);
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
        var diff = health - healthBarSlider.value;
        // if player took damage
        if (diff < 0)
        {
            diff = -diff;
            hitFlashOverlay.DOKill();
            hitFlashOverlay.color = new Color(hitFlashOverlay.color.r, hitFlashOverlay.color.g, hitFlashOverlay.color.b, Mathf.Lerp(0.2f, 1, diff / 30));
            hitFlashOverlay.DOFade(0, Mathf.Lerp(1.5f, 2.5f, diff/30));
        }
        else if (diff > 0)
        {
            // if player healed
        }
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

    public void InvincibleOverlay(float duration)
    {

    }

    /// <summary>
    /// Highlights the gun at position index.
    /// </summary>
    /// <param name="index">The position of the gun, starting at 0</param>
    public void SetEquippedGun(int index)
    {

    }

    public void DisplayText(string text, float time)
    {

    }
}
