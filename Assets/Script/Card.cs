using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Image))]
public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool dragOnSurfaces = true;
    public const float width = 160;
    public const float height = 240;

    public int Number { get; set; }

    private GameObject m_DraggingIcon;
    private RectTransform m_DraggingPlane;

    public void OnBeginDrag(PointerEventData eventData)
    {
        var canvas = FindInParents<Canvas>(gameObject);
        if (canvas == null) {
            return;
        }

        // We have clicked something that can be dragged.
        // What we want to do is create an icon for this.
        m_DraggingIcon = new GameObject("icon");

        m_DraggingIcon.transform.SetParent(canvas.transform, false);
        m_DraggingIcon.transform.SetAsLastSibling();

        var collider = m_DraggingIcon.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(160, 240);

        var rigid = m_DraggingIcon.AddComponent<Rigidbody2D>();
        rigid.constraints = RigidbodyConstraints2D.FreezeAll;

        var image = m_DraggingIcon.AddComponent<Image>();

        image.sprite = GetComponent<Image>().sprite;
        image.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
//        image.SetNativeSize();

        if (dragOnSurfaces) {
            m_DraggingPlane = transform as RectTransform;
        } else {
            m_DraggingPlane = canvas.transform as RectTransform;
        }

        SetDraggedPosition(eventData);
    }

    public void OnDrag(PointerEventData data)
    {
        if (m_DraggingIcon != null) {
            SetDraggedPosition(data);
        }
    }

    private void SetDraggedPosition(PointerEventData data)
    {
        if (dragOnSurfaces && data.pointerEnter != null && data.pointerEnter.transform as RectTransform != null)
            m_DraggingPlane = data.pointerEnter.transform as RectTransform;

        var rt = m_DraggingIcon.GetComponent<RectTransform>();
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, data.position, data.pressEventCamera, out globalMousePos)) {
            rt.position = globalMousePos;
            rt.rotation = m_DraggingPlane.rotation;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var iconCollider = m_DraggingIcon.GetComponent<BoxCollider2D>();
        var removeCardNums = new List<int>();
        var point = Common.Instance.point;
        int p = System.Int32.Parse(point.GetComponent<Text>().text);
        int tmpNumber = 99999;

        for (int i = 0; i < Common.Instance.cards.Count; i++) {
            var card = Common.Instance.cards[i];
            var collider = card.GetComponent<BoxCollider2D>();
            if (iconCollider.IsTouching(collider) == false) {
                continue;
            }

            if (card.gameObject == this.gameObject) {
                continue;
            }

            int num = collider.gameObject.GetComponent<Card>().Number;
            Debug.Log(num.ToString());
            if (num != this.Number && this.Number != 0 && num != 0) {
                continue;
            }
            if (this.Number == 0) {
                this.Number = num;
            }

            Common.Instance.cards.RemoveAt(i);
            if (num == 0) {
                removeCardNums.Add(this.Number);
            } else if (this.Number == 0) {
                removeCardNums.Add(num);
            } else {
                removeCardNums.Add(num);
            }
            Destroy(collider.gameObject);
        }

        if (removeCardNums.Count > 0) {
            removeCardNums.Add(removeCardNums[0]);

            if (removeCardNums.Count >= 3) {
                Common.Instance.CreateJoker();
                removeCardNums.RemoveAt(0);
            }
            foreach (var removeCardNum in removeCardNums) {
                p += removeCardNum;
                if (removeCardNum + 1 <= 13) {
                    Common.Instance.CreateCard(removeCardNum + 1);
                } else {
                    Common.Instance.CreateCard(1);
                }
            }
            point.GetComponent<Text>().text = p.ToString();

            for (int j = 0; j < Common.Instance.cards.Count; j++) {
                if (Common.Instance.cards[j] == this.gameObject) {
                    Common.Instance.cards.RemoveAt(j);
                }
            }
            Destroy(this.gameObject);
        }

        if (m_DraggingIcon != null) {
            Destroy(m_DraggingIcon);
        }
    }

    static public T FindInParents<T>(GameObject go) where T : Component
    {
        if (go == null) {
            return null;
        }
        var comp = go.GetComponent<T>();

        if (comp != null) {
            return comp;
        }

        Transform t = go.transform.parent;
        while (t != null && comp == null) {
            comp = t.gameObject.GetComponent<T>();
            t = t.parent;
        }
        return comp;
    }

}