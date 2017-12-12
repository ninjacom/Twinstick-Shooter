using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {
    // How many times should I be hit before I die
    public int health = 2;
    // When the enemy dies, we play an explosion
    public Transform explosion;
    // What sound to play when hit
    public AudioClip hitSound;

    void OnCollisionEnter2D(Collision2D theCollision)
    {
        // Uncomment this line to check for collision
        //Debug.Log("Hit"+ theCollision.gameObject.name);
        // this line looks for "laser" in the names of
        // anything collided.
        if (theCollision.gameObject.name.Contains("Laser"))
        {
            LaserBehaviour laser = theCollision.gameObject.GetComponent("LaserBehaviour") as LaserBehaviour;
            health -= laser.damage;
            Destroy(theCollision.gameObject);
            // Plays a sound from this object's AudioSource
            GetComponent<AudioSource>().PlayOneShot(hitSound);
        }
        if (health <= 0)
        {
            Destroy(this.gameObject);
            // Check if explosion was set
            if (explosion)
            {
                GameObject exploder = ((Transform)Instantiate(explosion, this.
                transform.position, this.transform.rotation)).gameObject;
                Destroy(exploder, 2.0f);
            }
        }
        GameController controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        controller.KilledEnemy();
        controller.IncreaseScore(10);
    }
}
