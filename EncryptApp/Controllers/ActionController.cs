using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EncryptApp.Models;
using System.Reflection.Metadata;
using System.IO;

namespace EncryptApp.Controllers
{
    public class ActionController : Controller
    {
        private static TextLoader textLoader;
        private static EncryptingMachine machine = new EncryptingMachine();
        public IActionResult Index()
        {
            return Redirect("/Home/Index");
        }
        [HttpGet]
        public IActionResult Download()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Download(DownloadFileInfoModel model)
        {
            //Validate
            if (Path.GetExtension(model.FilePath) == ".txt" && textLoader is DocxLoader)
            {
                ModelState.AddModelError("Invalid Path", "This path is invalid: file must be .docx");
            }
            if (Path.GetExtension(model.FilePath) == ".docx" && !(textLoader is DocxLoader))
            {
                ModelState.AddModelError("Invalid Path", "This path is invalid: file must be .txt");
            }
            if (System.IO.File.Exists(model.FilePath) && !model.Overwrite)
            {
                ModelState.AddModelError("File already exist", "This file already exists");
            }
            //Show validation summary
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //Do the work
            try
            {
                textLoader.Download(model.FilePath, model.Overwrite);
            }
            catch (Exception)
            {
                ModelState.AddModelError("File wasn't downloaded", "File could't be downloaded to this path");
            }
            //Show error
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            return Redirect("/Home");
        }
        public IActionResult Result()
        {
            if (textLoader != null)
            {
                //display lines
                ViewBag.Name = textLoader.HtmlPrint();
            }
            return View();
        }
        [HttpGet]
        public IActionResult Decrypt()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Decrypt(FileInfoModel model)
        {
            //Validate
            model.Encrypt = false;
            Validate(model);
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            InitializeTextLoader(model);
            //Do the work
            textLoader.Decrypt(machine);
            //Redirect
            return Redirect("/Action/Result");
        }
        [HttpGet]
        public IActionResult Encrypt()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Encrypt(FileInfoModel model)
        {
            //Validate
            model.Encrypt = true;
            Validate(model);
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            InitializeTextLoader(model);
            //Do the work
            textLoader.Encrypt(machine);
            //Redirect
            return Redirect("/Action/Result");
        }

        //Helper functions
        private void Validate(FileInfoModel model)
        {
            if (!IsPathValid(model.FilePath))
            {
                ModelState.AddModelError("Invalid Path", "This path is invalid: file must exist, and be .txt or .docx");
            }
            if (!IsKeyValid(model.Key))
            {
                ModelState.AddModelError("Invalid Key", "Invalid Key, must have only russian letters!");
            }
        }
        private void InitializeTextLoader(FileInfoModel model)
        {
            //Path
            if (Path.GetExtension(model.FilePath) == ".txt")
            {
                textLoader = new TextLoader(model.FilePath, model.GetEncoding());
            }
            else if (Path.GetExtension(model.FilePath) == ".docx")
            {
                textLoader = new DocxLoader(model.FilePath, model.GetEncoding());
            }
            //Key
            machine.Key = model.Key;
        }
        private bool IsPathValid(string path)
        {
            if (!System.IO.File.Exists(path) || !(Path.GetExtension(path) == ".txt" || Path.GetExtension(path) == ".docx"))
            {
                return false;
            }
            return true;
        }
        private bool IsKeyValid(string key)
        {
            string alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
            if (key != null && !key.ToLower().All(alphabet.Contains))
            {
                return false;
            }
            return true;
        }
    }
}

