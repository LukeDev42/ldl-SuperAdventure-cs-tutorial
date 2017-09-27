using Engine;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

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
            if(!_player.HasRequiredItemToEnterThisLocation(newLocation))
            {
                rtbMessages.Text += "You must have a " +
                    newLocation.ItemRequiredToEnter.Name +
                    " to enter this location." + Environment.NewLine;
                return;
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
                //See if the player already has the quest, and if they've completed it
                bool playerAlreadyHasQuest = _player.HasThisQuest(newLocation.QuestAvailableHere);
                bool playerAlreadyCompletedQuest = _player.ComletedThisQuest(newLocation.QuestAvailableHere);

                //See if the player already has the quest
                if (playerAlreadyHasQuest)
                {
                    //If the player has not completed the quest yet
                    if (!playerAlreadyCompletedQuest)
                    {
                        //See if the player has all the completed items
                        bool playerHasAllItemsToCompleteQuest = _player.HasAllQuestCompletionItem(newLocation.QuestAvailableHere);

                        //player has all the required items
                        if (playerHasAllItemsToCompleteQuest)
                        {
                            //Display messages
                            rtbMessages.Text += Environment.NewLine;
                            rtbMessages.Text += "You complete the " + newLocation.QuestAvailableHere.Name + " quest." + Environment.NewLine;

                            //remove quest items
                            _player.RemoveQuestCompletionItems(newLocation.QuestAvailableHere);

                            //give quest rewards
                            rtbMessages.Text += "You receive: " + Environment.NewLine;
                            rtbMessages.Text += newLocation.QuestAvailableHere.RewardExperiencePoints.ToString() + " experience points" + Environment.NewLine;
                            rtbMessages.Text += newLocation.QuestAvailableHere.RewardGold.ToString() + " gold" + Environment.NewLine;
                            rtbMessages.Text += newLocation.QuestAvailableHere.RewardItem.Name + Environment.NewLine;
                            rtbMessages.Text += Environment.NewLine;

                            _player.ExperiencePoints += newLocation.QuestAvailableHere.RewardExperiencePoints;
                            _player.Gold += newLocation.QuestAvailableHere.RewardGold;

                            //Add reward item to player's inventory
                            _player.AddItemToInventory(newLocation.QuestAvailableHere.RewardItem);

                            //Mark quest complete
                            //Find quest in player's quest list
                            _player.MarkQuestCompleted(newLocation.QuestAvailableHere);
                        }
                    }
                }
                else
                {
                    //The player does not alread have the quest

                    //Display message
                    rtbMessages.Text += "You receive the " + newLocation.QuestAvailableHere.Name + " quest." + Environment.NewLine;
                    rtbMessages.Text += newLocation.QuestAvailableHere.Description + Environment.NewLine;
                    rtbMessages.Text += "To complete it, return with:" + Environment.NewLine;
                    foreach (QuestCompletionItem qci in newLocation.QuestAvailableHere.QuestCompletionItem)
                    {
                        if (qci.Quantity == 1)
                        {
                            rtbMessages.Text += qci.Quantity.ToString() + " " + qci.Details.Name + Environment.NewLine;
                        }
                        else
                        {
                            rtbMessages.Text += qci.Quantity.ToString() + " " + qci.Details.NamePlural + Environment.NewLine;
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
                    standardMonster.RewardExperiencePoints,standardMonster.RewardGold, standardMonster.CurrentHitPoints, standardMonster.MaximumHitPoints);

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
            UpdateInventoryListInUI();

            //Refresh player's quest list
            UpdateQuestListInUI();

            //Refresh player's weapons list
            UpdateWeaponListInUI();

            //Refresh player's potions list
            UpdatePotionListInUI();
        }

        private void UpdateInventoryListInUI()
        {
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
                    dgvInventory.Rows.Add(new[]{
                        inventoryItem.Details.Name,
                        inventoryItem.Quantity.ToString() });
                }
            }
        }

        private void UpdateQuestListInUI()
        {
            dgvQuests.RowHeadersVisible = false;

            dgvQuests.ColumnCount = 2;
            dgvQuests.Columns[0].Name = "Name";
            dgvQuests.Columns[0].Width = 197;
            dgvQuests.Columns[1].Name = "Done?";

            dgvQuests.Rows.Clear();

            foreach (PlayerQuest playerQuest in _player.Quests)
            {
                dgvQuests.Rows.Add(new[] {
                    playerQuest.Details.Name,
                    playerQuest.IsCompleted.ToString() });
            }
        }

        private void UpdateWeaponListInUI()
        {
            List<Weapon> weapons = new List<Weapon>();

            foreach (InventoryItem inventoryItem in _player.Inventory)
            {
                if (inventoryItem.Details is Weapon)
                {
                    if (inventoryItem.Quantity > 0)
                    {
                        weapons.Add((Weapon)inventoryItem.Details);
                    }
                }
            }

            if (weapons.Count == 0)
            {
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
        }

        private void UpdatePotionListInUI()
        {
            List<HealingPotion> healingPotions = new List<HealingPotion>();

            foreach (InventoryItem inventoryItem in _player.Inventory)
            {
                if(inventoryItem.Quantity >0)
                {
                    healingPotions.Add((HealingPotion)inventoryItem.Details);
                }
            }

            if (healingPotions.Count == 0)
            {
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
            //Get the seleced wepon from the cboWeapons
            Weapon currentWeapon = (Weapon)cboWeapons.SelectedItem;

            //Determine damage
            int damageToMonster = RandomNumberGenerator.NumberBetween(
                currentWeapon.MinimumDamage, currentWeapon.MaximumDamage);

            //Apply the damage
            _currentMonster.CurrentHitPoints -= damageToMonster;

            //Display message
            rtbMessages.Text += "You hit the " + _currentMonster.Name + " for " +
                damageToMonster.ToString() + " points." + Environment.NewLine;

            //Check for monster death
            if (_currentMonster.CurrentHitPoints <= 0)
            {
                //monster is dead
                rtbMessages.Text += Environment.NewLine;
                rtbMessages.Text += "You defeated the " + _currentMonster.Name + Environment.NewLine;

                //Give exp
                _player.ExperiencePoints += _currentMonster.RewardExperiencePoints;
                rtbMessages.Text += "You receive " + _currentMonster.RewardExperiencePoints.ToString() +
                    " experience points" + Environment.NewLine;

                //Give gold
                _player.Gold += _currentMonster.RewardGold;
                rtbMessages.Text += "You recieve " + _currentMonster.RewardGold.ToString() +
                    " gold." + Environment.NewLine;

                //Get random loot item
                List<InventoryItem> lootedItems = new List<InventoryItem>();

                //Add items to the lootedItems list, comparing a random number to the drop percentage
                foreach (LootItem  lootItem in _currentMonster.LootTable)
                {
                    if(RandomNumberGenerator.NumberBetween(1,100) <= lootItem.DropPercentage)
                    {
                        lootedItems.Add(new InventoryItem(lootItem.Details, 1));
                    }
                }

                //If no items were selected, add default items
                if(lootedItems.Count == 0)
                {
                    foreach (LootItem lootItem in _currentMonster.LootTable)
                    {
                        if (lootItem.IsDefaultItem)
                        {
                            lootedItems.Add(new InventoryItem(lootItem.Details, 1));
                        }
                    }
                }

                //Add loot items to inventory
                foreach (InventoryItem inventoryItem in lootedItems)
                {
                    _player.AddItemToInventory(inventoryItem.Details);

                    if (inventoryItem.Quantity == 1)
                    {
                        rtbMessages.Text += "You loot " + inventoryItem.Quantity.ToString() + " "
                            + inventoryItem.Details.Name + Environment.NewLine;
                    }
                    else
                    {
                        rtbMessages.Text += "You loot " + inventoryItem.Quantity.ToString() + " "
                            + inventoryItem.Details.NamePlural + Environment.NewLine;
                    }
                }

                //Refresh player info and inventory
                lblHitPoint.Text = _player.CurrentHitPoints.ToString();
                lblGold.Text = _player.Gold.ToString();
                lblExperience.Text = _player.ExperiencePoints.ToString();
                lblLevel.Text = _player.Level.ToString();

                UpdateInventoryListInUI();
                UpdateWeaponListInUI();
                UpdatePotionListInUI();

                rtbMessages.Text += Environment.NewLine;

                MoveTo(_player.CurrentLocation);
            }
        }

        private void btnUsePotion_Click(object sender, EventArgs e)
        {
            //get selected potion
            HealingPotion potion = (HealingPotion)cboPotions.SelectedItem;

            //Add healing amount to player
            _player.CurrentHitPoints = (_player.CurrentHitPoints + potion.AmountToHeal);

            //Dont go over the max hp
            if (_player.CurrentHitPoints > _player.MaximumHitPoints)
            {
                _player.CurrentHitPoints = _player.MaximumHitPoints;
            }

            //Remove Potion
            foreach (InventoryItem ii in _player.Inventory)
            {
                if(ii.Details.ID == potion.ID)
                {
                    ii.Quantity--;
                    break;
                }
            }

            //DisplayMessage
            rtbMessages.Text = "You drink a " + potion.Name + Environment.NewLine;

            //Monster Attacks

            //Determine monsters damage
            int damageToPlayer = RandomNumberGenerator.NumberBetween(0, _currentMonster.MaximumDamage);

            rtbMessages.Text = "The " + _currentMonster.Name + " did " + damageToPlayer.ToString()
                 + " points of damage." + Environment.NewLine;

            //Subract damage from hp
            _player.CurrentHitPoints -= damageToPlayer;

            if (_player.CurrentHitPoints <= 0)
            {
                rtbMessages.Text += "The " + _currentMonster.Name + " killed you." + Environment.NewLine;

                MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
            }

            //refresh UI
            lblHitPoint.Text = _player.CurrentHitPoints.ToString();
            UpdateInventoryListInUI();
            UpdatePotionListInUI();
        }
    }
}