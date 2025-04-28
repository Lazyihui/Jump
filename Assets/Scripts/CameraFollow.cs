using UnityEngine;

public class CameraFollow : MonoBehaviour
{

	public Transform target; // 目标物体
	public float smoothAmount; // 平滑移动的速度

	Vector3 offset; // 相机与目标物体之间的偏移量
	Vector3 originalPos; // 相机的原始位置

	void Start()
	{
		// 计算相机与目标物体之间的偏移量
		offset = transform.position - target.position;
		originalPos = transform.position;
	}

	// Update is called once per frame
	void LateUpdate()
	{
		Vector3 targetPos = target.position + offset; // 计算目标位置
		targetPos.y = originalPos.y; // 保持相机的高度不变
		transform.position = Vector3.Lerp(transform.position, targetPos, smoothAmount); // 平滑移动相机
	}
}
