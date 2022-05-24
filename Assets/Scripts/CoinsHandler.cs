using UnityEngine.UI;
using UnityEngine;

public class CoinsHandler : MonoBehaviour
{
    public GameObject Player;
    public Text Coins;
    void Update()
    {
        Coins.text = "$" + Player.GetComponent<PlayerController>().Coins.ToString();
    }
}