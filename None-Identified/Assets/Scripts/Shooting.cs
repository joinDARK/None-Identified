using System.Collections;
using UnityEditor;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] GameObject _weaponMuzzleFlash;
    [SerializeField] ParticleSystem _weaponMuzzleFlashParticle;
    [SerializeField] AudioClip _weaponShootSFX;
    [SerializeField] AudioSource _weaponAudioSource;
    [SerializeField] TrailRenderer _trailEffect;
    [SerializeField] Transform _posGun;
    [SerializeField] private Transform _weaponBarrelLocation;
    [SerializeField] private LayerMask _layerMask; // Слои, которые мы не будем игнорировать


    [Header("Параметры оружия | Weapon Settings")]
    [SerializeField] float _weaponDamage = 1f;
    [SerializeField] float _weaponFireRate = 1f;
    [SerializeField] float _weaponRange = 10f;
    [SerializeField, Min(1)] int _weaponCountShoot = 1;
    [SerializeField] private bool _weaponUseSpreadOn;
    [SerializeField, Min(0)] private float _weaponSpreadFactor = 1f;

	RaycastHit hit;
    TrailRenderer trail;

	void Update()
    {
        Shoot();
        Aim();
    }

    void Shoot() {
        if (Input.GetButtonDown("Fire1")) {
            var direction = _weaponUseSpreadOn ? _weaponBarrelLocation.forward + CalculateSpread() : _weaponBarrelLocation.forward;
            var ray = new Ray(_weaponBarrelLocation.position, direction);

			Instantiate(_weaponMuzzleFlashParticle, _posGun.position, Quaternion.identity);

			if (Physics.Raycast(ray, out hit, _weaponRange, _layerMask)) {
			    trail = Instantiate(_trailEffect, _posGun.position, Quaternion.identity);
			    StartCoroutine(SpawnTrail());

                if(hit.collider.TryGetComponent(out Enemy enemy))
                    enemy.TakeDamage(_weaponDamage);
            }
        }
    }

    void Aim() {
        if (Input.GetButtonDown("Fire2")) {
            
        }
    }

    private Vector3 CalculateSpread() {
        return new Vector3 {
            x = Random.Range(-_weaponSpreadFactor, _weaponSpreadFactor),
            y = Random.Range(-_weaponSpreadFactor, _weaponSpreadFactor),
            z = Random.Range(-_weaponSpreadFactor, _weaponSpreadFactor)
        };
    }

    private void OnDrawGizmosSelected() {
        var ray = new Ray(_weaponBarrelLocation.position, _weaponBarrelLocation.forward);

        if (Physics.Raycast(ray, out var hitInfo, _weaponRange, _layerMask)) {
            DrawRay(ray, hitInfo.point, hitInfo.distance, Color.red);
        }
        else {
            var hitPosition = ray.origin + ray.direction * _weaponRange;

            DrawRay(ray, hitPosition, _weaponRange, Color.green);
        }
    }

    private static void DrawRay(Ray ray, Vector3 hitPosition, float distance, Color color) {
        const float hitPointRadius = 0.20f;

        Debug.DrawRay(ray.origin, ray.direction * distance, color);

        Gizmos.color = color;
        Gizmos.DrawSphere(hitPosition, hitPointRadius);
    }

    private IEnumerator SpawnTrail()
    {
        float t = 0;
        Vector3 startPos = _posGun.position;

        while (t < 1)
        {
			trail.transform.position = Vector3.Lerp(_posGun.position, hit.point, t);
            t += Time.deltaTime / trail.time;

            yield return null;
        }

        trail.transform.position = hit.point;
        // Добавить эффект пули попадания
        //Instantiate();

    }
}
