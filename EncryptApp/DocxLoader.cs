using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace EncryptApp
{
    public class DocxLoader : TextLoader
    {
        public DocxLoader(string path, Encoding encoding) : base(path, encoding)
        {
        }
        
        //Methods
        public override void Download(string newPath, bool overwrite)
        {
            HandleOverwriting(newPath, overwrite);

            //Create
            using (WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Create(newPath, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordprocessingDocument.AddMainDocumentPart();
                new Document(new Body()).Save(mainPart);

                //Assign ref
                Body body = mainPart.Document.Body;

                //Add text
                var paragraphs = PrintParagraphs();
                for (int i = 0; i < paragraphs.Count; i++)
                {
                    body.Append(new Paragraph(new Run(new Text(paragraphs[i]))));
                }

                //Save the document
                mainPart.Document.Save();
            }
        }
        public override void Load()
        {
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(path, false))
            {
                OpenXmlElement element = wordDocument.MainDocumentPart.Document.Body;
                text = GetPlainText(element).ToCharArray();
            }
        }
        
        //Helper methods
        private List<string> PrintParagraphs()
        {
            List<string> paragraphs = new List<string>();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '\n')
                {
                    paragraphs.Add(sb.ToString());
                    sb.Clear();
                }
                sb.Append(text[i]);
            }
            return paragraphs;
        }
        private string GetPlainText(OpenXmlElement element)
        {
            StringBuilder text = new StringBuilder();
            foreach (OpenXmlElement section in element.Elements())
            {
                switch (section.LocalName)
                {
                    case "t":
                        text.Append(section.InnerText);
                        break;
                    case "br": //Page break
                        text.Append(Environment.NewLine);
                        break;
                    case "tab":
                        text.Append("\t");
                        break;
                    case "p":
                        text.Append(GetPlainText(section));
                        text.Append(Environment.NewLine);
                        break;
                    default:
                        text.Append(GetPlainText(section));
                        break;
                }
            }
            return text.ToString();
        }
    }
}
