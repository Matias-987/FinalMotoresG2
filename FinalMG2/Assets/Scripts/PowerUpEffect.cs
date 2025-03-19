using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPowerUp", menuName = "PowerUps/Base Effect")]
public class PowerUpEffect : ScriptableObject
{
    public string effectName;

    public virtual void ApplyEffect(GunCTRL gun)
    {
        
    }

    public virtual void RemoveEffect(GunCTRL gun)
    {

    }
}
