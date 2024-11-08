using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Character
{
    public class Character : MonoBehaviour
    {
        [Header("生命")]
        protected float MaxHealth;

        private float currentHealth;

        [Header("无敌")]
        public bool invulnerable;
        public float invulnerableDuration;//无敌时间

        [FormerlySerializedAs("OnHurt")] public UnityEvent onHurt;
        [FormerlySerializedAs("OnDie")] public UnityEvent onDie;

        protected virtual void  OnEnable()
        {
            currentHealth = MaxHealth;
        }

        public virtual void TakeDamage(float damage)
        {
            if (invulnerable)
                return;
            if (currentHealth - damage > 0f)
            {
                currentHealth -= damage;
                StartCoroutine(nameof(InvulnerableCoroutine));//启动无敌时间协程
                //执行角色受伤动画
                onHurt?.Invoke();
            }
            else
            {
                //死亡
                Die();
            }
        }

        protected virtual void Die()
        {
            currentHealth = 0f;

            //执行角色死亡动画
            onDie?.Invoke();
        }

        //无敌
        protected virtual IEnumerator InvulnerableCoroutine()
        {
            invulnerable = true;

            //等待无敌时间
            yield return new WaitForSeconds(invulnerableDuration);

            invulnerable = false;
        }
    }
}
