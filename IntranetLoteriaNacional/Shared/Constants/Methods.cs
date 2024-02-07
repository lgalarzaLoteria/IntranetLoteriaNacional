using Microsoft.AspNetCore.Components.Forms;
using OfficeOpenXml;
using System.Text;
using System.Globalization;
using Newtonsoft.Json;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.FileIO;


namespace IntranetLoteriaNacional.Shared.Constants
{
    public class Methods
    {
        public static byte[] ObjToExcel<T>(List<T> data, string sheetName)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(sheetName);
                worksheet.Cells.LoadFromCollection(data, true);
                return package.GetAsByteArray();
            }
        }

        public static T[] ToClassGeneric<T>(string body)
        {
            return JsonConvert.DeserializeObject<T[]>(body)!;
        }

        
        private string[] ParseCsvLinePas1(string line)
        {
            using (var parser = new TextFieldParser(new StringReader(line)))
            {
                parser.HasFieldsEnclosedInQuotes = true;
                parser.SetDelimiters(",");

                return parser.ReadFields();
            }
        }
        private string[] ParseCsvLine(string line)
        {
            var values = new List<string>();
            var sb = new StringBuilder();
            bool inQuotes = false;

            foreach (char c in line)
            {
                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ',' && !inQuotes)
                {
                    values.Add(sb.ToString().Trim('"'));
                    sb.Clear();
                    continue;
                }

                sb.Append(c);
            }

            if (sb.Length > 0)
            {
                values.Add(sb.ToString().Trim('"'));
            }

            return values.ToArray();
        }

               
        public async Task<string> GetResizedImageBase64(IBrowserFile file, int width, int height)
        {
            const int bufferSize = 4096;
            var base64String = "";

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var stream = file.OpenReadStream())
                    {
                        await stream.CopyToAsync(memoryStream);
                    }

                    var imageBytes = memoryStream.ToArray();
                    var resizedImageBytes = ResizeImage(imageBytes, width, height);

                    base64String = Convert.ToBase64String(resizedImageBytes);
                }
            }
            catch (Exception ex)
            {
                // Manejar el error de manera adecuada
            }

            return $"data:{file.ContentType};base64,{base64String}";
        }

        private byte[] ResizeImage(byte[] imageBytes, int width, int height)
        {
            using (var originalImageStream = new MemoryStream(imageBytes))
            using (var originalImage = System.Drawing.Image.FromStream(originalImageStream))
            {
                // Calcular nuevas dimensiones manteniendo la proporción original
                int newWidth, newHeight;
                if (originalImage.Width > originalImage.Height)
                {
                    newWidth = width;
                    newHeight = (int)(originalImage.Height * (float)width / originalImage.Width);
                }
                else
                {
                    newWidth = (int)(originalImage.Width * (float)height / originalImage.Height);
                    newHeight = height;
                }

                var resizedImage = new System.Drawing.Bitmap(newWidth, newHeight);
                using (var graphics = System.Drawing.Graphics.FromImage(resizedImage))
                {
                    graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                }

                using (var resizedImageStream = new MemoryStream())
                {
                    resizedImage.Save(resizedImageStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    return resizedImageStream.ToArray();
                }
            }
        }


         public async Task<string> ConvertToBase64(IBrowserFile selectedFile)
        {
            if (selectedFile != null)
            {
                byte[] fileBytes;
                using (var memoryStream = new MemoryStream())
                {
                    await selectedFile.OpenReadStream().CopyToAsync(memoryStream);
                    fileBytes = memoryStream.ToArray();
                }

                return Convert.ToBase64String(fileBytes);
            }
            return "";
        }

        public string GetMimeTypeFromExtension(string extension)
        {
            if (extension.StartsWith("."))
            {
                extension = extension.TrimStart('.');
            }

            switch (extension.ToLower())
            {
                case "pdf":
                    return "application/pdf";
                case "xls":
                case "xlsx":
                    return "application/vnd.ms-excel";
                case "csv":
                    return "text/csv";
                case "jpg":
                case "jpeg":
                    return "image/jpeg";
                case "png":
                    return "image/png";
                // Agrega más casos según las extensiones de archivo que deseas manejar
                default:
                    return "application/octet-stream";
            }
        }

    }

}
