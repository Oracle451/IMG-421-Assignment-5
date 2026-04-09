using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(Rigidbody) )]
public class Projectile : MonoBehaviour
{
    const int  LOOKBACK_COUNT = 10;
    static List<Projectile> PROJECTILES = new List<Projectile>();

    [SerializeField]
    private bool _awake = true;

    public bool awake {
        get { return _awake; }
        private set { _awake = value; }
    }
 
    private Vector3       prevPos;
    private List<float>   deltas = new List<float>();
    private Rigidbody     rigid;
 
    void Start() {
        rigid = GetComponent<Rigidbody>();
        awake = true;
        prevPos = new Vector3(1000,1000,0);
        deltas.Add( 1000 );

        PROJECTILES.Add( this );
    }
 
    void FixedUpdate() {
        if ( rigid.isKinematic || !awake ) return;

        if (GameManager.Instance.currentDifficulty == GameManager.Difficulty.Hard)
        {
            rigid.AddForce(Vector3.down * 5f, ForceMode.Acceleration);
        }

        Vector3 deltaV3 = transform.position - prevPos;
        deltas.Add( deltaV3.magnitude );
        prevPos = transform.position;
  
        // Limit lookback; one of very few times that I’ll use while!
        while ( deltas.Count > LOOKBACK_COUNT ) {
            deltas.RemoveAt( 0 );
        }
 
        // Iterate over deltas and find the greatest one
        float maxDelta = 0;
        foreach ( float f in deltas ) {
            if ( f > maxDelta ) maxDelta = f;
        }
 
        // If the Projectile hasn’t moved more than the sleepThreshold        
        if ( maxDelta <= Physics.sleepThreshold ) {
            // Set awake to false and put the Rigidbody to sleep
            awake = false;
            rigid.Sleep();
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (GameManager.Instance.currentDifficulty == GameManager.Difficulty.Hard)
        {
            Explode();
        }
    }

    void Explode()
    {
        float explosionRadius = 5f;
        float explosionForce = 800f;
        float upwardsModifier = 0.5f; // adds a slight upward bias to the blast

        // Find everything in radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null && rb != rigid)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifier, ForceMode.Impulse);
            }
        }
        
        Destroy(gameObject);
    }

    private void OnDestroy() {
        PROJECTILES.Remove( this );                                          // c
    }

    static public void DESTROY_PROJECTILES() {                               // d
        foreach ( Projectile p in PROJECTILES ) {
            Destroy( p.gameObject );
        }
    }
}
