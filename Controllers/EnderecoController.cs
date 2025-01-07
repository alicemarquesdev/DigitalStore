using DigitalStore.Enums;
using DigitalStore.Helper;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Controllers
{
    public class EnderecoController : Controller
    {
        private readonly IEnderecoRepositorio _enderecoRepositorio;
        private readonly IItensDoPedidoRepositorio _itensDoPedidoRepositorio;
        private readonly IPedidoRepositorio _pedidoRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ICarrinhoRepositorio _carrinhoRepositorio;
        private readonly ISessao _sessao;

        public EnderecoController(IEnderecoRepositorio enderecoRepositorio,
                      IItensDoPedidoRepositorio itensDoPedidoRepositorio,
                      IPedidoRepositorio pedidoRepositorio,
                      IUsuarioRepositorio usuarioRepositorio,
                      ICarrinhoRepositorio carrinhoRepositorio,
                      ISessao sessao)
        {
            _enderecoRepositorio = enderecoRepositorio;
            _itensDoPedidoRepositorio = itensDoPedidoRepositorio;
            _pedidoRepositorio = pedidoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _carrinhoRepositorio = carrinhoRepositorio;
            _sessao = sessao;
        }

        public async Task<IActionResult> Endereco(int id)
        {
            //Buscar todos os enerecos do Usuario
            List<EnderecoModel> enderecos = await _enderecoRepositorio.BuscarTodosOsEnderecosDoUsuarioAsync(id);
            var usuario = await _usuarioRepositorio.BuscarUsuarioPorIdAsync(id);

            if (usuario == null)
            {
                TempData["MensagemErro"] = "Usuario não encontrado";
                return RedirectToAction("Endereco");
            }
            ViewBag.UsuarioId = id;

            return View(enderecos);
        }


        public IActionResult AddEndereco(int id)
        {
            ViewBag.UsuarioId = id;
            return View();
        }

        public async Task<IActionResult> AtualizarEndereco(int id)
        {
            var endereco = await _enderecoRepositorio.BuscarEnderecoPorIdAsync(id);

            return View(endereco);
        }

        [HttpPost]
        public async Task<IActionResult> AddEndereco(EnderecoModel endereco, int id)
        {
            try
            {
                var usuario = await _usuarioRepositorio.BuscarUsuarioPorIdAsync(id);

                if (usuario == null)
                {
                    TempData["MensagemErro"] = "Usuário não encontrado";
                    return View(endereco);

                }

                endereco.UsuarioId = id;
                Console.WriteLine($"CEP: {endereco.CEP}");
                Console.WriteLine($"Cidade: {endereco.Cidade}");
                Console.WriteLine($"Estado: {endereco.Estado}");
                Console.WriteLine($"UsuarioId: {endereco.UsuarioId}");
                Console.WriteLine($"Rua: {endereco.Rua}");
                Console.WriteLine($"Bairro: {endereco.Bairro}");

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
                    return View(endereco);
                }
                if (ModelState.IsValid)
                {
                    await _enderecoRepositorio.AddEnderecoAsync(endereco);
                    TempData["MensagemSucesso"] = "Endereco adicionado com sucesso!";
                    return RedirectToAction("Endereco", new { id = id });
                }
               
                TempData["MensagemErro"] = "As informações não estão corretas.";
                return View(endereco);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarEndereco(EnderecoModel endereco)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _enderecoRepositorio.AtualizarEnderecoAsync(endereco);
                    TempData["MensagemSucesso"] = "Endereco atualizado com sucesso!";
                    return RedirectToAction("Endereco");
                }
                TempData["MensagemErro"] = "Não conseguimos atualizar o endereço";
                return View(endereco);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoverEndereco(int id)
        {
            await _enderecoRepositorio.RemoverEnderecoAsync(id);
            return RedirectToAction("Endereco");
        }
    }
}