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
    [SerializeField] private Transform firePoint;

    public bool doubleShootActive = false;
    public bool missilesActive = false;
    public int powerUpCount = 0;

    public GameObject missilePrefab;
    private float nextMissileTime;

    void Start()
    {
        nextMissileTime = 0f;
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
            if (doubleShootActive == true) DoubleShoot();
            if (missilesActive) ShootMissile();
        }
    }

    private void FireProjectile()
    {
        Ray ray = fpsCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 shootDirection = ray.direction;

        Vector3 spreadOffset = new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), Random.Range(-spread, spread));
        shootDirection = (shootDirection + spreadOffset).normalized;

        GameObject pro = Instantiate( projectilePrefab, firePoint.position, Quaternion.LookRotation(shootDirection));
    }

    private void DoubleShoot()
    {
        if (firePoint == null) return;

        Vector3 offset = firePoint.right * 0.2f;
        Instantiate(projectilePrefab, firePoint.position + offset, firePoint.rotation);
    }

    private void ShootMissile()
    {
        const float missileCooldown = 2f;
        if (Time.time > nextMissileTime)
        {
            Instantiate(missilePrefab, transform.position, Quaternion.Euler(0, 0, 45));
            nextMissileTime = Time.time + missileCooldown;
        }
    }

    public void ActivateDoubleShoot()
    {
        doubleShootActive = true;
        Debug.Log("Disparo doble activado");
    }

    public void ActivateMissiles()
    {
        missilesActive = true;
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
        missilesActive = false;
        powerUpCount = 0;
    }
}
