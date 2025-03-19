using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/DoubleShoot")]
public class DoubleShoot : PowerUpEffect
{
    public override void ApplyEffect(GunCTRL gun)
    {
        if(gun != null)
        {
            gun.ActivateDoubleShoot();
        }
    }

    public override void RemoveEffect(GunCTRL gun)
    {
        if (gun != null)
        {
            gun.ResetPowerUps();
        }
    }
}
