using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponNet : MonoBehaviour
{

    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Text netsText;
    [SerializeField] int nets;
    [Header("Sonidos")]
    [SerializeField] private AudioSource soundPlayer;
    [SerializeField] private AudioClip soundNet;
    [SerializeField] private AudioClip soundFail;

    private void Awake()
    {
        soundPlayer = GetComponent<AudioSource>();
    }
    void Start()
    {
        netsText.text = "x" + nets.ToString();
    }

    public void Shoot()
    {
        if (nets > 0)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            nets--;
            netsText.text = "x" + nets.ToString();
            soundPlayer.PlayOneShot(soundNet, 0.5f);
        }
        else
        {
            soundPlayer.PlayOneShot(soundFail, 0.5f);
        }
    }

    public void AddNets()
    {
        nets += 3;
        netsText.text = "x" + nets.ToString();
    }
}
