using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Biblioteca.Models;
using System.Linq;
using System.Collections.Generic;


namespace Biblioteca.Controllers
{
    public class Autenticacao
    {
        public static void CheckLogin(Controller controller)
        {   
            if(string.IsNullOrEmpty(controller.HttpContext.Session.GetString("user")))
            {
                controller.Request.HttpContext.Response.Redirect("/Home/Login");
            }
        }
        public static bool verificaLoginSenha(string login, string senha,Controller controller)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
        {
            verificaSeUsuarioAdminExiste(bc);

            senha = Criptografo.TextoCriptografado(senha);

            IQueryable<Usuario> UsuarioEncontrado = bc.usuarios.Where(u => u.login==login && u.senha==senha);
            List<Usuario>ListaUsuarioEncontrado = UsuarioEncontrado.ToList();

            if(ListaUsuarioEncontrado.Count==0)
            {
                return false;
            }
            else{
                controller.HttpContext.Session.SetString("login",ListaUsuarioEncontrado[0].login);
                controller.HttpContext.Session.SetString("Nome",ListaUsuarioEncontrado[0].Nome);
                controller.HttpContext.Session.SetInt32("tipo",ListaUsuarioEncontrado[0].tipo);
                return true;
            }
        }
    } 
        public static void verificaSeUsuarioAdminExiste(BibliotecaContext bc)
        {
            IQueryable<Usuario> userEncontrado = bc.usuarios.Where(u => u.login=="admin");

            if (userEncontrado.ToList().Count==0)
            {
               Usuario admin = new Usuario();
               admin.login = "admin";
               admin.senha = Criptografo.TextoCriptografado("123");
               admin.tipo = Usuario.ADMIN;
               admin.Nome = "Administrator";

               bc.usuarios.Add(admin);
               bc.SaveChanges();

            }
        }
    
        public static void verificaSeUsuarioEAdmin(Controller controller)
        {
            if(!(controller.HttpContext.Session.GetInt32("tipo")==Usuario.ADMIN))
            {
                controller.Request.HttpContext.Response.Redirect("/Usuario/NeedAdmin");
            }
        }
    
    }
}
