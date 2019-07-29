namespace QuestLike
{
    interface ILocatable
    {
        GameObject[] Locate(string id, bool firstcall = true);
        void LocateSingleObject(string id, PromptObjectResponse<GameObject> response, bool firstcall = true);
        T[] LocateObjectsWithType<T>(bool firstcall = true) where T : class;
        void LocateSingleObjectOfType<T>(PromptObjectResponse<T> response, bool firstcall = true) where T : class;
        T[] LocateObjectsWithType<T>(string id, bool firstcall = true) where T : class;
        void LocateSingleObjectOfType<T>(string id, PromptObjectResponse<T> response, bool firstcall = true) where T : class;
        IHoldable[] LocateHoldables(string id, bool firstcall = true);
        void LocateSingleHoldableObject(string id, PromptObjectResponse<IHoldable> response, bool firstcall = true);
        IInventory[] LocateInventories(string id, bool firstcall = true);
        void LocateSingleInventory(string id, PromptObjectResponse<IInventory> response, bool firstcall = true);
        GameObject LocateWithGameID(int gameID);
    }
}
