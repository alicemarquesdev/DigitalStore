﻿using DigitalStore.Helper;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Controllers
{
    public class LoginController : Controller
    {
        private readonly IEmail _email;
        private readonly ISessao _sessao;
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public LoginController(ISessao sessao, IUsuarioRepositorio usuarioRepositorio, IEmail email)
        {
            _sessao = sessao;
            _usuarioRepositorio = usuarioRepositorio;
            _email = email;
        }

        public IActionResult Index()
        {
            if (_sessao.BuscarSessaoDoUsuario() != null) RedirectToAction("Index", "Home");
            return View();
        }

        public async Task<IActionResult> RedefinirSenha(int id)
        {
            UsuarioModel usuario = await _usuarioRepositorio.BuscarUsuarioPorIdAsync(id);
            return View(usuario);
        }

        public IActionResult Sair()
        {
            _sessao.RemoverSessaoDoUsuario();
            return RedirectToAction("Index", "Login");
        }

        public IActionResult CriarUsuario()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CriarUsuario(UsuarioModel usuario)
        {
            if (ModelState.IsValid)
            {
                await _usuarioRepositorio.AddUsuarioAsync(usuario);
                TempData["MensagemSucesso"] = "O usuário foi criado com sucesso!";
                return RedirectToAction("Index", "Login");
            }

            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Entrar(LoginModel loginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioModel usuario = await _usuarioRepositorio.BuscarUsuarioExistenteAsync(loginModel.Email);

                    if (usuario != null)
                    {
                        if (usuario.SenhaValida(loginModel.Senha))
                        {
                            _sessao.CriarSessaoDoUsuario(usuario);
                            return RedirectToAction("Index", "Home");
                        }
                        TempData["MensagemErro"] = "Senha do usuário é inválida, tente novamente.";
                    }
                    TempData["MensagemErro"] = "Usuário não encontrado. Por favor, tente novamente.";
                }
                TempData["MensagemErro"] = "Por favor verifique os dados inseridos, tente novamente.";
                return View("Index", loginModel);
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "Usuário e/ou senha inválido(s). Por favor, tente novamente.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EnviarLinkParaRedefinirSenha(RedefinirSenhaModel redefinirSenhaModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioModel usuario = await _usuarioRepositorio.BuscarUsuarioExistenteAsync(redefinirSenhaModel.Email);

                    if (usuario != null)
                    {
                        string novaSenha = usuario.GerarNovaSenha();
                        string mensagem = $"Sua nova senha é {novaSenha}";

                        bool emailEnviado = _email.Enviar(usuario.Email, "DigitalStore - NovaSenha", mensagem);

                        if (emailEnviado)
                        {
                            await _usuarioRepositorio.AtualizarUsuarioAsync(usuario);
                            TempData["MensagemSucesso"] = $"Enviamos para seu e-mail cadastrado uma nova senha.";
                        }
                        else
                        {
                            TempData["MensagemErro"] = $"Não conseguimos enviar e-mail. Por favor, tente novamente.";
                        }
                        return RedirectToAction("Index", "Login");
                    }

                    return View("Index");
                }
                return View("Index");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos enviar o email, tente novamante, detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}