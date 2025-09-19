using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{

    [System.Serializable]
    public class Wave
    {
        [Header("通常敵設定")]
        public GameObject[] enemyPrefabs;       // このWaveで出す通常敵Prefabの配列
        public int enemyCount = 5;              // このWaveで出す通常敵の総数
        public float spawnInterval = 1f;        // 通常敵を出す間隔（秒）
        public float enemySpeed = 3f;           // 通常敵の移動速度

        [Header("同時出現設定")]
        public bool spawnMultipleAtOnce = false; // trueなら同時に複数の敵を出す
        public int simultaneousCount = 1;        // 同時出現数（spawnMultipleAtOnce=trueのとき使用）

        [Header("ボスWave設定")]
        public bool isBossWave = false;         // このWaveがボスWaveかどうか
        public GameObject bossPrefab;           // ボスPrefab
        public Transform bossSpawnPoint;        // ボスの出現位置

        [HideInInspector] public bool bossSpawned = false; // ボスが既に出たかどうかのフラグ
        // → Wave中にボスが何度も生成されないよう制御する
    }

    [Header("Wave設定")]
    public Wave[] waves;                        // Waveの配列をInspectorで設定

    [Header("通常敵用スポーンポイント")]
    public Transform[] spawnPoints;             // 通常敵のスポーンポイント配列

    private int currentWaveIndex = 0;           // 現在のWave番号
    private int spawnedEnemies = 0;             // そのWaveで出現済みの通常敵数
    private float timer = 0f;                   // 出現間隔用タイマー
    private bool spawning = false;              // Wave出現中かどうか

    void Start()
    {
        // ゲーム開始時に最初のWaveを開始
        StartWave(0);
    }

    void Update()
    {
        if (!spawning) return; // Wave出現中でなければ何もしない

        Wave wave = waves[currentWaveIndex];

        // ボス生成処理
        // ボスWaveでまだボスが出ていなければ1体だけ生成
        if (wave.isBossWave && !wave.bossSpawned)
        {
            SpawnBoss();           // ボス生成
            wave.bossSpawned = true; // フラグを立てることで再生成防止
        }

        // 通常敵生成処理
        if (wave.enemyCount > 0 && spawnedEnemies < wave.enemyCount)
        {
            timer += Time.deltaTime; // タイマーを進める（経過時間を加算）

            if (timer >= wave.spawnInterval) // 一定間隔経過で敵生成
            {
                timer = 0f; // タイマーリセット

                int spawnNum = 1; // デフォルトは1体ずつ生成

                if (wave.spawnMultipleAtOnce)
                {
                    // 同時出現数と残りの敵数の小さい方を生成
                    spawnNum = Mathf.Min(wave.simultaneousCount, wave.enemyCount - spawnedEnemies);
                }

                // 敵を spawnNum 体生成
                for (int i = 0; i < spawnNum; i++)
                {
                    SpawnEnemy();
                    spawnedEnemies++;
                }
            }
        }

        // Wave終了判定
        // 通常敵を全て出し切ったらWave終了
        if (spawnedEnemies >= wave.enemyCount)
        {
            spawning = false;

            // ボスWaveの場合は自動で次Waveに進まない
            // （ボス撃破後に別処理で次Waveに進む想定）
            if (!wave.isBossWave)
                Invoke(nameof(StartNextWave), 3f); // 3秒後に次Wave開始
        }
    }

    // Wave開始処理
    void StartWave(int index)
    {
        if (index >= waves.Length)
        {
            Debug.Log("全てのWave終了！");
            return;
        }

        currentWaveIndex = index;
        spawnedEnemies = 0;        // 通常敵出現カウンターリセット
        timer = 0f;                // タイマーリセット
        spawning = true;           // Wave出現中フラグON

        // ボス生成フラグをWave開始時に必ずfalseにリセット
        waves[currentWaveIndex].bossSpawned = false;

        Debug.Log($"Wave {currentWaveIndex + 1} 開始！");
    }

    // 次のWave開始
    void StartNextWave()
    {
        StartWave(currentWaveIndex + 1);
    }

    // 通常敵生成
    void SpawnEnemy()
    {
        Wave wave = waves[currentWaveIndex];

        if (wave.enemyPrefabs.Length == 0 || spawnPoints.Length == 0)
        {
            Debug.LogWarning("敵Prefabまたはスポーンポイントが設定されていません。");
            return;
        }

        // ランダムにPrefabを選択
        GameObject prefab = wave.enemyPrefabs[Random.Range(0, wave.enemyPrefabs.Length)];
        // ランダムにスポーンポイントを選択
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // 敵を生成
        GameObject enemy = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        // 敵の速度を設定
        EnemyController ec = enemy.GetComponent<EnemyController>();
        if (ec != null) ec.speed = wave.enemySpeed;
    }

    // ボス生成
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
