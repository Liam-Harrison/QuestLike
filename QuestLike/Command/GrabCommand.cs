using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZorkLike.Command
{
    class GrabCommand : Command
    {
        public GrabCommand()
        {
            keywords = new string[] { "grab", "pickup"};
            usecases = new string[] { "_ ^ with my ^", "_ ^ with ^", "grab +", "_ ^" };
            tags = new string[] { "grab", "pickup" };
            adminCommands = new int[] { 2 };
            commandName = "Grab";
        }

        public override bool Execute(bool admin)
        {
            switch (usecaseID)
            {
                case 0:
                    StandardLookAtMe();
                    break;
                case 1:
                    StandardLookAtMe();
                    break;
                case 2:
                    if (!admin) return false;

                    if (int.TryParse(GetArg(0), out int id))
                    {
                        var gameobject = Game.LocateWithGameID(id);
                        if (gameobject == null) return false;
                        if (gameobject as Item == null) return false;

                        GrabItem(gameobject as Item, false);
                    }
                    break;
                case 3:

                    Game.LocateSingleObjectOfType<Item>(GetArg(0), GrabItem);

                    break;
            }

            return false;
        }

        public void GrabItem(Item item, bool canceled)
        {

            var holdables = Game.GetPlayer.LocateObjectsWithType<IHoldable>(false);
            var inventories = Game.GetPlayer.LocateObjectsWithType<IInventory>(false);

            if (item == null)
            {
                GameScreen.PrintLine("\nCould not find any items named \"" + GetArg(0) + "\".");
                return;
            }

            if (holdables.Length == 0 && inventories.Length == 0)
            {
                GameScreen.PrintLine("\nCould not find anywhere to place \"" + item.Name + "\".");
                return;
            }

            List<GameObject> options = new List<GameObject>();
            options.AddRange(Utilities.CastArray<GameObject, IHoldable>(holdables));
            options.AddRange(Utilities.CastArray<GameObject, IInventory>(inventories));

            for (int i = 0; i < options.Count; i++)
            {
                if (options[i] is IInventory)
                {
                    // Remove any full inventories.
                    var casted = options[i] as IInventory;
                    if (!casted.GetInventory.EnoughSpace(item)) options.RemoveAt(i);
                    if (casted.GetInventory.GetItems.Contains(item)) options.RemoveAt(i);
                }
                else if (options[i] is IHoldable)
                {
                    var casted = options[i] as IHoldable;
                    if (casted.HoldingItem == item) options.RemoveAt(i);
                }
            }

            if (options.Count == 1)
            {
                if (options[0] is IHoldable)
                {
                    HoldableSafePlace(item, options[0] as IHoldable);
                }
                else if (options[0] is IInventory)
                {
                    InventorySafePlace(item, options[0] as IInventory);
                }
            }
            else
            {
                Utilities.PromptSelection<GameObject>("Select one of the following destinations", options.ToArray(), (selection, c) =>
                {
                    if (selection is IHoldable)
                    {
                        HoldableSafePlace(item, selection as IHoldable);
                    }
                    else if (selection is IInventory)
                    {
                        InventorySafePlace(item, selection as IInventory);
                    }
                });
            }
        }

        bool InventorySafePlace(Item item, IInventory inventory)
        {
            if (inventory.GetInventory.EnoughSpace(item))
            {
                var tempItem = item;
                item.container.GetTypedCollection().RemoveObject(item);
                inventory.GetInventory.AddItem(tempItem);
                GameScreen.Print("\nMoved \"" + item.Name + "\" to \"" + (inventory as GameObject).Name + "\".");
            }
            else
            {
                GameScreen.Print("\nYou do not have enough space in your inventory for \"" + item.Name + "\".");
            }
            return false;
        }

        bool HoldableSafePlace(Item item, IHoldable holdable)
        {
            if (holdable.IsHoldingItem)
            {
                GameScreen.PrintLine("\nThe item \"" + holdable.HoldingItem.Name + "\" is already being held - would you like to swap it for \"" + item.Name + "\"?");

                Utilities.PromptYesNo((answer, cancelled) =>
                {
                    if (answer && !cancelled)
                    {
                        var old = holdable.SwitchItems(item);

                        GameScreen.PrintLine("\nMoved \"" + item.Name + "\" to \"" + (holdable as GameObject).Name + "\".");
                        GameScreen.PrintLine("Moved \"" + old.Name + "\" to \"" + old.container.GetTypedCollection().owner.Name + "\".");
                    }
                    else
                    {
                        GameScreen.PrintLine("\nLeft the \"" + item.Name + "\" where it is.");
                    }
                });
            }
            else
            {
                holdable.PutItem(item);
                GameScreen.PrintLine("\nMoved \"" + item.Name + "\" to \"" + (holdable as GameObject).Name + "\".");
            }
            return false;
        }

        void StandardLookAtMe()
        {
            Game.LocateSingleObjectOfType<Item>(GetArg(0), (item, b) =>
            {
                Game.GetPlayer.LocateSingleObjectOfType<IHoldable>(GetArg(1), (holdable, c) =>
                {
                    if (item == null)
                    {
                        GameScreen.PrintLine("\nCould not find any items named \"" + GetArg(0) + "\".");
                        return;
                    }
                    else if (holdable == null)
                    {
                        GameScreen.PrintLine("\nCould anything named \"" + GetArg(1) + "\" that can hold something.");
                        return;
                    }

                    HoldableSafePlace(item, holdable);

                    return;
                });
            });
        }

    }
}
