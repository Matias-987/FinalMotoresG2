using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static bool isRestarting = false;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            SceneManager.sceneLoaded += OnSceneReloaded;
        }
        else Destroy(gameObject);
    }
    public static void Restart()
    {
        GunCTRL gun = FindObjectOfType<GunCTRL>();
        if (gun != null) gun.ResetPowerUps();
        isRestarting = true;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnSceneReloaded(Scene scene, LoadSceneMode mode)
    {
        isRestarting = false;
        RestartGameState();
    }

    private void RestartGameState()
    {
        if(PlayerCTRL.Instance != null)
        {
            Destroy(PlayerCTRL.Instance.gameObject);
        }

        PlayerCTRL.ResetStaticInstance();

        Time.timeScale = 1.0f;

        DestroyObjectsPerTag("Enemy");
        DestroyObjectsPerTag("Projectile");
        DestroyObjectsPerTag("PowerUp");
    }

    private void DestroyObjectsPerTag(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
    }
}
