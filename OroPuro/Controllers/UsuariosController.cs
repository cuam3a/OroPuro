using OroPuro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.Entity;
using System.Activities.Expressions;

namespace OroPuro.Controllers
{
    public class UsuariosController : Controller
    {
        // GET: Usuarios
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["nombre"] != null)
            {
                return RedirectToAction("Index", "Usuarios");
            }
            //else
            //{
            //    return RedirectToAction("AdminPanel", "Usuarios");
            //}
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LogInModel user)
        {
            if (ModelState.IsValid) //Verificar que el modelo de datos sea válido en cuanto a la definición de las propiedades
            {
                if (Isvalid(user.usuario, user.password))//Verificar que el email y clave exista utilizando el método privado
                {
                    using (var db = new UserEntities())
                    {
                        var nomUsr = db.Usuarios.FirstOrDefault(u => u.usuarioUsr == user.usuario && u.passUsr == user.password); //consultar el primer registro con los el email del usuario
                        if (nomUsr != null)
                        {
                            Session["nombre"] = nomUsr.nomUsr;
                            Session["tipo"] = nomUsr.tipoUsr;
                        }
                    }
                    //FormsAuthentication.SetAuthCookie(user.usuario, false); //crea variable de usuario con el nombre de usuario
                    return RedirectToAction("AdminPanel", "Usuarios"); //dirigir al controlador home vista Index una vez se a autenticado en el sistema
                }
                else
                {
                    ModelState.AddModelError("", "Usuario o Contraseña Incorrecta"); //adicionar mensaje de error al model
                }
            }
            return View(user);
        }

        public ActionResult AdminPanel()
        {
            if (Session["nombre"] == null)
            {
                return RedirectToAction("Index", "Usuarios");
            }
            return View();
        }

        public ActionResult ListaUsuarios()
        {
            if (Session["nombre"] == null)
            {
                return RedirectToAction("Index", "Usuarios");
            }
            if (Session["tipo"].ToString() == "Administrador")
            {
                UserEntities db = new UserEntities();
                return View(db.Usuarios.ToList());
            }
            return RedirectToAction("AdminPanel", "Usuarios");
        }

        public ActionResult AgregarUsuarios()
        {
            if (Session["nombre"] == null)
            {
                return RedirectToAction("Index", "Usuarios");
            }
            if (Session["tipo"].ToString() == "Administrador")
            {
                Usuarios model = new Usuarios();
                return View(model);
            }
            return RedirectToAction("AdminPanel", "Usuarios");
        }

        [HttpPost]
        public ActionResult AgregarUsuarios(Usuarios usuario)
        {
            if (Session["nombre"] == null)
            {
                return RedirectToAction("Index", "Usuarios");
            }
            if (Session["tipo"].ToString() == "Administrador")
            {
                try
                {
                    UserEntities db = new UserEntities();
                    var nomUsr = db.Usuarios.FirstOrDefault(u => u.usuarioUsr == usuario.usuarioUsr); //consultar el primer registro con los el email del usuario
                    if (nomUsr == null)
                    {
                        db.Usuarios.Add(usuario);
                        db.SaveChanges();
                        db.Dispose();
                        return RedirectToAction("ListaUsuarios", "Usuarios");
                    }
                    else
                    {
                        db.Dispose();
                        ViewBag.error = "El usuario ya existe";
                        return View(usuario);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    ViewBag.Error = "Error en Registro, verifique que todos los campos se encuentren correctos";
                    return View(usuario);
                }
            }
            return RedirectToAction("AdminPanel", "Usuarios");
        }

        public ActionResult EditarUsuarios(int? id)
        {
            if (Session["nombre"] == null)
            {
                return RedirectToAction("Index", "Usuarios");
            }
            else
            {
                if (Session["tipo"].ToString() == "Administrador")
                {
                    try
                    {
                        UserEntities db = new UserEntities();
                        Usuarios model = db.Usuarios.Find(id);
                        if (model == null)
                        {
                            ViewBag.error = "Usuario Incorrecto";
                            db.Dispose();
                            return RedirectToAction("ListaUsuarios", "Usuarios");
                        }
                        else
                        {
                            db.Dispose();
                            return View(model);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        ViewBag.Error = "Error, No se puede editar usuario";
                        return RedirectToAction("ListaUsuarios", "Usuarios");
                    }
                }
                return RedirectToAction("AdminPanel", "Usuarios");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarUsuarios(Usuarios model)
        {
            if (Session["nombre"] == null)
            {
                return RedirectToAction("Index", "Usuarios");
            }
            else
            {
                if (Session["tipo"].ToString() == "Administrador")
                {
                    try
                    {
                        if (ModelState.IsValid)
                        {
                            UserEntities db = new UserEntities();
                            db.Entry(model).State = EntityState.Modified;
                            db.SaveChanges();
                            db.Dispose();
                            return RedirectToAction("ListaUsuarios", "Usuarios");
                        }
                        else
                        {
                            ViewBag.Error = "Error, Verifique que todos los datos esten correctos";
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        ViewBag.Error = "Error, No se puede editar usuario";
                        return View(model);
                    }
                    return View(model);
                }
                return RedirectToAction("AdminPanel", "Usuarios");
            }
        }

        public ActionResult EliminarUsuarios(int id)
        {
            if (Session["nombre"] == null)
            {
                return RedirectToAction("Index", "Usuarios");
            }
            if (Session["tipo"].ToString() == "Administrador")
            {
                try
                {
                    UserEntities db = new UserEntities();
                    var usr = db.Usuarios.Where(x => x.idUsr == id).FirstOrDefault();
                    db.Usuarios.Attach(usr);
                    db.Usuarios.Remove(usr);
                    db.SaveChanges();

                    return RedirectToAction("ListaUsuarios", "Usuarios");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    return RedirectToAction("ListaUsuarios", "Usuarios");
                }
            }
            return RedirectToAction("AdminPanel", "Usuarios");
        }

        public ActionResult Salir()
        {
            Session.Clear();
            Session.RemoveAll();
            return RedirectToAction("Index", "Usuarios");
        }
        //Metodo para validar usuario
        private bool Isvalid(string usuario, string password)
        {
            bool Isvalid = false;
            using (var db = new UserEntities())
            {
                var user = db.Usuarios.FirstOrDefault(u => u.usuarioUsr == usuario); //consultar el primer registro con los el email del usuario
                if (user != null)
                {
                    if (user.passUsr == password) //Verificar password del usuario
                    {
                        Isvalid = true;
                    }
                }
            }
            return Isvalid;
        }
    }
}