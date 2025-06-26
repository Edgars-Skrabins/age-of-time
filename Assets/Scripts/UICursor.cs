using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UICursor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform m_cursorRect;
    [SerializeField] private Canvas m_canvas;
    [SerializeField] private Animator m_cursorAnimator;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    private void Update()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        DetectClick();
        FollowMouse();
        DetectHover();
    }

    private void FollowMouse()
    {
        Vector2 mousePos = Input.mousePosition;

        if (m_canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            m_cursorRect.position = mousePos;
        }
        else
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                m_canvas.transform as RectTransform,
                mousePos,
                m_canvas.worldCamera,
                out Vector2 localPos);

            m_cursorRect.localPosition = localPos;
        }
    }

    private void DetectHover()
    {
        PointerEventData data = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);

        bool isHovering = false;

        foreach (RaycastResult result in results)
        {
            GameObject go = result.gameObject;

            if (
                go.GetComponent<Button>() ||
                go.GetComponent<Toggle>() ||
                go.GetComponent<Slider>() ||
                go.GetComponent<Enemy>() ||
                go.GetComponentInParent<Enemy>()
            )
            {
                isHovering = true;
                break;
            }
        }

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;

        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero);
        if (hit.collider != null)
        {
            Enemy enemy = hit.collider.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                isHovering = true;
            }
        }

        m_cursorAnimator.SetBool("Hover", isHovering);
    }

    private void DetectClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_cursorAnimator.SetTrigger("Click");
        }
    }
    public void Click()
    {
        m_cursorAnimator.SetTrigger("Click");
    }
}
