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
        #region ���
        [HideInInspector] public Rigidbody2D rb;
        [HideInInspector] public SpriteRenderer sr;
        [HideInInspector] public Animator anim;
        [HideInInspector] public Seeker seeker;
        [HideInInspector] public Player.Player player;
        #endregion

        #region ��������
        [Header("�ƶ�")]
        public float defaultSpeed;
        [HideInInspector] public float speed;
        [HideInInspector] public Vector2 moveDirection;

        [Header("����")]
        [HideInInspector] public float distance;
        public float idleDistance = 1f;
        public float followDistance = 10f;
        
        [Header("�Զ�Ѱ·")]
        public List<Vector3> pathPointList;//·�����б�
        public int currentIndex;//·��������
        private readonly float pathGenerateInterval = 0.5f;//·��ˢ�¼��
        private float pathGernerateTimer;//��ʱ��
        
        [Header("����")]
        public Enemy.Enemy attackTarget;
        public float attackPower;
        public float attackDistance;
        #endregion

        #region ״̬��

        private IState currentState;

        private readonly Dictionary<PetStateType,IState> states = new Dictionary<PetStateType,IState>();
        #endregion

        #region ���ý���
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

        #region  ���ߺ���
        //״̬�л�����
        public void TransitionState(PetStateType type)
        {
            if (currentState != null)
            {
                currentState.OnExit();
            }

            currentState = states[type];
            currentState.OnEnter();
        }
        
        //�Զ�Ѱ·
        public void AutoPath(Transform target)
        {
            if (target == null)
            {
                return;
            }

            pathGernerateTimer += Time.deltaTime;

            //�̶�ʱ��ˢ��·��
            if (pathGernerateTimer >= pathGenerateInterval)
            {
                GeneratePath(target.position);
                pathGernerateTimer = 0;//���ü�ʱ��
            }

            //·����Ϊ��ʱ����·��
            if (pathPointList == null || pathPointList.Count <= 0)
            {
                GeneratePath(target.position);
            }
            //���ﵱǰ·����
            else if (Vector2.Distance(pathPointList[currentIndex], transform.position) <= 0.1f)
            {
                //����ָ����һ��·����
                currentIndex++;
                //�����յ�ʱ��������·��
                if (currentIndex >= pathPointList.Count)
                {
                    GeneratePath(target.position);
                }
            }
        }

        //·�����ɺ���
        private void GeneratePath(Vector3 target)
        {
            currentIndex = 0;

            //��㣬�յ㣬�ص���������ȡ·���㲢����·�����б�
            seeker.StartPath(transform.position, target, path => { pathPointList = path.vectorPath; });
        }

        #endregion

        #region ����
        private void Update()
        {
            currentState.OnUpdate();
        }

        private void FixedUpdate()
        {
            currentState.OnFixedUpdate();
        }
        #endregion

        #region �ƶ�
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

        #region ����

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
