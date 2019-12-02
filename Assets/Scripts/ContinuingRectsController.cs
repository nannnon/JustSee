using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuingRectsController : MonoBehaviour
{
    private const int kFRN = 32;
    private GameObject[] m_frs = new GameObject[kFRN];
    private int m_dir;
    private float m_vel;

    // Start is called before the first frame update
    void Start()
    {
        // 出現位置、大きさ、色、向き、速さを設定
        Vector3 pos = new Vector3(Random.Range(-9, 9), Random.Range(-4, 4));
        Vector3 scale = new Vector3(Random.Range(0.1f, 0.5f), Random.Range(0.1f, 0.5f));
        Color color = new Color(Random.Range(0f,1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        this.m_dir = (int)Random.Range(0, 4);
        this.m_vel = Random.Range(0.01f, 0.1f);
        this.transform.position = pos;
        this.transform.localScale = scale;
        this.GetComponent<SpriteRenderer>().color = color;

        // 当たり判定を調整
        {
            Vector2 objectSize = (this.transform as RectTransform).sizeDelta;
            BoxCollider2D boxCollider2D = this.GetComponent<BoxCollider2D>();
            boxCollider2D.size = objectSize;
        }

        // 後続の矩形を生成
        GameObject fr = Resources.Load<GameObject>("Prefabs/FollowingRect");
        GameObject parent = GameObject.Find("Rects");

        for (int i = 0; i < kFRN; ++i)
        {
            GameObject go = Instantiate(fr);
            this.m_frs[i] = go;
            go.transform.SetParent(parent.transform);
            go.GetComponent<Transform>().position = pos;
            go.GetComponent<Transform>().localScale = scale;
            go.GetComponent<SpriteRenderer>().color = color;

            Vector2 objectSize = go.GetComponent<RectTransform>().sizeDelta;
            BoxCollider2D boxCollider2D = go.GetComponent<BoxCollider2D>();
            boxCollider2D.size = objectSize;
        }
    }

    void Move(float vel)
    {
        Vector3 pos = this.transform.position;

        switch (this.m_dir)
        {
            case 0:
                pos.y += vel;
                break;
            case 1:
                pos.x += vel;
                break;
            case 2:
                pos.y -= vel;
                break;
            case 3:
                pos.x -= vel;
                break;
            default:
                throw new System.Exception();
        }

        this.transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        // 先頭の矩形を前進
        this.Move(this.m_vel);

        // 後続も前進
        for (int i = kFRN - 1; i > 0; --i)
        {
            this.m_frs[i].transform.position = this.m_frs[i - 1].transform.position;
        }
        this.m_frs[0].transform.position = this.transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 後続と当たっている時は何もしない
        foreach (GameObject go in this.m_frs)
        {
            if (go == other.gameObject)
            {
                return;
            }
        }

        // 後退して方向転換
        this.Move(-this.m_vel);
        this.m_dir = (this.m_dir + 1) % 4;
        this.Move(this.m_vel);
    }
}
