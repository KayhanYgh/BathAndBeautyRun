using UnityEngine;

public class ExplosionForce : MonoBehaviour
{
    public float explosionForce;
    public float upwardExplosionForce;

    public float radius;
    public Collider[] objects;

    void OnEnable()
    {
        //  Collider[] objects = UnityEngine.Physics.OverlapSphere(transform.position, radius);

        foreach (Collider h in objects)
        {
            Rigidbody r = h.GetComponent<Rigidbody>();
            if (r != null)
            {
                r.AddExplosionForce(explosionForce, transform.position, radius, upwardExplosionForce);
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}