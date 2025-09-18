using UnityEngine;

public class WaveManager : MonoBehaviour
{

    [System.Serializable] // �C���X�y�N�^�ɕ\���ł���悤�ɂ���
    public class Wave
    {
        public int enemyCount = 5;        // ����Wave�ŏo���G�̐�
        public int enemiesPerSpawn = 1;   // 1���Spawn�ŉ��̏o����
        public float spawnInterval = 2f;  // �G���o���Ԋu�i�b�j
        public float enemySpeed = 3f;     // ����Wave�̓G�̈ړ����x
    }

    [Header("Wave�ݒ�")]
    public Wave[] waves; // ������Wave���C���X�y�N�^�Őݒ�ł���悤�ɔz��

    [Header("�o���Ɏg���v���n�u / �X�|�[���|�C���g")]
    public GameObject enemyPrefab;   // �o��������G�v���n�u
    public Transform[] spawnPoints;  // �����̏o���ʒu��z��ŊǗ��i�����_���őI�ԁj

    private int currentWaveIndex = 0; // ���ǂ�Wave��
    private int spawnedEnemies = 0;   // ����Wave�ŏo�����G�̐�
    private float timer = 0f;         // �o���Ԋu�𑪂�^�C�}�[
    private bool spawning = false;    // ��Wave�i�s�����ǂ���

    void Start()
    {
        StartWave(0); // �ŏ���Wave���J�n
    }

    void Update()
    {
        if (!spawning) return; // Wave�i�s���łȂ���Ή������Ȃ�

        timer += Time.deltaTime; // �o�ߎ��Ԃ𑫂��Ă���
        if (timer >= waves[currentWaveIndex].spawnInterval)
        {
            timer = 0f; // �^�C�}�[���Z�b�g
            SpawnEnemy(); // �G�𐶐�

            spawnedEnemies++; // �o�����J�E���g

            // Wave�̓G�����o���I������玟��Wave��
            if (spawnedEnemies >= waves[currentWaveIndex].enemyCount)
            {
                spawning = false; // ����Wave�I��
                Invoke(nameof(StartNextWave), 3f); // 3�b��Ɏ���Wave�J�n
            }
        }
    }

    void StartWave(int index)
    {
        if (index >= waves.Length)
        {
            Debug.Log("�S�Ă�Wave�I���I");
            return; // Wave�����������ꍇ�͏I��
        }

        currentWaveIndex = index; // ����Wave�ԍ����X�V
        spawnedEnemies = 0;       // �o�������Z�b�g
        timer = 0f;               // �^�C�}�[���Z�b�g
        spawning = true;          // Wave�J�n�t���O

        Debug.Log($"Wave {currentWaveIndex + 1} �J�n�I");
    }

    void StartNextWave()
    {
        StartWave(currentWaveIndex + 1);
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null || spawnPoints.Length == 0)
        {
            Debug.LogError("[WaveManager] enemyPrefab �܂��� spawnPoints ���ݒ肳��Ă��܂���B");
            return;
        }

        // 1���Spawn�ŕ����̐���
        for (int i = 0; i < waves[currentWaveIndex].enemiesPerSpawn; i++)
        {
            // �����_���ȏo���|�C���g��I��
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

            EnemyController ec = enemy.GetComponent<EnemyController>();
            if (ec != null)
            {
                ec.speed = waves[currentWaveIndex].enemySpeed;
            }

            spawnedEnemies++; // ���������Ԃ�J�E���g
            if (spawnedEnemies >= waves[currentWaveIndex].enemyCount)
                break; // �o�����𒴂�����I��
        }
    }
}
