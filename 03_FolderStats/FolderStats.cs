namespace _03_FolderStats
{
    public class FolderStats : IFolderStats
    {
        private Statuses _status;
        private string _rootPath;
        readonly private List<Folder> _folders;

        public IEnumerable<Folder> Folders { get => _folders; }

        public string RootPath { get => _rootPath; }

        public Statuses Status { get => _status; }

        public event Action Progress;

        public FolderStats()
        {
            _status = Statuses.Waiting;
            _rootPath = string.Empty;
            _folders = [];
        }

        public void Connect(string rootpath)
        {
            if (string.IsNullOrEmpty(rootpath))
                throw new ArgumentException("Root path is missing");

            if (!Directory.Exists(rootpath))
                throw new ArgumentException("Invalid folderpath");

            _rootPath = rootpath;
            _status = Statuses.Connected;

            _folders.Clear();
            InsertFolder();
        }

        private void InsertFolder(string path = "", int depth = 0)
        {
            if (path == string.Empty)
                path = RootPath;

            _folders.Add(new()
            {
                Path = path,
                NumberOfFiles = 0,
                TotalBytes = 0,
                Depth = depth
            });

            foreach (var dir in Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly))
            {
                InsertFolder(dir, depth + 1);
            };
        }

        public void Pause()
        {
            _status = Statuses.Paused;
        }

        public void Resume()
        {
            _status = Statuses.Running;
        }

        public void Start()
        {
            _status = Statuses.Running;
        }

        public void Stop()
        {
            _status = Statuses.Connected;
        }
    }
}
