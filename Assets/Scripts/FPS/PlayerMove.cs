using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
    [SerializeField] private string horizontalName;
    [SerializeField] private string verticalName;

    [SerializeField] private float moveSpeed;

    private CharacterController charController;

    [SerializeField] private AnimationCurve jumpFallOff;
    [SerializeField] float jumpmultiplier;

    bool isJumping = false;

    private void Awake() {
        charController = GetComponent<CharacterController>();
    }

    private void Update() {
        PlayerMovement();
    }

    private void PlayerMovement() {
        float vertInput = Input.GetAxis(verticalName) * moveSpeed;
        float horizInput = Input.GetAxis(horizontalName) * moveSpeed;
        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horizInput;

        charController.SimpleMove(forwardMovement + rightMovement);

        JumpInput();
    }

    private void JumpInput() {
        if(Input.GetKeyDown(KeyCode.Space) && !isJumping) {
            isJumping = true;
            StartCoroutine(JumpEvent());
        }
    }

    private IEnumerator JumpEvent() {
        charController.slopeLimit = 90.0f;
        float timeInAir = 0f;
        do {
            float jumpForce = jumpFallOff.Evaluate(timeInAir);
            charController.Move(Vector3.up * jumpForce * jumpmultiplier * Time.deltaTime);
            timeInAir += Time.deltaTime;
            yield return null;
        } while (!charController.isGrounded && charController.collisionFlags != CollisionFlags.Above);
        charController.slopeLimit = 45f;
        isJumping = false;
    }
}
