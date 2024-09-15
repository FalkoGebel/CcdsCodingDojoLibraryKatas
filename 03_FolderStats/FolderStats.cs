namespace _03_FolderStats
{
    public class FolderStats : IFolderStats
    {
        private Statuses _currentStatus;
        private Statuses _newStatus;
        private string _rootPath;
        readonly private List<Folder> _folders;
        private Thread _process;

        public IEnumerable<Folder> Folders { get => _folders; }

        public string RootPath { get => _rootPath; }

        public Statuses Status { get => _currentStatus; }

        public event Action Progress;

        public FolderStats()
        {
            _currentStatus = Statuses.Waiting;
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
            _currentStatus = Statuses.Connected;

            _folders.Clear();
            InsertFolder();
        }

        private void InsertFolder(string parentPath = "", string path = "", int depth = 0)
        {
            if (path == string.Empty)
                path = RootPath;

            _folders.Add(new()
            {
                ParentPath = parentPath,
                Path = path,
                NumberOfFiles = 0,
                TotalBytes = 0,
                Depth = depth,
                Ready = false
            });

            foreach (var dir in Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly))
            {
                InsertFolder(path, dir, depth + 1);
            };
        }

        public void Pause()
        {
            if (_currentStatus != Statuses.Running)
                throw new InvalidOperationException("Folder Stats not running - cannot be paused");

            _newStatus = Statuses.Paused;
        }

        public void Resume()
        {
            if (_currentStatus != Statuses.Running)
                throw new InvalidOperationException("Folder Stats not paused - cannot be resumed");

            StartProcessingThread();
        }

        public void Start()
        {
            if (_currentStatus != Statuses.Connected)
                throw new InvalidOperationException("Folder Stats not connected - cannot be started");

            StartProcessingThread();
        }

        public void Stop()
        {
            if (_currentStatus != Statuses.Running && _currentStatus != Statuses.Paused)
                throw new InvalidOperationException("Folder Stats not running or paused - cannot be stopped");

            _newStatus = Statuses.Connected;
        }

        private void Processing()
        {
            // TODO - Fresh start or resume -> Distinction necessary?
            if (_currentStatus == Statuses.Paused)
            {

            }

            _currentStatus = Statuses.Running;
            _newStatus = _currentStatus;

            for (; ; )
            {
                if (_folders.All(f => f.Ready))
                    break;

                int maxDepth = _folders.Where(f => !f.Ready).Select(f => f.Depth).Max();
                int idx = _folders.FindIndex(f => !f.Ready && f.Depth == maxDepth);

                // implement the folder stats determination
                Folder currentFolder = _folders[idx];
                currentFolder.TotalBytes = 0;
                currentFolder.NumberOfFiles = 0;

                // get values from subfolders for current folder from list
                foreach (var subFolder in _folders.Where(f => f.ParentPath == currentFolder.Path))
                {
                    currentFolder.NumberOfFiles += subFolder.NumberOfFiles;
                    currentFolder.TotalBytes += subFolder.TotalBytes;
                }

                // get values for items for current folder from IO
                foreach (var file in Directory.GetFiles(currentFolder.Path, "*", SearchOption.TopDirectoryOnly))
                {
                    currentFolder.NumberOfFiles++;
                    FileInfo fileInfo = new FileInfo(file);
                    currentFolder.TotalBytes += fileInfo.Length;
                };

                currentFolder.Ready = true;

                _folders[idx] = currentFolder;

                // check, if threaded processing is to stop
                if (_newStatus != _currentStatus)
                {
                    switch (_newStatus)
                    {
                        case Statuses.Paused:
                            _currentStatus = _newStatus;
                            return;
                        case Statuses.Connected:
                            ResetFolders();
                            _currentStatus = _newStatus;
                            return;
                    }
                }
            }

            _currentStatus = Statuses.Finished;
        }

        private void StartProcessingThread()
        {
            _process = new Thread(Processing)
            {
                // This is important as it allows the process to exit while this thread is running
                IsBackground = true
            };
            _process.Start();
        }

        private void ResetFolders()
        {
            for (int i = 0; i < _folders.Count; i++)
            {
                _folders[i].NumberOfFiles = 0;
                _folders[i].TotalBytes = 0;
                _folders[i].Ready = false;
            }
        }
    }
}