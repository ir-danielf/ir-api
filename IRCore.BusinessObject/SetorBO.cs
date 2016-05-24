using IRCore.BusinessObject.Enumerator;
using IRCore.BusinessObject.Estrutura;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using IRCore.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace IRCore.BusinessObject
{
    public class SetorBO : MasterBO<SetorADO>
    {
        public SetorBO(MasterADOBase ado) : base(ado) { }

        public SetorBO() : base(null) { }

        public List<Setor> ListarVoucher(Voucher voucher, int apresentacaoID)
        {
            return ado.ListarVoucher(voucher.ParceiroMidiaID, apresentacaoID, voucher.ParceiroMidiaClasse.Nivel);
        }

        public List<Setor> ListarParceiro(ParceiroMidia parceiro, int apresentacaoID)
        {
            return ado.ListarIn(parceiro.ApresentacaoSetorIDs.Where(t => t.ApresentacaoID == apresentacaoID).Select(t => t.IR_SetorID).ToList(), apresentacaoID);
        }

        public Setor Consultar(int setorId, int apresentacaoId)
        {
            Setor setor = ado.Consultar(setorId, apresentacaoId);
            return setor;
        }

        public Image ConsultarMapaLugar(int setorId)
        {
            string caminho = ConfiguracaoAppUtil.Get(enumConfiguracaoBO.setorFundoUpload);
            string path = caminho + "s" + setorId.ToString("000000") + ".gif";
            return Image.FromFile(path);
        }

        public List<Setor> consultarEvento(int apresentacaoid, string date)
        {
            return ado.consultarEvento(apresentacaoid, date);
        }

        public List<Setor> Listar(int idApresentacao, int eventoID = 0, int idCanal = 2, bool comCotaNominal = true, bool comCotaPromcional = true, bool canalPOS = false, string sessionId = null, bool mostrarEstatistica = false)
        {
            LogUtil.Debug(string.Format("##Get.Listar## SESSION {0}, CANALID {1}, DATA: {2}, MSG: Início do método de listar setores", sessionId, idCanal, DateTime.Now));
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var result = ado.Listar(idApresentacao, eventoID, idCanal, comCotaNominal, comCotaPromcional, canalPOS);
            if (result != null && mostrarEstatistica)
            {
                result.ForEach(r =>
                    r.Estatistica = ado.ConsultarEstatisticaSetor(eventoID, idApresentacao, r.IR_SetorID)
                );
            }

            stopwatch.Stop();

            LogUtil.Debug(string.Format("##Get.Listar## SESSION {0}, TEMPO DECORRIDO: {1}, DATA: {2}, MSG: Fim do método de listar setores", sessionId, stopwatch.Elapsed, DateTime.Now));

            return result;
        }

        public List<Setor> ListarOSESP(int idApresentacao, bool comCotaNominal = true, bool comCotaPromcional = true, string osespVIEW = "")
        {
            var result = ado.ListarOSESP(idApresentacao, comCotaNominal, comCotaPromcional);
            return result;

        }
    }
}