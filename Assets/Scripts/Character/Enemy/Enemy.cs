using System;
using System.Collections;
using System.Collections.Generic;
using Atsar;
using Pathfinding;
using StateMachine;
using StateMachine.Enemy;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Character.Enemy
{
    public enum EnemyStateType
    {
        Idle,Chase,Patrol,Attack,Hurt,Death
    }

    public class Enemy : Character
    {
        #region 组件
        [HideInInspector] public Rigidbody2D rb;
        [HideInInspector] public SpriteRenderer sr;
        [HideInInspector] public Animator anim;
        [HideInInspector] public Collider2D col;
        [HideInInspector] public Transform player;
        private Seeker seeker;
        private AstarSeeker astar;
        #endregion

        #region 变量定义
        [Header("移动")]
        public float speed = 1f;//移动速度
        public Vector2 moveDirection;//移动方向
        public float distance;//实时距离（追击范围内）
        [FormerlySerializedAs("ChaseDistance")] public float chaseDistance = 3f;//追击距离
        [FormerlySerializedAs("AttackDistance")] public float attackDistance = 1f;//攻击距离

        [Header("巡逻")]
        public float idleDuration;//待机时间
        public Transform[] patrolPointList;//巡逻点
        public int targetPointIndex;//目标路径点索引

        [Header("自动寻路")]
        public List<Vector3> pathPointList;//路径点列表
        public int currentIndex;//路径点索引
        private readonly float pathGenerateInterval = 0.5f;//路径刷新间隔
        private float pathGernerateTimer;//计时器

        [Header("攻击")]
        public float attackPower;//攻击力
        public float attackCoolDownDuration;//攻击冷却时间
        public bool isAttack = true;//攻击许可
        public Vector2 attackSize = new Vector2(1f, 1f);//攻击范围大小
        private Vector2 attackAreaPos;//攻击范围中心
        public float offsetX = 0.5f;//x轴偏移
        public float offsetY = -0.2f;//y轴偏移

        [Header("击退")]
        public bool isKnockback = true;//是否在被击退
        public float knockbackForce = 10f;//击退力
        public float knockbackDuration = 0.1f;//击退持续时间

        [Header("受伤死亡")]
        public bool isHurt;
        public bool isDeath;
        #endregion

        #region 状态机

        private IState currentState;

        private readonly Dictionary<EnemyStateType, IState> states = new Dictionary<EnemyStateType, IState>();
        #endregion

        #region 启用禁用
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponent<SpriteRenderer>();
            anim = GetComponent<Animator>();
            col = GetComponent<Collider2D>();
            seeker = GetComponent<Seeker>();
            astar = GetComponent<AstarSeeker>();

            states.Add(EnemyStateType.Idle, new EnemyIdleState(this));
            states.Add(EnemyStateType.Attack, new EnemyAttackState(this));
            states.Add(EnemyStateType.Hurt, new EnemyHurtState(this));
            states.Add(EnemyStateType.Death, new EnemyDeathState(this));
            states.Add(EnemyStateType.Chase, new EnemyChaseState(this));
            states.Add(EnemyStateType.Patrol, new EnemyPatrolState(this));

            TransitionState(EnemyStateType.Idle);
        }

        protected override void OnEnable()
        {
            MaxHealth = 100f;
            base.OnEnable();
            // EnemyManager.Instance.enemyCount++;
        }

        private void OnDisable()
        {
            // EnemyManager.Instance.enemyCount--;
        }

        #endregion

        #region 工具函数
        //状态切换函数
        public void TransitionState(EnemyStateType type)
        {
            if (currentState != null)
            {
                currentState.OnExit();
            }

            currentState = states[type];
            currentState.OnEnter();
        }

        //获取玩家位置（追击范围内）
        public void GetPlayerTransfrom()
        {
            Collider2D[] chaseColliders = Physics2D.OverlapCircleAll(transform.position, chaseDistance);

            foreach (Collider2D collider in chaseColliders)
            {
                if (collider.CompareTag("Player"))
                {
                    player = collider.transform;
                    distance = Vector2.Distance(transform.position, player.position);
                    break;
                }
                else
                {
                    player = null;
                }
            }
        }

        //生成到巡逻点的路径
        public void GeneratePatrolPoint()
        {
            //随机获得一个巡逻点（非当前巡逻点）
            while (true)
            {
                int i = Random.Range(0, patrolPointList.Length);

                if(targetPointIndex != i)
                {
                    targetPointIndex = i;
                    break;
                }
            }

            //生成到巡逻点的路径
            GeneratePath(patrolPointList[targetPointIndex].position);
        }


        //自动寻路
        public void AutoPath()
        {
            if (player == null)
            {
                return;
            }

            pathGernerateTimer += Time.deltaTime;

            //固定时间刷新路径
            if (pathGernerateTimer >= pathGenerateInterval)
            {
                GeneratePath(player.position);
                pathGernerateTimer = 0;//重置计时器
            }

            //路径点为空时生成路径
            if (pathPointList == null || pathPointList.Count <= 0)
            {
                GeneratePath(player.position);
            }
            //到达当前路径点
            else if (pathPointList.Count>0 && Vector2.Distance(pathPointList[currentIndex], transform.position) <= 0.1f)
            {
                //索引指向下一个路径点
                currentIndex++;
                //到达终点时重新生成路径
                if (currentIndex >= pathPointList.Count)
                {
                    GeneratePath(player.position);
                }
            }
        }

        //路径生成函数
        // ReSharper disable Unity.PerformanceAnalysis
        private void GeneratePath(Vector3 target)
        {
            currentIndex = 0;

            // //起点，终点，回调函数（获取路径点并存入路径点列表）
            // seeker.StartPath(transform.position, target, path => { pathPointList = path.vectorPath; });
            
            pathPointList = astar.pathFind(transform.position, target);
        }

        //绘图测试
        private void OnDrawGizmos()
        {
            //攻击范围
            //Gizmos.color = Color.red;
            //Gizmos.DrawWireSphere(transform.position, AttackDistance);

            //追击范围
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
        #endregion

        #region 更新
        private void Update()
        {
            currentState.OnUpdate();
        }

        private void FixedUpdate()
        {
            currentState.OnFixedUpdate();
        }
        #endregion

        #region 移动
        public void Move()
        {
            if (moveDirection.magnitude >= 0.1f && speed > 0f) 
            {
                rb.velocity = moveDirection.normalized * speed;
                if (moveDirection.x < 0)//??
                {
                    sr.flipX = true;
                }
                if (moveDirection.x > 0)//??
                {
                    sr.flipX = false;
                }
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
        #endregion

        #region 攻击
        public void EnemyAttackAnimationEvent()
        {
            //攻击范围中心
            attackAreaPos = transform.position;
            //角色翻转
            offsetX = sr.flipX ? -Mathf.Abs(offsetX) : Mathf.Abs(offsetX);

            attackAreaPos.x += offsetX;
            attackAreaPos.y += offsetY;

            Collider2D[] hitColliders = Physics2D.OverlapBoxAll(attackAreaPos, attackSize, 0f);
            //伤害判定
            foreach (Collider2D col in hitColliders)
            {
                if (col.CompareTag("Player"))
                {
                    col.GetComponent<Character>().TakeDamage(attackPower);
                }
            }
        }

        public void AttackCoolDown()
        {
            StartCoroutine(nameof(AttackOnCoolDownCoroutine));
        }

        public IEnumerable AttackOnCoolDownCoroutine()
        {
            isAttack = false;

            yield return new WaitForSeconds(attackCoolDownDuration);

            isAttack = true;
        }
        #endregion

        #region 受伤
        public void EnemyHurt()
        {
            isHurt = true;
        }
        #endregion

        #region 死亡
        public void EnemyDeath()
        {
            TransitionState(EnemyStateType.Death);
        }
        public void DestroyEnemy()
        {
            Destroy(this.gameObject);
        }
        #endregion
    }
}