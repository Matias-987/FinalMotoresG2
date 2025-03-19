using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private PowerUpEffect[] possibleEffects;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GunCTRL gun = other.GetComponentInChildren<GunCTRL>();
            ApplyRandomEffect(gun);
            Destroy(gameObject);
        }
    }

    private void ApplyRandomEffect(GunCTRL gun)
    {
        PowerUpEffect selected = possibleEffects[Random.Range(0, possibleEffects.Length)];
        selected.ApplyEffect(gun);
    }
}
