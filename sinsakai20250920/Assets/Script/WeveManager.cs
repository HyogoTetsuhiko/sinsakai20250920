using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    // --- Wave設定用クラス ---
    [System.Serializable]
    public class Wave
    {
        [Header("通常敵設定")]
        public GameObject[] enemyPrefabs;       // このWaveで出す敵Prefabの配列
        public int enemyCount = 5;              // このWaveで出す総数
        public float spawnInterval = 1f;        // 敵を出す間隔
        public float enemySpeed = 3f;           // 敵の移動速度

        [Header("同時出現設定")]
        public bool spawnMultipleAtOnce = false; // trueなら同時に複数出す
        public int simultaneousCount = 1;        // 同時出現数（spawnMultipleAtOnce=true時に使用）

        [Header("ボスWave設定")]
        public bool isBossWave = false;         // このWaveがボスWaveか
        public GameObject bossPrefab;           // ボスPrefab
        public Transform bossSpawnPoint;        // ボスの出現位置
    }

    [Header("Wave設定")]
    public Wave[] waves;                        // インスペクタでWaveを設定

    [Header("通常敵用スポーンポイント")]
    public Transform[] spawnPoints;             // 通常敵の出現ポイント配列

    private int currentWaveIndex = 0;           // 現在のWave番号
    private int spawnedEnemies = 0;             // 出現済み敵の数
    private float timer = 0f;                   // 出現間隔タイマー
    private bool spawning = false;              // Wave出現中フラグ

    void Start()
    {
        // ゲーム開始時に最初のWaveをスタート
        StartWave(0);
    }

    void Update()
    {
        if (!spawning) return; // Wave中でなければ何もしない

        Wave wave = waves[currentWaveIndex];

        // --- ボスWave処理 ---
        if (wave.isBossWave)
        {
            // ボスは1体のみ生成
            if (spawnedEnemies == 0)
            {
                SpawnBoss();
                spawnedEnemies = 1;
                spawning = false;

                // 次Wave開始まで少し余裕をもたせる
                Invoke(nameof(StartNextWave), 5f);
            }
            return;
        }

        // --- 通常敵出現処理 ---
        timer += Time.deltaTime;
        if (timer >= wave.spawnInterval)
        {
            timer = 0f;

            if (wave.spawnMultipleAtOnce)
            {
                // 同時出現数だけ敵を生成
                int spawnNum = Mathf.Min(wave.simultaneousCount, wave.enemyCount - spawnedEnemies);

                for (int i = 0; i < spawnNum; i++)
                {
                    SpawnEnemy();
                    spawnedEnemies++;
                }

                // Wave終了判定
                if (spawnedEnemies >= wave.enemyCount)
                {
                    spawning = false;
                    Invoke(nameof(StartNextWave), 3f); // 次Wave開始まで少し遅延
                }
            }
            else
            {
                // 1体ずつ出現
                SpawnEnemy();
                spawnedEnemies++;

                if (spawnedEnemies >= wave.enemyCount)
                {
                    spawning = false;
                    Invoke(nameof(StartNextWave), 3f);
                }
            }
        }
    }

    // --- Wave開始 ---
    void StartWave(int index)
    {
        if (index >= waves.Length)
        {
            Debug.Log("全てのWave終了！");
            return;
        }

        currentWaveIndex = index;
        spawnedEnemies = 0;
        timer = 0f;
        spawning = true;

        Debug.Log($"Wave {currentWaveIndex + 1} 開始！");
    }

    // --- 次のWaveを開始 ---
    void StartNextWave()
    {
        StartWave(currentWaveIndex + 1);
    }

    // --- 通常敵生成 ---
    void SpawnEnemy()
    {
        Wave wave = waves[currentWaveIndex];

        if (wave.enemyPrefabs.Length == 0 || spawnPoints.Length == 0)
        {
            Debug.LogError("敵Prefabまたはスポーンポイントが設定されていません。");
            return;
        }

        // ランダムにPrefabを選択
        GameObject prefab = wave.enemyPrefabs[Random.Range(0, wave.enemyPrefabs.Length)];

        // ランダムにスポーンポイントを選択
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // 敵生成
        GameObject enemy = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        // 敵の速度を設定
        EnemyController ec = enemy.GetComponent<EnemyController>();
        if (ec != null)
        {
            ec.speed = wave.enemySpeed;
        }
    }

    // --- ボス生成 ---
    void SpawnBoss()
    {
        Wave wave = waves[currentWaveIndex];

        if (wave.bossPrefab == null || wave.bossSpawnPoint == null)
        {
            Debug.LogError("ボスPrefabまたはボスSpawnPointが設定されていません。");
            return;
        }

        Instantiate(wave.bossPrefab, wave.bossSpawnPoint.position, Quaternion.identity);
        Debug.Log("ボス出現！");
    }
}
