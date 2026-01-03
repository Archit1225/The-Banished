using UnityEngine;

public class UI_Controller : MonoBehaviour
{
    public GameObject enemyHealthBar;
    public bool bossPresent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (bossPresent)
        {
            enemyHealthBar.SetActive(true);
        }
    }


}
