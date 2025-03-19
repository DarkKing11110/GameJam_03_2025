//using UnityEngine;

//public class PlayerSpawnManager : MonoBehaviour
//{
//    public static PlayerSpawnManager Instance { get; private set; }

//    [SerializeField] 
//    private Transform player;
//    [SerializeField] 
//    private Vector3 defaultSpawnPoint;

//    private void Awake()
//    {
//        if (Instance != null && Instance != this)
//        {
//            Destroy(gameObject);
//            return;
//        }

//        Instance = this;
//    }

//    private void Start()
//    {
//        Vector3 checkpoint = SaveGameManager.Instance.GetCheckpointPosition();

//        if (checkpoint != Vector3.zero)
//        {
//            SpawnAtPosition(checkpoint);
//        }
//        else
//        {
//            SpawnAtPosition(defaultSpawnPoint);
//        }
//    }



//    /// <summary>
//    /// Teleports Player to a defined position
//    /// </summary>
//    /// <param name="spawnPosition">Position where the player gets teleported</param>
//    public void SpawnAtPosition(Vector3 spawnPosition)
//    {
//        if (player == null)
//        {
//            return;
//        }

//        player.position = spawnPosition;
//    }
//}
