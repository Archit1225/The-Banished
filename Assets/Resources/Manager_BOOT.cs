using UnityEngine;

public static class Manager_BOOT
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute()
    {
        if (Object.FindAnyObjectByType<GameManager>() == null)
        {
            GameObject managerPrefab = Resources.Load<GameObject>("Managers");
            GameObject instantiatedManagers = Object.Instantiate(managerPrefab);

            Object.DontDestroyOnLoad(instantiatedManagers);
        }
    }
}
