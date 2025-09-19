using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{

    [System.Serializable]
    public class Wave
    {
        [Header("�ʏ�G�ݒ�")]
        public GameObject[] enemyPrefabs;       // ����Wave�ŏo���ʏ�GPrefab�̔z��
        public int enemyCount = 5;              // ����Wave�ŏo���ʏ�G�̑���
        public float spawnInterval = 1f;        // �ʏ�G���o���Ԋu�i�b�j
        public float enemySpeed = 3f;           // �ʏ�G�̈ړ����x

        [Header("�����o���ݒ�")]
        public bool spawnMultipleAtOnce = false; // true�Ȃ瓯���ɕ����̓G���o��
        public int simultaneousCount = 1;        // �����o�����ispawnMultipleAtOnce=true�̂Ƃ��g�p�j

        [Header("�{�XWave�ݒ�")]
        public bool isBossWave = false;         // ����Wave���{�XWave���ǂ���
        public GameObject bossPrefab;           // �{�XPrefab
        public Transform bossSpawnPoint;        // �{�X�̏o���ʒu

        [HideInInspector] public bool bossSpawned = false; // �{�X�����ɏo�����ǂ����̃t���O
        // �� Wave���Ƀ{�X�����x����������Ȃ��悤���䂷��
    }

    [Header("Wave�ݒ�")]
    public Wave[] waves;                        // Wave�̔z���Inspector�Őݒ�

    [Header("�ʏ�G�p�X�|�[���|�C���g")]
    public Transform[] spawnPoints;             // �ʏ�G�̃X�|�[���|�C���g�z��

    private int currentWaveIndex = 0;           // ���݂�Wave�ԍ�
    private int spawnedEnemies = 0;             // ����Wave�ŏo���ς݂̒ʏ�G��
    private float timer = 0f;                   // �o���Ԋu�p�^�C�}�[
    private bool spawning = false;              // Wave�o�������ǂ���

    void Start()
    {
        // �Q�[���J�n���ɍŏ���Wave���J�n
        StartWave(0);
    }

    void Update()
    {
        if (!spawning) return; // Wave�o�����łȂ���Ή������Ȃ�

        Wave wave = waves[currentWaveIndex];

        // �{�X��������
        // �{�XWave�ł܂��{�X���o�Ă��Ȃ����1�̂�������
        if (wave.isBossWave && !wave.bossSpawned)
        {
            SpawnBoss();           // �{�X����
            wave.bossSpawned = true; // �t���O�𗧂Ă邱�ƂōĐ����h�~
        }

        // �ʏ�G��������
        if (wave.enemyCount > 0 && spawnedEnemies < wave.enemyCount)
        {
            timer += Time.deltaTime; // �^�C�}�[��i�߂�i�o�ߎ��Ԃ����Z�j

            if (timer >= wave.spawnInterval) // ���Ԋu�o�߂œG����
            {
                timer = 0f; // �^�C�}�[���Z�b�g

                int spawnNum = 1; // �f�t�H���g��1�̂�����

                if (wave.spawnMultipleAtOnce)
                {
                    // �����o�����Ǝc��̓G���̏��������𐶐�
                    spawnNum = Mathf.Min(wave.simultaneousCount, wave.enemyCount - spawnedEnemies);
                }

                // �G�� spawnNum �̐���
                for (int i = 0; i < spawnNum; i++)
                {
                    SpawnEnemy();
                    spawnedEnemies++;
                }
            }
        }

        // Wave�I������
        // �ʏ�G��S�ďo���؂�����Wave�I��
        if (spawnedEnemies >= wave.enemyCount)
        {
            spawning = false;

            // �{�XWave�̏ꍇ�͎����Ŏ�Wave�ɐi�܂Ȃ�
            // �i�{�X���j��ɕʏ����Ŏ�Wave�ɐi�ޑz��j
            if (!wave.isBossWave)
                Invoke(nameof(StartNextWave), 3f); // 3�b��Ɏ�Wave�J�n
        }
    }

    // Wave�J�n����
    void StartWave(int index)
    {
        if (index >= waves.Length)
        {
            Debug.Log("�S�Ă�Wave�I���I");
            return;
        }

        currentWaveIndex = index;
        spawnedEnemies = 0;        // �ʏ�G�o���J�E���^�[���Z�b�g
        timer = 0f;                // �^�C�}�[���Z�b�g
        spawning = true;           // Wave�o�����t���OON

        // �{�X�����t���O��Wave�J�n���ɕK��false�Ƀ��Z�b�g
        waves[currentWaveIndex].bossSpawned = false;

        Debug.Log($"Wave {currentWaveIndex + 1} �J�n�I");
    }

    // ����Wave�J�n
    void StartNextWave()
    {
        StartWave(currentWaveIndex + 1);
    }

    // �ʏ�G����
    void SpawnEnemy()
    {
        Wave wave = waves[currentWaveIndex];

        if (wave.enemyPrefabs.Length == 0 || spawnPoints.Length == 0)
        {
            Debug.LogWarning("�GPrefab�܂��̓X�|�[���|�C���g���ݒ肳��Ă��܂���B");
            return;
        }

        // �����_����Prefab��I��
        GameObject prefab = wave.enemyPrefabs[Random.Range(0, wave.enemyPrefabs.Length)];
        // �����_���ɃX�|�[���|�C���g��I��
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // �G�𐶐�
        GameObject enemy = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        // �G�̑��x��ݒ�
        EnemyController ec = enemy.GetComponent<EnemyController>();
        if (ec != null) ec.speed = wave.enemySpeed;
    }

    // �{�X����
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
