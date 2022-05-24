using System;
using UnityEngine;
using UnityEditor;

[Serializable]
public class OnHit
{
    //Any OnHit Params
    [HideInInspector] public bool IsOnHit;
    [HideInInspector] public bool HasDoT;
    [HideInInspector] public float DoT_Duration;
    [HideInInspector] public int Damage_Flat;
    [HideInInspector] public float Damage_Coefficient;
}

[Serializable]
public class OnKill
{
    //Any OnKill Params
    [HideInInspector] public bool IsOnKill;
}

[Serializable]
public class AoE
{
    //Area of Effect Params
    [HideInInspector] public bool HasAoE;
    [HideInInspector] public float Radius;
    [HideInInspector] public int Damage_Flat;
    [HideInInspector] public float Damage_Coefficient;
}

[Serializable]
public class Piercing
{
    //Any Piercing related Parameters
    [HideInInspector] public bool IsPiercing;
    [HideInInspector] public int Piercing_Power;
}

[Serializable]
public class Sticky
{
    //Any Sticky related Parameters
    [HideInInspector] public bool IsSticky;
    [HideInInspector] public float Sticky_Duration;
}

[Serializable]
public class Other
{
    //Very Niche things
    [HideInInspector] public bool Death;
}

[RequireComponent(typeof(Collider))]
public class Effect : MonoBehaviour
{
    [SerializeField] public string Name;                //Name of the Effect
    [SerializeField] public int Damage;                 //Damage Dealt
    [SerializeField] public GameObject Particle;        //Particle of the Effect(s)
    [SerializeField] public bool CanProc;               //Particle of the Effect(s)
    [SerializeField] public Item[] ProcEffects;         //Use a pre-made Item(s)
    [HideInInspector] public Collider Target;           //Target Hit

    [HideInInspector] public OnHit OnHit;
    [HideInInspector] public OnKill OnKill;
    [HideInInspector] public AoE AoE;
    [HideInInspector] public Piercing Piercing;
    [HideInInspector] public Sticky Sticky;
    [HideInInspector] public Other Other;

    private void ApplyEffect(Collider other)
    {
        if (Other.Death) other.gameObject.GetComponent<EnemyFollow>().Death();
        if (Target != other)
        {
            Target = other;
            if (this.Piercing.IsPiercing)
            {
                if (Piercing.Piercing_Power <= 0) Destroy(gameObject);
                Piercing.Piercing_Power--;
            }
        }
        if (OnHit.IsOnHit)
        {
            Instantiate(Particle, transform.position, transform.rotation);
            if (OnHit.Damage_Flat > 0) other.gameObject.GetComponent<EnemyFollow>().TakeDamage(OnHit.Damage_Flat, "head");
            else if (OnHit.Damage_Coefficient > 0) other.gameObject.GetComponent<EnemyFollow>().TakeDamage((int)(Damage * (OnHit.Damage_Coefficient / 100)), "head");
            if (AoE.HasAoE) ApplyAoe(other.transform, Damage);
        }
        other.gameObject.GetComponent<EnemyFollow>().TakeDamage(Damage, "head");
        if (OnKill.IsOnKill)
        {
            if (other.gameObject.GetComponent<EnemyFollow>().Health <= 0)
            {
                Instantiate(Particle, transform.position, transform.rotation);
                if (AoE.HasAoE) ApplyAoe(other.transform, Damage);
            }
        }
        if (!Sticky.IsSticky && !Piercing.IsPiercing) Destroy(gameObject);
        if (Sticky.IsSticky)
        {
            gameObject.GetComponent<Collider>().isTrigger = true;
            gameObject.transform.position = other.gameObject.transform.position;
            Destroy(gameObject, Sticky.Sticky_Duration);
        }
        if (CanProc) foreach (Item proc in ProcEffects) if (proc.Proc.ProcChance >= UnityEngine.Random.Range(0, 100))Instantiate(proc.gameObject, transform.position, transform.rotation);
    }

    public void ApplyAoe(Transform origin, int Damage)
    {
        RaycastHit[] hits = Physics.SphereCastAll(origin.position, AoE.Radius, origin.forward, 0);
        foreach (var hit in hits)
        {
            if (hit.collider.gameObject.tag != "Enemy") continue;
            if (AoE.Damage_Flat > 0) hit.collider.gameObject.GetComponent<EnemyFollow>().TakeDamage(AoE.Damage_Flat, "head");
            else if (AoE.Damage_Coefficient > 0) hit.collider.gameObject.GetComponent<EnemyFollow>().TakeDamage((int)(Damage * (AoE.Damage_Coefficient / 100)), "head");

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy") ApplyEffect(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy") ApplyEffect(other);
    }

    //Unity Editor Things
#if UNITY_EDITOR
    [CustomEditor(typeof(Effect))]
    public class Effect_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GUILayout.Space(10);
            var EffectGUI = target as Effect;

            //OnHit
            EffectGUI.OnHit.IsOnHit = GUILayout.Toggle(EffectGUI.OnHit.IsOnHit, "On Hit");
            if (EffectGUI.OnHit.IsOnHit)
            {
                EffectGUI.OnHit.HasDoT = EditorGUILayout.Toggle("DoT: ", EffectGUI.OnHit.HasDoT);
                if (EffectGUI.OnHit.HasDoT) EffectGUI.OnHit.DoT_Duration = EditorGUILayout.FloatField("Duration: ", EffectGUI.OnHit.DoT_Duration);
                EffectGUI.OnHit.Damage_Flat = EditorGUILayout.IntField("Flat Damage: ", EffectGUI.OnHit.Damage_Flat);
                EffectGUI.OnHit.Damage_Coefficient = EditorGUILayout.FloatField("Percentage Damage (%): ", EffectGUI.OnHit.Damage_Coefficient);
            }
            GUILayout.Space(10);
            //OnKill
            EffectGUI.OnKill.IsOnKill = GUILayout.Toggle(EffectGUI.OnKill.IsOnKill, "On Kill");
            GUILayout.Space(10);

            //AoE
            EffectGUI.AoE.HasAoE = GUILayout.Toggle(EffectGUI.AoE.HasAoE, "Area of Effect");
            if (EffectGUI.AoE.HasAoE)
            {
                EffectGUI.AoE.Radius = EditorGUILayout.FloatField("AOe Radius: ", EffectGUI.AoE.Radius);
                EffectGUI.AoE.Damage_Flat = EditorGUILayout.IntField("Flat Damage: ", EffectGUI.AoE.Damage_Flat);
                EffectGUI.AoE.Damage_Coefficient = EditorGUILayout.FloatField("Percentage Damage (%): ", EffectGUI.AoE.Damage_Coefficient);
            }
            GUILayout.Space(10);

            //Sticky
            EffectGUI.Piercing.IsPiercing = GUILayout.Toggle(EffectGUI.Piercing.IsPiercing, "Piercing");
            if (EffectGUI.Piercing.IsPiercing)
            {
                EffectGUI.Sticky.IsSticky = false;
                EffectGUI.Piercing.Piercing_Power = EditorGUILayout.IntField("Piercing Power: ", EffectGUI.Piercing.Piercing_Power);
            }
                GUILayout.Space(10);

            //Sticky
            EffectGUI.Sticky.IsSticky = GUILayout.Toggle(EffectGUI.Sticky.IsSticky, "Sticky");
            if (EffectGUI.Sticky.IsSticky)
            {
                EffectGUI.Piercing.IsPiercing = false;
                EffectGUI.Sticky.Sticky_Duration = EditorGUILayout.FloatField("Duration: ", EffectGUI.Sticky.Sticky_Duration);
            }
            GUILayout.Space(10);

            //Other
            EffectGUI.Other.Death = GUILayout.Toggle(EffectGUI.Other.Death, "Death");
            GUILayout.Space(10);
        }
    }
#endif
}
