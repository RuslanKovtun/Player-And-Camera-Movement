using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(AudioSource))]
public class Movement : MonoBehaviour
{
    public static Transform singlton { get; private set; }

    private const string HORIZONTAL_AXIS = "Horizontal";
    private const string VERTICAL_AXIS = "Vertical";

    private bool _onGround;

    private AudioSource _audioSource;
    private Rigidbody _rigidbody;
    private Vector3 _velocity;

    [SerializeField] private LayerMask _collisionLayerMask;
    [Header("�����")]
    [SerializeField] private AudioClip[] _walkSounds;
    
    [Header("�������������")]
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private int _jumpForce;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();

        singlton = transform;
    }

    private void FixedUpdate()
    {
        MoveMode();
    }

    private void Update()
    {
        PlayerOnGround();
        JumpPlayer();
    }

    private void JumpPlayer()
    {
        if (_onGround)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TryJump();
            }
        }
    }

    private void PlayerOnGround()
    {
        if (Physics.SphereCast(transform.position, 0.25f, -transform.up, out var hitInfo, 1f, _collisionLayerMask))
        {
            _onGround = true;
        }
        else
        {
            _onGround = false;
        }
    }

    private void MoveMode()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            MoveTransform(_runSpeed);
        }
        else
        {
            MoveTransform(_walkSpeed);
        }
    }

    private void TryJump()
    {
        var jumpVelocity = Vector3.up * _jumpForce;
        _rigidbody.AddForce(jumpVelocity, ForceMode.Impulse);
    }

    private void MoveTransform(float speed)
    {
        float horizontal = Input.GetAxis(HORIZONTAL_AXIS) * speed * Time.deltaTime;
        float vertical = Input.GetAxis(VERTICAL_AXIS) * speed * Time.deltaTime;

        if (horizontal != 0 || vertical != 0)
        {
            _velocity = new Vector3(horizontal, 0f, vertical);
            transform.Translate(_velocity);

            PlayWalkSound();
        }
    }

    private void PlayWalkSound()
    {
        if (_onGround)
        {
            if (_audioSource.isPlaying || _walkSounds.Length == 0)
            {
                return;
            }

            var sound = _walkSounds[Random.Range(0, _walkSounds.Length)];
            _audioSource.PlayOneShot(sound);
        }
    }
}
