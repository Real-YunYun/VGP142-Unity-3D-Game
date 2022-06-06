using System;
using UnityEngine;

public enum EntityType
{
    Binary   = 0b_0000_0000,
    SBinary  = 0b_0000_0001,
    Pointer  = 0b_0000_0010,
    BPointer = 0b_0000_0011,
    NPointer = 0b_0000_0100,
    Constant = 0b_0000_0101,
    
}

[Serializable]
public struct EntityStats
{
    [Tooltip("What Type of Entity is this")]
    public EntityType Type;
    [Tooltip("How long this will take the Director to build (s)")]
    public float BuildTime;
    [Tooltip("How much this will cost the Director queue (credits)")]
    public float BuildCost;
}

public class Entity : MonoBehaviour
{
    [Header("Entity Parameters")]
    [Tooltip("Dictates if this Entity is Invincable")]
    [SerializeField] private bool Invincible = false;
    [Tooltip("Dictates this Entity Health")]
    [SerializeField] private int _health;
    [Tooltip("Dictates What this Entity stats for the Director")]
    [SerializeField] public EntityStats Stats;
    [Tooltip("Particle Effect for when this Entity dies")]
    [SerializeField] public GameObject DeathParticle;
    [HideInInspector] public int Health
    {
        get { return _health; }
        set { _health = value; }
    }

    //Delegates and Events
    public delegate void OnDeathDelegate(GameObject entity);
    public event OnDeathDelegate OnDeathEvent;

    public virtual void TakeDamage()
    {
        if (!Invincible)
        {
            Health -= 1;
            if (Health <= 0) Death();
        }
    }

    public virtual void Death()
    {
        OnDeathEvent(this.gameObject);
        GameManager.Instance.Data.Bits += (int)Stats.BuildCost;
        Instantiate(DeathParticle, transform.position, DeathParticle.transform.rotation);
        Destroy(gameObject);
    }
}
