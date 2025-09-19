using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    // --- Wave設定用クラス ---
    [System.Serializable]
    public class Wave
    {
        [Header("通常敵")]
        public int enemyCount = 5;           // このWaveで出す通常敵の数
        public float spawnInterval = 2f;     // 通常敵の出現間隔
        public float enemySpeed = 3f;        // 通常敵の移動速度
        public GameObject[] enemyPrefabs;    // このWaveで使う通常敵Prefabの配列

        [Header("ボス用設定")]
        public bool isBossWave = false;      // ボスWaveかどうかのフラグ
        public GameObject bossPrefab;        // ボスPrefab
        public Transform bossSpawnPoint;     // ボス出現位置（固定）
    }

    [Header("Wave設定")]
    public Wave[] waves;                     // Wave配列（インスペクタで設定）

    [Header("スポーンポイント（通常敵用）")]
    public Transform[] spawnPoints;          // 通常敵の出現ポイント配列

    private int currentWaveIndex = 0;        // 現在のWave番号
    private int spawnedEnemies = 0;          // 現在出現済み敵の数
    private float timer = 0f;                // 出現タイマー
    private bool spawning = false;           // Wave中かどうか

    void Start()
    {
        // ゲーム開始時は最初のWaveをスタート
        StartWave(0);
    }

    void Update()
    {
        if (!spawning) return; // Wave中でなければ何もしない

        timer += Time.deltaTime; // タイマー更新

        Wave wave = waves[currentWaveIndex];

        // --- 通常敵の出現処理 ---
        if (!wave.isBossWave && timer >= wave.spawnInterval)
        {
            timer = 0f;
            SpawnEnemy(); // 敵を生成
            spawnedEnemies++;

            // Wave終了判定
            if (spawnedEnemies >= wave.enemyCount)
            {
                spawning = false;
                // 次のWaveを少し遅らせて開始
                Invoke(nameof(StartNextWave), 3f);
            }
        }

        // --- ボスWave処理 ---
        if (wave.isBossWave && spawnedEnemies == 0)
        {
            SpawnBoss();         // ボス生成
            spawnedEnemies = 1;  // ボスは1体のみ
            spawning = false;

            // 次Wave開始まで少し余裕をもたせる
            Invoke(nameof(StartNextWave), 5f);
        }
    }

    // --- Wave開始処理 ---
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

    // --- 次のWave開始 ---
    void StartNextWave()
    {
        StartWave(currentWaveIndex + 1);
    }

    // --- 通常敵出現処理 ---
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

        // 敵を生成
        GameObject enemy = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        // 敵の移動速度をWaveに合わせる
        EnemyController ec = enemy.GetComponent<EnemyController>();
        if (ec != null)
        {
            ec.speed = wave.enemySpeed;
        }
    }

    // --- ボス出現処理 ---
    void SpawnBoss()
    {
        Wave wave = waves[currentWaveIndex];

        if (wave.bossPrefab == null || wave.bossSpawnPoint == null)
        {
            Debug.LogError("ボスPrefabまたはボスSpawnPointが設定されていません。");
            return;
        }

        // ボスを生成
        Instantiate(wave.bossPrefab, wave.bossSpawnPoint.position, Quaternion.identity);

        Debug.Log("ボス出現！");
    }
}
