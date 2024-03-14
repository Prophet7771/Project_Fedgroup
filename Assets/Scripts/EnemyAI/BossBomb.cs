using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBomb : MonoBehaviour
{
    #region Variables

    [SerializeField] Rigidbody rb;
    Vector3 direction;
    [SerializeField] float damageAmount = 35;

    [Header("FVX")]
    [SerializeField] GameObject startParticle;
    [SerializeField] GameObject endParticle;

    [Header("Player Info")]
    private HealthSystem playerhealth;
    private GameObject player;

    #endregion

    #region Start Functions

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        direction = new Vector3(Random.Range(Random.Range(-360, -150), Random.Range(150, 361)), Random.Range(100, 200), Random.Range(Random.Range(-360, -150), Random.Range(150, 361)));
        rb.AddForce(direction, ForceMode.Impulse);
        Invoke("TurnOff", 7f);
    }

    #endregion

    #region Update Functions

    void Update()
    {

    }

    #endregion

    #region Start Functions

    private void TurnOff()
    {
        gameObject.SetActive(false);
    }

    #endregion

    #region Basic Functions

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player = other.gameObject;
            playerhealth = player.GetComponent<HealthSystem>();

            Explode();
        }
    }

    private void Explode()
    {
        startParticle.SetActive(false);
        endParticle.SetActive(true);

        playerhealth.TakeDamage(damageAmount);

        Invoke("TurnOff", 1f);
    }

    #endregion
}
