using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool _isDeath = false;

    [SerializeField] private float _hp = 100f;
    [SerializeField] private float _armor = 1f;
    [SerializeField] private ParticleSystem _particles;
    [SerializeField] private AudioSource _enemyDeadAudio;
    [SerializeField] private AudioSource _shot;

    public void TakeDamage(float damage)
    {
		if(_isDeath) return;

		_hp -= damage - _armor;
		_shot.Play();

		Instantiate(_particles, transform.position, Quaternion.identity);

        if (_hp < 0f)
        {
			_isDeath = true;
			if (_enemyDeadAudio != null) _enemyDeadAudio.Play();
            GetComponent<Rigidbody>().isKinematic = false;
            StartCoroutine(Destroy(8f));
        }
    }

    private IEnumerator Destroy(float time)
    {
        while (true)
        {
			yield return new WaitForSeconds(time);
            Destroy(gameObject);
		}
	}
}
