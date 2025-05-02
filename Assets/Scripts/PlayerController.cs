using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    float chargeTimer; // 蓄力时间

    [Header("蓄力跳参数")]
    public float maxChargeTime = 1.5f; // 缩短最大蓄力时间
    public float minJumpPower = 8f;    // 提高最小跳跃力度
    public float maxJumpPower = 20f;   // 提高最大跳跃力度
    public float jumpHeightMultiplier = 1.2f; // 降低垂直跳跃倍率
    public float groundCheckDistance = 0.1f;
    public LayerMask groundLayer;
    public float fastFallGravity = 30f; // 快速下落时的重力
    public float normalGravity = 10f;   // 正常重力

    bool isJumping;
    bool isGrounded;
    bool isFalling = false;

    Vector3 originalScale;
    Vector3 compressedScale;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = 1;
        rb.drag = 0;
        originalScale = transform.localScale;
        compressedScale = new Vector3(originalScale.x, originalScale.y * 0.5f, originalScale.z);
    }

    void Update()
    {
        CheckGrounded();

        if (!isGrounded)
        {
            // 空中时检测下落状态
            isFalling = rb.velocity.y < 0;
            return;
        }

        // 蓄力阶段
        if (Input.GetKey(KeyCode.Mouse0))
        {
            chargeTimer = Mathf.Min(chargeTimer + Time.deltaTime, maxChargeTime);
            // 平滑压缩角色
            float compressRatio = Mathf.Lerp(1f, 0.5f, chargeTimer / maxChargeTime);
            transform.localScale = new Vector3(
                originalScale.x, 
                originalScale.y * compressRatio, 
                originalScale.z
            );
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && isGrounded)
        {
            isJumping = true;
            transform.localScale = originalScale;
        }
    }

    void FixedUpdate()
    {
        // 应用不同重力
        if (isFalling)
        {
            rb.AddForce(Vector3.down * fastFallGravity, ForceMode.Acceleration);
        }
        else
        {
            rb.AddForce(Vector3.down * normalGravity, ForceMode.Acceleration);
        }

        if (isJumping)
        {
            PerformJump();
            isJumping = false;
        }
    }

    void PerformJump()
    {
        float chargeRatio = Mathf.Clamp01(chargeTimer / maxChargeTime);
        float currentJumpPower = Mathf.Lerp(minJumpPower, maxJumpPower, chargeRatio);

        Vector3 jumpDirection = GameManager.S.PlayerIsFacingXAxis ? -Vector3.right : Vector3.forward;
        
        // 更符合跳一跳的跳跃曲线：水平速度恒定，垂直速度基于蓄力
        Vector3 jumpForce = new Vector3(
            jumpDirection.x * currentJumpPower * 0.8f, // 降低水平速度比例
            currentJumpPower * jumpHeightMultiplier,
            jumpDirection.z * currentJumpPower * 0.8f
        );

        // 重置速度确保每次跳跃一致
        rb.velocity = Vector3.zero;
        rb.AddForce(jumpForce, ForceMode.VelocityChange);

        chargeTimer = 0f;
    }

    void CheckGrounded()
    {
        RaycastHit hit;
        isGrounded = Physics.Raycast(
            transform.position + Vector3.up * 0.1f, 
            Vector3.down, 
            out hit, 
            groundCheckDistance, 
            groundLayer
        );
        
        if (isGrounded)
        {
            isFalling = false;
        }
        
        Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, Color.red);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                if (contact.normal.y > 0.7f)
                {
                    GameManager.S.HitGround(transform.position);
                    break;
                }
            }
        }
        else if (collision.gameObject.name != "StartCube")
        {
            GameManager.S.GameOver();
        }
    }
}