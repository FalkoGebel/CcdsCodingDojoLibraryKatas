using System.Collections;

namespace _02_FindFileDuplicates
{
    public interface ICheckForDuplicates
    {
        IEnumerable Compile_candidates(string folderpath);
        IEnumerable Compile_candidates(string folderpath, CompareModes mode);
        IEnumerable Check_candidates(IEnumerable candidates);
    }
}