using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Engine
{
    public class Vendor
    {
        public string Name { get; set; }
        public BindingList<InventoryItem> Inventory { get; private set; }

        public Vendor(string name)
        {
            Name = name;
            Inventory = new BindingList<InventoryItem>();
        }

        public void AddItemToInventory(Item itemToAdd, int quantity = 1)
        {
            InventoryItem item = Inventory.SingleOrDefault(ii => ii.Details.ID == itemToAdd.ID);

            if (item == null)
            {
                //They didn't have the item, so add it to their inventory
                Inventory.Add(new InventoryItem(itemToAdd, quantity));
            }
            else
            {
                item.Quantity += quantity;
            }

            OnPropertyChanged("Inventory");
        }

        public void RemoveItemFromInventory(Item itemToRemove, int quantity = 1)
        {
            InventoryItem item = Inventory.SingleOrDefault(ii => ii.Details.ID == itemToRemove.ID);

            if (item == null)
            {
                //The item is not in the palyer's inventory, so ignore it.
                //We might want to raise an error for this situation
            }
            else
            {
                //They have the item, decrease the quantity
                item.Quantity -= quantity;

                //Dont allow negative quantities
                //Might want to raise an error
                if (item.Quantity < 0)
                {
                    item.Quantity = 0;
                }

                //if the quantity is zero, remove the item from the list
                if (item.Quantity == 0)
                {
                    Inventory.Remove(item);
                }

                //Notify UI that the inventory has changed
                OnPropertyChanged("Inventory");
            }
        }

        public event PropertyChangingEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangingEventArgs(name));
            }
        }
    }
}

