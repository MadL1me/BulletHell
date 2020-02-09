using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;


public class Shooter : Enemy
{
    private List<Projectile> projectiles = new List<Projectile>();

    protected override void AIDecision()
    {
        Attack();
        Move() ;
    }

    protected override void Attack()
    {
        
    }

    protected override void Move()
    {
    }
}

