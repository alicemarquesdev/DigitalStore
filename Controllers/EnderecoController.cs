using DigitalStore.Helper;
using DigitalStore.Models;
using DigitalStore.Models.ViewModels;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Controllers
{
    public class EnderecoController : Controller
    {
        private readonly IEnderecoRepositorio _enderecoRepositorio;
       
        public EnderecoController(IEnderecoRepositorio enderecoRepositorio)
        {
            _enderecoRepositorio = enderecoRepositorio;
           
        }

        [HttpPost]
        public async Task<IActionResult> AddEndereco(EnderecoModel endereco)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    foreach (var key in ModelState.Keys)
                    {
                        foreach (var error in ModelState[key].Errors)
                        {
                            Console.WriteLine($"Campo: {key}, Erro: {error.ErrorMessage}");
                            TempData["MensagemErro"] += $"Campo: {key}, Erro: {error.ErrorMessage}<br>";
                        }
                    }
                    return RedirectToAction("Endereco", new { id = endereco.UsuarioId });
                }
                if (ModelState.IsValid)
                {
                    var enderecosLimite = await _enderecoRepositorio.BuscarTodosOsEnderecosDoUsuarioAsync(endereco.UsuarioId);

                    if (enderecosLimite.Count >= 4)
                    {
                        TempData["MensagemErro"] = "Você só pode ter no máximo 4 endereços cadastrados";
                        return RedirectToAction("Endereco", new { id = endereco.UsuarioId });
                    }

                    await _enderecoRepositorio.AddEnderecoAsync(endereco);
                    TempData["MensagemSucesso"] = "Endereco adicionado com sucesso!";
                    return RedirectToAction("Endereco", new { id = endereco.UsuarioId });
                }

                TempData["MensagemErro"] = "As informações não estão corretas.";
                return RedirectToAction("Carrinho", new { id = endereco.UsuarioId });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoverEndereco(int id)
        {
            var endereco = await _enderecoRepositorio.BuscarEnderecoPorIdAsync(id);

            if (endereco == null)
            {
                TempData["MensagemErro"] = "Endereço não encontrado";
                return RedirectToAction("Index", "Home");
            }

            await _enderecoRepositorio.RemoverEnderecoAsync(id);
            return RedirectToAction("Endereco");
        }
    }
}