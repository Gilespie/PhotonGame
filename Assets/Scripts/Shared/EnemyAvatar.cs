using Fusion;

public class EnemyAvatar : NetworkBehaviour
{
    Enemy _parent;

    void Start()
    {
        _parent = GetComponentInParent<Enemy>();
    }

    public void StopAttacking()
    {
        _parent.SetBoolAttacking(false);
    }

    public void ApplyDamage()
    {
        _parent.ApplyDamage();
    }
}