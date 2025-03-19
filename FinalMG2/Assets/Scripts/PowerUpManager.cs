using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;

    [Header("Power-Up Effects")]
    public PowerUpEffect doubleShotEffect;
    public PowerUpEffect missileEffect;
    public PowerUpEffect laserEffect;

    private GunCTRL currentGun;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ApplyPowerUp(GunCTRL gun)
    {
        currentGun = gun;

        switch (gun.powerUpCount)
        {
            case 1:
                doubleShotEffect.ApplyEffect(gun);
                break;
            case 2:
                missileEffect.ApplyEffect(gun);
                break;
        }
    }
}
