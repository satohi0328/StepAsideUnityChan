﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemGenerator : MonoBehaviour
{
    //carPrefabを入れる
    public GameObject carPrefab;
    //coinPrefabを入れる
    public GameObject coinPrefab;
    //cornPrefabを入れる
    public GameObject conePrefab;
    //carPrefabを入れる
    public List<GameObject> carPrefabList = new List<GameObject>();
    //coinPrefabを入れる
    public List<GameObject> coinPrefabList = new List<GameObject>();
    //cornPrefabを入れる
    public List<GameObject> conePrefabList = new List<GameObject>();

    // MainCameraを入れる
    public GameObject mainCameraObj;
    // Unitychanを入れる
    public GameObject unitychanObj;

    //スタート地点
    private int startPos = -160;
    //ゴール地点
    private int goalPos = 120;
    //アイテムを出すx方向の範囲
    private float posRange = 3.4f;

    // アイテム生成のチェックポイント
    private int chkPoint = -160;
    // アイテムの間隔
    private int itemRange = 15;

    // アイテムの生成範囲
    private int generateRange = 40;

    // Use this for initialization
    void Start()
    {

        // MainCameraコンポーネントを設定
        this.mainCameraObj = Camera.main.gameObject;
        // unitychanコンポーネントを設定
        this.unitychanObj = GameObject.Find("unitychan");

    }

    // Update is called once per frame
    void Update()
    {
        // 一定距離ごとにアイテム生成メソッド呼び出し
        generateItem();

        //画面外オブジェクト削除メソッド呼び出し
        destroyItemOverScreen(carPrefabList);
        destroyItemOverScreen(conePrefabList);
        destroyItemOverScreen(coinPrefabList);
    }

    // 一定距離ごとにアイテム生成
    private void generateItem()
    {
        // unitychanの位置 + アイテム生成範囲がチェックポイント内、かつゴールポイント内の場合
        if (unitychanObj.transform.position.z + generateRange >= chkPoint && chkPoint <= goalPos - itemRange)
        {
            //どのアイテムを出すのかをランダムに設定
            int num = Random.Range(1, 11);
            if (num <= 2)
            {
                //コーンをx軸方向に一直線に生成
                for (float j = -1; j <= 1; j += 0.4f)
                {
                    GameObject cone = Instantiate(conePrefab) as GameObject;
                    cone.transform.position = new Vector3(4 * j, cone.transform.position.y, chkPoint);

                    // 生成したconeをListに保持
                    conePrefabList.Add(cone);
                }
            }
            else
            {

                //レーンごとにアイテムを生成
                for (int j = -1; j <= 1; j++)
                {
                    //アイテムの種類を決める
                    int item = Random.Range(1, 11);
                    //アイテムを置くZ座標のオフセットをランダムに設定
                    int offsetZ = Random.Range(-5, 6);
                    //60%コイン配置:30%車配置:10%何もなし
                    if (1 <= item && item <= 6)
                    {
                        //コインを生成
                        GameObject coin = Instantiate(coinPrefab) as GameObject;
                        coin.transform.position = new Vector3(posRange * j, coin.transform.position.y, chkPoint + offsetZ);

                        // 生成したcoinをListに保持
                        coinPrefabList.Add(coin);

                    }
                    else if (7 <= item && item <= 9)
                    {
                        //車を生成
                        GameObject car = Instantiate(carPrefab) as GameObject;
                        car.transform.position = new Vector3(posRange * j, car.transform.position.y, chkPoint + offsetZ);

                        // 生成したcarをListに保持
                        carPrefabList.Add(car);

                    }
                }
            }
            // チェックポイント更新
            chkPoint = chkPoint + itemRange;
        }

    }

    // 画面外オブジェクト削除
    private void destroyItemOverScreen(List<GameObject> argList)
    {
        // 引数配列の要素数ループ
        for (int i = 0; i < argList.Count; i++)
        {
            // メインカメラのz座標より小さい場合
            if (this.mainCameraObj.transform.position.z > argList[i].transform.position.z)
            {
                // オブジェクト削除
                Destroy(argList[i]);
                // 配列から削除
                argList.RemoveAt(i);
                // インデントが下がるため、デクリメント
                i--;
            }
        }
    }
}