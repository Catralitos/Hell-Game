using Events.ScriptableObjects;

namespace Characters.Enemies
{
    public class Angel : Enemy
    {
        public int angelBatch;

        public IntEventChannelSO angelDiedEvent;
    
        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
            this.info.Type = "Angel";
            this.combat.simpleDamage = 5;
            this.combat.DmgRoll = () => this.combat.simpleDamage;
            this.info.awakeDistance = 5;
            this.combat.attackRange = 1;
            this.combat.AC = 20;
            this.combat.attackCooldown = 2;
            this.combat.cooldownLeft = 0;
        }

        private void OnDestroy()
        {
            angelDiedEvent.RaiseEvent(angelBatch);
        }
    }
}
