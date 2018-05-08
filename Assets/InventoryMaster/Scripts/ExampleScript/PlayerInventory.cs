using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerInventory : MonoBehaviour
{
    public GameObject inventory;
    public GameObject characterSystem;
    public GameObject craftSystem;
    private Inventory craftSystemInventory;
    private CraftSystem cS;
    private Inventory mainInventory;
    private Inventory characterSystemInventory;
    private Tooltip toolTip;

    private InputManager inputManagerDatabase;

    Image hpImage;
    Image manaImage;

	Text textHpMana;
	Text textAttackDef;


    public float maxHealth = 100;
    public float maxMana = 100;
    float maxDamage = 0;
    float maxArmor = 0;

    public float currentHealth = 60;
    public float currentMana = 90;
	public float currentDamage = 0;
	public float currentArmor = 0;

    int normalSize = 3;

    public void OnEnable()
    {
        Inventory.ItemEquip += OnBackpack;
        Inventory.UnEquipItem += UnEquipBackpack;

        Inventory.ItemEquip += OnGearItem;
        Inventory.ItemConsumed += OnConsumeItem;
        Inventory.UnEquipItem += OnUnEquipItem;

        Inventory.ItemEquip += EquipWeapon;
        Inventory.UnEquipItem += UnEquipWeapon;
    }

    public void OnDisable()
    {
        Inventory.ItemEquip -= OnBackpack;
        Inventory.UnEquipItem -= UnEquipBackpack;

        Inventory.ItemEquip -= OnGearItem;
        Inventory.ItemConsumed -= OnConsumeItem;
        Inventory.UnEquipItem -= OnUnEquipItem;

        Inventory.UnEquipItem -= UnEquipWeapon;
        Inventory.ItemEquip -= EquipWeapon;
    }

    void EquipWeapon(Item item)
    {
        if (item.itemType == ItemType.Weapon)
        {
            //add the weapon if you unequip the weapon
        }
    }

    void UnEquipWeapon(Item item)
    {
        if (item.itemType == ItemType.Weapon)
        {
            //delete the weapon if you unequip the weapon
        }
    }

    void OnBackpack(Item item)
    {
		//print ("EQUIP0");
        if (item.itemType == ItemType.Backpack)
        {
			//print ("EQUIP1");
			changeInventorySize(16);

//            for (int i = 0; i < item.itemAttributes.Count; i++)
//            {
//                if (mainInventory == null)
//                    mainInventory = inventory.GetComponent<Inventory>();
//                mainInventory.sortItems();
////				if (item.itemAttributes [i].attributeName == "Slots") {
////					changeInventorySize(item.itemAttributes[i].attributeValue);
////				}
//            }
        }
    }

    void UnEquipBackpack(Item item)
    {
        if (item.itemType == ItemType.Backpack)
            changeInventorySize(normalSize);
    }

    void changeInventorySize(int size)
    {
        dropTheRestItems(size);
		//print ("IM HERE BITCH");

        if (mainInventory == null)
            mainInventory = inventory.GetComponent<Inventory>();
        if (size == 3)
        {
            mainInventory.width = 3;
            mainInventory.height = 1;
            mainInventory.updateSlotAmount();
            mainInventory.adjustInventorySize();
        }
        if (size == 6)
        {
            mainInventory.width = 3;
            mainInventory.height = 2;
            mainInventory.updateSlotAmount();
            mainInventory.adjustInventorySize();
        }
        else if (size == 12)
        {
            mainInventory.width = 4;
            mainInventory.height = 3;
            mainInventory.updateSlotAmount();
            mainInventory.adjustInventorySize();
        }
        else if (size == 16)
        {
            mainInventory.width = 4;
            mainInventory.height = 4;
            mainInventory.updateSlotAmount();
            mainInventory.adjustInventorySize();
        }
        else if (size == 24)
        {
            mainInventory.width = 6;
            mainInventory.height = 4;
            mainInventory.updateSlotAmount();
            mainInventory.adjustInventorySize();
        }
    }

    void dropTheRestItems(int size)
    {
        if (size < mainInventory.ItemsInInventory.Count)
        {
            for (int i = size; i < mainInventory.ItemsInInventory.Count; i++)
            {
                GameObject dropItem = (GameObject)Instantiate(mainInventory.ItemsInInventory[i].itemModel);
                dropItem.AddComponent<PickUpItem>();
                dropItem.GetComponent<PickUpItem>().item = mainInventory.ItemsInInventory[i];
                dropItem.transform.localPosition = GameObject.FindGameObjectWithTag("Player").transform.localPosition;
            }
        }
    }
	Text textHelp;

	respawn playerScriptRespawn;

//	print (playerScriptRespawn.isDead);
//
    void Start()
    {
		hpImage = GameObject.Find ("currentHP").GetComponent<Image>();
		manaImage = GameObject.Find ("currentMana").GetComponent<Image>();
		textHelp = GameObject.Find ("TextHelp").GetComponent<Text> ();

		textHpMana = GameObject.Find ("info_PV_MANA").GetComponent<Text>();
		textAttackDef = GameObject.Find ("info_ATTACK_DEF").GetComponent<Text>();
//		textHpMana.text = "PENIS";
		refreshStats();

		playerScriptRespawn = GetComponent<respawn>();
		//print (playerScriptRespawn.isDead);
		//playerScriptRespawn.isDead = true;

        if (inputManagerDatabase == null)
            inputManagerDatabase = (InputManager)Resources.Load("InputManager");

        if (craftSystem != null)
            cS = craftSystem.GetComponent<CraftSystem>();

        if (GameObject.FindGameObjectWithTag("Tooltip") != null)
            toolTip = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<Tooltip>();
        if (inventory != null)
            mainInventory = inventory.GetComponent<Inventory>();
        if (characterSystem != null)
            characterSystemInventory = characterSystem.GetComponent<Inventory>();
        if (craftSystem != null)
            craftSystemInventory = craftSystem.GetComponent<Inventory>();

		changeInventorySize (normalSize);
    }

	public void ApplyDamage(float damage) {
		float tmpCurrentArmor = currentArmor;

		if (tmpCurrentArmor > 100) {
			tmpCurrentArmor = 100;
		}
		currentHealth = currentHealth - (damage - ((damage * tmpCurrentArmor) / 100));
		if (currentHealth <= 0) {
			playerScriptRespawn.isDead = true;
		}
	}
	 

    public void OnConsumeItem(Item item)
    {
        for (int i = 0; i < item.itemAttributes.Count; i++)
        {
            if (item.itemAttributes[i].attributeName == "Health")
            {
                if ((currentHealth + item.itemAttributes[i].attributeValue) > maxHealth)
                    currentHealth = maxHealth;
                else
                    currentHealth += item.itemAttributes[i].attributeValue;
            }
            if (item.itemAttributes[i].attributeName == "Mana")
            {
                if ((currentMana + item.itemAttributes[i].attributeValue) > maxMana)
                    currentMana = maxMana;
                else
                    currentMana += item.itemAttributes[i].attributeValue;
            }
            if (item.itemAttributes[i].attributeName == "Armor")
            {
                if ((currentArmor + item.itemAttributes[i].attributeValue) > maxArmor)
                    currentArmor = maxArmor;
                else
                    currentArmor += item.itemAttributes[i].attributeValue;
            }
            if (item.itemAttributes[i].attributeName == "Damage")
            {
                if ((currentDamage + item.itemAttributes[i].attributeValue) > maxDamage)
                    currentDamage = maxDamage;
                else
                    currentDamage += item.itemAttributes[i].attributeValue;
            }
        }
       
    }

    public void OnGearItem(Item item)
    {
        for (int i = 0; i < item.itemAttributes.Count; i++)
        {
			if (item.itemAttributes [i].attributeName == "Health") {
				currentHealth += item.itemAttributes [i].attributeValue;
				maxHealth += item.itemAttributes [i].attributeValue;
			}
			if (item.itemAttributes [i].attributeName == "Mana") {
				currentMana += item.itemAttributes [i].attributeValue;
				maxMana += item.itemAttributes [i].attributeValue;
			}
            if (item.itemAttributes[i].attributeName == "Armor")
				currentArmor += item.itemAttributes[i].attributeValue;
            if (item.itemAttributes[i].attributeName == "Damage")
				currentDamage += item.itemAttributes[i].attributeValue;
        }
		refreshStats ();
    }

	public void refreshStats () {
		textHpMana.text = "Hp : " + maxHealth + "      Mana : " + maxMana;
		textAttackDef.text = "Attack : " + currentDamage + "      Armor : " + currentArmor;
	}

    public void OnUnEquipItem(Item item)
    {
        for (int i = 0; i < item.itemAttributes.Count; i++)
        {
            if (item.itemAttributes[i].attributeName == "Health")
			{
				currentHealth -= item.itemAttributes[i].attributeValue;
				maxHealth -= item.itemAttributes [i].attributeValue;
			}
            if (item.itemAttributes[i].attributeName == "Mana")
			{
				currentMana -= item.itemAttributes[i].attributeValue;
				maxMana -= item.itemAttributes [i].attributeValue;
			}
            if (item.itemAttributes[i].attributeName == "Armor")
				currentArmor -= item.itemAttributes[i].attributeValue;
            if (item.itemAttributes[i].attributeName == "Damage")
				currentDamage -= item.itemAttributes[i].attributeValue;
        }
		refreshStats ();
    }



    // Update is called once per frame
    void Update()
    {
		if (playerScriptRespawn.isDead)
			return;
		// bar de vie
		float percentageHp = ((currentHealth * 100) / maxHealth) / 100;
		hpImage.fillAmount = percentageHp;

		// bar de mana
		float percentageMana = ((currentMana * 100) / maxMana) / 100;
		manaImage.fillAmount = percentageMana;

		//print (currentHealth);
		//ApplyDamage (1);

        if (Input.GetKeyDown(inputManagerDatabase.CharacterSystemKeyCode))
        {
            if (!characterSystem.activeSelf)
            {
                characterSystemInventory.openInventory();
            }
            else
            {
                if (toolTip != null)
                    toolTip.deactivateTooltip();
                characterSystemInventory.closeInventory();
            }
        }

		if (Input.GetKeyDown (inputManagerDatabase.Help)) {
			textHelp.enabled = true;
		}
		if (Input.GetKeyUp (inputManagerDatabase.Help)) {
			textHelp.enabled = false;
		}

        if (Input.GetKeyDown(inputManagerDatabase.InventoryKeyCode))
        {
            if (!inventory.activeSelf)
            {
                mainInventory.openInventory();
            }
            else
            {
                if (toolTip != null)
                    toolTip.deactivateTooltip();
                mainInventory.closeInventory();
            }
        }

        if (Input.GetKeyDown(inputManagerDatabase.CraftSystemKeyCode))
        {
            if (!craftSystem.activeSelf)
                craftSystemInventory.openInventory();
            else
            {
                if (cS != null)
                    cS.backToInventory();
                if (toolTip != null)
                    toolTip.deactivateTooltip();
                craftSystemInventory.closeInventory();
            }
        }

    }

}
