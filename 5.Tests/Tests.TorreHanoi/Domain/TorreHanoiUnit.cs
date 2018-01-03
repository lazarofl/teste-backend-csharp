using Infrastructure.TorreHanoi.Log;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Tests.TorreHanoi.Domain
{
    [TestClass]
    public class TorreHanoiUnit
    {
        private const string CategoriaTeste = "Domain/TorreHanoi";

        private Mock<ILogger> _mockLogger;

        [TestInitialize]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogger>();
            _mockLogger.Setup(s => s.Logar(It.IsAny<string>(), It.IsAny<TipoLog>()));
        }

        [TestMethod]
        [TestCategory(CategoriaTeste)]
        public void Construtor_Deve_Retornar_StatusPendente()
        {
            var torreHanoi = new global::Domain.TorreHanoi.TorreHanoi(3, _mockLogger.Object);

            Assert.AreEqual(global::Domain.TorreHanoi.TipoStatus.Pendente, torreHanoi.Status);
        }

        [TestMethod]
        [TestCategory(CategoriaTeste)]
        public void Construtor_Deve_Gerar_Id_Para_A_TorreDeHanoi()
        {
            var torreHanoi = new global::Domain.TorreHanoi.TorreHanoi(3, _mockLogger.Object);
            Assert.AreNotEqual(new Guid(), torreHanoi.Id);
        }

        [TestMethod]
        [TestCategory(CategoriaTeste)]
        public void Processar_Deverar_Retornar_Sucesso()
        {
            var torreHanoi = new global::Domain.TorreHanoi.TorreHanoi(3, _mockLogger.Object);

            torreHanoi.Processar();

            Assert.AreEqual(global::Domain.TorreHanoi.TipoStatus.FinalizadoSucesso, torreHanoi.Status);
        }

        [TestMethod]
        [TestCategory(CategoriaTeste)]
        public void Processar_Deverar_Retornar_Torre_Com_Um_Disco_No_Destino()
        {
            var totalDeDiscos = 1;
            var torreHanoi = new global::Domain.TorreHanoi.TorreHanoi(totalDeDiscos, _mockLogger.Object);

            torreHanoi.Processar();

            Assert.AreEqual(totalDeDiscos, torreHanoi.Destino.Discos.Count);
        }

        [TestMethod]
        [TestCategory(CategoriaTeste)]
        public void Processar_Deverar_Retornar_Torre_Com_Dois_Discos_No_Destino()
        {
            var totalDeDiscos = 2;
            var torreHanoi = new global::Domain.TorreHanoi.TorreHanoi(totalDeDiscos, _mockLogger.Object);

            torreHanoi.Processar();

            Assert.AreEqual(totalDeDiscos, torreHanoi.Destino.Discos.Count);
        }

        [TestMethod]
        [TestCategory(CategoriaTeste)]
        public void Processar_Deverar_Retornar_Torre_Com_Tres_Discos_No_Destino()
        {
            var totalDeDiscos = 3;
            var torreHanoi = new global::Domain.TorreHanoi.TorreHanoi(totalDeDiscos, _mockLogger.Object);

            torreHanoi.Processar();

            Assert.AreEqual(totalDeDiscos, torreHanoi.Destino.Discos.Count);
        }
    }
}
