using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class static_data : MonoBehaviour
{
    public int temp;
    public float hunger,health,money,cotton,coal,ammo;
    public float meth, medicine;
    public GameObject sun,death_screen,pause_screen;
    public Slider health_bar, hunger_bar;
    public bool paused = false;

    public int kfc = 0, melon = 2;
    public float[] location;

    public TextMeshProUGUI status;

    player_movement player_script;
    mission_script mission_script;
    int in_cave;

    void Start()
    {
        sun.transform.rotation = Quaternion.Euler(90,0,0);
        player_script = GameObject.FindGameObjectWithTag("Player").GetComponent<player_movement>();
        mission_script = GameObject.FindGameObjectWithTag("mission data").GetComponent<mission_script>();
        location = new float[3];
        temp = open_screen_ui.flag;

        if (temp == 0)
        {
            health = 100;
            hunger = 100;
            money = 1000;
            cotton = 0;
            coal = 0;
            location[0] = -389;
            location[1] = 3;
            location[2] = 133;
            in_cave = 0;
        }
        else
        {
            health = PlayerPrefs.GetFloat("player_health");
            hunger = PlayerPrefs.GetFloat("player_hunger");
            money = PlayerPrefs.GetFloat("player_money");
            cotton = PlayerPrefs.GetFloat("player_cotton");
            coal = PlayerPrefs.GetFloat("player_coal");
            ammo = PlayerPrefs.GetFloat("player_ammo");
            location[0] = PlayerPrefs.GetFloat("player_x");
            location[1] = PlayerPrefs.GetFloat("player_y");
            location[2] = PlayerPrefs.GetFloat("player_z");
            in_cave = PlayerPrefs.GetInt("player_in_cave");
            if (in_cave == 0)
            {
                sun.SetActive(true);
                player_script.in_cave = false;
            }
            else
            {
                sun.SetActive(false);
                player_script.in_cave = true;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (paused == false)
        {
            sun.transform.Rotate(0.2f * Time.deltaTime, 0, 0);
            location[0] = player_script.player.transform.position.x;
            location[1] = player_script.player.transform.position.y;
            location[2] = player_script.player.transform.position.z;

            health_bar.value = health;
            hunger_bar.value = hunger;

            if (player_script.in_cave == true)
                in_cave = 1;
            else
                in_cave = 0;

            if (in_cave == 0)
                sun.SetActive(true);
            else
                sun.SetActive(false);

            if (hunger >= -1)
                hunger -= Time.deltaTime * 0.2f;

            if (health <= 0)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                death_screen.SetActive(true);
            }

            if (hunger <= 0.7)
            {
                health -= Time.deltaTime * 0.7f;
            }

            //for pausing the game
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                paused = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                mission_script.canvas.SetActive(false);
                pause_screen.SetActive(true);

                status.text = "Nicros: " + money.ToString() + "\n\nAmmo: " + ammo.ToString() + "\n\nMelon: " + melon.ToString() + "\n\nKFC: " + kfc.ToString() + "\n\nCotton: " + cotton.ToString() + "\n\nCoal: " +coal.ToString() + "\n\nMedicine: " + medicine.ToString() + "\n\nMeth: " + meth.ToString();
            }
        }
    }

    public void retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        health = PlayerPrefs.GetFloat("player_health");
        hunger = PlayerPrefs.GetFloat("player_hunger");
        money = PlayerPrefs.GetFloat("player_money");
        cotton = PlayerPrefs.GetFloat("player_cotton");
        coal = PlayerPrefs.GetFloat("player_coal");
        ammo = PlayerPrefs.GetFloat("player_ammo");
        location[0] = PlayerPrefs.GetFloat("player_x");
        location[1] = PlayerPrefs.GetFloat("player_y");
        location[2] = PlayerPrefs.GetFloat("player_z");
        in_cave = PlayerPrefs.GetInt("player_in_cave");

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void back() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void buy(int n)
    {
        if (n == 1 && money >= 10)
        {
            melon += n;
            money -= 10;
        }

        if (n == 10 && money >= 95)
        {
            melon += n;
            money -= 95;
        }
    }

    public void buykfc()
    {
        if (money >= 50)
        {
            kfc += 1;
            money -= 50;
        }
    }

    public void sell_cotton(int n)
    {
        if(cotton >=n)
        {
            cotton -= n;
            money += n * 5;
        }
    }

    public void sell_coal()
    {
        if(coal>=5)
        {
            coal -= 5;
            money += 25;
        }
    }

    public void savegame()
    {
        PlayerPrefs.SetFloat("player_health",health);
        PlayerPrefs.SetFloat("player_hunger",hunger);
        PlayerPrefs.SetFloat("player_money",money);
        PlayerPrefs.SetFloat("player_cotton",cotton);
        PlayerPrefs.SetFloat("player_coal", coal);
        PlayerPrefs.SetFloat("player_ammo",ammo);
        PlayerPrefs.SetFloat("player_x", location[0]);
        PlayerPrefs.SetFloat("player_y", location[1]);
        PlayerPrefs.SetFloat("player_z", location[2]);
        PlayerPrefs.SetInt("player_in_cave", in_cave);
    }

    public void resume()
    {
        paused = false;
        pause_screen.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
