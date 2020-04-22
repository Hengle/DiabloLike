using UnityEngine;
using System.Collections;
using FairyGUI;

public class aRPG_Input : MonoBehaviour
{
    GameObject m;
    aRPG_Master ms;

    Ray rayFire;
    RaycastHit hitFire;
    float h = 0.0f;
    float v = 0.0f;
    
    public bool wasdMovement = false;
    public bool joystickMovement = false;
    public bool mouseInput = false;
    public bool actionButtonsInput = false;

    int i;
    string pressedObject;
    bool pointerIsDown = false;
    GameObject pressedMobileButton;
    int pressedMobileButtonIndex;
    Vector3 targetDirection;
    
    internal string inputLockedBy = "";
    
    void Start()
    {
        m = gameObject;
        ms = m.GetComponent<aRPG_Master>();

    }

    void Update()
    {
        if (wasdMovement) { WASD_Input(); }

        if (joystickMovement) { Joystick_Input(); }

        if (actionButtonsInput) { InputActionButtons(); }

        // # this blocks input when pointer is over GUI element.
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) { return; }
        if (Stage.isTouchOnUI) { return; }
        if (mouseInput)
        {
            InputMouseRight();
            InputMouseLeft();
        }
    }

    // Skills input from keyboard(1,2,3, etc.)
    /// <summary>
    /// 技能按钮输入
    /// </summary>
    public void InputActionButtons()
    {
        // UI Action Buttons
        for (i = 0; i < SpecialUIManager.Instance.main.InputKeys.Length ; i++)
        {
            if (Input.GetKeyDown(SpecialUIManager.Instance.main.InputKeys[i]))
            {
                // block input from more then one key, with exeption of consumables
                //阻止来自多个密钥的输入，消耗品除外
                if (inputLockedBy == SpecialUIManager.Instance.main.InputKeys[i] || inputLockedBy == "" || SpecialUIManager.Instance.main.InputSkills[i].skillArchetype == archetype.BuffDebuff)
                {
                    if (SpecialUIManager.Instance.main.InputSkills[i].skillArchetype != archetype.BuffDebuff) { inputLockedBy = SpecialUIManager.Instance.main.InputKeys[i]; }
                    ms.psMovement.trackingEnemy = false;
                    ms.psMovement.pendingEnemyMeleeAtack = false;

                    ExecuteInput(SpecialUIManager.Instance.main.InputSkills[i], "down", false);
                }
            }

            if (Input.GetKey(SpecialUIManager.Instance.main.InputKeys[i]))
            {
                if (inputLockedBy == SpecialUIManager.Instance.main.InputKeys[i] || inputLockedBy == "" || SpecialUIManager.Instance.main.InputSkills[i].skillArchetype == archetype.BuffDebuff)
                {
                    ExecuteInput(SpecialUIManager.Instance.main.InputSkills[i], "held", false);
                }
            }

            if (Input.GetKeyUp(SpecialUIManager.Instance.main.InputKeys[i]))
            {
                if (inputLockedBy == SpecialUIManager.Instance.main.InputKeys[i] || inputLockedBy == "" || SpecialUIManager.Instance.main.InputSkills[i].skillArchetype == archetype.BuffDebuff)
                {
                    ExecuteInput(SpecialUIManager.Instance.main.InputSkills[i], "up", false);

                    if (SpecialUIManager.Instance.main.InputSkills[i].skillArchetype != archetype.BuffDebuff) { inputLockedBy = ""; }
                }
            }
        }
    }

    // Mouse input
    public void InputMouseRight()
    {
        // MouseButtons
        // (MouseRight)
        // # first two conditions in below If statement prevents execution from more then one source of input at the same time(meaning if you press two buttons execution from second one wont happen).
        // # the third condition is an exception from the rule based on the current skill of the button that you pressed as a second one. Thats usefull because you want to be able to use medkit while you are running away for example.
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (inputLockedBy == "mouse1" || inputLockedBy == "" || SpecialUIManager.Instance.main.rightMouseSkill.skillArchetype == archetype.BuffDebuff)
            {
                if (SpecialUIManager.Instance.main.rightMouseSkill.skillArchetype != archetype.BuffDebuff) { inputLockedBy = "mouse1"; }
                ms.psMovement.trackingEnemy = false;
                ms.psMovement.pendingEnemyMeleeAtack = false;

                ExecuteInput(SpecialUIManager.Instance.main.rightMouseSkill, "down", false);
            }
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (inputLockedBy == "mouse1" || inputLockedBy == "" || SpecialUIManager.Instance.main.rightMouseSkill.skillArchetype == archetype.BuffDebuff)
            {
                ExecuteInput(SpecialUIManager.Instance.main.rightMouseSkill, "held", false);
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            if (inputLockedBy == "mouse1" || inputLockedBy == "" || SpecialUIManager.Instance.main.rightMouseSkill.skillArchetype == archetype.BuffDebuff)
            {
                ExecuteInput(SpecialUIManager.Instance.main.rightMouseSkill, "up", false);

                if (SpecialUIManager.Instance.main.rightMouseSkill.skillArchetype != archetype.BuffDebuff) { inputLockedBy = ""; }
            }
        }
    }

    public void InputMouseLeft()
    {
        // (MouseLeft)
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (inputLockedBy == "mouse0" || inputLockedBy == "" || SpecialUIManager.Instance.main.leftMouseSkill.skillArchetype == archetype.BuffDebuff)
            {
                if (SpecialUIManager.Instance.main.leftMouseSkill.skillArchetype != archetype.BuffDebuff) { inputLockedBy = "mouse0"; }
                ms.psMovement.trackingEnemy = false;
                ms.psMovement.pendingEnemyMeleeAtack = false;

                pressedObject = CheckHitOnLeftMouseDown();
                if (pressedObject == "enemy")
                { ExecuteInput(SpecialUIManager.Instance.main.leftMouseSkill, "down", true); }
                if (pressedObject == "waypoint")
                { ms.psSkills.WaypointClick(hitFire.transform.gameObject); }
               
            }
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (inputLockedBy == "mouse0" || inputLockedBy == "" || SpecialUIManager.Instance.main.leftMouseSkill.skillArchetype == archetype.BuffDebuff)
            {
                if (pressedObject == "targetingPlane")
                { ms.psMovement.SetDestinationCustom(MoveDestination()); }
                //if (pressedObject == "itemDrops")
                //{
                //    ItemDropsManager.Instance.PickUpItem(ClickItem());
                //}
                if (pressedObject == "door")
                {
                    ms.psSkills.PlayerOpensDoor();
                }
                if (pressedObject == "enemy")
                { ExecuteInput(SpecialUIManager.Instance.main.leftMouseSkill, "held", true); }

            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (inputLockedBy == "mouse0" || inputLockedBy == "" || SpecialUIManager.Instance.main.leftMouseSkill.skillArchetype == archetype.BuffDebuff)
            {
                if (pressedObject == "enemy")
                { ExecuteInput(SpecialUIManager.Instance.main.leftMouseSkill, "up", true); }

                inputLockedBy = "";
            }
        }
    }

    // Skills from keyboard
    public void ExecuteInput(SkillData skill, string inputType, bool isMouseLeft)
    {
        // (Ray)
        if (skill.skillArchetype == archetype.DoT && inputType == "down")
        { ms.psSkills.CastDoT_ColliderDown(skill); }
        if (skill.skillArchetype == archetype.DoT && inputType == "held")
        { ms.psSkills.CastDoT_ColliderHeld(skill); }
        if (skill.skillArchetype == archetype.DoT && inputType == "up")
        { ms.psSkills.CastDoT_ColliderUp(skill); }

        // (Fireball)
        if (skill.skillArchetype == archetype.Projectile && inputType == "down")
        { ms.psSkills.CastFireballDown(skill); }
        if (skill.skillArchetype == archetype.Projectile && inputType == "held")
        { ms.psSkills.CastFireballHeld(skill); }
        if (skill.skillArchetype == archetype.Projectile && inputType == "up")
        { ms.psSkills.CastFireballUp(); }

        // (AoE)
        if (skill.skillArchetype == archetype.AoE && inputType == "down")
        { ms.psSkills.CastAoEDown(skill); }
        if (skill.skillArchetype == archetype.AoE && inputType == "held")
        { ms.psSkills.CastAoEHeld(); }
        if (skill.skillArchetype == archetype.AoE && inputType == "up")
        { ms.psSkills.CastAoEUp(); }

        // (Medkit)急救包
        if (skill.skillArchetype == archetype.BuffDebuff && inputType == "down")
        { ms.psSkills.Consumable_Down(skill); }

        // (MoveSkill)
        if (skill.skillArchetype == archetype.Move && inputType == "down")
        { ms.psSkills.MoveSkillDown(); }
        if (skill.skillArchetype == archetype.Move && inputType == "held")
        { ms.psSkills.MoveSkillHeld(); }
        if (skill.skillArchetype == archetype.Move && inputType == "up")
        { ms.psSkills.MoveSkillUp(); }

        //MeleeSweep
        if (skill.skillArchetype == archetype.MeleeSweep && inputType == "down")
        { ms.psSkills.MeleeSweep_Down(skill); }
        if (skill.skillArchetype == archetype.MeleeSweep && inputType == "held")
        { ms.psSkills.MeleeSweep_Held(skill); }
        if (skill.skillArchetype == archetype.MeleeSweep && inputType == "up")
        { ms.psSkills.MeleeSweep_Up(); }

        if (skill.skillArchetype == archetype.BasicAttack && isMouseLeft == true && ms.psInventory.equippedWeaponCategory == weaponCategories.Melee1H)
        {
            if (inputType == "down")
            { ms.psSkills.PreciseWithLockOnMeleeDown(); }
            if (inputType == "held")
            { ms.psSkills.PreciseWithLockOnMeleeHeld(); }
            if (inputType == "up")
            { ms.psSkills.PreciseWithLockOnMeleeUp(); }
        }

        // (BaseAttack)
        if (skill.skillArchetype == archetype.BasicAttack && inputType == "down")
        {
            if (ms.psInventory.equippedWeaponCategory == weaponCategories.Melee1H && isMouseLeft == false)
            { ms.psSkills.FreeTargetMeleeDown(); }
            if (ms.psInventory.equippedWeaponCategory == weaponCategories.Gun1H)
            { ms.psSkills.ShootGunDown(); }
            if (ms.psInventory.equippedWeaponCategory == weaponCategories.Shotgun)
            { ms.psSkills.ShootShotgunDown(); }
            if (ms.psInventory.equippedWeaponCategory == weaponCategories.AssaultRifle)
            { ms.psSkills.ShootRifleDown(); }
        }
        if (skill.skillArchetype == archetype.BasicAttack && inputType == "held")
        {
            if (ms.psInventory.equippedWeaponCategory == weaponCategories.Melee1H && isMouseLeft == false)
            { ms.psSkills.FreeTargetMeleeHeld(); }
            if (ms.psInventory.equippedWeaponCategory == weaponCategories.Gun1H)
            { ms.psSkills.ShootGunHeld(true); }
            if (ms.psInventory.equippedWeaponCategory == weaponCategories.Shotgun)
            { ms.psSkills.ShootShotgunHeld(); }
            if (ms.psInventory.equippedWeaponCategory == weaponCategories.AssaultRifle)
            { ms.psSkills.ShootRifleHeld(); }
        }
        if (skill.skillArchetype == archetype.BasicAttack && inputType == "up")
        {
            if (ms.psInventory.equippedWeaponCategory == weaponCategories.Melee1H && isMouseLeft == false)
            { ms.psSkills.FreeTargetMeleeUp(); }
            if (ms.psInventory.equippedWeaponCategory == weaponCategories.Gun1H)
            { ms.psSkills.ShootGunUp(); }
            if (ms.psInventory.equippedWeaponCategory == weaponCategories.Shotgun)
            { ms.psSkills.ShootShotgunUp(); }
            if (ms.psInventory.equippedWeaponCategory == weaponCategories.AssaultRifle)
            { ms.psSkills.ShootRifleUp(); }
        }
    }
    // Skills from mobile pointer
    public void ExecuteMobileInput(SkillData skill, string inputType)
    {
        Debug.Log(ms.psInventory.equippedWeaponCategory);
        // (Ray)
        if (skill.skillArchetype == archetype.DoT && inputType == "down")
        { ms.psSkills.Mobile_DoT_Collider_Down(skill); }
        if (skill.skillArchetype == archetype.DoT && inputType == "held")
        { ms.psSkills.Mobile_DoT_Collider_Held(skill); }
        if (skill.skillArchetype == archetype.DoT && inputType == "up")
        { ms.psSkills.Mobile_DoT_Collider_Up(skill); }

        // (Fireball)
        if (skill.skillArchetype == archetype.Projectile && inputType == "down")
        { ms.psSkills.Mobile_CastFireball_Down(skill); }
        if (skill.skillArchetype == archetype.Projectile && inputType == "held")
        { ms.psSkills.Mobile_CastFireball_Held(skill); }
        if (skill.skillArchetype == archetype.Projectile && inputType == "up")
        { ms.psSkills.Mobile_CastFireball_Up(); }

        // (AoE)
        if (skill.skillArchetype == archetype.AoE && inputType == "down")
        { Debug.Log("NOTE: this skill archetype does not work directly in mobile/wasd input mode, use it only as a linked skill to other archetype"); }

        // (Medkit)
        if (skill.skillArchetype == archetype.BuffDebuff && inputType == "down")
        { ms.psSkills.Consumable_Down(skill); }

        // (MoveSkill)
        if (skill.skillArchetype == archetype.Move)
        {
            Debug.Log("NOTE: this skill archetype does not work in mobile/wasd input mode");
        }

            // (BaseAttack)
         if (skill.skillArchetype == archetype.BasicAttack && inputType == "down")
        {
            if (ms.psInventory.equippedWeaponCategory == weaponCategories.Melee1H)
            { ms.psSkills.Mobile_Melee_Down(); }
            if (ms.psInventory.equippedWeaponCategory == weaponCategories.Gun1H)
            { ms.psSkills.Mobile_ShootGun_Down(); }
            if (ms.psInventory.equippedWeaponCategory == weaponCategories.Shotgun)
            { ms.psSkills.Mobile_ShootShotgun_Down(); }
            if (ms.psInventory.equippedWeaponCategory == weaponCategories.AssaultRifle)
            { ms.psSkills.Mobile_ShootRifle_Down(); }
        }
        if (skill.skillArchetype == archetype.BasicAttack && inputType == "held")
        {
            if (ms.psInventory.equippedWeaponCategory == weaponCategories.Melee1H)
            { ms.psSkills.Mobile_Melee_Held(); }
            if (ms.psInventory.equippedWeaponCategory == weaponCategories.Gun1H)
            { ms.psSkills.Mobile_ShootGun_Held(); }
            if (ms.psInventory.equippedWeaponCategory == weaponCategories.Shotgun)
            { ms.psSkills.Mobile_ShootShotgun_Held(); }
            if (ms.psInventory.equippedWeaponCategory == weaponCategories.AssaultRifle)
            { ms.psSkills.Mobile_ShootRifle_Held(); }
        }
        if (skill.skillArchetype == archetype.BasicAttack && inputType == "up")
        {
            if (ms.psInventory.equippedWeaponCategory == weaponCategories.Melee1H)
            { ms.psSkills.Mobile_Melee_Up(); }
            if (ms.psInventory.equippedWeaponCategory == weaponCategories.Gun1H)
            { ms.psSkills.Mobile_ShootGun_Up(); }
            if (ms.psInventory.equippedWeaponCategory == weaponCategories.Shotgun)
            { ms.psSkills.Mobile_ShootShotgun_Up(); }
            if (ms.psInventory.equippedWeaponCategory == weaponCategories.AssaultRifle)
            { ms.psSkills.Mobile_ShootRifle_Up(); }
        }
    }
    // "Free target" type of input from left mouse button
    string CheckHitOnLeftMouseDown()
    {
        rayFire = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayFire, out hitFire, 60.0f, ms.maskMoveEnemiesDoors))
        {
            if (hitFire.transform.tag == "enemy")
            {
                return "enemy";
            }
            if (hitFire.transform.tag == "targetingPlane")
            {
                return "targetingPlane";
            }
            if (hitFire.transform.tag == "door")
            {
                return "door";
            }
            if (hitFire.transform.name == "Waypoint")
            {
                return "waypoint";
            }
            //if (hitFire.transform.name == "itemDrops")
            //{
            //    return "itemDrops";
            //}
            return "";
        }
        else { return ""; }
    }
    // Processing mouse movement
    Vector3 MoveDestination()
    {
        rayFire = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayFire, out hitFire, 60.0f, ms.layerTargetingPlaneToMove))
        {
            return hitFire.point;
        }
        else { return ms.player.transform.position; }
    }
    /// <summary>
    /// 获取点击的物品
    /// </summary>
    /// <returns></returns>
    Transform ClickItem()
    {
        rayFire = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayFire, out hitFire, 60.0f, ms.layerTargetingPlaneToMove))
        {
            return hitFire.transform;
        }
        return null;
    }
    /*Mobile pointer*/
    public void PointerDown(GameObject pressedButtonToPass)
    {
        if(actionButtonsInput == true)
        { return; }
        //移动端输入要再处理，现在不做===================================================
        //for (i = 0; i < ms.gsManagement.Buttons_numberOf; i++)
        //{
        //    if (pressedButtonToPass == ms.gsManagement.Buttons[i])
        //    {
        //        pressedMobileButtonIndex = i;
        //    }
        //}
        inputLockedBy = SpecialUIManager.Instance.main.InputSkills[pressedMobileButtonIndex].skillArchetype.ToString();
        ExecuteMobileInput(SpecialUIManager.Instance.main.InputSkills[pressedMobileButtonIndex], "down");
        pointerIsDown = true;
    }

    public void PointerHeld()
    {
        if(actionButtonsInput == true)
        { return; }

        if (pointerIsDown == false) { return; }
        ExecuteMobileInput(SpecialUIManager.Instance.main.InputSkills[pressedMobileButtonIndex], "held");
    }

    public void PointerUp()
    {
        if (actionButtonsInput == true)
        { return; }
        inputLockedBy = "";
        ExecuteMobileInput(SpecialUIManager.Instance.main.InputSkills[pressedMobileButtonIndex], "up");
        pointerIsDown = false;

    }
    /*Mobile pointer*/

    /*Keyboard movement*/
    void WASD_Input()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        WASD_Joy_Movement();
    }
    /*Keyboard movement*/

    /*Mobile joystick movement*/
    public void Joystick_Input()
    {
        h = ms.gsJoystick.joyPosX;
        v = ms.gsJoystick.joyPosY;

        WASD_Joy_Movement();
    }
    /*Mobile joystick movement*/

    // Processing mobile/wasd movement
    public void WASD_Joy_Movement()
    {
        if (h != 0f || v != 0f)
        {
            RotatePlayer(h, v);

            if (inputLockedBy == "" || inputLockedBy == "Consumable")
            {
                //if (ms.mcsManagement.movementLocked == false)
                //{
                //    ms.pAnimator.SetFloat("Move", 0.9f);
                //}
            }
            else
            {
                ms.pAnimator.SetFloat("Move", 0.0f);
            }
        }
        else
        {
            ms.pAnimator.SetFloat("Move", 0.0f);
        }
    }

    void RotatePlayer(float horizontal, float vertical)
    {
        Vector3 horizontalRelative = horizontal * ms.cam.transform.right;
        Vector3 verticalRelative = vertical * ms.cam.transform.forward;

        Vector3 relativeVector = horizontalRelative + verticalRelative;

        float finalRelativeX = relativeVector.x;
        float finalRelativeZ = relativeVector.z;
        if (ms.cam.transform.forward == Vector3.down && vertical != 0f) { finalRelativeZ = vertical; }

        targetDirection = new Vector3(ms.player.transform.position.x + finalRelativeX, ms.player.transform.position.y, ms.player.transform.position.z + finalRelativeZ);

        ms.player.transform.LookAt(targetDirection);
    }
    // Processing mobile/wasd movement

}
