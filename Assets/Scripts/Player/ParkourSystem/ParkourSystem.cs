using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class ParkourSystem : MonoBehaviour
{
    private PlayerController playerController;
    
    private EnvironmentScanner environmentScanner;
    private bool isAction = false;
    private Animator animator;
    private float rotationSpeed = 0.3f;
    [Header("---------Parkour Actions---------")]
    [SerializeField] private List<ParkourAction> parkourActions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        environmentScanner = GetComponent<EnvironmentScanner>();
        playerController = GetComponent<PlayerController>();

    }
    
    private void Update()
    {
        if (playerController.parkourAction.IsPressed() && !isAction)
        {
            var hitData = environmentScanner.ScanObstacle();

            if (hitData.forwardHitFound)
            {

                foreach (var action in parkourActions)
                {
                    if (action.IsValid(hitData.forwardHit, hitData.heightHit, transform))
                    {
                        StartCoroutine(DoParkour(action));
                        break;
                    }
                }
            }
        }
    }

    IEnumerator DoParkour(ParkourAction action)
    {
        isAction = true;

        UnityEngine.Debug.Log("1. Starting parkour: " + action.animName);

        playerController.SetControl(false);

        animator.CrossFade(action.animName, 0.1f);

        UnityEngine.Debug.Log("2. CrossFade called");

        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).IsName(action.animName)
        );

        UnityEngine.Debug.Log("3. Animation state reached");

        yield return new WaitUntil(() =>
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

            UnityEngine.Debug.Log("Normalized Time: " + state.normalizedTime);

            return state.normalizedTime >= 1f;
        });

        UnityEngine.Debug.Log("4. Animation finished");

        float timer = 0f;

        while (timer < 0.5f)
        {
            timer += Time.deltaTime;

            if (action.rotateTowardsObstacle)
            {
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    action.targetRotation,
                    rotationSpeed
                );
            }

            if (action.enableTargetMatching)
            {
                matchTarget(action);
            }

            yield return null;
        }

        UnityEngine.Debug.Log("5. Re-enabling player control");

        playerController.SetControl(true);

        UnityEngine.Debug.Log("control");

        isAction = false;
    }

    private void matchTarget(ParkourAction action)
    {
        animator.MatchTarget(
        action.matchPos,
        action.targetRotation,
        action.target,
        new MatchTargetWeightMask(Vector3.up, 1f),
        action.startMatch,
        action.targetMatch
);
    }
}