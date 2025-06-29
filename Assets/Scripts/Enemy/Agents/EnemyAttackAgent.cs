using System;
using System.Diagnostics;
using GameCycleLogic.GameCycleInterfaces;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace ShootEmUp
{
    public sealed class EnemyAttackAgent : 
        MonoBehaviour,
        IFixedUpdatable
    {
        public event Action<GameObject,Vector2,Vector2> OnFire;

        [SerializeField] 
        private WeaponComponent m_weaponComponent;
        
        [SerializeField] 
        private EnemyMoveAgent m_moveAgent;
        
        [SerializeField] 
        private float m_countdown;
        
        public GameObject m_target;
        private float m_currentTime;

        public void SetTarget(GameObject target)
        {
            this.m_target = target;
        }

        public void Reset()
        {
            this.m_currentTime = this.m_countdown;
        }

        void IFixedUpdatable.OnFixedUpdate(float fixedDeltaTime)
        {
            if (!this.m_moveAgent.IsReached)
            {
                return;
            }
            
            if (this.m_target.TryGetComponent(out HitPointsComponent hitPoints))
            {
                if (!hitPoints.IsHitPointsExists())
                {
                    return;
                }
            }

            this.m_currentTime -= fixedDeltaTime;
            
            if (this.m_currentTime <= 0)
            {
                this.Fire();
                this.m_currentTime += this.m_countdown;
            }
        }

        private void Fire()
        {
            var startPosition = this.m_weaponComponent.Position;
            var vector = (Vector2) this.m_target.transform.position - startPosition;
            var direction = vector.normalized;
            this.OnFire?.Invoke(this.gameObject, startPosition, direction);
        }
    }
}