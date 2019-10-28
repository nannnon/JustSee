using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    const int kCRSNum = 16;

    // Start is called before the first frame update
    void Start()
    {
        // 連なる矩形を生成
        GameObject crs = Resources.Load< GameObject>("Prefabs/ContinuingRects");
        GameObject parent = GameObject.Find("Rects");

        for (int i = 0; i < kCRSNum; ++i)
        {
            GameObject go = Instantiate(crs);
            go.transform.SetParent(parent.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
