using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("InputAction")] [SerializeField]
    private InputAction moveAction;

    [SerializeField] private float moveInput;

    [Header("Animator")] [SerializeField] private Animator animator;

    [Header("Movements")] [SerializeField] private CharacterController controller;

    [SerializeField] private float moveSpeed;

    [SerializeField] private PlayerInput.ActionEvent gameOverAction;

    private void Start()
    {
        moveAction.Enable();
    }

    private void FixedUpdate()
    {
        if (!GameController.instance.hasGameStarted)
        {
            animator.SetFloat("Speed", 0);
            return;
        }
        moveInput = moveAction.ReadValue<float>();
        var moveX = moveInput < 0 ? -2 : moveInput == 0 ? 0 : 2;
        var pos = transform.position.x;
        if (moveX > 0)
        {
            if (pos >= 2f)
                moveX = 0;
        }
        else if (moveX < 0)
        {
            if (pos <= -2f)
                moveX = 0;
        }

        var movement = new Vector3(moveX, 0f, moveSpeed);
        controller.Move(movement * moveSpeed * Time.deltaTime);
        animator.SetFloat("Speed", 1);
    }

    public void GameOver()
    {
        gameOverAction.Invoke(default);
        animator.SetBool("Victory", true);
    }
}