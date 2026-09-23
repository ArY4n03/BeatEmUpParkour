using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    private Animator animator;
    private PlayerController playerController;
    private ParkourSystem parkourSystem;
    private bool isAttacking = false;


    private float combatAmt = 0f;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        parkourSystem = GetComponent<ParkourSystem>();
    }


    private void Update()
    {

    }


    private void handleAttack()
    {

    }
}

