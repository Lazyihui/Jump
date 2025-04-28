using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody rb;

    float timer;

    public float maxChargeTime;
    bool doJump;
    bool isGround = true;

    public float jumpPower;

    Vector3 originalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = 1; // 确保质量合理（默认1）
        rb.drag = 0;  // 空气阻力设为0
        originalScale = transform.localScale;
    }

    void Update()
    {


        if (Input.GetKey(KeyCode.Mouse0) && timer < maxChargeTime)
        {
            timer += Time.deltaTime;
            // 根据 timer 的蓄力进度（timer / maxChargeTime）计算压缩比例
            // 根据 timer 动态计算压缩比例（范围：0.5f ~ 1）
            float compressRatio = Mathf.Lerp(0.5f, 1f, 1 - (timer / maxChargeTime));
            transform.localScale = new Vector3(1, compressRatio * originalScale.y, 1); // 压缩Player
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            doJump = true;
            transform.localScale = new Vector3(1, 1, 1);//恢复Player
        }
    }

    void FixedUpdate()
    {
        if (doJump && isGround)
        {
            Jump();
            doJump = false;
            isGround = false;
            timer = 0;
        }

        rb.AddForce(Vector3.down * 50); // 添加重力
    }
    void Jump()
    {
        Vector3 dir = GameManager.S.PlayerIsFacingXAxis ? -Vector3.right : Vector3.forward;
        dir.y = 1;
        dir = dir.normalized; // 归一化确保力度一致
        rb.velocity = dir * jumpPower * timer;
        Debug.Log("Jump Power: " + (dir * jumpPower * timer));
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // 判断是否与地面接触 第一个接触点的法线方向
            if (collision.contacts[0].normal == Vector3.up)
            {
                isGround = true;
                GameManager.S.HitGround(transform.position);
            }
        }
    }
}