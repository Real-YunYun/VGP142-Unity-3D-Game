using UnityEngine.UI;
using UnityEngine;

public class HealthBarHandler : MonoBehaviour
{
    public GameObject Player;
    public EnemyFollow Enemy;
    public Text HealthBarText;

    void Update()
    {
        if (Enemy && Player)
        {
            this.gameObject.GetComponent<Slider>().value = Enemy.Health;
            HealthBarText.text = Enemy.Health.ToString();
        } 
        else if (Player)
        {
            this.gameObject.GetComponent<Slider>().value = Player.GetComponent<PlayerController>().Health;
            HealthBarText.text = Player.GetComponent<PlayerController>().Health.ToString();
        }
    }
}
