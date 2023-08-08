using PhantomJs.NetCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ERPServer.Helpers
{
    public class PdfGenerateClass
    {
        public string generatePdf(string html, string directory, string filename)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            currentDirectory += "\\..\\..\\.."; 
            string phantomJsRootFolder = Path.Combine(currentDirectory, "PhantomJsRoot");
            // the pdf generator needs to know the path to the folder with .exe files.
            PdfGenerator generator = new PdfGenerator(phantomJsRootFolder);
            // Generate pdf from html and place in the current folder.
            string pathOftheGeneratedPdf = generator.GeneratePdf(html, directory, filename);

            return pathOftheGeneratedPdf;
        }
    }
}
