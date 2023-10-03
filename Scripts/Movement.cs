using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(AudioSource))]
public class Movement : MonoBehaviour
{
    private const string HORIZONTAL_AXIS = "Horizontal";
    private const string VERTICAL_AXIS = "Vertical";

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
    }

    private void FixedUpdate()
    {
        MoveMode();
    }

    private void Update()
    {
        JumpPlayer();
    }

    private void JumpPlayer()
    {
        if (Physics.SphereCast(transform.position, 0.25f, -transform.up, out var hitInfo, 1f))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TryJump();
            }
        }
    }

    private void MoveMode()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            MovePlayer(_runSpeed);
        }
        else
        {
            MovePlayer(_walkSpeed);
        }
    }

    private void TryJump()
    {
        var jumpVelocity = Vector3.up * _jumpForce;
        _rigidbody.AddForce(jumpVelocity, ForceMode.VelocityChange);
    }

    private void MovePlayer(float speed)
    {
        float horizontal = Input.GetAxis(HORIZONTAL_AXIS) * speed * Time.fixedDeltaTime;
        float vertical = Input.GetAxis(VERTICAL_AXIS) * speed * Time.fixedDeltaTime;

        if (horizontal != 0 || vertical != 0)
        {
            _velocity = new Vector3(horizontal, 0f, vertical);
            transform.Translate(_velocity);

            PlayWalkSound();
        }
    }

    private void PlayWalkSound()
    {
        if (Physics.SphereCast(transform.position, 0.25f, -transform.up, out var hitInfo, 1f))
        {
            if (_audioSource.isPlaying || _walkSounds.Length == 0)
            {
                return;
            }

            int index = Random.Range(0, _walkSounds.Length);
            _audioSource?.PlayOneShot(_walkSounds[index]);
        }
    }
}
