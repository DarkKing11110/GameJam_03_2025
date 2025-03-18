using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Checkpoint erreicht bei: " + transform.position);

            CheckpointManager.Instance.SetCheckpoint(transform.position);
            SaveGameManager.Instance.SaveGame(transform.position);
        }
    }
}
