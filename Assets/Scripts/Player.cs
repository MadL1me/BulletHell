using Unity;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{

	private float _movingSpeed = 300f;
	private int _lifeCount = 3;
    private bool isDashing = false;
    private float _dashingSpeed = 200f;

    private async void Dash(float delta) 
    {
        isDashing = true;        
        var directionVector = GetDirection();
        directionVector.Normalize();

        for (int i = 0; i<30; i++) 
        {
          
        } 

        isDashing = false;
    }

    private async void UseActiveItem() 
    {

    }

    private async void Shoot() 
    {

    }
    
    private async void Reload() 
    {

    }

    private void Move(float delta) 
    {
        var moveVect = GetDirection();   
        moveVect.Normalize();
    }

    private Vector2 GetDirection()
    {
        return new Vector2(1, 1);
    }

    public void PhysicsProcess(float delta) 
    {

        if (!isDashing) 
        {
            Move(delta);

            if (Input.IsActionJustPressed("Shoot")) 
            {
                Shoot();
            }
            if (Input.IsActionJustPressed("Dash"))
            {
                Dash(delta);
            }
        }
    }
}
