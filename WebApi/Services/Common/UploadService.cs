using Microsoft.Extensions.Primitives;
using WebApi.ViewModels;

namespace WebApi.Services.Common
{
    public class UploadService
    {
        private static readonly string _contentFolder = "content";
        private static readonly string _templFolder = $"{_contentFolder}\\uploads\\temp";
        private static readonly string _chatUploadsFolder = $"{_contentFolder}\\uploads\\chat";
        private readonly string _filesBaseUrl;
        private readonly TokenService _tokenGen;
        private readonly IWebHostEnvironment _webHost;
        protected readonly IConfiguration _configs;
        public UploadService(TokenService tokenGen,
            IWebHostEnvironment webHost,
            IConfiguration configs)
        {
            _tokenGen = tokenGen;
            _webHost = webHost;
            _configs = configs;
            _filesBaseUrl = configs.GetValue<string>("FilesBaseUrl");
        }

        //public string GetLocalPath(string? path)
        //{
        //    if (!string.IsNullOrEmpty(path))
        //    {
        //        var localPath = Path.Combine(_webHost.ContentRootPath, Path.Combine(path.Split('/')));

        //        if (!File.Exists(localPath))
        //            throw new Exception($"File '{localPath}' not found.");
        //        return localPath;
        //    }
        //    return "";
        //}
        public string? GetFullPath(string? path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
            return _filesBaseUrl + path;
        }
        public string? GetResourceFullPath(string? path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
            return _filesBaseUrl + path;
        }
        //public string GetBase64(string? path)
        //{
        //    var localPath = GetLocalPath(path);
        //    if (string.IsNullOrEmpty(localPath)) return "";
        //    var bytes = File.ReadAllBytes(localPath);
        //    return Convert.ToBase64String(bytes);
        //}


        public async Task<IEnumerable<FileToken>> SaveFiles(List<IFormFile> files, bool skipToken = false)
        {
            if (!Directory.Exists(_templFolder))
            {
                Directory.CreateDirectory(_templFolder);
            }

            var tokens = await SaveFilesInFolder(files, _templFolder, skipToken);
            return tokens;
        }

        private async Task<List<FileToken>> SaveFilesInFolder(List<IFormFile> files, string folder, bool skipToken)
        {
            var tokens = new List<FileToken>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var ext = Path.GetExtension(formFile.FileName);
                    var fileId = Guid.NewGuid().ToString();
                    var filePath = Path.Combine(folder, fileId + ext);

                    using (var stream = File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    var relPath = "/" + filePath
                        //.Replace(_contentFolder, "")
                        .Replace('\\', '/');
                    var fullPath = GetFullPath(relPath);
                    var fileMeta = new FileModel
                    {
                        MimeType = formFile.ContentType,
                        Name = formFile.FileName,
                        RelativePath = relPath,
                        FullPath = fullPath,
                        Size = formFile.Length
                    };
                    tokens.Add(new FileToken
                    {
                        Token = skipToken ? null : _tokenGen.CreateToken(fileMeta),
                        RelativePath = relPath,
                        //Path = fullPath,
                        FullPath = fullPath,
                        Name = formFile.FileName,
                        IsAudio = fileMeta.IsAudio,
                        IsPhoto = fileMeta.IsPhoto,
                        IsVideo = fileMeta.IsVideo,
                    });
                }
            }

            return tokens;
        }

        //    public List<string> GetFileType(string fn)
        //    {
        //        var list = new List<string>();
        //        var files = Directory.EnumerateFiles("temp");
        //        foreach (var file in files)
        //        {


        //            var bytes = File.ReadAllBytes(file);

        //            using var fileStream = File.OpenRead(file);
        //            fileStream.Seek(0, SeekOrigin.Begin);
        //            //fileStream.
        //            var isRecognizableType = FileTypeValidator.IsTypeRecognizable(fileStream);
        //            Console.WriteLine("Is Bitmap?: {0}", fileStream.Is<AudioM4a>());

        //            if (!isRecognizableType)
        //            {
        //                list.Add(file + "------ not recognized");
        //                continue;
        //                // Do something ...
        //            }

        //            IFileType fileType = FileTypeValidator.GetFileType(fileStream);
        //            Console.WriteLine("Type Name: {0}", fileType.Name);
        //            Console.WriteLine("Type Extension: {0}", fileType.Extension);
        //            Console.WriteLine("Is Image?: {0}", fileStream.IsImage());
        //            list.Add(file + "------ Extension: " + fileType.Extension);
        //        }
        //        return list.ToList();
        //    }
        //}
    }
    public class FileModel
    {
        public string? MimeType { get; set; }
        public string? Name { get; set; }
        public string? RelativePath { get; set; }
        public long Size { get; set; }
        public string? FullPath { get; set; }
        public bool IsAudio =>
                       (MimeType?.StartsWith("audio/") ?? false) ||
                    (Name?.ToLower().EndsWith(".m4a") ?? false) ||
                    (Name?.ToLower().EndsWith(".aac") ?? false) ||
                    (Name?.ToLower().EndsWith(".m4r") ?? false) ||
                    (Name?.ToLower().EndsWith(".mp3") ?? false);
        public bool IsPhoto =>
                       (MimeType?.StartsWith("image/") ?? false) ||
                     (Name?.ToLower().EndsWith(".jpg") ?? false) ||
                    (Name?.ToLower().EndsWith(".jpeg") ?? false) ||
                     (Name?.ToLower().EndsWith(".png") ?? false) ||
                     (Name?.ToLower().EndsWith(".gif") ?? false);
        public bool IsVideo { get { return MimeType?.StartsWith("video/") ?? false; } }
    }

    public class FileToken
    {
        //public string? Path { get; set; }
        public string? Name { get; set; }
        public string? Token { get; set; }
        public string? RelativePath { get; set; }
        public bool IsAudio { get; set; }
        public bool IsPhoto { get; set; }
        public bool IsVideo { get; set; }
        public string? FullPath { get; set; }
    }
}
