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
            usecases = new string[] { "_ ^ with my ^", "_ ^ with ^", "_ ^" };
            tags = new string[] { "grab", "pickup" };
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

                    Game.LocateSingleObjectOfType<Item>(GetArg(0), (item, b) => {

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
                                if (!(options[i] as IInventory).GetInventory.EnoughSpace(item)) options.RemoveAt(i);
                            }
                        }

                        if (options.Count == 1)
                        {
                            foreach (var holdable in holdables)
                            {
                                if (holdable == options[0])
                                {
                                    HoldableSafePlace(item, holdable);
                                }
                            }
                            foreach (var inventory in inventories)
                            {
                                if (inventory == options[0])
                                {
                                    InventorySafePlace(item, inventory);
                                }
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

                    });
                    break;
            }

            return false;
        }

        bool InventorySafePlace(Item item, IInventory inventory)
        {
            if (inventory.GetInventory.EnoughSpace(item))
            {
                var tempItem = item;
                item.container.GetTypedCollection().RemoveObject(item);
                inventory.GetInventory.AddItem(item);
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
                GameScreen.PrintLine("\nThe item \"" + holdable.GetHoldingItem().Name + "\" is already being held - would you like to swap it for \"" + item.Name + "\"?");

                Utilities.PromptYesNo((answer, cancelled) =>
                {
                    if (answer && !cancelled)
                    {
                        var container = item.container;
                        var tempItem = item;
                        item.container.GetTypedCollection().RemoveObject(item);
                        var oldItem = holdable.SwitchItems(tempItem);
                        container.GetTypedCollection().AddObject(oldItem);
                        GameScreen.PrintLine("\nMoved \"" + tempItem.Name + "\" to \"" + (holdable as GameObject).Name + "\".");
                        GameScreen.PrintLine("Moved \"" + oldItem.Name + "\" to \"" + container.owner.Name + "\".");
                    }
                    else
                    {
                        GameScreen.PrintLine("\nLeft the \"" + item.Name + "\" where it is.");
                    }
                });
            }
            else
            {
                var tempItem = item;
                item.container.GetTypedCollection().RemoveObject(item);
                holdable.PutItem(tempItem);
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
