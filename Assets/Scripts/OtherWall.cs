using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherWall : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Ball")
        {
            BallControl ball = collision.gameObject.GetComponent<BallControl>();
            ball.directionBall = new Vector2(ball.directionBall.x, -ball.directionBall.y);
            ball.getRigidBody().velocity = Vector2.zero;
            ball.getRigidBody().AddForce(ball.directionBall * 50f * Time.deltaTime);
        }
    }
}