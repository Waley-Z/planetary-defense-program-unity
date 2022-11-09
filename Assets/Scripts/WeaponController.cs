using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public int weaponNum;
    public WeaponAbstract[] weapons;
    private bool tutorialDone = false;
    private float lastFire = 0f;

    // Start is called before the first frame update
    void Start()
    {
        weaponNum = 0;
        weapons = new WeaponAbstract[3];
        weapons[0] = new MachineGun();
        weapons[1] = new LaserBeam();
        weapons[2] = new GravityTrap();
    }

    private void Update()
    {
        if (!Utils.IsMouseOverMoon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ToastManager.ToastErrorMsg("switched to mechine gun");
                weaponNum = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ToastManager.ToastErrorMsg("switched to laser beam");
                weaponNum = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ToastManager.ToastErrorMsg("switched to gravity trap");
                weaponNum = 2;
            }
        }
        if (GameManager.TutorialStage == 3 && tutorialDone)
            EventBus.Publish(new TutorialStageChangeEvent(4));
        if (weaponNum == 2)
            tutorialDone = true;
    }

    public void fire(GameObject go)
    {
        WeaponAbstract weapon = weapons[weaponNum];

        if (weaponNum == 2)
        {
            if (Time.time - lastFire > 0.8f)
                lastFire = Time.time;
            else
                return;
        }

        if (weapon.useRocks())
        {
            Instantiate(weapon.prefab, go.transform.position, go.transform.rotation);
            weapon.addImpluse(go);
        }
    }
}

public abstract class WeaponAbstract
{
    public GameObject prefab;
    protected int cost;
    protected float impluse;

    public bool useRocks()
    {
        return GameManager.Player.GetComponent<Inventory>().useRocks(cost);
    }

    public void addImpluse(GameObject go)
    {
        go.GetComponent<Rigidbody>().AddForce(-go.transform.up * impluse, ForceMode.VelocityChange);
    }
}

public class MachineGun: WeaponAbstract
{
    public MachineGun()
    {
        cost = 0;
        impluse = 0.15f;
        prefab = GameAssets.GetPrefab("MachineGunBullet");
    }
}

public class LaserBeam : WeaponAbstract
{
    public LaserBeam()
    {
        cost = 0;
        prefab = GameAssets.GetPrefab("LaserBeamBullet");
    }
}

public class GravityTrap : WeaponAbstract
{
    public GravityTrap()
    {
        cost = 3;
        prefab = GameAssets.GetPrefab("GravityTrapBullet");
    }
}