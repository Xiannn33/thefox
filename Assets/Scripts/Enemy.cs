using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;
    protected AudioSource deathSource;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        deathSource = GetComponent<AudioSource>();
    }
    public void Death()
    {
        Destroy(gameObject);
    }
    public void JumpOn()
    {
        deathSource.Play();
        anim.SetTrigger("death");
    }

}
