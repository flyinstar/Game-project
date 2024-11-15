using System.Collections;
using System.Collections.Generic;
using Input;
using StateMachine;
using StateMachine.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character.Player
{
    public enum PlayerStateType
    {
        Idle,Move,Attack,Dodge,Hurt,Death
    }

    public class Player : Character
    {
        #region 编译器优化
        private static readonly int StopName = Animator.StringToHash("AnimationStop");
        private static readonly int AttackName = Animator.StringToHash("Attack");
        #endregion
        
        #region 组件
        [Header("组件")]
        public PlayerInput input;
        [HideInInspector] public Rigidbody2D rb;
        [HideInInspector] public SpriteRenderer sr;
        [HideInInspector] public Animator anim;
        #endregion

        #region 变量定义
        [Header("移动")]
        public Vector2 inputDirection;
        [FormerlySerializedAs("BaseSpeed")] public float baseSpeed;
        public float speed;
        
        [Header("召唤")]
        public GameObject petPrefab;
        private GameObject petInstance;
        private bool isCalled;

        [Header("攻击")]
        public float attackPower;//攻击力
        public bool isAttack;
        public Vector2 attackSize = new Vector2(1f, 1f);
        private Vector2 attackAreaPos;
        public float offsetX =1f;
        public float offsetY =1f;
        public Enemy.Enemy attackTarget;

        [Header("闪避/冲刺")]
        public float dodgeDuration;//闪避持续
        public float dodgeForce;//推力
        public bool isDodge;
        public float dodgeCoolDownDuration;//闪避冷却时间
        public bool isDodgeOnCoolDown;

        [Header("受伤死亡")]
        public bool isHurt;
        public bool isDeath;
        #endregion
    
        #region 状态机
        private IState currentstate;

        private readonly Dictionary <PlayerStateType,IState> states = new Dictionary<PlayerStateType,IState>();
        #endregion
    
        #region 启用禁用
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponent<SpriteRenderer>();
            anim = GetComponent<Animator>();

            states.Add(PlayerStateType.Idle, new PlayerIdleState(this));
            states.Add(PlayerStateType.Move, new PlayerMoveState(this));
            states.Add(PlayerStateType.Attack, new PlayerAttackState(this));
            states.Add(PlayerStateType.Dodge, new PlayerDodgeState(this));
            states.Add(PlayerStateType.Hurt, new PlayerHurtState(this));
            states.Add(PlayerStateType.Death, new PlayerDeathState(this));

            input.EnablePlayerInput();
            TransitionState(PlayerStateType.Idle);
        }

        protected override void OnEnable()
        {
            input.onMove += Move;
            input.onMoveStop += OnMoveStop;
            input.onAttack += Attack;
            input.onDodge += Dodge;
            input.onCallPet += CallPet;
            // input.onFire += Fire;
            base.OnEnable();
        }

        private void OnDisable()
        {
            input.onMove -= Move;
            input.onMoveStop -= OnMoveStop;
            input.onAttack -= Attack;
            input.onDodge -= Dodge;
        }
        #endregion
    
        #region 工具函数
        //状态切换函数
        public void TransitionState(PlayerStateType type)
        {
            if (currentstate != null)
            {
                currentstate.OnExit();
            }

            currentstate = states[type];
            currentstate.OnEnter();
        }

        //动画停止（动画行为异常时用）
        public void AnimationStop()
        {
            anim.SetTrigger(StopName);
        }

        //绘图测试
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(attackAreaPos, attackSize);
        }
        #endregion

        #region 更新
        private void Update()
        {
            currentstate.OnUpdate();
        }

        private void FixedUpdate()
        {
            currentstate.OnFixedUpdate();
        }
        #endregion
    
        #region 移动
        //PlayerInput调用
        private void Move(Vector2 moveInput)
        {
            inputDirection = moveInput.normalized;
            speed = baseSpeed;
        }
        //PlayerMoveState调用
        public void Move()
        {
            rb.velocity = inputDirection * speed;
            if (inputDirection.x < 0)
            {
                sr.flipX = true;
            }
            if (inputDirection.x > 0)
            {
                sr.flipX = false;
            }
        }
        //PlayerInput调用
        private void OnMoveStop()
        {
            speed = 0;
        }
        #endregion
    
        #region 攻击

        private void Attack()
        {
            anim.SetTrigger(AttackName);
            isAttack = true;
        }

        // private void Fire()
        // {
        //     anim.SetTrigger("Fire");
        // }

        //攻击动画帧事件
        void AttackAnimationEvent(float magnification)
        {
            //攻击范围中心
            attackAreaPos = transform.position;
            //人物翻转
            offsetX = sr.flipX ? -Mathf.Abs(offsetX) : Mathf.Abs(offsetX);

            attackAreaPos.x += offsetX;
            attackAreaPos.y += offsetY;

            Collider2D[] hitColliders = Physics2D.OverlapBoxAll(attackAreaPos, attackSize, 0f);
            //伤害判定
            foreach (Collider2D col in hitColliders)
            {
                if (col.CompareTag("Enemy"))
                {
                    attackTarget = col.GetComponent<Enemy.Enemy>();
                    col.GetComponent<Character>().TakeDamage(attackPower * magnification);
                }
            }
        }
        #endregion
    
        #region 闪避

        private void Dodge()
        {
            if (!isDodge && !isDodgeOnCoolDown)
            {
                isDodge = true;
            }
        }

        //闪避冷却
        public void DodgeOnCoolDown()
        {
            StartCoroutine(nameof(DodgeOnCoolDownCoroutine));
        }

        public IEnumerator DodgeOnCoolDownCoroutine()
        {
            yield return new WaitForSeconds(dodgeCoolDownDuration);
        
            isDodgeOnCoolDown = false;
        }
        #endregion

        #region 受伤
        public void PlayerHurt()
        {
            isHurt = true;
        }
        #endregion

        #region 死亡
        public void PlayerDeath()
        {
            isDeath = true;
            TransitionState(PlayerStateType.Death);
        }
        #endregion

        #region 召唤
        private void CallPet()
        {
            if (petInstance == null)
            {
                petInstance = Instantiate(petPrefab, transform.position, Quaternion.identity);
                petInstance.transform.SetParent(this.transform);
            }
            
            if (!isCalled)
            {
                petInstance.transform.position = transform.position + new Vector3(0, -1f, 0);
                petInstance.SetActive(true);
                isCalled = true;
            }
            else if (isCalled)
            {
                petInstance.SetActive(false);
                isCalled = false;
            }
        }
        #endregion
    }
}