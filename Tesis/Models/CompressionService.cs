using System.IO.Compression;

namespace Tesis.Models
{
    public class CompressionService
    {
        public string CompressFolder(string folderPath)
        {
            var compressedFilePath = $"{folderPath}.zip";
            try
            {
                ZipFile.CreateFromDirectory(folderPath, compressedFilePath);
                return compressedFilePath;
            }catch(IOException ex)
            {
                if(ex.Message == $"The file '{compressedFilePath}' already exists.")
                {
                    return compressedFilePath;
                }

                throw new IOException();
            }
            
        }
    }
}
