using UnityEngine;
using System.Collections;

public class GunCTRL : MonoBehaviour
{
    [SerializeField] private Camera fpsCamera;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float spread = 0.05f;

    [SerializeField] private float normalFOV = 60f;
    [SerializeField] private float zoomFOV = 30f;
    [SerializeField] private float zoomSmoothness = 5f;

    private float lastFireTime;
    private bool isZoomed;
    private float currentZoom;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Vector3 positionOffset = new Vector3(0.3f, -0.2f, 0.5f);

    private bool doubleShootActive = false;

    void Start()
    {
        currentZoom = normalFOV;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        FollowCamera();
        HandleZoom();
        HandleShooting();
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButton(0) && Time.time >= lastFireTime + fireRate)
        {
            FireProjectile();
            lastFireTime = Time.time;
            if(!doubleShootActive)
            {
                Vector3 offset = new Vector3(0, 0.5f, 0);
                Instantiate(projectilePrefab, cameraTransform.position + offset, cameraTransform.rotation);
            }
        }
    }

    private void FireProjectile()
    {
        Ray ray = fpsCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 shootDirection = ray.direction;

        Vector3 spreadOffset = new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), Random.Range(-spread, spread));

        shootDirection = (shootDirection + spreadOffset).normalized;

        GameObject pro = Instantiate(projectilePrefab, ray.origin, Quaternion.LookRotation(shootDirection));

        Projectile projectileScript = pro.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.speed = 15f;
        }
    }

    public void ActivateDoubleShoot()
    {
        doubleShootActive = true;
    }

    private void HandleZoom()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isZoomed = !isZoomed;
        }

        float targetFOV = isZoomed ? zoomFOV : normalFOV;
        currentZoom = Mathf.Lerp(currentZoom, targetFOV, zoomSmoothness * Time.deltaTime);
        fpsCamera.fieldOfView = currentZoom;
    }

    private void FollowCamera()
    {
        transform.position = cameraTransform.position + cameraTransform.TransformDirection(positionOffset);
        transform.rotation = cameraTransform.rotation;
    }

    public void ResetPowerUps()
    {
        doubleShootActive = false;
    }
}
