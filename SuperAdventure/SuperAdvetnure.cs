using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Engine;

namespace SuperAdventure
{

    public partial class SuperAdvetnure : Form
    {
        private Player _player;
        private Monster _currentMonster;

        public SuperAdvetnure()
        {
            InitializeComponent();

            Location location = new Location(1, "Home", "This is your house.", null, null, null);


            _player = new Player(10, 10, 20, 0, 1);

            lblHitPoint.Text = _player.CurrentHitPoints.ToString();
            lblGold.Text = _player.Gold.ToString();
            lblExperience.Text = _player.ExperiencePoints.ToString();
            lblLevel.Text = _player.Level.ToString();


        }

        private void lblExperience_Click(object sender, EventArgs e)
        {

        }

        private void btnNorth_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToNorth);
        }

        private void btnWest_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToWest);
        }

        private void btnSouth_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToSouth);
        }

        private void btnEast_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToEast);
        }



        private void MoveTo(Location newLocation)
        {
            //Does the location require any items
            if (newLocation.ItemRequiredToEnter != null)
            {
                //see if player has required item
                bool playerHasRequiredItem = false;

                foreach (InventoryItem ii in _player.Inventory)
                {
                    if (ii.Details.ID == newLocation.ItemRequiredToEnter.ID)
                    {
                        //We found required Item
                        playerHasRequiredItem = true;
                        break;
                    }
                }

                if (!playerHasRequiredItem)
                {
                    //We didn't find the required item
                    //display a message and stop the move
                    rtbMessages.Text += "You must have a " +
                        newLocation.ItemRequiredToEnter.Name +
                        " to enter this location." + Environment.NewLine;
                    return;
                }
            }

            //Update players location
            _player.CurrentLocation = newLocation;


            //show/hide availbale movement buttons
            btnNorth.Visible = (newLocation.LocationToNorth != null);
            btnEast.Visible = (newLocation.LocationToEast != null);
            btnSouth.Visible = (newLocation.LocationToSouth != null);
            btnWest.Visible = (newLocation.LocationToWest != null);

            //Display current location name and description
            rtbLocation.Text = newLocation.Name + Environment.NewLine;
            rtbLocation.Text = newLocation.Description + Environment.NewLine;

            //Completely heal the player
            _player.CurrentHitPoints = _player.MaximumHitPoints;

            //Update hit point in UI
            lblHitPoint.Text = _player.CurrentHitPoints.ToString();

            //Does the location have a quest?
            if (newLocation.QuestAvailableHere != null)
            {
                bool playerAlreadyHasQuest = false;
                bool playerAlreadyCompletedQuest = false;

                foreach (PlayerQuest playerQuest in _player.Quests)
                {
                    if (playerQuest.Details.ID == newLocation.QuestAvailableHere.ID)
                    {
                        playerAlreadyHasQuest = true;

                        if (playerQuest.IsCompleted)
                        {
                            playerAlreadyCompletedQuest = true;
                        }
                    }
                }

                //See if the player already has the quest
                if (playerAlreadyHasQuest)
                {
                    if (!playerAlreadyCompletedQuest)
                    {
                        bool PlayerHasAllItemsToCompleteQuest = true;

                        foreach (QuestCompletionItem qci in
                            newLocation.QuestAvailableHere.QuestCompletionItem)
                        {
                            bool foundItemInPlayersInventory = false;

                            //check each item in player's inventory, do they have it, enough of it
                            foreach (InventoryItem ii in _player.Inventory)
                            {
                                if (ii.Details.ID == qci.Details.ID)
                                {
                                    foundItemInPlayersInventory = true;
                                    if (ii.Quantity < qci.Quantity)
                                    {
                                        //The player doesn't have enough of this item
                                        PlayerHasAllItemsToCompleteQuest = false;

                                        //No reason to continue checking
                                        break;
                                    }
                                    //We found the item, dont check the rest of the inventory
                                    break;
                                }
                            }
                            //if we dont find item, set variable and stop looking for other items
                            if (!foundItemInPlayersInventory)
                            {
                                //player does not have item in inventory
                                PlayerHasAllItemsToCompleteQuest = false;

                                break;
                            }
                        }

                        //player has all the required items
                        if (PlayerHasAllItemsToCompleteQuest)
                        {
                            //Display messages
                            rtbMessages.Text += Environment.NewLine;
                            rtbMessages.Text += "You complete the " +
                                newLocation.QuestAvailableHere.Name +
                                " quest." + Environment.NewLine;

                            //remove quest items
                            foreach (QuestCompletionItem qci in
                                newLocation.QuestAvailableHere.QuestCompletionItem)
                            {
                                foreach (InventoryItem ii in _player.Inventory)
                                {
                                    if (ii.Details.ID == qci.Details.ID)
                                    {
                                        //Subract quantity from player inventory to complete quest
                                        ii.Quantity -= qci.Quantity;
                                        break;
                                    }

                                }
                            }

                            //give quest rewards
                            rtbMessages.Text += "You receive: " + Environment.NewLine;
                            rtbMessages.Text +=
                                newLocation.QuestAvailableHere.RewardExperiencePoints.ToString() +
                                " experience points" + Environment.NewLine;
                            rtbMessages.Text +=
                                newLocation.QuestAvailableHere.RewardGold.ToString() +
                                " gold" + Environment.NewLine;
                            rtbMessages.Text +=
                                newLocation.QuestAvailableHere.RewardItem.Name +
                                Environment.NewLine;
                            rtbMessages.Text += Environment.NewLine;

                            _player.ExperiencePoints +=
                                newLocation.QuestAvailableHere.RewardExperiencePoints;
                            _player.Gold += newLocation.QuestAvailableHere.RewardGold;

                            //Add reward item to player's inventory
                            bool addedItemToPlayerInventory = false;

                            foreach (InventoryItem ii in _player.Inventory)
                            {
                                if (ii.Details.ID ==
                                    newLocation.QuestAvailableHere.RewardItem.ID)
                                {
                                    //They have the item in their inventory
                                    //so increase the quantity by one
                                    ii.Quantity++;

                                    addedItemToPlayerInventory = true;

                                    break;
                                }
                            }
                            if (!addedItemToPlayerInventory)
                            {
                                _player.Inventory.Add(new InventoryItem(
                                    newLocation.QuestAvailableHere.RewardItem, 1));
                            }
                            //Mark quest complete, find quest in player's quest list
                            foreach (PlayerQuest pq in _player.Quests)
                            {
                                if (pq.Details.ID == newLocation.QuestAvailableHere.ID)
                                {
                                    //mark as complete
                                    pq.IsCompleted = true;

                                    break;
                                }

                            }

                        }
                    }
                }
                else
                {
                    //The player does not alread have the quest

                    //Display message
                    rtbMessages.Text += "You receive the " +
                        newLocation.QuestAvailableHere.Name +
                        " quest." + Environment.NewLine;
                    rtbMessages.Text += newLocation.QuestAvailableHere.Description +
                        Environment.NewLine;
                    rtbMessages.Text += "To complete it, return with:" +
                        Environment.NewLine;
                    foreach (QuestCompletionItem qci in
                        newLocation.QuestAvailableHere.QuestCompletionItem)
                    {
                        if (qci.Quantity == 1)
                        {
                            rtbMessages.Text += qci.Quantity.ToString() + " " +
                                qci.Details.Name + Environment.NewLine;
                        }
                        else
                        {
                            rtbMessages.Text += qci.Quantity.ToString() + " " +
                                qci.Details.NamePlural + Environment.NewLine;
                        }
                    }
                    rtbMessages.Text += Environment.NewLine;

                    //Add the quest to the player's quest list
                    _player.Quests.Add(new PlayerQuest(newLocation.QuestAvailableHere));
                }
            }

            //Does the location have a monster
            if (newLocation.MonsterLivingHere != null)
            {
                rtbMessages.Text += "You see a " + newLocation.MonsterLivingHere.Name + Environment.NewLine;

                //Make a new monster, Using values from the standard monster in the World.Monster list
                Monster standardMonster = World.MonsterByID(newLocation.MonsterLivingHere.ID);
                
                _currentMonster = new Monster(standardMonster.ID, standardMonster.Name, standardMonster.MaximumDamage, 
                    standardMonster.RewardExperiencePoints,standardMonster.RewardGold, standardMonster.CurrentHitPoints, 
                        standardMonster.MaximumHitPoints);

                foreach (LootItem lootItem in standardMonster.LootTable)
                {
                    _currentMonster.LootTable.Add(lootItem);
                }

                cboWeapons.Visible = true;
                cboPotions.Visible = true;
                btnUseWeapon.Visible = true;
                btnUsePotion.Visible = true;
            }
            else
            {
                _currentMonster = null;

                cboWeapons.Visible = false;
                cboPotions.Visible = false;
                btnUseWeapon.Visible = false;
                btnUsePotion.Visible = false;
            }

            //Refresh player's inventory list
            dgvInventory.RowHeadersVisible = false;

            dgvInventory.ColumnCount = 2;
            dgvInventory.Columns[0].Name = "Name";
            dgvInventory.Columns[0].Width = 197;
            dgvInventory.Columns[1].Name = "Quantity";

            dgvInventory.Rows.Clear();

            foreach (InventoryItem inventoryItem in _player.Inventory)
            {
                if(inventoryItem.Quantity > 0)
                {
                    dgvInventory.Rows.Add(new[] { inventoryItem.Details.Name,
                    inventoryItem.Quantity.ToString() });
                }
            }

            //Refresh player's quest list
            dgvQuests.RowHeadersVisible = false;

            dgvQuests.ColumnCount = 2;
            dgvQuests.Columns[0].Name = "Name";
            dgvQuests.Columns[0].Width = 197;
            dgvQuests.Columns[1].Name = "Done?";

            dgvQuests.Rows.Clear();

            foreach(PlayerQuest playerQuest in _player.Quests)
            {
                dgvQuests.Rows.Add(new[] { playerQuest.Details.Name,
                playerQuest.IsCompleted.ToString() });
            }

            //Refresh player's weapons combobox
            List<Weapon> weapons = new List<Weapon>();

            foreach (InventoryItem inventoryItem in _player.Inventory)
            {
                if(inventoryItem.Details is Weapon)
                {
                    if(inventoryItem.Quantity >0)
                    {
                        weapons.Add((Weapon)inventoryItem.Details);
                    }
                }
            }

            if(weapons.Count == 0)
            {
                //no weapons, hide the weapon combobox and the "use" button
                cboWeapons.Visible = false;
                btnUseWeapon.Visible = false;
            }
            else
            {
                cboWeapons.DataSource = weapons;
                cboWeapons.DisplayMember = "Name";
                cboWeapons.ValueMember = "ID";

                cboWeapons.SelectedIndex = 0;
            }

            //Refresh player's potions combobox
            List<HealingPotion> healingPotions = new List<HealingPotion>();

            foreach (InventoryItem inventoryItem in _player.Inventory)
            {
                if(inventoryItem.Details is HealingPotion)
                {
                    if(inventoryItem.Quantity > 0)
                    {
                        healingPotions.Add((HealingPotion)inventoryItem.Details);
                    }
                }
            }

            if (healingPotions.Count == 0)
            {
                //no potions, no combobox or button
                cboPotions.Visible = false;
                btnUsePotion.Visible = false;
            }
            else
            {
                cboPotions.DataSource = healingPotions;
                cboPotions.DisplayMember = "Name";
                cboPotions.ValueMember = "ID";

                cboPotions.SelectedIndex = 0;
            }
        }



        private void btnUseWeapon_Click(object sender, EventArgs e)
        {

        }

        private void btnUsePotion_Click(object sender, EventArgs e)
        {

        }
    }
}
