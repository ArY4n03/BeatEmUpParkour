using UnityEngine;
using System.Collections;
using UnityEditor.Rendering;
using System.Collections.Generic;

public class CombatSystem : MonoBehaviour
{
    [SerializeField] private List<CombatActions> combatActions;
    private Animator animator;
    private PlayerController playerController;
    private ParkourSystem parkourSystem;

    private bool isAttacking = false;
    private int combatAmt = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        parkourSystem = GetComponent<ParkourSystem>();
    }

    private void Update()
    {
        if (playerController.atkInput.WasPressedThisFrame() && !isAttacking)
        {


            
            //animator.SetFloat("combatAmt", combatAmt);

            StartCoroutine(Attack(combatActions[combatAmt]));
            combatAmt = (combatAmt + 1) % 3;
        }
    }

    IEnumerator Attack(CombatActions action)
    {


        isAttacking = true;

        animator.CrossFade(action.animationName, 0.1f);


        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).IsName(action.animationName)
        );


        yield return new WaitUntil(() =>
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName(action.animationName) == false)
            {
                return true;
            }
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

            UnityEngine.Debug.Log("Normalized Time: " + state.normalizedTime);

            return state.normalizedTime >= 1f;
        });

        UnityEngine.Debug.Log("animation finished");
        isAttacking = false;
    }
 }