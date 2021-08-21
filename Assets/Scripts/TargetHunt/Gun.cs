using UnityEngine;

namespace TargetHunt
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private float _speed = 40;
        [SerializeField] private GameObject _bullet;
        [SerializeField] private Transform _barrel;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _audioClip;

        public void FireGun()
        {
            GameObject spawnedBullet = Instantiate(_bullet, _barrel.position, _barrel.rotation);
            spawnedBullet.GetComponent<Rigidbody>().velocity = _speed * _barrel.forward;
            _audioSource.PlayOneShot(_audioClip);
            Destroy(spawnedBullet, 2);
        }
    }
}
