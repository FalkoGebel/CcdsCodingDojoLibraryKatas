
namespace _02_FindFileDuplicates
{
    public class Duplicates : IDuplicates
    {
        readonly List<Duplicate> _duplicates;
        public IEnumerable<string> Filepath { get => _duplicates.Select(d => d.Path); }

        public Duplicates()
        {
            _duplicates = [];
        }

        public bool Contains(string name, long sizeInBytes)
        {
            return _duplicates.Any(d => d.Name == name.ToLower() && d.SizeInBytes == sizeInBytes);
        }

        public int Count()
        {
            return Filepath.Count();
        }

        internal void Add(string name, long sizeInBytes, string file)
        {
            _duplicates.Add(new() { Name = name.ToLower(), SizeInBytes = sizeInBytes, Path = file });
        }
    }
}