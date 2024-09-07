
using System.Collections;

namespace _02_FindFileDuplicates
{
    public class FindFileDuplicatesLogic : ICheckForDuplicates
    {
        public static List<string> GetFileForPaths(string folderpath)
            => Directory.GetFiles(folderpath, "*", SearchOption.AllDirectories)
                        .Select(f => (new FileInfo(f)).FullName)
                        .ToList();

        public IEnumerable CheckCandidates(IEnumerable candidates)
        {
            throw new NotImplementedException();
        }

        public IEnumerable CompileCandidates(string folderpath) => CompileCandidates(folderpath, CompareModes.SizeAndName);

        public IEnumerable CompileCandidates(string folderpath, CompareModes mode)
        {
            if (string.IsNullOrEmpty(folderpath) || !Directory.Exists(folderpath))
                throw new ArgumentException("Missing or invalid folderpath");

            Duplicates duplicates = new();
            List<string> files = GetFileForPaths(folderpath);
            List<FileInfo> fileInfos = [];

            foreach (string file in files)
                fileInfos.Add(new FileInfo(file));

            foreach (FileInfo fileInfo in fileInfos)
            {
                switch (mode)
                {
                    case CompareModes.SizeAndName:
                        if (fileInfos.Count(fi => fileInfo.Name.ToLower() == fi.Name.ToLower() && fileInfo.Length == fi.Length) > 1)
                            duplicates.Add(fileInfo.Name, fileInfo.Length, fileInfo.FullName);
                        break;

                    case CompareModes.Size:
                        if (fileInfos.Count(fi => fileInfo.Length == fi.Length) > 1)
                            duplicates.Add(fileInfo.Name, fileInfo.Length, fileInfo.FullName);
                        break;

                    default: throw new ArgumentException($"Compare mode \"{mode}\" not supported");
                }
            }

            return duplicates.Filepath;
        }
    }
}