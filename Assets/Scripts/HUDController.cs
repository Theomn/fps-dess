using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : SingletonMonoBehaviour<HUDController>
{

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
        
    }

    /// <summary>
    /// Sets the health bar to the given number.
    /// </summary>
    /// <param name="health"></param>
    public void SetHealth(float health)
    {

    }

    /// <summary>
    /// Sets the weapon energy bar to the given number.
    /// </summary>
    /// <param name="energy"></param>
    public void SetEnergy(float energy)
    {

    }

    /// <summary>
    /// Highlights the gun at position index.
    /// </summary>
    /// <param name="index">The position of the gun, starting at 0</param>
    public void SetEquippedGun(int index)
    {

    }
}
