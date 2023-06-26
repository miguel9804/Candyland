using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PickupController : MonoBehaviour
{
    //Script Del Jugador
    public TextMeshProUGUI scoreBoard;
    int collectedCandies;

    //Dulces necesarios para la primera estrella
    [SerializeField] int maxCandies;
    [SerializeField] TextMeshProUGUI maxCandiesText;

    //Controlador del Juego, DEBE ser referenciado
    [SerializeField] GameObject levelDoor;

    AudioSource audioSource;
    [SerializeField] AudioClip pickSound;
    float randomPitch;

    // Start is called before the first frame update
    void Start()
    {
        collectedCandies = 0;
        audioSource = GetComponent<AudioSource>();
        maxCandiesText.text = maxCandies.ToString();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("PickUp"))
        {
            PickItem();
            Destroy(collider.gameObject);
        }
    }

    void PickItem()
    {
        audioSource.clip = pickSound;

        randomPitch = Random.Range(0f, 2f);
        audioSource.pitch = randomPitch;

        audioSource.Play();
        collectedCandies++;
        UpdateScoreText();

        if (collectedCandies >= maxCandies)
            levelDoor.GetComponent<WinController>().AddStar();
    }

    void UpdateScoreText()
    {
        scoreBoard.text = collectedCandies.ToString();
    }
}
