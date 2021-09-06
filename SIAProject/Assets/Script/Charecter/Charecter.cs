using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Charecter : MonoBehaviour
{
    public int hp { get; protected set; }
    public float speed { get; protected set; }
    protected virtual void Init(int hp, float speed)
    {
        this.hp = hp;
        this.speed = speed;
    }
    public virtual void Attack() 
    { 

    }
}
