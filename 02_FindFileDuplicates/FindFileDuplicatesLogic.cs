namespace _02_FindFileDuplicates
{
    public class FindFileDuplicatesLogic : ICheckForDuplicates
    {
        private float _percentageCheckCandidates;
        private float _percentageCompileCandidates;

        public float PercentageCheckCandidates { get => _percentageCheckCandidates; }
        public float PercentageCompileCandidates { get => _percentageCompileCandidates; }

        public static List<FileInfo> GetFilesForPath(string folderpath)
            => Directory.GetFiles(folderpath, "*", SearchOption.AllDirectories)
                        .Select(f => new FileInfo(f))
                        .ToList();

        public static string GetMd5(string input)
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = System.Security.Cryptography.MD5.HashData(inputBytes);

            return Convert.ToHexString(hashBytes).ToLower();
        }

        public IEnumerable<string> CheckCandidates(IEnumerable<string> candidates)
        {
            Dictionary<string, string> candidatesWithMd5 = [];

            int counter = 0;

            foreach (string c in (candidates))
            {
                candidatesWithMd5[c] = GetMd5(string.Concat(File.ReadAllBytes(c)));

                counter++;
                _percentageCheckCandidates = counter / (float)(candidates).Count() * 100;
            }

            return candidatesWithMd5.Where(c => candidatesWithMd5.Count(cc => c.Value == cc.Value) > 1)
                                    .Select(c => c.Key)
                                    .ToList();
        }

        public IEnumerable<string> CompileCandidates(string folderpath) => CompileCandidates(folderpath, CompareModes.SizeAndName);

        public IEnumerable<string> CompileCandidates(string folderpath, CompareModes mode)
        {
            if (string.IsNullOrEmpty(folderpath) || !Directory.Exists(folderpath))
                throw new ArgumentException("Missing or invalid folderpath");

            Duplicates duplicates = new();
            List<FileInfo> fileInfos = GetFilesForPath(folderpath);
            int counter = 0;

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

                counter++;
                _percentageCompileCandidates = counter / (float)fileInfos.Count * 100;
            }

            return duplicates.Filepath;
        }
    }
}