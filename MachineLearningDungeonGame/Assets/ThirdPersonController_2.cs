using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
	[RequireComponent(typeof(PlayerInput))]
#endif

	public class ThirdPersonController_2 : MonoBehaviour
	{
		//[SerializeField] private GameObject sword;
		private CapsuleCollider swordCollider;

		[SerializeField] private GameObject sword;
		[SerializeField] private float backwardSpeed = 3.0f;
		[SerializeField] private float runningSpeed = 7.0f;
		[SerializeField] private float speedChangeRate = 10.0f;
		[SerializeField] private float rotationSmoothTimeWalk = 0.7f;

		private float rotationSmoothTime;
		private float _speed;
		private float turnSmoothVelocity;
		private float  targetSpeed;
		private float angle;

		private PlayerInput _playerInput;
		private Animator _animator;
		private CharacterController _controller;
		private StarterAssetsInputs _input;
		private GameObject _mainCamera;

		private bool _hasAnimator;
		private bool performingAttack;
		private bool performingDoubleAttack;

		private int _animIDSpeed;
		private int _animIDAttack;
		private int _animIDDoubleAttack;
		private int _animIDIsTurning;

		private Vector3 moveDirection;

		private void Awake()
		{
			// get a reference to our main camera
			if (_mainCamera == null)
			{
				_mainCamera = GameObject.FindGameObjectWithTag("PlayerFollowCamera");
			}
		}

		private void Start()
		{
			_hasAnimator = TryGetComponent(out _animator);
			_controller = GetComponent<CharacterController>();
			_input = GetComponent<StarterAssetsInputs>();
			_playerInput = GetComponent<PlayerInput>();
			swordCollider = sword.GetComponent<CapsuleCollider>();

			AssignAnimationIDs();
		}

		private void Update()
		{
			Attack();
			Move();

		}

		private void AssignAnimationIDs()
		{
			_animIDSpeed = Animator.StringToHash("Speed");
			_animIDAttack = Animator.StringToHash("Attack");
			_animIDDoubleAttack = Animator.StringToHash("DoubleAttack");
			_animIDIsTurning = Animator.StringToHash("isTurning");
		}

		private void Move()
		{
			//if (_input.move.y == 0)
   //         {
			//	rotationSmoothTime = 0.3f;
			//}
   //         else
   //         {
			//	rotationSmoothTime = rotationSmoothTimeWalk;
			//}
			rotationSmoothTime = rotationSmoothTimeWalk;

			if (_input.move == Vector2.zero || performingAttack)
			{
				targetSpeed = 0f;
				_speed = 0f;
			}
			else if(_input.move.y < 0)
            {
				targetSpeed = backwardSpeed;
			}
            else
            {
				targetSpeed = runningSpeed;

			}

			_animator.SetBool(_animIDIsTurning, false);


			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
			float speedOffset = 0.2f;

			float inputMagnitude = _input.move.magnitude;
			if ((currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset) && _input.move != Vector2.zero)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * speedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}

			Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
			if (_input.move != Vector2.zero)
			{
				float targetAngle = Mathf.Atan2(_input.move.x , 0f) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;

				angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, rotationSmoothTime);
				if (_input.move.x == 0)
				{
					transform.rotation = Quaternion.Euler(0f,transform.eulerAngles.y,0f);
				}
				else
                {
					transform.rotation = Quaternion.Euler(0f, angle, 0f);
				}


				if (_input.move.x != 0 && _input.move.y == 0)
                {
					 moveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.zero;
					_animator.SetBool(_animIDIsTurning, true);
				}
				else if(_input.move.y < 0 && !performingAttack)
                {
					moveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.back;
				}
				else
				{
					 moveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
				}
				_controller.Move(moveDirection.normalized * _speed * Time.deltaTime);

			}
			_animator.SetFloat(_animIDSpeed, _speed);


		}
		private void Attack()
		{
			if (!performingDoubleAttack)
			{
				if (_input.attack)
				{
					_animator.SetBool(_animIDAttack, true);
					performingAttack = true;
				}
			}


		}
		private void FinishAttack()
		{
			if (_input.attack)
			{
				_animator.SetBool(_animIDAttack, false);
				_animator.SetBool(_animIDDoubleAttack, true);
				performingDoubleAttack = true;
			}
			else
			{
				_animator.SetBool(_animIDAttack, false);
				performingAttack = false;
			}

		}
		private void FinishDoubleAttack()
		{
			if (_input.attack)
			{
				_animator.SetBool(_animIDDoubleAttack, false);
				_animator.SetBool(_animIDAttack, true);
				performingDoubleAttack = false;
			}
			else
			{
				_animator.SetBool(_animIDDoubleAttack, false);
				performingAttack = false;
				performingDoubleAttack = false;
			}
		}
		private void TurnOnSwordCollider()
		{
			swordCollider.enabled = true;

		}
		private void TurnOffSwordCollider()
		{
			swordCollider.enabled = false;

		}
	}
}
