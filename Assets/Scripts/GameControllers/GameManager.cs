using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Track your dungeon progress here
    public bool hasDefeatedSkeleton = false;
    public bool hasDefeatedGoblin = false;
    public bool hasDefeatedWizard = false;
    public bool talkedToWizard_1 = false;

    private void Awake()
    {
        // Keeps this object alive when switching scenes
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}