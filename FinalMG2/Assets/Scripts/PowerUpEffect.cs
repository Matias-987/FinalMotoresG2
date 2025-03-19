using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPowerUp", menuName = "PowerUps/Base Effect")]
public class PowerUpEffect : ScriptableObject
{
    public string effectName;
    public Sprite icon;

    public virtual void ApplyEffect(GunCTRL gun)
    {
        
    }
}
