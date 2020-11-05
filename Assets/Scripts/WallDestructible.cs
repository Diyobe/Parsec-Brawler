using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDestructible : MonoBehaviour
{
    [SerializeField]
    ParticleSystem particleDestroy;
    /*private void OnTriggerEnter(Collision other)
    {
        if (other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2" || other.gameObject.tag == "Player3" || other.gameObject.tag == "Player4")
        {
            Destroy(this.gameObject);
        }
        
    }*/

    public void Damage(Vector3 impactPosition)
    {
        ParticleSystem particle = Instantiate(particleDestroy, this.transform.position, Quaternion.identity);
        particle.transform.LookAt(impactPosition);
        //particle.transform.eulerAngles += new Vector3(0, 0, 180);
        Destroy(particle.gameObject, 3f);
        Destroy(this.gameObject);
    }
}
