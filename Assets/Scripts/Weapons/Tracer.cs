using UnityEngine;

public class Tracer : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject impactParticel;

    #endregion
    private void Start() => Destroy(gameObject, 1f);

    void Update() => transform.position += transform.forward * 200 * Time.deltaTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("HIT ENEMY");
            // Instantiate(impactParticel, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
