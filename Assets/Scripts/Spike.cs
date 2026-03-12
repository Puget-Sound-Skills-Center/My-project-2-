using UnityEngine;

public class Spikes : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.GetComponent<Player>())
        {

            collision.collider.GetComponent<Player>().Die();

        }

    }

}