using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    public RumbleEventChannel channel;
    public InputProvider InputProvider;
    public GameObject Bullet;
    public Transform GunEdge;
    public Hand Hand;
    public int BulletSpeed = 150;
    public int BoostPower;
    public Rigidbody2D PlayerRigidbody;
    public float ShootMoveBlockTime;

    public void Start()
    {
        InputProvider.Shot += OnShoot;
    }

    public void OnDisable()
    {
        InputProvider.Shot -= OnShoot;
    }

    private void OnShoot()
    {
        channel.PerformRumble(1, 0, 0.25f);

        GameObject bullet = Instantiate(Bullet, GunEdge.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().AddForce(Hand.GrappleDirection * BulletSpeed);
        StartCoroutine(BulletWait(bullet));
        CameraEffects.PerformShake(40, 0.1f, 0.15f);
        GetComponent<Animator>().Play("Shoot");
    }


    private IEnumerator BulletWait(GameObject bullet)
    {
        yield return new WaitForSeconds(5);

        Destroy(bullet);
    }
}
