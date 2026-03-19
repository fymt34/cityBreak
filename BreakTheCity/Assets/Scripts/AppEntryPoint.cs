using UnityEngine;

public class AppEntryPoint : MonoBehaviour
{
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	public static void EntryPoint()
	{
		Application.targetFrameRate = 60;
	}
}
