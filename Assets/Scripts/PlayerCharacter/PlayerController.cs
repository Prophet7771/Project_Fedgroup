using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Variables

    [Header("Player Data")]
    Rigidbody rb;
    Camera playerCam;
    Transform camTransform;

    Vector3 camForward;
    Vector3 move;

    float forwardAmount;
    float turnAmount;
    bool isIdle = true;
    HealthSystem playerHealth;

    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private GameObject playerMesh;

    [Header("Weapons")]
    private int currentWeapon = 0;
    [SerializeField] GameObject assaultRifle;
    [SerializeField] GameObject sniper;
    [SerializeField] GameObject shotgun;

    [Header("Player Input")]
    PlayerInputActions inputActions;
    Vector3 moveInput;

    [Header("Raycast Data")]
    Ray aimRay;
    bool didRayHit;
    RaycastHit hit;
    Vector2 mousePos;

    #endregion

    #region Event Handlers

    private void OnEnable()
    {
        inputActions.Enable();

        // Using new input system to handle player mouse position and execute the Mouse Move functionality as the mouse moves.
        inputActions.BasicActions.MousePosition.performed += OnMouseMove;
        inputActions.BasicActions.Scroll.performed += SwitchWeaponsScroll;
    }

    private void OnDisable()
    {
        inputActions.BasicActions.MousePosition.performed -= OnMouseMove;
        inputActions.BasicActions.Scroll.performed -= SwitchWeaponsScroll;

        inputActions.Disable();
    }

    #endregion

    #region Start Functions

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        rb = GetComponent<Rigidbody>();
        playerCam = Camera.main;
        camTransform = playerCam.transform;
        playerHealth = GetComponent<HealthSystem>();
    }

    #endregion

    #region Update Functions

    private void Update()
    {
        if (playerHealth.GetHealth <= 0)
        {
            assaultRifle.SetActive(false);
            sniper.SetActive(false);
            shotgun.SetActive(false);

            inputActions.BasicActions.MousePosition.performed -= OnMouseMove;
            inputActions.BasicActions.Scroll.performed -= SwitchWeaponsScroll;

            return;
        }

        aimRay = playerCam.ScreenPointToRay(Mouse.current.position.ReadValue());

        didRayHit = Physics.Raycast(aimRay, out hit);

        // Checking if ray hit what we want it to hit before rotating player.
        if (didRayHit)
        {
            // Finding basic location for player to look towards.
            Vector3 lookTowards = new Vector3(hit.point.x - playerMesh.transform.position.x, 0, hit.point.z - playerMesh.transform.position.z);

            // Rotating player in the right direction.
            playerMesh.transform.rotation = Quaternion.Slerp(playerMesh.transform.rotation, Quaternion.LookRotation(lookTowards), 15 * Time.deltaTime);
        }

        Move();
    }

    #endregion

    #region Basic Functions

    private void Move()
    {
        moveInput = inputActions.BasicActions.Movement.ReadValue<Vector3>();

        isIdle = moveInput == Vector3.zero;

        Vector3 movedirection = new Vector3(moveInput.x, 0, moveInput.z);

        rb.velocity = movedirection.normalized * moveSpeed; // If character spins, freeze the rotation on all axis
    }

    private void OnMouseMove(InputAction.CallbackContext ctx) => mousePos = playerCam.ScreenToWorldPoint(ctx.ReadValue<Vector2>());

    private void SwitchWeaponsScroll(InputAction.CallbackContext ctx)
    {
        float scroll = ctx.ReadValue<Vector2>().y;

        if (scroll > 0)
        {
            if (currentWeapon < 2)
                currentWeapon++;
        }
        else
        {
            if (currentWeapon > 0)
                currentWeapon--;
        }

        switch (currentWeapon)
        {
            case 0:
                assaultRifle.SetActive(true);
                sniper.SetActive(false);
                shotgun.SetActive(false);
                break;
            case 1:
                assaultRifle.SetActive(false);
                sniper.SetActive(true);
                shotgun.SetActive(false);
                break;
            case 2:
                assaultRifle.SetActive(false);
                sniper.SetActive(false);
                shotgun.SetActive(true);
                break;
        }
    }

    #endregion
}
