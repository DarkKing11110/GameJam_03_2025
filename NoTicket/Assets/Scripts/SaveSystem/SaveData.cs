using UnityEngine;

[System.Serializable]
public class SaveData
{
    [SerializeField] 
    private int coins;
    [SerializeField] 
    private int lollis;
    [SerializeField] 
    private Vector3 playerPosition;

    public int Coins => coins;
    public int Lollis => lollis;
    public Vector3 PlayerPosition => playerPosition;

    public SaveData(int coins, int lollis, Vector3 playerPosition)
    {
        this.coins = coins;
        this.lollis = lollis;
        this.playerPosition = playerPosition;
    }
}
