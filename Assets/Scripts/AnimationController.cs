using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void PlayAnimation(int state) {
        animator.SetInteger("State", state);
    }
}
