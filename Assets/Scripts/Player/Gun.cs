using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    public InputProvider InputProvider;
    public GameObject Bullet;
    public Transform GunEdge;
    public Hand Hand;
    public int BulletSpeed = 150;
    public int BoostPower;
    public Rigidbody2D PlayerRigidbody;
    public float ShootMoveBlockTime;
    public Audio GunShot;

    public RumbleEventChannel RumbleEventChannel;
    public CameraEventChannel CamEventChannel;

    public void OnEnable() 
    {
        InputProvider.Shot += OnShoot;
    }

    public void OnDisable()
    {
        InputProvider.Shot -= OnShoot;
    }

    private void OnShoot()
    {
        AudioMaster.Instance.Play(GunShot, MixerGroup.Player);
        RumbleEventChannel.PerformRumble(1, 0, 0.25f);

        GameObject bullet = Instantiate(Bullet, GunEdge.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().AddForce(Hand.GrappleDirection * BulletSpeed);
        StartCoroutine(BulletWait(bullet));
        CamEventChannel.PerformShake(40, 0.1f, 0.15f);
        GetComponent<Animator>().Play("Shoot");
    }


    private IEnumerator BulletWait(GameObject bullet)
    {
        yield return new WaitForSeconds(5);

        Destroy(bullet);
    }
}
