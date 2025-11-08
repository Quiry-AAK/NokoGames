using UnityEngine;

namespace NokoGames.Assets._Main.Scripts.Player.Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private CharacterController controller;
        [SerializeField] private float moveSpeed;

        private VariableJoystick variableJoystick;

        private void Awake()
        {
            variableJoystick = FindAnyObjectByType<VariableJoystick>();
        }

        private void FixedUpdate()
        {
            var moveDir = new Vector3(variableJoystick.Direction.x, 0f, variableJoystick.Direction.y);
            var isMoving = moveDir.sqrMagnitude > 0.01f;
            if (isMoving)
            {
                controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);

                // Rotate
                transform.forward = moveDir;
            }
            animator.SetBool("Running", isMoving);
        }
    }
}