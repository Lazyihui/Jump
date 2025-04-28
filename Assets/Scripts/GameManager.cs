using UnityEngine;

public class GameManager : MonoBehaviour
{

	public static GameManager S;

	public GameObject cubePrefab;

	public GameObject startCube;
	Transform currentCube;

	public bool PlayerIsFacingXAxis;


	// awake 比 Start更早运行
	void Awake()
	{
		S = this;
	}

	void Start()
	{
		currentCube = startCube.transform;
		GenerateNewCube();
	}

	void GenerateNewCube()
	{
		float dist = Random.Range(3, 10) *1.5f;
		Vector3 dir = PlayerIsFacingXAxis ? -Vector3.right : Vector3.forward;
		// Quaternion.identity 不旋转的意思
		GameObject cube = Instantiate(cubePrefab, currentCube.position + dir * dist, Quaternion.identity);
		cube.transform.parent = transform;

		currentCube = cube.transform;
	}

	public void HitGround(Vector3 hitPos)
	{
		PlayerIsFacingXAxis = !PlayerIsFacingXAxis;
		GenerateNewCube();
	}
}
