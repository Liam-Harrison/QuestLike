using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestLike.Command
{
    class TalkCommand : Command
    {
        public TalkCommand()
        {
            keywords = new string[] { "talk", "speak" };
            usecases = new string[] { "converse +", "respond + x +",  "_ to ^" };
            tags = new string[] { "talk", "speak" };
            adminCommands = new int[] { 0, 1 };
            commandName = "Talk";
        }

        public override bool Execute(bool admin)
        {
            switch(usecaseID)
            {
                case 0:
                    if (!admin) return false;

                    if (int.TryParse(GetArg(0), out int id))
                    {
                        var gameobject = Game.LocateWithGameID(id);
                        if (gameobject is ITalkable)
                        {
                            (gameobject as ITalkable).TalkTo();
                        }
                    }

                    break;
                case 1:

                    if (!admin) return false;

                    if (int.TryParse(GetArg(0), out id))
                    {
                        var gameobject = Game.LocateWithGameID(id);
                        if (gameobject is ITalkable)
                        {
                            if (int.TryParse(GetArg(1), out int response))
                            {
                                (gameobject as ITalkable).OnReponse(response);
                            }
                        }
                    }

                    break;
                case 2:
                    Game.LocateSingleObjectOfType<ITalkable>(GetArg(0), (gameobject, canceled) => {
                        if (canceled) return;
                        gameobject.TalkTo();
                    }, false);

                    break;
            }
            return false;
        }
    }
}
