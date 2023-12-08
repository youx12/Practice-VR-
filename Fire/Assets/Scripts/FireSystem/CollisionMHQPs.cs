using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionMHQPs : MonoBehaviour
{
    GameMain gameMain;
    // Start is called before the first frame update
    void Start()
    {
        gameMain = GameObject.FindObjectOfType<GameMain>();
    }
    void OnParticleCollision(GameObject other)
    {
        if(other.gameObject.name== "MieHuoQiSmoke"&&GameMain.isFire)
        {
            GameMain.isFire = false;
            gameMain.CloseFire();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
