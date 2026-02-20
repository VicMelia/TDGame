using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour
{
    [SerializeField] Image imageIcon;
    [SerializeField] TextMeshProUGUI textName;
    [SerializeField] GameObject descriptionPanel;
    public InventoryItemDefinition definition;
    InventoryUI inventoryUI;
    Button[] buttons;

    enum ButtonType
    {
        Use,
        Discard,
        Info
    }
    void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
        inventoryUI = GetComponentInParent<InventoryUI>();
    }
    void OnEnable()
    {
        buttons[(int)ButtonType.Use].onClick.AddListener(OnUse);
        buttons[(int)ButtonType.Discard].onClick.AddListener(OnDiscard);
        buttons[(int)ButtonType.Info].onClick.AddListener(OnInfo);
    }
    void OnDisable()
    {
        buttons[(int)ButtonType.Use].onClick.RemoveListener(OnUse);
        buttons[(int)ButtonType.Discard].onClick.RemoveListener(OnDiscard);
        buttons[(int)ButtonType.Info].onClick.RemoveListener(OnInfo);
    }
    public void Init(InventoryItemDefinition itemDefinition)
    {
        imageIcon.sprite = itemDefinition.icon;
        textName.text = itemDefinition.uniqueItemName;
        descriptionPanel.GetComponentInChildren<TextMeshProUGUI>().text = itemDefinition.description;
        definition = Instantiate(itemDefinition); // Create a copy of the definition to track uses
    }
    void OnUse()
    {
        SoundManager.Instance.PlaySfx("ClickBoton");
        if(definition.numUses <= 0)
        {
            return;
        }
        inventoryUI.NotifyInventoryItemUsed(definition);
        if (--definition.numUses <= 0)
        {
            Destroy(gameObject);
        }
    }
    void OnDiscard()
    {
        SoundManager.Instance.PlaySfx("ClickBoton");
        Destroy(gameObject);
    }
    void OnInfo()
    {
        SoundManager.Instance.PlaySfx("ClickBoton");
        descriptionPanel.SetActive(!descriptionPanel.activeSelf);
    }
}
