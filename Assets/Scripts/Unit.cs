using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool isPlayer;
    public float speed = 2f;
    public float hp = 10f;
    public float attack = 2f;
    public float attackRange = 0.5f;
    public float attackInterval = 1f;
    private float attackTimer = 0f;
    private Transform hpBar;
    private float maxHp;

    void Start() { maxHp = hp; hpBar = transform.Find("HPBarBG/HPBar"); }

    void Update()
    {
        Move();
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackInterval) TryAttack();
    }

    void Move()
    {
        string targetTag = isPlayer ? "Enemy" : "Player";
        float dir = isPlayer ? 1f : -1f;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (var hit in hits) { if (hit.CompareTag(targetTag)) return; }
        transform.Translate(Vector2.right * dir * speed * Time.deltaTime);
    }

    void TryAttack()
    {
        string targetTag = isPlayer ? "Enemy" : "Player";
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (var hit in hits)
        {
            if (hit.CompareTag(targetTag))
            {
                hit.GetComponent<Unit>()?.TakeDamage(attack);
                attackTimer = 0f;
                if (GameManager.instance != null)
                    GameManager.instance.PlayAttackSE();
                return;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hpBar != null)
        {
            float ratio = hp / maxHp;
            hpBar.localScale = new Vector3(ratio, 1, 1);
            hpBar.localPosition = new Vector3((ratio - 1) / 2f, 0, -0.1f);
        }
        if (hp <= 0f) Destroy(gameObject);
    }
}