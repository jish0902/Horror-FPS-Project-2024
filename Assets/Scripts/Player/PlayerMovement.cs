using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 30f; // �յ� �������� �ӵ�
    public float rotationSpeed = 10.0f; // �յ� �������� �ӵ�

    private Animator playerAnimator; // �÷��̾� ĳ������ �ִϸ�����
    private PlayerInput playerInput; // �÷��̾� �Է��� �˷��ִ� ������Ʈ
    private Rigidbody playerRigidbody; // �÷��̾� ĳ������ ������ٵ�

    private bool isRunning = false;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();

    }

    private void Move()
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = forward * playerInput.move.y + right * playerInput.move.x;
        Vector3 moveDistance = desiredMoveDirection.normalized * moveSpeed * 10 * Time.fixedDeltaTime;


        moveDistance.y = playerRigidbody.velocity.y;

        playerRigidbody.velocity = moveDistance;

        playerAnimator.SetFloat("Horizontal", playerInput.move.x);

        if(isRunning)
            playerAnimator.SetFloat("Vertical", playerInput.move.y * 2);
        else
            playerAnimator.SetFloat("Vertical", playerInput.move.y);

        if (desiredMoveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(desiredMoveDirection),
                Time.deltaTime * rotationSpeed
            );
        }


        if(Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
            moveSpeed = 50f;
        }
        else
        {
            isRunning= false;
            moveSpeed = 30f;
        }





        if (playerInput.move != Vector2.zero)
        {

            playerAnimator.SetBool("Move", true);
           
        }
        else
        {
            playerAnimator.SetBool("Move", false);

        }



    }



}
