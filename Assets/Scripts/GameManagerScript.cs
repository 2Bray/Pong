
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    // Pemain 1
    [SerializeField] private PlayerControl player1;
    // skrip
    private Rigidbody2D player1Rigidbody;

    // Pemain 2
    [SerializeField] private PlayerControl player2;
    // skrip
    private Rigidbody2D player2Rigidbody;

    // Bola
    [SerializeField] private BallControl ball;
    // skrip
    private Rigidbody2D ballRigidbody;
    private CircleCollider2D ballCollider;

    // Skor maksimal
    private int maxScore = 5;

    // Apakah debug window ditampilkan?
    private bool isDebugWindowShown = false;

    // Objek untuk menggambar prediksi lintasan bola
    public TrajectoryScript trajectory;

    private bool pauseGame = false;

    [HideInInspector] public bool showRestart = false;

    // Inisialisasi rigidbody dan collider
    private void Start()
    {
        player1Rigidbody = player1.GetComponent<Rigidbody2D>();
        player2Rigidbody = player2.GetComponent<Rigidbody2D>();
        ballRigidbody = ball.GetComponent<Rigidbody2D>();
        ballCollider = ball.GetComponent<CircleCollider2D>();
        trajectory.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseGame = true;
            Time.timeScale = 0f;
        }
    }

    // Untuk menampilkan GUI
    void OnGUI()
    {
        // Tampilkan skor pemain 1 di kiri atas dan pemain 2 di kanan atas
        GUI.Label(new Rect(Screen.width / 2 - 150 - 12, 20, 100, 100), "" + player1.Score());
        GUI.Label(new Rect(Screen.width / 2 + 150 + 12, 20, 100, 100), "" + player2.Score());

        // Tombol restart untuk memulai game dari awal
        if (showRestart) { 
            if (GUI.Button(new Rect(Screen.width / 2 - 60, 35, 120, 53), "RESTART"))
            {
                // Ketika tombol restart ditekan, Reset Scene
                SceneManager.LoadScene("Pong");
            }
        }

        // Jika pemain 1 menang (mencapai skor maksimal), ...
        if (player1.Score() == maxScore)
        {
            // ...tampilkan teks "PLAYER ONE WINS" di bagian kiri layar...
            GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 10, 2000, 1000), "PLAYER ONE WINS");

            // ...dan kembalikan bola ke tengah.
            ball.SendMessage("ResetBall", null, SendMessageOptions.RequireReceiver);
        }
        // Sebaliknya, jika pemain 2 menang (mencapai skor maksimal), ...
        else if (player2.Score() == maxScore)
        {
            // ...tampilkan teks "PLAYER TWO WINS" di bagian kanan layar... 
            GUI.Label(new Rect(Screen.width / 2 + 30, Screen.height / 2 - 10, 2000, 1000), "PLAYER TWO WINS");

            // ...dan kembalikan bola ke tengah.
            ball.SendMessage("ResetBall", null, SendMessageOptions.RequireReceiver);
        }

        // Jika isDebugWindowShown == true, tampilkan text area untuk debug window.
        if (isDebugWindowShown)
        {
            // Simpan nilai warna lama GUI
            Color oldColor = GUI.backgroundColor;
            // Beri warna baru
            GUI.backgroundColor = Color.red;

            // Simpan variabel-variabel fisika yang akan ditampilkan. 
            float ballMass = ballRigidbody.mass;
            Vector2 ballVelocity = ballRigidbody.velocity;
            float ballSpeed = ballRigidbody.velocity.magnitude;
            Vector2 ballMomentum = ballMass * ballVelocity;
            float ballFriction = ballCollider.friction;

            float impulsePlayer1X = player1.LastContactPoint().normalImpulse;
            float impulsePlayer1Y = player1.LastContactPoint().tangentImpulse;
            float impulsePlayer2X = player2.LastContactPoint().normalImpulse;
            float impulsePlayer2Y = player2.LastContactPoint().tangentImpulse;

            // Tentukan debug text-nya
            string debugText =
                "Ball mass = " + ballMass + "\n" +
                "Ball velocity = " + ballVelocity + "\n" +
                "Ball speed = " + ballSpeed + "\n" +
                "Ball momentum = " + ballMomentum + "\n" +
                "Ball friction = " + ballFriction + "\n" +
                "Last impulse from player 1 = (" + impulsePlayer1X + ", " + impulsePlayer1Y + ")\n" +
                "Last impulse from player 2 = (" + impulsePlayer2X + ", " + impulsePlayer2Y + ")\n";

            // Tampilkan debug window
            GUIStyle guiStyle = new GUIStyle(GUI.skin.textArea);
            guiStyle.alignment = TextAnchor.UpperCenter;
            GUI.TextArea(new Rect(Screen.width / 2 - 200, Screen.height - 200, 400, 110), debugText, guiStyle);

            // Kembalikan warna lama GUI
            GUI.backgroundColor = oldColor;
        }

        // Toggle nilai debug window ketika pemain mengeklik tombol ini.
        if (GUI.Button(new Rect(Screen.width / 2 - 60, Screen.height - 73, 120, 53), "TOGGLE\nDEBUG INFO"))
        {
            isDebugWindowShown = !isDebugWindowShown;
            trajectory.enabled = !trajectory.enabled;
        }

        if (pauseGame)
        {
            GUI.Label(new Rect(0,0,Screen.width,Screen.height),"Pause");
            if(GUI.Button(new Rect(Screen.width/2-160, Screen.height / 2, 100, 40), "Quit"))
            {
                Application.Quit();
            }
            if(GUI.Button(new Rect(Screen.width/2+40, Screen.height / 2, 100, 40), "Resume"))
            {
                Time.timeScale = 1f;
                pauseGame = false;
            }
        }
    }

    public int getMaxScore() => maxScore;
}
