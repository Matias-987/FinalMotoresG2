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
            if (gun != null && possibleEffects.Length > 0)
            {
                ApplyRandomEffect(gun);
                Destroy(gameObject);
            }
        }
    }

    private void ApplyRandomEffect(GunCTRL gun)
    {
        int randomIndex = Random.Range(0, possibleEffects.Length);
        PowerUpEffect selectedEffect = possibleEffects[randomIndex];

        if (selectedEffect != null)
        {
            selectedEffect.ApplyEffect(gun);
            Debug.Log($"Efecto aplicado: {selectedEffect.effectName}");
        }
    }
}
