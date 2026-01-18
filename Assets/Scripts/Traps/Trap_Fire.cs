using System.Collections;
using UnityEngine;

public class Trap_Fire : MonoBehaviour
{
    [SerializeField] private float offDuration;
    [SerializeField] private Trap_Fire_Button fire_Button;
    private Animator anim;
    private CapsuleCollider2D fireCollider;
    private bool isActive;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        fireCollider = GetComponent<CapsuleCollider2D>();
    }
    private void Start()
    {
        if (fire_Button ==null)
        {
            //Debug.LogWarning("Trap_Fire: Fire Button reference is missing!" + gameObject.name + "!");
        }
        SetFire(true);  
    }
    public void SwitchOffFire() 
    { 
        if (isActive == false) 
            return;
        StartCoroutine(FireCoroutine());
    }
    private IEnumerator FireCoroutine()
    {
        SetFire(false);
        yield return new WaitForSeconds(offDuration);
        SetFire(true);
    }

    private void SetFire(bool active)   
    {
        anim.SetBool("active", active);
        fireCollider.enabled = active;
        isActive = active;
    }
}
