using UnityEngine;

public class WaveManager : MonoBehaviour
{

    [System.Serializable] // インスペクタに表示できるようにする
    public class Wave
    {
        public int enemyCount = 5;        // このWaveで出す敵の数
        public int enemiesPerSpawn = 1;   // 1回のSpawnで何体出すか
        public float spawnInterval = 2f;  // 敵を出す間隔（秒）
        public float enemySpeed = 3f;     // このWaveの敵の移動速度
    }

    [Header("Wave設定")]
    public Wave[] waves; // 複数のWaveをインスペクタで設定できるように配列化

    [Header("出現に使うプレハブ / スポーンポイント")]
    public GameObject enemyPrefab;   // 出現させる敵プレハブ
    public Transform[] spawnPoints;  // 複数の出現位置を配列で管理（ランダムで選ぶ）

    private int currentWaveIndex = 0; // 今どのWaveか
    private int spawnedEnemies = 0;   // このWaveで出した敵の数
    private float timer = 0f;         // 出現間隔を測るタイマー
    private bool spawning = false;    // 今Wave進行中かどうか

    void Start()
    {
        StartWave(0); // 最初のWaveを開始
    }

    void Update()
    {
        if (!spawning) return; // Wave進行中でなければ何もしない

        timer += Time.deltaTime; // 経過時間を足していく
        if (timer >= waves[currentWaveIndex].spawnInterval)
        {
            timer = 0f; // タイマーリセット
            SpawnEnemy(); // 敵を生成

            spawnedEnemies++; // 出現数カウント

            // Waveの敵数を出し終わったら次のWaveへ
            if (spawnedEnemies >= waves[currentWaveIndex].enemyCount)
            {
                spawning = false; // 今のWave終了
                Invoke(nameof(StartNextWave), 3f); // 3秒後に次のWave開始
            }
        }
    }

    void StartWave(int index)
    {
        if (index >= waves.Length)
        {
            Debug.Log("全てのWave終了！");
            return; // Waveがもう無い場合は終了
        }

        currentWaveIndex = index; // 今のWave番号を更新
        spawnedEnemies = 0;       // 出現数リセット
        timer = 0f;               // タイマーリセット
        spawning = true;          // Wave開始フラグ

        Debug.Log($"Wave {currentWaveIndex + 1} 開始！");
    }

    void StartNextWave()
    {
        StartWave(currentWaveIndex + 1);
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null || spawnPoints.Length == 0)
        {
            Debug.LogError("[WaveManager] enemyPrefab または spawnPoints が設定されていません。");
            return;
        }

        // 1回のSpawnで複数体生成
        for (int i = 0; i < waves[currentWaveIndex].enemiesPerSpawn; i++)
        {
            // ランダムな出現ポイントを選択
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

            EnemyController ec = enemy.GetComponent<EnemyController>();
            if (ec != null)
            {
                ec.speed = waves[currentWaveIndex].enemySpeed;
            }

            spawnedEnemies++; // 生成したぶんカウント
            if (spawnedEnemies >= waves[currentWaveIndex].enemyCount)
                break; // 出す数を超えたら終了
        }
    }
}
