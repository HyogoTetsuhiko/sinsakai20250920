using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    // --- Wave�ݒ�p�N���X ---
    [System.Serializable]
    public class Wave
    {
        [Header("�ʏ�G")]
        public int enemyCount = 5;           // ����Wave�ŏo���ʏ�G�̐�
        public float spawnInterval = 2f;     // �ʏ�G�̏o���Ԋu
        public float enemySpeed = 3f;        // �ʏ�G�̈ړ����x
        public GameObject[] enemyPrefabs;    // ����Wave�Ŏg���ʏ�GPrefab�̔z��

        [Header("�{�X�p�ݒ�")]
        public bool isBossWave = false;      // �{�XWave���ǂ����̃t���O
        public GameObject bossPrefab;        // �{�XPrefab
        public Transform bossSpawnPoint;     // �{�X�o���ʒu�i�Œ�j
    }

    [Header("Wave�ݒ�")]
    public Wave[] waves;                     // Wave�z��i�C���X�y�N�^�Őݒ�j

    [Header("�X�|�[���|�C���g�i�ʏ�G�p�j")]
    public Transform[] spawnPoints;          // �ʏ�G�̏o���|�C���g�z��

    private int currentWaveIndex = 0;        // ���݂�Wave�ԍ�
    private int spawnedEnemies = 0;          // ���ݏo���ςݓG�̐�
    private float timer = 0f;                // �o���^�C�}�[
    private bool spawning = false;           // Wave�����ǂ���

    void Start()
    {
        // �Q�[���J�n���͍ŏ���Wave���X�^�[�g
        StartWave(0);
    }

    void Update()
    {
        if (!spawning) return; // Wave���łȂ���Ή������Ȃ�

        timer += Time.deltaTime; // �^�C�}�[�X�V

        Wave wave = waves[currentWaveIndex];

        // --- �ʏ�G�̏o������ ---
        if (!wave.isBossWave && timer >= wave.spawnInterval)
        {
            timer = 0f;
            SpawnEnemy(); // �G�𐶐�
            spawnedEnemies++;

            // Wave�I������
            if (spawnedEnemies >= wave.enemyCount)
            {
                spawning = false;
                // ����Wave�������x�点�ĊJ�n
                Invoke(nameof(StartNextWave), 3f);
            }
        }

        // --- �{�XWave���� ---
        if (wave.isBossWave && spawnedEnemies == 0)
        {
            SpawnBoss();         // �{�X����
            spawnedEnemies = 1;  // �{�X��1�̂̂�
            spawning = false;

            // ��Wave�J�n�܂ŏ����]�T����������
            Invoke(nameof(StartNextWave), 5f);
        }
    }

    // --- Wave�J�n���� ---
    void StartWave(int index)
    {
        if (index >= waves.Length)
        {
            Debug.Log("�S�Ă�Wave�I���I");
            return;
        }

        currentWaveIndex = index;
        spawnedEnemies = 0;
        timer = 0f;
        spawning = true;

        Debug.Log($"Wave {currentWaveIndex + 1} �J�n�I");
    }

    // --- ����Wave�J�n ---
    void StartNextWave()
    {
        StartWave(currentWaveIndex + 1);
    }

    // --- �ʏ�G�o������ ---
    void SpawnEnemy()
    {
        Wave wave = waves[currentWaveIndex];

        if (wave.enemyPrefabs.Length == 0 || spawnPoints.Length == 0)
        {
            Debug.LogError("�GPrefab�܂��̓X�|�[���|�C���g���ݒ肳��Ă��܂���B");
            return;
        }

        // �����_����Prefab��I��
        GameObject prefab = wave.enemyPrefabs[Random.Range(0, wave.enemyPrefabs.Length)];

        // �����_���ɃX�|�[���|�C���g��I��
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // �G�𐶐�
        GameObject enemy = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        // �G�̈ړ����x��Wave�ɍ��킹��
        EnemyController ec = enemy.GetComponent<EnemyController>();
        if (ec != null)
        {
            ec.speed = wave.enemySpeed;
        }
    }

    // --- �{�X�o������ ---
    void SpawnBoss()
    {
        Wave wave = waves[currentWaveIndex];

        if (wave.bossPrefab == null || wave.bossSpawnPoint == null)
        {
            Debug.LogError("�{�XPrefab�܂��̓{�XSpawnPoint���ݒ肳��Ă��܂���B");
            return;
        }

        // �{�X�𐶐�
        Instantiate(wave.bossPrefab, wave.bossSpawnPoint.position, Quaternion.identity);

        Debug.Log("�{�X�o���I");
    }
}
