using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Common : SingletonMonoBehaviour<Common>
{

    public GameObject playBoard;
    public GameObject point;
    public GameObject cardPrefab;
    public List<GameObject> cards;

    private List<string> suits = new List<string> { "c", "s", "h", "d" };
    private int boardOnNum = 7;

    void Start()
    {
        for (int i = 0; i < boardOnNum; i++) {
            CreateCard(1);
        }
    }

    public void CreateCard(int number)
    {
        number = Random.Range(number, number + 3);
        if (number > 13) {
            number = 13;
        }
        GameObject card = (GameObject)Instantiate(cardPrefab);
        card.transform.SetParent(playBoard.transform);

        RectTransform rt = card.GetComponent<RectTransform>();
        float x = Random.Range(0, 1080 - Card.width);
        float y = Random.Range(0, 1920 - Card.height);
        rt.offsetMin = new Vector2(x, y);                                          // Left Bottom
        rt.offsetMax = new Vector2(x - 1080 + Card.width, y - 1920 + Card.height); // Right Top
        rt.localScale = new Vector2(1f, 1f);

        string suit = suits[Random.Range(1, suits.Count)];
        card.GetComponent<Card>().Number = number;

        Image img = card.GetComponent<Image>();
        img.sprite = Resources.Load <Sprite>(suit + number.ToString());

        var collider = card.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(160, 240);

        var rigid = card.AddComponent<Rigidbody2D>();
        rigid.constraints = RigidbodyConstraints2D.FreezeAll;

        cards.Add(card);
    }

    public void CreateJoker()
    {
        GameObject card = (GameObject)Instantiate(cardPrefab);
        card.transform.SetParent(playBoard.transform);

        RectTransform rt = card.GetComponent<RectTransform>();
        float x = Random.Range(0, 1080 - Card.width);
        float y = Random.Range(0, 1920 - Card.height);
        rt.offsetMin = new Vector2(x, y);                                          // Left Bottom
        rt.offsetMax = new Vector2(x - 1080 + Card.width, y - 1920 + Card.height); // Right Top
        rt.localScale = new Vector2(1f, 1f);

        Image img = card.GetComponent<Image>();
        img.sprite = Resources.Load <Sprite>("x1");

        var collider = card.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(160, 240);

        var rigid = card.AddComponent<Rigidbody2D>();
        rigid.constraints = RigidbodyConstraints2D.FreezeAll;

        cards.Add(card);
    }
}
