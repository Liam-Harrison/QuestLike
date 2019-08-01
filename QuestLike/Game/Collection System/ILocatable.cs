namespace QuestLike
{
    interface ILocatable
    {
        GameObject[] Locate(string id, bool firstcall = true, bool overrideShow = false);
        void LocateSingleObject(string id, PromptObjectResponse<GameObject> response, bool firstcall = true, bool overrideShow = false);
        T[] LocateObjectsWithType<T>(bool firstcall = true, bool overrideShow = false) where T : class;
        void LocateSingleObjectOfType<T>(PromptObjectResponse<T> response, bool firstcall = true, bool overrideShow = false) where T : class;
        T[] LocateObjectsWithType<T>(string id, bool firstcall = true, bool overrideShow = false) where T : class;
        void LocateSingleObjectOfType<T>(string id, PromptObjectResponse<T> response, bool firstcall = true, bool overrideShow = false) where T : class;
        GameObject LocateWithGameID(int gameID);
    }
}
