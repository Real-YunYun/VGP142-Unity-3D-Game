using System;
using System.Collections;
using UnityEngine;
using UnityEditor;

[Serializable]
public class Proc
{
    //Any Proc Params
    [HideInInspector] public bool CanProc;
    [HideInInspector] public float ProcChance;
}

[Serializable]
public class Fuse
{
    //Any Fuse Params
    [HideInInspector] public bool HasFuse;
    [HideInInspector] public float Duration;
}

[RequireComponent(typeof(Collider))]
public class Item : MonoBehaviour
{
    [SerializeField] public string Name;                //Name of the Item
    [SerializeField] public int Damage;                 //Damage Dealt
    [HideInInspector] public Effect ItemEffect;         //Item Effect(s)
    [HideInInspector] public Collider Target;           //Target Hit

    [HideInInspector] public Fuse Fuse;
    [HideInInspector] public Proc Proc;

    private void Start()
    {
        ItemEffect = gameObject.GetComponent<Effect>();
        Activate();
    }

    private void OnCollisionEnter(Collision collision)
    {
          
    }

    private void OnCollisionStay(Collision collision)
    {
        
    }

    private void Activate()
    {
        if (Fuse.HasFuse) StartCoroutine("FuseDelay");
        else
        {
            Instantiate(ItemEffect.Particle, transform.position, transform.rotation);
            ItemEffect.ApplyAoe(transform, Damage);
            Destroy(gameObject);
        }
    }

    IEnumerator FuseDelay()
    {
        yield return new WaitForSeconds(Fuse.Duration);
        Instantiate(ItemEffect.Particle, transform.position, transform.rotation);
        ItemEffect.ApplyAoe(transform, Damage);
        Destroy(gameObject);
    }

    //Unity Editor Things
#if UNITY_EDITOR
    [CustomEditor(typeof(Item))]
    public class Item_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GUILayout.Space(10);
            var ItemGUI = target as Item;

            //Fuse
            ItemGUI.Proc.CanProc = GUILayout.Toggle(ItemGUI.Proc.CanProc, "Can Proc:");
            if (ItemGUI.Proc.CanProc) ItemGUI.Proc.ProcChance = EditorGUILayout.FloatField("Proc Chance (%): ", ItemGUI.Proc.ProcChance);
            GUILayout.Space(10);

            //Fuse
            ItemGUI.Fuse.HasFuse = GUILayout.Toggle(ItemGUI.Fuse.HasFuse, "Has Fuse:");
            if (ItemGUI.Fuse.HasFuse) ItemGUI.Fuse.Duration = EditorGUILayout.FloatField("Duration: ", ItemGUI.Fuse.Duration);
            GUILayout.Space(10);

        }
    }
#endif
}
