using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{

	public static GameManager S;
	// awake 比 Start更早运行
	void Awake()
	{
		S = this;
	}

	public GameObject cubePrefab;

	public GameObject startCube;
	Transform currentCube;

	public bool PlayerIsFacingXAxis;

	int score;

	void Start()
	{
		currentCube = startCube.transform;
		GenerateNewCube();
	}

	void GenerateNewCube()
	{
		float dist = Random.Range(3, 10) * 1.5f;
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
		score++;
		UIManager.S.RefreshScore(score);
	}

	public void GameOver()
	{
		// 延长1秒运行
		Invoke("ReloadScene", 1);
	}

	public void ReloadScene()
	{
		// using UnityEngine.SceneManagement; 加载当前关卡 d
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

	}
}
