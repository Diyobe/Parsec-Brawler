using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDestructible : MonoBehaviour
{
    /*private void OnTriggerEnter(Collision other)
    {
        if (other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2" || other.gameObject.tag == "Player3" || other.gameObject.tag == "Player4")
        {
            Destroy(this.gameObject);
        }
        
    }*/

    public void Damage()
    {
        Destroy(this.gameObject);
    }
}
