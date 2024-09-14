namespace _03_FolderStats
{
    internal interface IFolderStats
    {
        void Connect(string rootpath);

        void Start();
        void Stop();

        void Pause();
        void Resume();

        IEnumerable<Folder> Folders { get; }
        string RootPath { get; }
        Statuses Status { get; }

        event Action Progress;
    }
}
