using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    // --- Wave�ݒ�p�N���X ---
    [System.Serializable]
    public class Wave
    {
        [Header("�ʏ�G�ݒ�")]
        public GameObject[] enemyPrefabs;       // ����Wave�ŏo���GPrefab�̔z��
        public int enemyCount = 5;              // ����Wave�ŏo������
        public float spawnInterval = 1f;        // �G���o���Ԋu
        public float enemySpeed = 3f;           // �G�̈ړ����x

        [Header("�����o���ݒ�")]
        public bool spawnMultipleAtOnce = false; // true�Ȃ瓯���ɕ����o��
        public int simultaneousCount = 1;        // �����o�����ispawnMultipleAtOnce=true���Ɏg�p�j

        [Header("�{�XWave�ݒ�")]
        public bool isBossWave = false;         // ����Wave���{�XWave��
        public GameObject bossPrefab;           // �{�XPrefab
        public Transform bossSpawnPoint;        // �{�X�̏o���ʒu
    }

    [Header("Wave�ݒ�")]
    public Wave[] waves;                        // �C���X�y�N�^��Wave��ݒ�

    [Header("�ʏ�G�p�X�|�[���|�C���g")]
    public Transform[] spawnPoints;             // �ʏ�G�̏o���|�C���g�z��

    private int currentWaveIndex = 0;           // ���݂�Wave�ԍ�
    private int spawnedEnemies = 0;             // �o���ςݓG�̐�
    private float timer = 0f;                   // �o���Ԋu�^�C�}�[
    private bool spawning = false;              // Wave�o�����t���O

    void Start()
    {
        // �Q�[���J�n���ɍŏ���Wave���X�^�[�g
        StartWave(0);
    }

    void Update()
    {
        if (!spawning) return; // Wave���łȂ���Ή������Ȃ�

        Wave wave = waves[currentWaveIndex];

        // --- �{�XWave���� ---
        if (wave.isBossWave)
        {
            // �{�X��1�̂̂ݐ���
            if (spawnedEnemies == 0)
            {
                SpawnBoss();
                spawnedEnemies = 1;
                spawning = false;

                // ��Wave�J�n�܂ŏ����]�T����������
                Invoke(nameof(StartNextWave), 5f);
            }
            return;
        }

        // --- �ʏ�G�o������ ---
        timer += Time.deltaTime;
        if (timer >= wave.spawnInterval)
        {
            timer = 0f;

            if (wave.spawnMultipleAtOnce)
            {
                // �����o���������G�𐶐�
                int spawnNum = Mathf.Min(wave.simultaneousCount, wave.enemyCount - spawnedEnemies);

                for (int i = 0; i < spawnNum; i++)
                {
                    SpawnEnemy();
                    spawnedEnemies++;
                }

                // Wave�I������
                if (spawnedEnemies >= wave.enemyCount)
                {
                    spawning = false;
                    Invoke(nameof(StartNextWave), 3f); // ��Wave�J�n�܂ŏ����x��
                }
            }
            else
            {
                // 1�̂��o��
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

    // --- Wave�J�n ---
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

    // --- ����Wave���J�n ---
    void StartNextWave()
    {
        StartWave(currentWaveIndex + 1);
    }

    // --- �ʏ�G���� ---
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

        // �G����
        GameObject enemy = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        // �G�̑��x��ݒ�
        EnemyController ec = enemy.GetComponent<EnemyController>();
        if (ec != null)
        {
            ec.speed = wave.enemySpeed;
        }
    }

    // --- �{�X���� ---
    void SpawnBoss()
    {
        Wave wave = waves[currentWaveIndex];

        if (wave.bossPrefab == null || wave.bossSpawnPoint == null)
        {
            Debug.LogError("�{�XPrefab�܂��̓{�XSpawnPoint���ݒ肳��Ă��܂���B");
            return;
        }

        Instantiate(wave.bossPrefab, wave.bossSpawnPoint.position, Quaternion.identity);
        Debug.Log("�{�X�o���I");
    }
}
