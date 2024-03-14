using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    #region Variables

    [Header("Player Info")]
    private HealthSystem playerhealth;
    private GameObject player;

    [Header("Projectile Info")]
    [SerializeField] bool specificTarget = true;
    [SerializeField] GameObject startParticle;
    [SerializeField] GameObject endParticle;
    [SerializeField] float damageAmount = 25;
    [SerializeField] float projectileRange = 2;
    private Vector3 target;
    private bool startProjectile = false;

    #endregion

    #region Getters and Setters

    public GameObject SetPlayer { set { player = value; } }
    public HealthSystem SetHealth { set { playerhealth = value; } }

    #endregion

    #region Start Functions

    private void Awake()
    {
        // target = player.transform.position;
    }

    #endregion

    #region Update Functions

    void Update()
    {
        if (!startProjectile) return;

        if (specificTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, target + new Vector3(0, 2, 0), 10 * Time.deltaTime);

            float travelDistance = Vector3.Distance(transform.position, target);

            if (travelDistance <= projectileRange)
                TurnOff();
        }
        else
        {
            transform.Translate(Vector3.forward * 10 * Time.deltaTime);

            Invoke("TurnOff", 5f);
        }
    }

    #endregion

    #region Basic Functions

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
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

    public void SetData()
    {
        target = player.transform.position;

        startProjectile = true;
    }

    private void TurnOff()
    {
        gameObject.SetActive(false);
    }

    #endregion
}
