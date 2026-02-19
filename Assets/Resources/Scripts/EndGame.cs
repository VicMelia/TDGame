using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().SetMovement(false);
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
