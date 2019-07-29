using Microsoft.Xna.Framework;
using System.Linq;

namespace QuestLike.Command
{
    class InteractionCommands : Command
    {
        public InteractionCommands()
        {
            keywords = new string[] { };
            usecases = new string[] { "select ^", "answer ^", "+", "yes", "no" };
            tags = new string[] { "interaction" };
            commandName = "Interaction";
            showInHelp = false;
        }

        public override bool Execute(bool admin)
        {

            switch (usecaseID)
            {
                case 0:
                    if (admin && Utilities.awaitedUserResponse == Utilities.UserResponseType.GameObject)
                    {
                        if (!int.TryParse(GetArg(0), out int objectID))
                        {
                            if (Settings.DebugMode) GameScreen.PrintLine($"\n<{Color.Red.ToInteger()},>[Warning] - selection link returned non-integer value@\n");
                            return false;
                        }

                        var gameObject = Game.LocateWithGameID(objectID);
                        if (gameObject == null)
                        {
                            if (Settings.DebugMode) GameScreen.PrintLine($"\n<{Color.Red.ToInteger()},>[Warning] - selection link gameobject ID could not be resolved to a gameobject@");
                            return false;
                        }
                        if (Settings.DebugMode) GameScreen.PrintLine($"\n<{Color.Yellow.ToInteger()},>[Info] - Selected gameobject {gameObject.Name} (ID {objectID})@");
                        Utilities.awaitedUserResponse = Utilities.UserResponseType.None;
                        Utilities.userResponse.Invoke(gameObject, false);
                        return false;
                    }

                    break;
                case 1:
                    if (!admin || Utilities.awaitedUserResponse != Utilities.UserResponseType.YesNo) break;

                    if (Utilities.awaitedUserResponse == Utilities.UserResponseType.YesNo)
                    {
                        if (GetArg(0) == "yes")
                        {
                            if (Settings.DebugMode) GameScreen.PrintLine($"\n<{Color.Yellow.ToInteger()},>[Info] - User responded with a yes@");
                            Utilities.awaitedUserResponse = Utilities.UserResponseType.None;
                            Utilities.userResponse.Invoke(true, false);
                        }
                        else if (GetArg(0) == "no")
                        {
                            if (Settings.DebugMode) GameScreen.PrintLine($"\n<{Color.Yellow.ToInteger()},>[Info] - User responded with a no@");
                            Utilities.awaitedUserResponse = Utilities.UserResponseType.None;
                            Utilities.userResponse.Invoke(false, false);
                        }
                        else if (Settings.DebugMode)
                        {
                            GameScreen.PrintLine($"\n<{Color.Red.ToInteger()},>[Warning] - selection returned unexpected response \"{GetArg(0)}\"@");
                        }
                    }

                    break;
                case 2:
                    if (Utilities.awaitedUserResponse != Utilities.UserResponseType.GameObject || !int.TryParse(GetArg(0), out int value)) break;

                    if (value <= 0 || value > Utilities.selectableObjects.Count())
                    {
                        GameScreen.PrintLine($"\nYou can only enter a value from 1 - {Utilities.selectableObjects.Count()}");
                        break;
                    }

                    var gameobject = Utilities.selectableObjects[value - 1];

                    if (Settings.DebugMode) GameScreen.PrintLine($"\n<{Color.Yellow.ToInteger()},>[Info] - Selected gameobject {gameobject.Name} (ID {gameobject.ID})@");
                    Utilities.awaitedUserResponse = Utilities.UserResponseType.None;
                    Utilities.userResponse.Invoke(gameobject, false);

                    break;
                case 3:
                    if (Utilities.awaitedUserResponse != Utilities.UserResponseType.YesNo) break;

                    if (Settings.DebugMode) GameScreen.PrintLine($"\n<{Color.Yellow.ToInteger()},>[Info] - User responded with a yes@");
                    Utilities.awaitedUserResponse = Utilities.UserResponseType.None;
                    Utilities.userResponse.Invoke(true, false);

                    break;
                case 4:
                    if (Utilities.awaitedUserResponse != Utilities.UserResponseType.YesNo) break;

                    if (Settings.DebugMode) GameScreen.PrintLine($"\n<{Color.Yellow.ToInteger()},>[Info] - User responded with a yes@");
                    Utilities.awaitedUserResponse = Utilities.UserResponseType.None;
                    Utilities.userResponse.Invoke(false, false);

                    break;
            }

            return false;
        }
    }
}
