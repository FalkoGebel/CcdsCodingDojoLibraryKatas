using System.Collections;

namespace _02_FindFileDuplicates
{
    public interface ICheckForDuplicates
    {
        IEnumerable CompileCandidates(string folderpath);
        IEnumerable CompileCandidates(string folderpath, CompareModes mode);
        IEnumerable CheckCandidates(IEnumerable candidates);
    }
}