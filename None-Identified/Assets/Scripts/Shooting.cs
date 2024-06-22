using UnityEditor;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] GameObject _weaponMuzzleFlash;
    [SerializeField] AudioClip _weaponShootSFX;
    [SerializeField] AudioSource _weaponAudioSource;
    [SerializeField] private Transform _weaponBarrelLocation;
    [SerializeField] private LayerMask _layerMask; // Слои, которые мы не будем игнорировать


    [Header("Параметры оружия | Weapon Settings")]
    [SerializeField] float _weaponDamage = 1f;
    [SerializeField] float _weaponFireRate = 1f;
    [SerializeField] float _weaponRange = 10f;
    [SerializeField, Min(1)] int _weaponCountShoot = 1;
    [SerializeField] private bool _weaponUseSpreadOn;
    [SerializeField, Min(0)] private float _weaponSpreadFactor = 1f;

    void Update()
    {
        Shoot();
        Aim();
    }

    void Shoot() {
        if (Input.GetButtonDown("Fire1")) {
            RaycastHit hit;
            var direction = _weaponUseSpreadOn ? _weaponBarrelLocation.forward + CalculateSpread() : _weaponBarrelLocation.forward;
            var ray = new Ray(_weaponBarrelLocation.position, direction);

            if (Physics.Raycast(ray, out hit, _weaponRange, _layerMask)) {
                Debug.Log("Попал =)" + hit.collider);
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
}
