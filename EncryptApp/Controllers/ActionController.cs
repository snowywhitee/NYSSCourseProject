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
        private static TextLoader textLoader = null;
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
            //validate

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


            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //do the work
            textLoader.Download(model.FilePath, model.Overwrite);
            return Redirect("/Home");
        }
        public IActionResult Result()
        {
            if (textLoader != null)
            {
                //display lines
                ViewBag.Name = $"{textLoader.HtmlPrint()}";
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
            model.Encrypt = false;
            Validate(model);
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            InitializeTextLoader(model);
            textLoader.Decrypt(machine);

            //redirect
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
            model.Encrypt = true;
            Validate(model);
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            InitializeTextLoader(model);

            textLoader.Encrypt(machine);

            //redirect
            return Redirect("/Action/Result");
        }

        //Helper functions
        private void Validate(FileInfoModel model)
        {
            //validate
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
            //path
            if (Path.GetExtension(model.FilePath) == ".txt")
            {
                textLoader = new TextLoader(model.FilePath, model.GetEncoding());
            }
            else if (Path.GetExtension(model.FilePath) == ".docx")
            {
                textLoader = new DocxLoader(model.FilePath, model.GetEncoding());
            }
            //key
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

