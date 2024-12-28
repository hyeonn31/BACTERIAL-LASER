using UnityEngine;
using UnityEngine.Events;

public class Mob : MonoBehaviour
{
    // 기존 변수들
    public float destroyDelay = 1f;
    public UnityEvent OnCreated;
    public UnityEvent OnDestroyed;
    private bool isDestroyed = false;

    // 점프 관련 변수들
    private Rigidbody rb;
    public float jumpForce = 5f;
    private bool isGrounded = true;
    public float jumpInterval = 3f;
    private float jumpTimer = 0f;

    private void Start()
    {
        // 기존 초기화
        OnCreated?.Invoke();
        MobManager.Instance.OnSpawned(this);

        // 점프 관련 초기화
        rb = GetComponent<Rigidbody>();
        jumpTimer = jumpInterval;
    }

    private void Update()
    {
        if (isDestroyed) // 파괴된 상태면 점프하지 않도록
            return;
            
        // 점프 로직
        jumpTimer -= Time.deltaTime;
        if (jumpTimer <= 0 && isGrounded)
        {
            Jump();
            jumpTimer = jumpInterval;
        }
    }

    // OnTriggerEnter 추가 (총알과의 충돌 처리용)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy();
        }
    }

    public void Destroy()
    {
        if (isDestroyed)
            return;
        isDestroyed = true;

        Destroy(gameObject, destroyDelay);

        OnDestroyed?.Invoke();
        MobManager.Instance.OnDestroyed(this);
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
} 