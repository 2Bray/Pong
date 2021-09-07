
using UnityEngine;

public class BallControl : MonoBehaviour
{
    // Rigidbody 2D bola
    private Rigidbody2D rigidBody2D;

    // Besarnya gaya awal yang diberikan untuk mendorong bola
    private float xInitialForce = 500;
    private float yInitialForce = 150;

    // Titik asal lintasan bola saat ini
    private Vector2 trajectoryOrigin;

    public Vector2 directionBall;

    [SerializeField] private GameManagerScript gameManager;

    private void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        trajectoryOrigin = transform.position;

        // Mulai game
        RestartGame();
    }

    // Ketika bola beranjak dari sebuah tumbukan, rekam titik tumbukan tersebut
    private void OnCollisionExit2D(Collision2D collision)
    {
        trajectoryOrigin = transform.position;
    }

    private void ResetBall()
    {
        // Reset posisi menjadi (0,0)
        transform.position = Vector2.zero;

        // Reset kecepatan menjadi (0,0)
        rigidBody2D.velocity = Vector2.zero;
    }

    private void PushBall()
    {
        // Tentukan nilai komponen y dari gaya dorong antara -yInitialForce dan yInitialForce
        float yRandomInitialForce = Random.Range(-yInitialForce, yInitialForce);

        // Tentukan nilai acak antara 0 (inklusif) dan 2 (eksklusif)
        float randomDirection = Random.Range(0, 2);

        // Jika nilainya di bawah 1, bola bergerak ke kiri. 
        // Jika tidak, bola bergerak ke kanan.
        if (randomDirection < 1.0f)
        {
            directionBall = new Vector2(-xInitialForce, yRandomInitialForce);
        }
        else
        {
            directionBall = new Vector2(xInitialForce, yRandomInitialForce);
        }
        rigidBody2D.AddForce(directionBall);
        gameManager.showRestart = false;
    }

    private void RestartGame()
    {
        gameManager.showRestart = true;

        // Kembalikan bola ke posisi semula
        ResetBall();

        // Setelah 2 detik, berikan gaya ke bola
        Invoke("PushBall", 2);
    }

    // Untuk mengakses informasi titik asal lintasan
    public Vector2 TrajectoryOrigin() => trajectoryOrigin;
    public Rigidbody2D getRigidBody() => rigidBody2D;
}
