
using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    private bool levelCompleted;

    private void OnTriggerEnter(Collider other)
    {
        if (levelCompleted || !other.CompareTag("Player"))
            return;

        levelCompleted = true;

        Debug.Log("Level completed!");

        // Level progression will be connected next.
        LevelManager manager = FindFirstObjectByType<LevelManager>();

        if (manager != null)
            manager.CompleteLevel();
        else
            Debug.LogWarning(
                "LevelManager has not been created yet."
            );
    }
}
