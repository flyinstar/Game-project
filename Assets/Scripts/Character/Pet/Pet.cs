using System.Collections.Generic;
using StateMachine;
using StateMachine.Pet;
using UnityEngine;
using Pathfinding;

namespace Character.Pet
{
    public enum PetStateType
    {
        Idle,Follow,Chase,Attack,Hurt,Death
    }
    
    public class Pet : Character
    {
        #region 组件
        [HideInInspector] public Rigidbody2D rb;
        [HideInInspector] public SpriteRenderer sr;
        [HideInInspector] public Animator anim;
        [HideInInspector] public Seeker seeker;
        [HideInInspector] public Player.Player player;
        #endregion

        #region 变量定义
        [Header("移动")]
        public float defaultSpeed;
        [HideInInspector] public float speed;
        [HideInInspector] public Vector2 moveDirection;

        [Header("跟随")]
        [HideInInspector] public float distance;
        public float idleDistance = 1f;
        public float followDistance = 10f;
        
        [Header("自动寻路")]
        public List<Vector3> pathPointList;//路径点列表
        public int currentIndex;//路径点索引
        private readonly float pathGenerateInterval = 0.5f;//路径刷新间隔
        private float pathGernerateTimer;//计时器
        
        [Header("攻击")]
        public Enemy.Enemy attackTarget;
        public float attackPower;
        public float attackDistance;
        #endregion

        #region 状态机

        private IState currentState;

        private readonly Dictionary<PetStateType,IState> states = new Dictionary<PetStateType,IState>();
        #endregion

        #region 启用禁用
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponent<SpriteRenderer>();
            anim = GetComponent<Animator>();
            seeker = GetComponent<Seeker>();
            player = GetComponentInParent<Player.Player>();

            states.Add(PetStateType.Idle, new PetIdleState(this));
            states.Add(PetStateType.Follow, new PetFollowState(this));
            states.Add(PetStateType.Chase, new PetChaseState(this));
            states.Add(PetStateType.Attack, new PetAttackState(this));
            states.Add(PetStateType.Hurt, new PetHurtState(this));
            states.Add(PetStateType.Death, new PetDeathState(this));

            TransitionState(PetStateType.Idle);
        }

        protected override void OnEnable()
        {
            //base.maxHealth = 100f;
            //base.OnEnable();
        }
        #endregion

        #region  工具函数
        //状态切换函数
        public void TransitionState(PetStateType type)
        {
            if (currentState != null)
            {
                currentState.OnExit();
            }

            currentState = states[type];
            currentState.OnEnter();
        }
        
        //自动寻路
        public void AutoPath(Transform target)
        {
            if (target == null)
            {
                return;
            }

            pathGernerateTimer += Time.deltaTime;

            //固定时间刷新路径
            if (pathGernerateTimer >= pathGenerateInterval)
            {
                GeneratePath(target.position);
                pathGernerateTimer = 0;//重置计时器
            }

            //路径点为空时生成路径
            if (pathPointList == null || pathPointList.Count <= 0)
            {
                GeneratePath(target.position);
            }
            //到达当前路径点
            else if (Vector2.Distance(pathPointList[currentIndex], transform.position) <= 0.1f)
            {
                //索引指向下一个路径点
                currentIndex++;
                //到达终点时重新生成路径
                if (currentIndex >= pathPointList.Count)
                {
                    GeneratePath(target.position);
                }
            }
        }

        //路径生成函数
        private void GeneratePath(Vector3 target)
        {
            currentIndex = 0;

            //起点，终点，回调函数（获取路径点并存入路径点列表）
            seeker.StartPath(transform.position, target, path => { pathPointList = path.vectorPath; });
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

        public void GetDistance()
        {
            distance = Vector2.Distance(transform.position, player.transform.position);
        }
        #endregion

        #region 攻击

        public void GetTarget()
        {
            attackTarget = player.attackTarget;
            if (attackTarget!=null && Vector2.Distance(attackTarget.transform.position, transform.position) > followDistance)
            {
                attackTarget = null;
            }
        }
        
        public void Attack()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackDistance);

            foreach (var variable in colliders)
            {
                if (variable.CompareTag("Enemy"))
                {
                    variable.GetComponent<Character>().TakeDamage(attackPower);
                }
            }
        }
        #endregion
    }
}
