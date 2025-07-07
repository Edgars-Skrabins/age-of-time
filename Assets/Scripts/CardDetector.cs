using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDetector : MonoBehaviour
{
    [SerializeField] private GraphicRaycaster m_raycaster;
    [SerializeField] private EventSystem m_eventSystem;
    [SerializeField] private TMP_Text m_descriptionText;

    private PointerEventData m_pointerData;
    private List<RaycastResult> m_raycastResults = new List<RaycastResult>();

    private void Update()
    {
        m_pointerData = new PointerEventData(m_eventSystem)
        {
            position = Input.mousePosition
        };

        m_raycastResults.Clear();
        m_raycaster.Raycast(m_pointerData, m_raycastResults);

        bool foundCard = false;

        foreach (RaycastResult result in m_raycastResults)
        {
            UpgradeCard card = result.gameObject.GetComponentInParent<UpgradeCard>();
            if (card != null)
            {
                m_descriptionText.text = card.GetDescription();
                foundCard = true;
                break;
            }
        }

        if (!foundCard)
        {
            m_descriptionText.text = "";
        }
    }
}