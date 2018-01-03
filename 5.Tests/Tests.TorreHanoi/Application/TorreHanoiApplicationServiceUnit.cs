using System;
using System.Collections.Generic;
using System.Net;
using Application.TorreHanoi.Implementation;
using Application.TorreHanoi.Interface;
using Domain.TorreHanoi.Interface.Service;
using Infrastructure.TorreHanoi.ImagemHelper;
using Infrastructure.TorreHanoi.Log;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Tests.TorreHanoi.Application
{
    [TestClass]
    public class TorreHanoiApplicationServiceUnit
    {
        private const string CategoriaTeste = "Application/Service/TorreHanoi";

        private ITorreHanoiApplicationService _service;

        private Mock<ILogger> mockLogger;
        private Mock<IDesignerService> mockDesignerService;
        private Mock<ITorreHanoiDomainService> mockTorreHanoiDomainService;

        [TestInitialize]
        public void SetUp()
        {
            mockLogger = new Mock<ILogger>();
            mockLogger.Setup(s => s.Logar(It.IsAny<string>(), It.IsAny<TipoLog>()));

            mockDesignerService = new Mock<IDesignerService>();
            mockDesignerService.Setup(x => x.Desenhar()).Returns(new System.Drawing.Bitmap(900, 600));

            mockTorreHanoiDomainService = new Mock<ITorreHanoiDomainService>();
            mockTorreHanoiDomainService.Setup(s => s.Criar(It.IsAny<int>())).Returns(Guid.NewGuid);
            mockTorreHanoiDomainService.Setup(s => s.ObterPor(It.IsAny<Guid>())).Returns(() => new global::Domain.TorreHanoi.TorreHanoi(3, mockLogger.Object));
            mockTorreHanoiDomainService.Setup(s => s.ObterTodos()).Returns(() => new List<global::Domain.TorreHanoi.TorreHanoi> { new global::Domain.TorreHanoi.TorreHanoi(3, mockLogger.Object) });

            _service = new TorreHanoiApplicationService(mockTorreHanoiDomainService.Object, mockLogger.Object, mockDesignerService.Object);
        }

        [TestMethod]
        [TestCategory(CategoriaTeste)]
        public void AdicionarNovoProcesso_Deve_Retornar_Sucesso()
        {
            var response = _service.AdicionarNovoPorcesso(3);

            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Accepted);
            Assert.AreNotEqual(response.IdProcesso, new Guid());
            Assert.IsTrue(response.IsValid);
            Assert.IsTrue(response.MensagensDeErro.Count == 0);
        }

        [TestMethod]
        [TestCategory(CategoriaTeste)]
        public void AdicionarNovoProcesso_Deve_Retornar_Falha_Caso_Numero_De_Discos_For_Inferior_A_Um()
        {
            var response = _service.AdicionarNovoPorcesso(0);

            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
            Assert.IsFalse(response.IsValid);
            Assert.IsTrue(response.MensagensDeErro.Count == 1);
        }

        [TestMethod]
        [TestCategory(CategoriaTeste)]
        public void ObterProcessoPor_Deverar_Retornar_Sucesso()
        {
            var response = _service.ObterProcessoPor(Guid.NewGuid().ToString());

            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(response.Processo);
            Assert.IsTrue(response.IsValid);
            Assert.IsTrue(response.MensagensDeErro.Count == 0);
        }

        [TestMethod]
        [TestCategory(CategoriaTeste)]
        public void ObterTodosProcessos_Deverar_Retornar_Sucesso()
        {
            var response = _service.ObterTodosProcessos();

            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(response.Processos);
            Assert.IsTrue(response.Processos.Count > 0);
            Assert.IsTrue(response.IsValid);
            Assert.IsTrue(response.MensagensDeErro.Count == 0);
        }

        [TestMethod]
        [TestCategory(CategoriaTeste)]
        public void ObterImagemProcessoPor_Deve_Retornar_BadRequest_Caso_Id_Possua_Formato_Invalido()
        {
            var id = "1";

            var response = _service.ObterImagemProcessoPor(id);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        [TestCategory(CategoriaTeste)]
        public void ObterImagemProcessoPor_Deve_Retornar_Imagem()
        {
            var id = Guid.NewGuid().ToString();

            var response = _service.ObterImagemProcessoPor(id);

            Assert.IsNotNull(response.Imagem);
            Assert.AreEqual(900, response.Imagem.Width);
            Assert.AreEqual(600, response.Imagem.Height);
        }

        [TestMethod]
        [TestCategory(CategoriaTeste)]
        public void ObterImagemProcessoPor_Deve_Processar_Desenhar()
        {
            var id = Guid.NewGuid().ToString();

            var response = _service.ObterImagemProcessoPor(id);

            mockDesignerService.Verify(x => x.Desenhar());

        }

    }
}
