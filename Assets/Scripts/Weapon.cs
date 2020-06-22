using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float fireRate = 0f;
    public int damage = 10;
    public float effectSpawnRate = 10f;
    public LayerMask whatToHit;

    public Transform bulletTrailPrefab;
    public Transform muzzleFlashPrefab;
    public Transform hitPrefab;

    public float camShakeAmount = 0.05f;
    public float camShakeLength = 0.1f;
    CameraShake cameraShake;

    float timeToFire = 0;
    float timeToSpawnEffect = 0;
    public string shootSound = "DefaultShootSound";

    //Caching
    AudioManager audioManager;
    Transform firePoint;

    // Start is called before the first frame update
    void Awake()
    {
        firePoint = transform.Find("FirePoint");
        if (firePoint == null)
        {
            Debug.LogError("firePoint null!");
        }
    }

    private void Start()
    {
        cameraShake = GameManager.gameManager.GetComponent<CameraShake>();
        if (cameraShake == null)
        {
            Debug.LogError(" Camera shake NOT available");
        }
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("No AudioManager Found! ");
        }
    }
    // Update is called once per frame
    void Update()
    {

        //Shoot();
        if (fireRate == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButton("Fire1") && Time.time > timeToFire)
            {
                timeToFire = Time.time + 1 / fireRate;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);

        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, 100, whatToHit);

        Debug.DrawLine(firePointPosition, (mousePosition - firePointPosition) * 100, Color.cyan);

        if (hit.collider != null)
        {
            Debug.DrawLine(firePointPosition, hit.point, Color.red);
            Debug.Log("We hit " + hit.collider.name + " and did damage " + damage);
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Damage(damage);
            }
        }

        if (Time.time >= timeToSpawnEffect)
        {
            Vector3 hitPosition;
            Vector3 hitNormal;

            if (hit.collider == null)
            {
                hitPosition = (mousePosition - firePointPosition) * 30;
                hitNormal = new Vector3(999, 999, 999);
            }
            else
            {
                hitPosition = hit.point;
                hitNormal = hit.normal;
            }

            Effect(hitPosition, hitNormal);
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
    }

    void Effect(Vector3 hitPosition, Vector3 hitNormal)
    {
        Transform trail = Instantiate(bulletTrailPrefab, firePoint.position, firePoint.rotation) as Transform;
        LineRenderer lineRenderer = trail.GetComponent<LineRenderer>();

        if (lineRenderer != null)
        {
            //Set positions
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitPosition);
        }
        Destroy(trail.gameObject, 0.04f);

        if (hitNormal != new Vector3(999, 999, 999))
        {
            Transform impactParticles = Instantiate(hitPrefab, hitPosition, Quaternion.FromToRotation(Vector3.right, hitNormal)) as Transform;
            Destroy(impactParticles.gameObject, 1f);
        }

        Transform muzzleFlashClone = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation) as Transform;
        muzzleFlashClone.parent = firePoint;
        float size = Random.Range(0.6f, 0.9f);
        muzzleFlashClone.localScale = new Vector3(size, size, size);
        Destroy(muzzleFlashClone.gameObject, 0.02f);

        //camera shake
        cameraShake.Shake(camShakeAmount, camShakeLength);

        //sound
        audioManager.PlaySound(shootSound);
    }
}
