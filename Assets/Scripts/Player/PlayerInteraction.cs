using System.Globalization;
using System.Xml.Schema;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI coinText;
    private int numberOfCoins = 0;
    [SerializeField] private int TotalCoins;

    private void Start()
    {
        // Initialize the coin text at the start of the game
        coinText.text = "Coins : " + numberOfCoins.ToString() + "/" + TotalCoins.ToString();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            
            numberOfCoins++;
            coinText.text = "Coins : " + numberOfCoins.ToString() + "/" + TotalCoins.ToString();
            Destroy(other.gameObject);
            if (numberOfCoins >= TotalCoins)
            {
                // Load the next level or show a victory message
                Debug.Log("All coins collected! Level complete!");
                // You can add your level completion logic here, like loading a new scene.
            }
        }
    }
}
