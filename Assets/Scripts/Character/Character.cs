using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Character
{
    public class Character : MonoBehaviour
    {
        [Header("����")]
        protected float MaxHealth;

        private float currentHealth;

        [Header("�޵�")]
        public bool invulnerable;
        public float invulnerableDuration;//�޵�ʱ��

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
                StartCoroutine(nameof(InvulnerableCoroutine));//�����޵�ʱ��Э��
                //ִ�н�ɫ���˶���
                onHurt?.Invoke();
            }
            else
            {
                //����
                Die();
            }
        }

        protected virtual void Die()
        {
            currentHealth = 0f;

            //ִ�н�ɫ��������
            onDie?.Invoke();
        }

        //�޵�
        protected virtual IEnumerator InvulnerableCoroutine()
        {
            invulnerable = true;

            //�ȴ��޵�ʱ��
            yield return new WaitForSeconds(invulnerableDuration);

            invulnerable = false;
        }
    }
}
