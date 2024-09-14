namespace _02_FindFileDuplicates
{
    public interface ICheckForDuplicates
    {
        IEnumerable<string> CompileCandidates(string folderpath);
        IEnumerable<string> CompileCandidates(string folderpath, CompareModes mode);
        IEnumerable<string> CheckCandidates(IEnumerable<string> candidates);
    }
}