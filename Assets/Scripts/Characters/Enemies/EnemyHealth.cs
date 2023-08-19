using Gameplay;

namespace Characters.Enemies
{
    /// <summary>
    /// This class handles enemy health and getting hit
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class EnemyHealth : Hittable
    {
        protected override void Die()
        {
            Destroy(transform.parent.gameObject);
            //Debug.Log("Parent destroyed");
            base.Die();
        }
    }
}