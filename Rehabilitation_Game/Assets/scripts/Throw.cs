using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Throw : MonoBehaviour
{
    [SerializeField] float Initial_known_equivalent_Angle = -92f;//45degree
    [SerializeField] float degree_equivalent = 45f;
    [SerializeField] float Velocity = 20f;
    [SerializeField] float Angle =Mathf.PI/4;
    [SerializeField] float MaxAngle = 12f;
    [SerializeField] float MinAngle = 0f;
    [SerializeField] float midpoint = 6f;
    [SerializeField] float max_x_camera = 21.333f;
    [SerializeField] float max_y_camera = 12;
    [SerializeField] float initial_Y_Arch = 5.206f;
    int HeightsInUnits = 12;
    int WidthInUnits = 16;
    float initial_position_x_circle;
    Vector2 starting_position;
    public bool stratingpoint = true;
    bool colision = false;
    Vector2 PositionColision;
    // Start is called before the first frame update
    void Start()
    {
        starting_position = new Vector2(transform.position.x, transform.position.y);

    }

    // Update is called once per frame
    void Update()
    {
        if (stratingpoint)
        {
            LanchOnclick();
            stick_to_archer();
            set_velocity_angle();
            DrawThePath();
            set_arch_inGame_angle();
        }
        else
        {
            set_arch_inGame_angle_afterThrow();
        }

        if (colision)
        {
            transform.position = PositionColision;
        }
    }

    public void set_arch_inGame_angle()
    {
        Vector3 temp = transform.rotation.eulerAngles;
        temp.z = (Angle * 180 / Mathf.PI) - degree_equivalent + Initial_known_equivalent_Angle;
        transform.rotation= Quaternion.Euler(temp);

        Bow Bow1 = FindObjectOfType<Bow>();
        Vector3 temp2 = Bow1.transform.rotation.eulerAngles;
        temp2.z = (Angle * 180 / Mathf.PI) - degree_equivalent + Initial_known_equivalent_Angle;
        Bow1.transform.rotation = Quaternion.Euler(temp2);

        Hand Hand1 = FindObjectOfType<Hand>();
        Vector3 temp3 = Hand1.transform.rotation.eulerAngles;
        temp3.z = (Angle * 180 / Mathf.PI) - degree_equivalent + Initial_known_equivalent_Angle;
        Hand1.transform.rotation = Quaternion.Euler(temp3);

    }
    void set_arch_inGame_angle_afterThrow()
    {
        float XPosition = transform.position.x;
        float Angle_arch = Mathf.Atan((Velocity * Mathf.Sin(Angle) - XPosition / (Velocity * Mathf.Cos(Angle))) / (Velocity * Mathf.Cos(Angle)));
        Vector3 temp = transform.rotation.eulerAngles;
        temp.z = (Angle_arch * 180 / Mathf.PI) - degree_equivalent + Initial_known_equivalent_Angle;
        transform.rotation = Quaternion.Euler(temp);
    }

    public void DrawThePath()
    {
        FindObjectOfType<CircleManager>().clear_circles();
        float pathStep = 1f;
        float Y_positions_circle;
        float X_positions_circle;
        float Delta_X_position;
        float g = 10f;
        float startingRaduis = 2f;
        float distance = 1f;
        float X_changable_circule = transform.position.x + 1;
        while(X_changable_circule<=max_x_camera/2 -1)
        {
            X_positions_circle = X_changable_circule;
            Delta_X_position = X_changable_circule - transform.position.x;
            Y_positions_circle = -g / (2 * (Mathf.Pow(Mathf.Cos(Angle) * Velocity, 2))) * Delta_X_position + Delta_X_position * Mathf.Tan(Angle) + initial_Y_Arch;
            if(Y_positions_circle > max_y_camera-1 )
            {
                break;
            }
            if (Y_positions_circle < 1)
            {
                break;
            }
            FindObjectOfType<CircleManager>().draw_circle(X_positions_circle, Y_positions_circle, startingRaduis);
            startingRaduis -= 0.3f;
            X_changable_circule += distance;
            distance -= 0.3f;

        }
    }
    public void stick_to_archer()
    {
        transform.position = starting_position;
    }
    public void LanchOnclick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(Velocity*Mathf.Cos(Angle), Velocity * Mathf.Sin(Angle));
            stratingpoint = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);

        PositionColision = transform.position;
        colision = true;
        GetComponent<AudioSource>().Play();
    }
    public void set_velocity_angle()
    {
        float mouseposition = Input.mousePosition.y / Screen.height * HeightsInUnits;
        mouseposition= Mathf.Clamp(mouseposition, MinAngle, MaxAngle);
        Angle = (mouseposition - midpoint) / midpoint * Mathf.PI / 4;
    }

}
