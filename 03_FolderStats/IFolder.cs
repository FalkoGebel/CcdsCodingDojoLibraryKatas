namespace _03_FolderStats
{
    public interface IFolder
    {
        public string Path { get; set; }
        public long NumberOfFiles { get; set; }
        public long TotalBytes { get; set; }
        public int Depth { get; set; }
    }
}
