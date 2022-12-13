using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class HUDController : SingletonMonoBehaviour<HUDController>
{
    public Slider healthBarSlider;
    public Slider energyBarSlider;

    public Image hitFlashOverlay;
    public Image healOverlay;
    public Image invulnerabilityOverlay;

    private bool isDisplaying = false;
    private bool isDispInvul = false;

    private float timeRemaining;
    private float invulRemaining;
    public TextMeshProUGUI textMesh;


    protected override void Awake()
    {
        base.Awake();
    }


    // Start is called before the first frame update
    void Start()
    {
        textMesh.color = Color.clear;
    }

    // Update is called once per frame
    void Update()
    {
        /// touche T permet de mettre 10 de damage au player
        if (Input.GetKeyDown(KeyCode.T))
        { 
            PlayerController.instance.Damage(10);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            PlayerController.instance.Heal(10);
        }

        // Check if text is currently being displayed
        if (isDisplaying)
        {
            // Decrement time remaining by delta time
            timeRemaining -= Time.deltaTime;

            // Check if time remaining is less than or equal to 0
            if (timeRemaining <= 0)
            {
                // Hide the text
                textMesh.DOFade(0, 1.5f);

                // Set isDisplaying flag to false
                isDisplaying = false;
            }
        }

        if (isDispInvul)
        {
            // Decrement time remaining by delta time
            invulRemaining -= Time.deltaTime;

            // Check if time remaining is less than or equal to 0
            if (invulRemaining <= 0)
            {
                // Hide the text
                invulnerabilityOverlay.DOFade(0, 1.5f);

                // Set isDisplaying flag to false
                isDispInvul = false;
            }
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
            healOverlay.DOKill();
            healOverlay.color = new Color(healOverlay.color.r, healOverlay.color.g, healOverlay.color.b, Mathf.Lerp(0.2f, 1, diff / 30));
            healOverlay.DOFade(0,1f);

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
        invulnerabilityOverlay.color = Color.white;

        isDispInvul = true;
        invulRemaining = duration;

    }

    /// <summary>
    /// Highlights the gun at position index.
    /// </summary>
    /// <param name="index">The position of the gun, starting at 0</param>
    public void SetEquippedGun(int index)
    {

    }

    public void DisplayText(string text, float duration)
    {

        // Show the text
        textMesh.DOKill();
        textMesh.color = Color.white;
        textMesh.text = text;

        // Set isDisplaying flag to true
        isDisplaying = true;

        // Reset time remaining to display time
        timeRemaining = duration;
    }
}
