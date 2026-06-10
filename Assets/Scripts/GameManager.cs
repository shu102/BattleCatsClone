using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Bases")]
    public GameObject playerBase;
    public GameObject enemyBase;

    [Header("Prefabs")]
    public GameObject playerUnitPrefab;
    public GameObject playerUnitTankPrefab;
    public GameObject playerUnitSpeedPrefab;
    public GameObject enemyUnitPrefab;

    [Header("Spawn Points")]
    public Transform playerSpawnPoint;
    public Transform enemySpawnPoint;

    [Header("UI")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI resultText;

    [Header("Money")]
    public float money = 100f;
    public float unitCost = 20f;
    public float tankCost = 40f;
    public float speedCost = 10f;
    public float moneyPerSecond = 5f;

    [Header("Enemy Spawn")]
    public float enemySpawnInterval = 3f;
    private float enemySpawnTimer = 0f;

    [Header("Audio")]
    public AudioClip bgmClip;
    public AudioClip attackSEClip;
    public AudioClip spawnSEClip;
    public AudioClip winSEClip;
    public AudioClip loseSEClip;

    private AudioSource bgmSource;
    private AudioSource seSource;
    private bool gameOver = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // BGM用AudioSource
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        bgmSource.volume = 0.5f;
        bgmSource.Play();

        // SE用AudioSource
        seSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (gameOver) return;

        money += moneyPerSecond * Time.deltaTime;
        moneyText.text = "Money: " + (int)money;

        enemySpawnTimer += Time.deltaTime;
        if (enemySpawnTimer >= enemySpawnInterval)
        {
            Instantiate(enemyUnitPrefab, enemySpawnPoint.position, Quaternion.identity);
            enemySpawnTimer = 0f;
        }

        if (playerBase == null) { EndGame(false); }
        else if (enemyBase == null) { EndGame(true); }
    }

    void EndGame(bool win)
    {
        gameOver = true;
        resultText.text = win ? "You Win!" : "You Lose...";
        bgmSource.Stop();
        seSource.PlayOneShot(win ? winSEClip : loseSEClip);
    }

    public void SpawnPlayerUnit()
    {
        if (money < unitCost) return;
        money -= unitCost;
        Instantiate(playerUnitPrefab, playerSpawnPoint.position, Quaternion.identity);
        seSource.PlayOneShot(spawnSEClip);
    }

    public void SpawnPlayerUnitTank()
    {
        if (money < tankCost) return;
        money -= tankCost;
        Instantiate(playerUnitTankPrefab, playerSpawnPoint.position, Quaternion.identity);
        seSource.PlayOneShot(spawnSEClip);
    }

    public void SpawnPlayerUnitSpeed()
    {
        if (money < speedCost) return;
        money -= speedCost;
        Instantiate(playerUnitSpeedPrefab, playerSpawnPoint.position, Quaternion.identity);
        seSource.PlayOneShot(spawnSEClip);
    }

    public void PlayAttackSE()
    {
        seSource.PlayOneShot(attackSEClip);
    }
}