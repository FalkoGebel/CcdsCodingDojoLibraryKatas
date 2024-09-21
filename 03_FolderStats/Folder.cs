namespace _03_FolderStats
{
    public class Folder : IFolder
    {
        public string ParentPath { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public long NumberOfFiles { get; set; }
        public long TotalBytes { get; set; }
        public int Depth { get; set; }
        public bool Ready { get; set; }
    }
}