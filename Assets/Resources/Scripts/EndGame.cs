using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] InventoryItemDefinition _coinDefinition;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CheckCoins(other.GetComponent<PlayerController>());
        }
    }

    void CheckCoins(PlayerController player)
    {
        if (InventoryUI.instance.HowMany(_coinDefinition) >= 4)
        {
            player.SetMovement(false);
            _canvas.enabled = true;
            StartCoroutine(FinishGame());
        }
    }

    IEnumerator FinishGame()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("MainMenu");
    }
}
