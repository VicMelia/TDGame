using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private Image _leftImage;
    [SerializeField] private Image[] _midImage;
    [SerializeField] private Image _rightImage;
    
    [SerializeField] private Sprite _leftSprite;
    [SerializeField] private Sprite _midSprite;
    [SerializeField] private Sprite _rightSprite;
    
    private Sprite _originalLeftSprite;
    private Sprite _originalMidSprite;
    private Sprite _originalRightSprite;

    private List<Image> _allParts;

    private Vector3 _originalScale;

    private void Start()
    {
        _originalLeftSprite = _leftImage.sprite;
        _originalMidSprite = _midImage[0].sprite;
        _originalRightSprite = _rightImage.sprite;
        
        _originalScale = transform.localScale;
        
        _allParts = new List<Image>();
        
        _allParts.Add(_leftImage);
        foreach (Image image in _midImage) _allParts.Add(image);
        _allParts.Add(_rightImage);

        StartCoroutine(ButtonAnimation());
    }

    public void OnHoverMouse()
    {
        _leftImage.sprite = _leftSprite;
        
        foreach (Image img in _midImage)
            img.sprite = _midSprite;
        
        _rightImage.sprite = _rightSprite;
        
        transform.localScale *= 1.1f;
        
    }

    public void OnNotHoverMouse()
    {
        _leftImage.sprite = _originalLeftSprite;
        
        foreach (Image img in _midImage)
            img.sprite = _originalMidSprite;
        
        _rightImage.sprite = _originalRightSprite;
        
        transform.localScale = _originalScale;
    }

    public void Play()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void Exit()
    {
        Application.Quit();
    }

    IEnumerator ButtonAnimation()
    {
        foreach (Image image in _allParts)
        {
            StartCoroutine(AnimatePart(image));
            yield return new WaitForSeconds(0.05f);
        }
    }

    private const float WAVE_MOVEMENT = 2f;
    IEnumerator AnimatePart(Image image)
    {
        while (true)
        {
            image.rectTransform.anchoredPosition += new Vector2(0, WAVE_MOVEMENT);
            yield return new WaitForSeconds(0.2f);
            image.rectTransform.anchoredPosition -= new Vector2(0, WAVE_MOVEMENT);
            yield return new WaitForSeconds(0.2f);
            image.rectTransform.anchoredPosition -= new Vector2(0, WAVE_MOVEMENT);
            yield return new WaitForSeconds(0.2f);
            image.rectTransform.anchoredPosition += new Vector2(0, WAVE_MOVEMENT);
            yield return new WaitForSeconds(0.2f);
        }
    }
}


