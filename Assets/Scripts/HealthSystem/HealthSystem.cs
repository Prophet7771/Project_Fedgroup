using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    #region Variables

    private float health;
    private float lerpTimer;

    [SerializeField] private float maxHealth = 100f;

    [SerializeField] private bool canDie = false;
    [SerializeField] private Image healthImage;

    #endregion

    #region Getters and Setters

    public float GetHealth { get { return health; } }

    #endregion

    #region Delegates

    public delegate void OnDeathEvent();
    public OnDeathEvent onDeath;

    #endregion

    #region Start Functions

    void Start()
    {
        health = maxHealth;
    }

    #endregion

    #region Update Functions

    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
    }

    #endregion

    #region Event Handlers

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DamageOutput")
        {
            TakeDamage(Random.Range(5, 10));
        }

        if (other.gameObject.tag == "HealingObject")
        {
            RestoreHealth(Random.Range(5, 10));
        }
    }

    #endregion

    #region Basic Functions

    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;

        if (health <= 0)
        {
            if (canDie)
                onDeath?.Invoke();
            else
                Destroy(transform.gameObject);
        }

        if (healthImage != null)
            healthImage.fillAmount = health / maxHealth;
    }

    public void RestoreHealth(float heal)
    {
        health += heal;
        lerpTimer = 0f;

        if (healthImage != null)
            healthImage.fillAmount = health / maxHealth;
    }

    #endregion
}
