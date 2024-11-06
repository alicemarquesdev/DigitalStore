using DigitalStore.Models;
using DigitalStore.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public UsuarioController(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }
        public IActionResult Index(UsuarioModel usuario)
        {
            _usuarioRepositorio.ListarPorId(usuario.Id);
            return View("Index", usuario);
        }

        public IActionResult CriarConta()
        {
            return View("CriarConta");
        }


        public IActionResult Editar(int id)
        {
            UsuarioModel usuario = _usuarioRepositorio.ListarPorId(id);
            return View(usuario);
        }

        public IActionResult RedefinirSenha(int id)
        {
            _usuarioRepositorio.ListarPorId(id);
            return View("RedefinirSenha");
        }

        public IActionResult ApagarConta()
        {
            return View("ApagarConta");
        }

       


        [HttpPost]
        public IActionResult Criar(UsuarioModel usuario)
        {
            if(ModelState.IsValid)
            {
                _usuarioRepositorio.Criar(usuario);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Editar(UsuarioSemSenhaModel usuario)
        {
            try
            {
                //UsuarioModel usuario = null;

                if (ModelState.IsValid)
                {
                   
                }
            }
            catch (Exception)
            {
                throw new Exception();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Entrar(UsuarioModel usuario)
        {
            try
            {
                if (ModelState.IsValid)

                {
                    //  UsuarioModel usuario = _usuarioRepositorio.BuscarPorEmail(LoginModel.Email);
                   // return RedirectToAction("Index", "Usuario");
                }
            }
            catch (Exception )
            {
                throw new Exception();
            }
            return RedirectToAction("Index");
        }

       



    }
}
