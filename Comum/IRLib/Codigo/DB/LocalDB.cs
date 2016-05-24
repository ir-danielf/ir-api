/******************************************************
* Arquivo LocalDB.cs
* Gerado em: 06/06/2014
* Autor: Celeritas Ltda
*******************************************************/

using System;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;
using CTLib;

namespace IRLib
{

    #region "Local_B"

    public abstract class Local_B : BaseBD
    {

        public empresaid EmpresaID = new empresaid();
        public contratoid ContratoID = new contratoid();
        public nome Nome = new nome();
        public contato Contato = new contato();
        public logradouro Logradouro = new logradouro();
        public cidade Cidade = new cidade();
        public estado Estado = new estado();
        public cep CEP = new cep();
        public dddtelefone DDDTelefone = new dddtelefone();
        public telefone Telefone = new telefone();
        public bairro Bairro = new bairro();
        public numero Numero = new numero();
        public estacionamento Estacionamento = new estacionamento();
        public estacionamentoobs EstacionamentoObs = new estacionamentoobs();
        public acessonecessidadeespecial AcessoNecessidadeEspecial = new acessonecessidadeespecial();
        public acessonecessidadeespecialobs AcessoNecessidadeEspecialObs = new acessonecessidadeespecialobs();
        public arcondicionado ArCondicionado = new arcondicionado();
        public comochegar ComoChegar = new comochegar();
        public complemento Complemento = new complemento();
        public comochegarinternet ComoChegarInternet = new comochegarinternet();
        public horariosbilheteria HorariosBilheteria = new horariosbilheteria();
        public retiradabilheteria RetiradaBilheteria = new retiradabilheteria();
        public paisid PaisID = new paisid();
        public imageminternet ImagemInternet = new imageminternet();
        public codigopraca CodigoPraca = new codigopraca();
        public latitude Latitude = new latitude();
        public longitude Longitude = new longitude();
        public alvara Alvara = new alvara();
        public avcb AVCB = new avcb();
        public dataemissaoalvara DataEmissaoAlvara = new dataemissaoalvara();
        public datavalidadealvara DataValidadeAlvara = new datavalidadealvara();
        public dataemissaoavcb DataEmissaoAvcb = new dataemissaoavcb();
        public datavalidadeavcb DataValidadeAvcb = new datavalidadeavcb();
        public lotacao Lotacao = new lotacao();
        public fonteimposto FonteImposto = new fonteimposto();
        public porcentagemimposto PorcentagemImposto = new porcentagemimposto();
        public ativo Ativo = new ativo();

        public Local_B() { }

        // passar o Usuario logado no sistema
        public Local_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de Local
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {

                string sql = "SELECT * FROM tLocal WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EmpresaID.ValorBD = bd.LerInt("EmpresaID").ToString();
                    this.ContratoID.ValorBD = bd.LerInt("ContratoID").ToString();
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.Contato.ValorBD = bd.LerString("Contato");
                    this.Logradouro.ValorBD = bd.LerString("Logradouro");
                    this.Cidade.ValorBD = bd.LerString("Cidade");
                    this.Estado.ValorBD = bd.LerString("Estado");
                    this.CEP.ValorBD = bd.LerString("CEP");
                    this.DDDTelefone.ValorBD = bd.LerString("DDDTelefone");
                    this.Telefone.ValorBD = bd.LerString("Telefone");
                    this.Bairro.ValorBD = bd.LerString("Bairro");
                    this.Numero.ValorBD = bd.LerInt("Numero").ToString();
                    this.Estacionamento.ValorBD = bd.LerString("Estacionamento");
                    this.EstacionamentoObs.ValorBD = bd.LerString("EstacionamentoObs");
                    this.AcessoNecessidadeEspecial.ValorBD = bd.LerString("AcessoNecessidadeEspecial");
                    this.AcessoNecessidadeEspecialObs.ValorBD = bd.LerString("AcessoNecessidadeEspecialObs");
                    this.ArCondicionado.ValorBD = bd.LerString("ArCondicionado");
                    this.ComoChegar.ValorBD = bd.LerString("ComoChegar");
                    this.Complemento.ValorBD = bd.LerString("Complemento");
                    this.ComoChegarInternet.ValorBD = bd.LerString("ComoChegarInternet");
                    this.HorariosBilheteria.ValorBD = bd.LerString("HorariosBilheteria");
                    this.RetiradaBilheteria.ValorBD = bd.LerString("RetiradaBilheteria");
                    this.PaisID.ValorBD = bd.LerInt("PaisID").ToString();
                    this.ImagemInternet.ValorBD = bd.LerString("ImagemInternet");
                    this.CodigoPraca.ValorBD = bd.LerString("CodigoPraca");
                    this.Latitude.ValorBD = bd.LerString("Latitude");
                    this.Longitude.ValorBD = bd.LerString("Longitude");
                    this.Alvara.ValorBD = bd.LerString("Alvara");
                    this.AVCB.ValorBD = bd.LerString("AVCB");
                    this.DataEmissaoAlvara.ValorBD = bd.LerString("DataEmissaoAlvara");
                    this.DataValidadeAlvara.ValorBD = bd.LerString("DataValidadeAlvara");
                    this.DataEmissaoAvcb.ValorBD = bd.LerString("DataEmissaoAvcb");
                    this.DataValidadeAvcb.ValorBD = bd.LerString("DataValidadeAvcb");
                    this.Lotacao.ValorBD = bd.LerInt("Lotacao").ToString();
                    this.FonteImposto.ValorBD = bd.LerString("FonteImposto");
                    this.PorcentagemImposto.ValorBD = bd.LerDecimal("PorcentagemImposto").ToString();
                    this.Ativo.ValorBD = bd.LerString("Ativo");
                }
                else
                {
                    this.Limpar();
                }
                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Preenche todos os atributos de Local do backup
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public void LerBackup(int id)
        {

            try
            {

                string sql = "SELECT * FROM xLocal WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.EmpresaID.ValorBD = bd.LerInt("EmpresaID").ToString();
                    this.ContratoID.ValorBD = bd.LerInt("ContratoID").ToString();
                    this.Nome.ValorBD = bd.LerString("Nome");
                    this.Contato.ValorBD = bd.LerString("Contato");
                    this.Logradouro.ValorBD = bd.LerString("Logradouro");
                    this.Cidade.ValorBD = bd.LerString("Cidade");
                    this.Estado.ValorBD = bd.LerString("Estado");
                    this.CEP.ValorBD = bd.LerString("CEP");
                    this.DDDTelefone.ValorBD = bd.LerString("DDDTelefone");
                    this.Telefone.ValorBD = bd.LerString("Telefone");
                    this.Bairro.ValorBD = bd.LerString("Bairro");
                    this.Numero.ValorBD = bd.LerInt("Numero").ToString();
                    this.Estacionamento.ValorBD = bd.LerString("Estacionamento");
                    this.EstacionamentoObs.ValorBD = bd.LerString("EstacionamentoObs");
                    this.AcessoNecessidadeEspecial.ValorBD = bd.LerString("AcessoNecessidadeEspecial");
                    this.AcessoNecessidadeEspecialObs.ValorBD = bd.LerString("AcessoNecessidadeEspecialObs");
                    this.ArCondicionado.ValorBD = bd.LerString("ArCondicionado");
                    this.ComoChegar.ValorBD = bd.LerString("ComoChegar");
                    this.Complemento.ValorBD = bd.LerString("Complemento");
                    this.ComoChegarInternet.ValorBD = bd.LerString("ComoChegarInternet");
                    this.HorariosBilheteria.ValorBD = bd.LerString("HorariosBilheteria");
                    this.RetiradaBilheteria.ValorBD = bd.LerString("RetiradaBilheteria");
                    this.PaisID.ValorBD = bd.LerInt("PaisID").ToString();
                    this.ImagemInternet.ValorBD = bd.LerString("ImagemInternet");
                    this.CodigoPraca.ValorBD = bd.LerString("CodigoPraca");
                    this.Latitude.ValorBD = bd.LerString("Latitude");
                    this.Longitude.ValorBD = bd.LerString("Longitude");
                    this.Alvara.ValorBD = bd.LerString("Alvara");
                    this.AVCB.ValorBD = bd.LerString("AVCB");
                    this.DataEmissaoAlvara.ValorBD = bd.LerString("DataEmissaoAlvara");
                    this.DataValidadeAlvara.ValorBD = bd.LerString("DataValidadeAlvara");
                    this.DataEmissaoAvcb.ValorBD = bd.LerString("DataEmissaoAvcb");
                    this.DataValidadeAvcb.ValorBD = bd.LerString("DataValidadeAvcb");
                    this.Lotacao.ValorBD = bd.LerInt("Lotacao").ToString();
                    this.FonteImposto.ValorBD = bd.LerString("FonteImposto");
                    this.PorcentagemImposto.ValorBD = bd.LerDecimal("PorcentagemImposto").ToString();
                    this.Ativo.ValorBD = bd.LerString("Ativo");
                }
                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void InserirControle(string acao)
        {

            try
            {

                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cLocal (ID, Versao, Acao, TimeStamp, UsuarioID) ");
                sql.Append("VALUES (@ID,@V,'@A','@TS',@U)");
                sql.Replace("@ID", this.Control.ID.ToString());

                if (!acao.Equals("I"))
                    this.Control.Versao++;

                sql.Replace("@V", this.Control.Versao.ToString());
                sql.Replace("@A", acao);
                sql.Replace("@TS", DateTime.Now.ToString("yyyyMMddHHmmssffff"));
                sql.Replace("@U", this.Control.UsuarioID.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void InserirLog()
        {

            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO xLocal (ID, Versao, EmpresaID, ContratoID, Nome, Contato, Logradouro, Cidade, Estado, CEP, DDDTelefone, Telefone, Bairro, Numero, Estacionamento, EstacionamentoObs, AcessoNecessidadeEspecial, AcessoNecessidadeEspecialObs, ArCondicionado, ComoChegar, Complemento, ComoChegarInternet, HorariosBilheteria, RetiradaBilheteria, PaisID, ImagemInternet, CodigoPraca, Latitude, Longitude, Alvara, AVCB, DataEmissaoAlvara, DataValidadeAlvara, DataEmissaoAvcb, DataValidadeAvcb, Lotacao, FonteImposto, PorcentagemImposto, Ativo) ");
                sql.Append("SELECT ID, @V, EmpresaID, ContratoID, Nome, Contato, Logradouro, Cidade, Estado, CEP, DDDTelefone, Telefone, Bairro, Numero, Estacionamento, EstacionamentoObs, AcessoNecessidadeEspecial, AcessoNecessidadeEspecialObs, ArCondicionado, ComoChegar, Complemento, ComoChegarInternet, HorariosBilheteria, RetiradaBilheteria, PaisID, ImagemInternet, CodigoPraca, Latitude, Longitude, Alvara, AVCB, DataEmissaoAlvara, DataValidadeAlvara, DataEmissaoAvcb, DataValidadeAvcb, Lotacao, FonteImposto, PorcentagemImposto, Ativo FROM tLocal WHERE ID = @I");
                sql.Replace("@I", this.Control.ID.ToString());
                sql.Replace("@V", this.Control.Versao.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Inserir novo(a) Local
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                bd.IniciarTransacao();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cLocal");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tLocal(ID, EmpresaID, ContratoID, Nome, Contato, Logradouro, Cidade, Estado, CEP, DDDTelefone, Telefone, Bairro, Numero, Estacionamento, EstacionamentoObs, AcessoNecessidadeEspecial, AcessoNecessidadeEspecialObs, ArCondicionado, ComoChegar, Complemento, ComoChegarInternet, HorariosBilheteria, RetiradaBilheteria, PaisID, ImagemInternet, CodigoPraca, Latitude, Longitude, Alvara, AVCB, DataEmissaoAlvara, DataValidadeAlvara, DataEmissaoAvcb, DataValidadeAvcb, Lotacao, FonteImposto, PorcentagemImposto, Ativo) ");
                sql.Append("VALUES (@ID,@001,@002,'@003','@004','@005','@006','@007','@008','@009','@010','@011',@012,'@013','@014','@015','@016','@017','@018','@019','@020','@021','@022',@023,'@024','@025','@026','@027','@028','@029','@030','@031','@032','@033',@034,'@035','@036', '@037')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EmpresaID.ValorBD);
                sql.Replace("@002", this.ContratoID.ValorBD);
                sql.Replace("@003", this.Nome.ValorBD);
                sql.Replace("@004", this.Contato.ValorBD);
                sql.Replace("@005", this.Logradouro.ValorBD);
                sql.Replace("@006", this.Cidade.ValorBD);
                sql.Replace("@007", this.Estado.ValorBD);
                sql.Replace("@008", this.CEP.ValorBD);
                sql.Replace("@009", this.DDDTelefone.ValorBD);
                sql.Replace("@010", this.Telefone.ValorBD);
                sql.Replace("@011", this.Bairro.ValorBD);
                sql.Replace("@012", this.Numero.ValorBD);
                sql.Replace("@013", this.Estacionamento.ValorBD);
                sql.Replace("@014", this.EstacionamentoObs.ValorBD);
                sql.Replace("@015", this.AcessoNecessidadeEspecial.ValorBD);
                sql.Replace("@016", this.AcessoNecessidadeEspecialObs.ValorBD);
                sql.Replace("@017", this.ArCondicionado.ValorBD);
                sql.Replace("@018", this.ComoChegar.ValorBD);
                sql.Replace("@019", this.Complemento.ValorBD);
                sql.Replace("@020", this.ComoChegarInternet.ValorBD);
                sql.Replace("@021", this.HorariosBilheteria.ValorBD);
                sql.Replace("@022", this.RetiradaBilheteria.ValorBD);
                sql.Replace("@023", this.PaisID.ValorBD);
                sql.Replace("@024", this.ImagemInternet.ValorBD);
                sql.Replace("@025", this.CodigoPraca.ValorBD);
                sql.Replace("@026", this.Latitude.ValorBD);
                sql.Replace("@027", this.Longitude.ValorBD);
                sql.Replace("@028", this.Alvara.ValorBD);
                sql.Replace("@029", this.AVCB.ValorBD);
                sql.Replace("@030", this.DataEmissaoAlvara.ValorBD);
                sql.Replace("@031", this.DataValidadeAlvara.ValorBD);
                sql.Replace("@032", this.DataEmissaoAvcb.ValorBD);
                sql.Replace("@033", this.DataValidadeAvcb.ValorBD);
                sql.Replace("@034", this.Lotacao.ValorBD);
                sql.Replace("@035", this.FonteImposto.ValorBD);
                sql.Replace("@036", this.PorcentagemImposto.ValorBD);
                sql.Replace("@037", this.Ativo.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                if (result)
                    InserirControle("I");

                bd.FinalizarTransacao();

                return result;

            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }

        /// <summary>
        /// Inserir novo(a) Local
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cLocal");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tLocal(ID, EmpresaID, ContratoID, Nome, Contato, Logradouro, Cidade, Estado, CEP, DDDTelefone, Telefone, Bairro, Numero, Estacionamento, EstacionamentoObs, AcessoNecessidadeEspecial, AcessoNecessidadeEspecialObs, ArCondicionado, ComoChegar, Complemento, ComoChegarInternet, HorariosBilheteria, RetiradaBilheteria, PaisID, ImagemInternet, CodigoPraca, Latitude, Longitude, Alvara, AVCB, DataEmissaoAlvara, DataValidadeAlvara, DataEmissaoAvcb, DataValidadeAvcb, Lotacao, FonteImposto, PorcentagemImposto, Ativo) ");
                sql.Append("VALUES (@ID,@001,@002,'@003','@004','@005','@006','@007','@008','@009','@010','@011',@012,'@013','@014','@015','@016','@017','@018','@019','@020','@021','@022',@023,'@024','@025','@026','@027','@028','@029','@030','@031','@032','@033',@034,'@035','@036','@037')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EmpresaID.ValorBD);
                sql.Replace("@002", this.ContratoID.ValorBD);
                sql.Replace("@003", this.Nome.ValorBD);
                sql.Replace("@004", this.Contato.ValorBD);
                sql.Replace("@005", this.Logradouro.ValorBD);
                sql.Replace("@006", this.Cidade.ValorBD);
                sql.Replace("@007", this.Estado.ValorBD);
                sql.Replace("@008", this.CEP.ValorBD);
                sql.Replace("@009", this.DDDTelefone.ValorBD);
                sql.Replace("@010", this.Telefone.ValorBD);
                sql.Replace("@011", this.Bairro.ValorBD);
                sql.Replace("@012", this.Numero.ValorBD);
                sql.Replace("@013", this.Estacionamento.ValorBD);
                sql.Replace("@014", this.EstacionamentoObs.ValorBD);
                sql.Replace("@015", this.AcessoNecessidadeEspecial.ValorBD);
                sql.Replace("@016", this.AcessoNecessidadeEspecialObs.ValorBD);
                sql.Replace("@017", this.ArCondicionado.ValorBD);
                sql.Replace("@018", this.ComoChegar.ValorBD);
                sql.Replace("@019", this.Complemento.ValorBD);
                sql.Replace("@020", this.ComoChegarInternet.ValorBD);
                sql.Replace("@021", this.HorariosBilheteria.ValorBD);
                sql.Replace("@022", this.RetiradaBilheteria.ValorBD);
                sql.Replace("@023", this.PaisID.ValorBD);
                sql.Replace("@024", this.ImagemInternet.ValorBD);
                sql.Replace("@025", this.CodigoPraca.ValorBD);
                sql.Replace("@026", this.Latitude.ValorBD);
                sql.Replace("@027", this.Longitude.ValorBD);
                sql.Replace("@028", this.Alvara.ValorBD);
                sql.Replace("@029", this.AVCB.ValorBD);
                sql.Replace("@030", this.DataEmissaoAlvara.ValorBD);
                sql.Replace("@031", this.DataValidadeAlvara.ValorBD);
                sql.Replace("@032", this.DataEmissaoAvcb.ValorBD);
                sql.Replace("@033", this.DataValidadeAvcb.ValorBD);
                sql.Replace("@034", this.Lotacao.ValorBD);
                sql.Replace("@035", this.FonteImposto.ValorBD);
                sql.Replace("@036", this.PorcentagemImposto.ValorBD);
                sql.Replace("@037", this.Ativo.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                if (result)
                    InserirControle("I");

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Atualiza Local
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                bd.IniciarTransacao();

                string sqlVersion = "SELECT MAX(Versao) FROM cLocal WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tLocal SET EmpresaID = @001, ContratoID = @002, Nome = '@003', Contato = '@004', Logradouro = '@005', Cidade = '@006', Estado = '@007', CEP = '@008', DDDTelefone = '@009', Telefone = '@010', Bairro = '@011', Numero = @012, Estacionamento = '@013', EstacionamentoObs = '@014', AcessoNecessidadeEspecial = '@015', AcessoNecessidadeEspecialObs = '@016', ArCondicionado = '@017', ComoChegar = '@018', Complemento = '@019', ComoChegarInternet = '@020', HorariosBilheteria = '@021', RetiradaBilheteria = '@022', PaisID = @023, ImagemInternet = '@024', CodigoPraca = '@025', Latitude = '@026', Longitude = '@027', Alvara = '@028', AVCB = '@029', DataEmissaoAlvara = '@030', DataValidadeAlvara = '@031', DataEmissaoAvcb = '@032', DataValidadeAvcb = '@033', Lotacao = @034, FonteImposto = '@035', PorcentagemImposto = '@036', Ativo = '@037' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EmpresaID.ValorBD);
                sql.Replace("@002", this.ContratoID.ValorBD);
                sql.Replace("@003", this.Nome.ValorBD);
                sql.Replace("@004", this.Contato.ValorBD);
                sql.Replace("@005", this.Logradouro.ValorBD);
                sql.Replace("@006", this.Cidade.ValorBD);
                sql.Replace("@007", this.Estado.ValorBD);
                sql.Replace("@008", this.CEP.ValorBD);
                sql.Replace("@009", this.DDDTelefone.ValorBD);
                sql.Replace("@010", this.Telefone.ValorBD);
                sql.Replace("@011", this.Bairro.ValorBD);
                sql.Replace("@012", this.Numero.ValorBD);
                sql.Replace("@013", this.Estacionamento.ValorBD);
                sql.Replace("@014", this.EstacionamentoObs.ValorBD);
                sql.Replace("@015", this.AcessoNecessidadeEspecial.ValorBD);
                sql.Replace("@016", this.AcessoNecessidadeEspecialObs.ValorBD);
                sql.Replace("@017", this.ArCondicionado.ValorBD);
                sql.Replace("@018", this.ComoChegar.ValorBD);
                sql.Replace("@019", this.Complemento.ValorBD);
                sql.Replace("@020", this.ComoChegarInternet.ValorBD);
                sql.Replace("@021", this.HorariosBilheteria.ValorBD);
                sql.Replace("@022", this.RetiradaBilheteria.ValorBD);
                sql.Replace("@023", this.PaisID.ValorBD);
                sql.Replace("@024", this.ImagemInternet.ValorBD);
                sql.Replace("@025", this.CodigoPraca.ValorBD);
                sql.Replace("@026", this.Latitude.ValorBD);
                sql.Replace("@027", this.Longitude.ValorBD);
                sql.Replace("@028", this.Alvara.ValorBD);
                sql.Replace("@029", this.AVCB.ValorBD);
                sql.Replace("@030", this.DataEmissaoAlvara.ValorBD);
                sql.Replace("@031", this.DataValidadeAlvara.ValorBD);
                sql.Replace("@032", this.DataEmissaoAvcb.ValorBD);
                sql.Replace("@033", this.DataValidadeAvcb.ValorBD);
                sql.Replace("@034", this.Lotacao.ValorBD);
                sql.Replace("@035", this.FonteImposto.ValorBD);
                sql.Replace("@036", this.PorcentagemImposto.ValorBD);
                sql.Replace("@037", this.Ativo.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                bd.FinalizarTransacao();

                return result;

            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }

        /// <summary>
        /// Atualiza Local
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cLocal WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tLocal SET EmpresaID = @001, ContratoID = @002, Nome = '@003', Contato = '@004', Logradouro = '@005', Cidade = '@006', Estado = '@007', CEP = '@008', DDDTelefone = '@009', Telefone = '@010', Bairro = '@011', Numero = @012, Estacionamento = '@013', EstacionamentoObs = '@014', AcessoNecessidadeEspecial = '@015', AcessoNecessidadeEspecialObs = '@016', ArCondicionado = '@017', ComoChegar = '@018', Complemento = '@019', ComoChegarInternet = '@020', HorariosBilheteria = '@021', RetiradaBilheteria = '@022', PaisID = @023, ImagemInternet = '@024', CodigoPraca = '@025', Latitude = '@026', Longitude = '@027', Alvara = '@028', AVCB = '@029', DataEmissaoAlvara = '@030', DataValidadeAlvara = '@031', DataEmissaoAvcb = '@032', DataValidadeAvcb = '@033', Lotacao = @034, FonteImposto = '@035', PorcentagemImposto = '@036', Ativo = '@037' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EmpresaID.ValorBD);
                sql.Replace("@002", this.ContratoID.ValorBD);
                sql.Replace("@003", this.Nome.ValorBD);
                sql.Replace("@004", this.Contato.ValorBD);
                sql.Replace("@005", this.Logradouro.ValorBD);
                sql.Replace("@006", this.Cidade.ValorBD);
                sql.Replace("@007", this.Estado.ValorBD);
                sql.Replace("@008", this.CEP.ValorBD);
                sql.Replace("@009", this.DDDTelefone.ValorBD);
                sql.Replace("@010", this.Telefone.ValorBD);
                sql.Replace("@011", this.Bairro.ValorBD);
                sql.Replace("@012", this.Numero.ValorBD);
                sql.Replace("@013", this.Estacionamento.ValorBD);
                sql.Replace("@014", this.EstacionamentoObs.ValorBD);
                sql.Replace("@015", this.AcessoNecessidadeEspecial.ValorBD);
                sql.Replace("@016", this.AcessoNecessidadeEspecialObs.ValorBD);
                sql.Replace("@017", this.ArCondicionado.ValorBD);
                sql.Replace("@018", this.ComoChegar.ValorBD);
                sql.Replace("@019", this.Complemento.ValorBD);
                sql.Replace("@020", this.ComoChegarInternet.ValorBD);
                sql.Replace("@021", this.HorariosBilheteria.ValorBD);
                sql.Replace("@022", this.RetiradaBilheteria.ValorBD);
                sql.Replace("@023", this.PaisID.ValorBD);
                sql.Replace("@024", this.ImagemInternet.ValorBD);
                sql.Replace("@025", this.CodigoPraca.ValorBD);
                sql.Replace("@026", this.Latitude.ValorBD);
                sql.Replace("@027", this.Longitude.ValorBD);
                sql.Replace("@028", this.Alvara.ValorBD);
                sql.Replace("@029", this.AVCB.ValorBD);
                sql.Replace("@030", this.DataEmissaoAlvara.ValorBD);
                sql.Replace("@031", this.DataValidadeAlvara.ValorBD);
                sql.Replace("@032", this.DataEmissaoAvcb.ValorBD);
                sql.Replace("@033", this.DataValidadeAvcb.ValorBD);
                sql.Replace("@034", this.Lotacao.ValorBD);
                sql.Replace("@035", this.FonteImposto.ValorBD);
                sql.Replace("@036", this.PorcentagemImposto.ValorBD);
                sql.Replace("@037", this.Ativo.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Exclui Local com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                bd.IniciarTransacao();

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cLocal WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tLocal WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);

                bool result = (x == 1);

                bd.FinalizarTransacao();

                return result;

            }
            catch (Exception ex)
            {
                bd.DesfazerTransacao();
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }

        }

        /// <summary>
        /// Exclui Local com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cLocal WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tLocal WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Exclui Local
        /// </summary>
        /// <returns></returns>		
        public override bool Excluir()
        {

            try
            {
                return this.Excluir(this.Control.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public override void Limpar()
        {

            this.EmpresaID.Limpar();
            this.ContratoID.Limpar();
            this.Nome.Limpar();
            this.Contato.Limpar();
            this.Logradouro.Limpar();
            this.Cidade.Limpar();
            this.Estado.Limpar();
            this.CEP.Limpar();
            this.DDDTelefone.Limpar();
            this.Telefone.Limpar();
            this.Bairro.Limpar();
            this.Numero.Limpar();
            this.Estacionamento.Limpar();
            this.EstacionamentoObs.Limpar();
            this.AcessoNecessidadeEspecial.Limpar();
            this.AcessoNecessidadeEspecialObs.Limpar();
            this.ArCondicionado.Limpar();
            this.ComoChegar.Limpar();
            this.Complemento.Limpar();
            this.ComoChegarInternet.Limpar();
            this.HorariosBilheteria.Limpar();
            this.RetiradaBilheteria.Limpar();
            this.PaisID.Limpar();
            this.ImagemInternet.Limpar();
            this.CodigoPraca.Limpar();
            this.Latitude.Limpar();
            this.Longitude.Limpar();
            this.Alvara.Limpar();
            this.AVCB.Limpar();
            this.DataEmissaoAlvara.Limpar();
            this.DataValidadeAlvara.Limpar();
            this.DataEmissaoAvcb.Limpar();
            this.DataValidadeAvcb.Limpar();
            this.Lotacao.Limpar();
            this.FonteImposto.Limpar();
            this.PorcentagemImposto.Limpar();
            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {

            this.Control.Desfazer();
            this.EmpresaID.Desfazer();
            this.ContratoID.Desfazer();
            this.Nome.Desfazer();
            this.Contato.Desfazer();
            this.Logradouro.Desfazer();
            this.Cidade.Desfazer();
            this.Estado.Desfazer();
            this.CEP.Desfazer();
            this.DDDTelefone.Desfazer();
            this.Telefone.Desfazer();
            this.Bairro.Desfazer();
            this.Numero.Desfazer();
            this.Estacionamento.Desfazer();
            this.EstacionamentoObs.Desfazer();
            this.AcessoNecessidadeEspecial.Desfazer();
            this.AcessoNecessidadeEspecialObs.Desfazer();
            this.ArCondicionado.Desfazer();
            this.ComoChegar.Desfazer();
            this.Complemento.Desfazer();
            this.ComoChegarInternet.Desfazer();
            this.HorariosBilheteria.Desfazer();
            this.RetiradaBilheteria.Desfazer();
            this.PaisID.Desfazer();
            this.ImagemInternet.Desfazer();
            this.CodigoPraca.Desfazer();
            this.Latitude.Desfazer();
            this.Longitude.Desfazer();
            this.Alvara.Desfazer();
            this.AVCB.Desfazer();
            this.DataEmissaoAlvara.Desfazer();
            this.DataValidadeAlvara.Desfazer();
            this.DataEmissaoAvcb.Desfazer();
            this.DataValidadeAvcb.Desfazer();
            this.Lotacao.Desfazer();
            this.FonteImposto.Desfazer();
            this.PorcentagemImposto.Desfazer();
        }

        public class empresaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "EmpresaID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class contratoid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "ContratoID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class nome : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Nome";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 50;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class contato : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Contato";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 50;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class logradouro : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Logradouro";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 100;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class cidade : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Cidade";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 50;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class estado : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Estado";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 2;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class cep : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CEP";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 8;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class dddtelefone : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "DDDTelefone";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 2;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class telefone : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Telefone";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 8;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class bairro : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Bairro";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 70;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class numero : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Numero";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 5;
                }
            }

            public override int Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class estacionamento : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "Estacionamento";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override bool Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class estacionamentoobs : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "EstacionamentoObs";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 400;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class acessonecessidadeespecial : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "AcessoNecessidadeEspecial";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override bool Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class acessonecessidadeespecialobs : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "AcessoNecessidadeEspecialObs";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 100;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class arcondicionado : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "ArCondicionado";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override bool Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class comochegar : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "ComoChegar";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 800;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class complemento : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Complemento";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 20;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class comochegarinternet : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "ComoChegarInternet";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 800;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class horariosbilheteria : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "HorariosBilheteria";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1000;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class retiradabilheteria : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "RetiradaBilheteria";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override bool Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class paisid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "PaisID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class imageminternet : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "ImagemInternet";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 30;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class codigopraca : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "CodigoPraca";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 20;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class latitude : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Latitude";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 50;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class longitude : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Longitude";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 50;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class alvara : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Alvara";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 20;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class avcb : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "AVCB";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 20;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class dataemissaoalvara : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataEmissaoAlvara";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override DateTime Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString("dd/MM/yyyy HH:mm");
            }

        }

        public class datavalidadealvara : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataValidadeAlvara";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override DateTime Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString("dd/MM/yyyy HH:mm");
            }

        }

        public class dataemissaoavcb : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataEmissaoAvcb";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override DateTime Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString("dd/MM/yyyy HH:mm");
            }

        }

        public class datavalidadeavcb : DateTimeProperty
        {

            public override string Nome
            {
                get
                {
                    return "DataValidadeAvcb";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override DateTime Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString("dd/MM/yyyy HH:mm");
            }

        }

        public class lotacao : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "Lotacao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        public class fonteimposto : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "FonteImposto";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 20;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }

        public class porcentagemimposto : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "PorcentagemImposto";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 3;
                }
            }

            public override decimal Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString("###,##0.00");
            }

        }

        public class ativo : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Ativo";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }

        /// <summary>
        /// Obtem uma tabela estruturada com todos os campos dessa classe.
        /// </summary>
        /// <returns></returns>
        public static DataTable Estrutura()
        {

            //Isso eh util para desacoplamento.
            //A Tabela fica vazia e usamos ela para associar a uma tela com baixo nivel de acoplamento.

            try
            {

                DataTable tabela = new DataTable("Local");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EmpresaID", typeof(int));
                tabela.Columns.Add("ContratoID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Contato", typeof(string));
                tabela.Columns.Add("Logradouro", typeof(string));
                tabela.Columns.Add("Cidade", typeof(string));
                tabela.Columns.Add("Estado", typeof(string));
                tabela.Columns.Add("CEP", typeof(string));
                tabela.Columns.Add("DDDTelefone", typeof(string));
                tabela.Columns.Add("Telefone", typeof(string));
                tabela.Columns.Add("Bairro", typeof(string));
                tabela.Columns.Add("Numero", typeof(int));
                tabela.Columns.Add("Estacionamento", typeof(bool));
                tabela.Columns.Add("EstacionamentoObs", typeof(string));
                tabela.Columns.Add("AcessoNecessidadeEspecial", typeof(bool));
                tabela.Columns.Add("AcessoNecessidadeEspecialObs", typeof(string));
                tabela.Columns.Add("ArCondicionado", typeof(bool));
                tabela.Columns.Add("ComoChegar", typeof(string));
                tabela.Columns.Add("Complemento", typeof(string));
                tabela.Columns.Add("ComoChegarInternet", typeof(string));
                tabela.Columns.Add("HorariosBilheteria", typeof(string));
                tabela.Columns.Add("RetiradaBilheteria", typeof(bool));
                tabela.Columns.Add("PaisID", typeof(int));
                tabela.Columns.Add("ImagemInternet", typeof(string));
                tabela.Columns.Add("CodigoPraca", typeof(string));
                tabela.Columns.Add("Latitude", typeof(string));
                tabela.Columns.Add("Longitude", typeof(string));
                tabela.Columns.Add("Alvara", typeof(string));
                tabela.Columns.Add("AVCB", typeof(string));
                tabela.Columns.Add("DataEmissaoAlvara", typeof(DateTime));
                tabela.Columns.Add("DataValidadeAlvara", typeof(DateTime));
                tabela.Columns.Add("DataEmissaoAvcb", typeof(DateTime));
                tabela.Columns.Add("DataValidadeAvcb", typeof(DateTime));
                tabela.Columns.Add("Lotacao", typeof(int));
                tabela.Columns.Add("FonteImposto", typeof(string));
                tabela.Columns.Add("PorcentagemImposto", typeof(decimal));
                tabela.Columns.Add("Ativo", typeof(decimal));

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public abstract DataTable Todos();

        public abstract DataTable SetoresLugarMarcado();

        public abstract DataTable Setores();

        public abstract DataTable Eventos();

        public abstract DataTable Estoques();

        public abstract DataTable Bloqueios();

        public abstract DataTable Pacotes();

        public abstract DataTable Cortesias();

        public abstract DataTable Lojas(string registrozero);

    }
    #endregion

    #region "LocalLista_B"

    public abstract class LocalLista_B : BaseLista
    {

        private bool backup = false;
        protected Local local;

        // passar o Usuario logado no sistema
        public LocalLista_B()
        {
            local = new Local();
        }

        // passar o Usuario logado no sistema
        public LocalLista_B(int usuarioIDLogado)
        {
            local = new Local(usuarioIDLogado);
        }

        public Local Local
        {
            get { return local; }
        }

        /// <summary>
        /// Retorna um IBaseBD de Local especifico
        /// </summary>
        public override IBaseBD this[int indice]
        {
            get
            {
                if (indice < 0 || indice >= lista.Count)
                {
                    return null;
                }
                else
                {
                    int id = (int)lista[indice];
                    local.Ler(id);
                    return local;
                }
            }
        }

        /// <summary>
        /// Carrega a lista
        /// </summary>
        /// <param name="tamanhoMax">Informe o tamanho maximo que a lista pode ter</param>
        /// <returns></returns>		
        public void Carregar(int tamanhoMax)
        {

            try
            {

                string sql;

                if (tamanhoMax == 0)
                    sql = "SELECT ID FROM tLocal";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tLocal";

                if (FiltroSQL != null && FiltroSQL.Trim() != "")
                    sql += " WHERE " + FiltroSQL.Trim();

                if (OrdemSQL != null && OrdemSQL.Trim() != "")
                    sql += " ORDER BY " + OrdemSQL.Trim();

                lista.Clear();

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                    lista.Add(bd.LerInt("ID"));

                lista.TrimToSize();

                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Carrega a lista
        /// </summary>
        public override void Carregar()
        {

            try
            {

                string sql;

                if (tamanhoMax == 0)
                    sql = "SELECT ID FROM tLocal";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tLocal";

                if (FiltroSQL != null && FiltroSQL.Trim() != "")
                    sql += " WHERE " + FiltroSQL.Trim();

                if (OrdemSQL != null && OrdemSQL.Trim() != "")
                    sql += " ORDER BY " + OrdemSQL.Trim();

                lista.Clear();

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                    lista.Add(bd.LerInt("ID"));

                lista.TrimToSize();

                bd.Fechar();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Carrega a lista pela tabela x (de backup)
        /// </summary>
        public void CarregarBackup()
        {

            try
            {

                string sql;

                if (tamanhoMax == 0)
                    sql = "SELECT ID FROM xLocal";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM xLocal";

                if (FiltroSQL != null && FiltroSQL.Trim() != "")
                    sql += " WHERE " + FiltroSQL.Trim();

                if (OrdemSQL != null && OrdemSQL.Trim() != "")
                    sql += " ORDER BY " + OrdemSQL.Trim();

                lista.Clear();

                bd.Consulta(sql);

                while (bd.Consulta().Read())
                    lista.Add(bd.LerInt("ID"));

                lista.TrimToSize();

                bd.Fechar();

                backup = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Preenche Local corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                if (!backup)
                    local.Ler(id);
                else
                    local.LerBackup(id);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Exclui o item corrente da lista
        /// </summary>
        /// <returns></returns>
        public override bool Excluir()
        {

            try
            {

                bool ok = local.Excluir();
                if (ok)
                    lista.RemoveAt(Indice);

                return ok;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Exclui todos os itens da lista carregada
        /// </summary>
        /// <returns></returns>
        public override bool ExcluirTudo()
        {

            try
            {
                if (lista.Count == 0)
                    Carregar();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            try
            {

                bool ok = false;

                if (lista.Count > 0)
                { //verifica se tem itens

                    Ultimo();
                    //fazer varredura de traz pra frente.
                    do
                        ok = Excluir();
                    while (ok && Anterior());

                }
                else
                { //nao tem itens na lista
                    //Devolve true como se os itens ja tivessem sido excluidos, com a premissa dos ids existirem de fato.
                    ok = true;
                }

                return ok;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Inseri novo(a) Local na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = local.Inserir();
                if (ok)
                {
                    lista.Add(local.Control.ID);
                    Indice = lista.Count - 1;
                }

                return ok;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        ///  Obtem uma tabela de todos os campos de Local carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("Local");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("EmpresaID", typeof(int));
                tabela.Columns.Add("ContratoID", typeof(int));
                tabela.Columns.Add("Nome", typeof(string));
                tabela.Columns.Add("Contato", typeof(string));
                tabela.Columns.Add("Logradouro", typeof(string));
                tabela.Columns.Add("Cidade", typeof(string));
                tabela.Columns.Add("Estado", typeof(string));
                tabela.Columns.Add("CEP", typeof(string));
                tabela.Columns.Add("DDDTelefone", typeof(string));
                tabela.Columns.Add("Telefone", typeof(string));
                tabela.Columns.Add("Bairro", typeof(string));
                tabela.Columns.Add("Numero", typeof(int));
                tabela.Columns.Add("Estacionamento", typeof(bool));
                tabela.Columns.Add("EstacionamentoObs", typeof(string));
                tabela.Columns.Add("AcessoNecessidadeEspecial", typeof(bool));
                tabela.Columns.Add("AcessoNecessidadeEspecialObs", typeof(string));
                tabela.Columns.Add("ArCondicionado", typeof(bool));
                tabela.Columns.Add("ComoChegar", typeof(string));
                tabela.Columns.Add("Complemento", typeof(string));
                tabela.Columns.Add("ComoChegarInternet", typeof(string));
                tabela.Columns.Add("HorariosBilheteria", typeof(string));
                tabela.Columns.Add("RetiradaBilheteria", typeof(bool));
                tabela.Columns.Add("PaisID", typeof(int));
                tabela.Columns.Add("ImagemInternet", typeof(string));
                tabela.Columns.Add("CodigoPraca", typeof(string));
                tabela.Columns.Add("Latitude", typeof(string));
                tabela.Columns.Add("Longitude", typeof(string));
                tabela.Columns.Add("Alvara", typeof(string));
                tabela.Columns.Add("AVCB", typeof(string));
                tabela.Columns.Add("DataEmissaoAlvara", typeof(DateTime));
                tabela.Columns.Add("DataValidadeAlvara", typeof(DateTime));
                tabela.Columns.Add("DataEmissaoAvcb", typeof(DateTime));
                tabela.Columns.Add("DataValidadeAvcb", typeof(DateTime));
                tabela.Columns.Add("Lotacao", typeof(int));
                tabela.Columns.Add("FonteImposto", typeof(string));
                tabela.Columns.Add("PorcentagemImposto", typeof(decimal));
                tabela.Columns.Add("Ativo", typeof(string));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = local.Control.ID;
                        linha["EmpresaID"] = local.EmpresaID.Valor;
                        linha["ContratoID"] = local.ContratoID.Valor;
                        linha["Nome"] = local.Nome.Valor;
                        linha["Contato"] = local.Contato.Valor;
                        linha["Logradouro"] = local.Logradouro.Valor;
                        linha["Cidade"] = local.Cidade.Valor;
                        linha["Estado"] = local.Estado.Valor;
                        linha["CEP"] = local.CEP.Valor;
                        linha["DDDTelefone"] = local.DDDTelefone.Valor;
                        linha["Telefone"] = local.Telefone.Valor;
                        linha["Bairro"] = local.Bairro.Valor;
                        linha["Numero"] = local.Numero.Valor;
                        linha["Estacionamento"] = local.Estacionamento.Valor;
                        linha["EstacionamentoObs"] = local.EstacionamentoObs.Valor;
                        linha["AcessoNecessidadeEspecial"] = local.AcessoNecessidadeEspecial.Valor;
                        linha["AcessoNecessidadeEspecialObs"] = local.AcessoNecessidadeEspecialObs.Valor;
                        linha["ArCondicionado"] = local.ArCondicionado.Valor;
                        linha["ComoChegar"] = local.ComoChegar.Valor;
                        linha["Complemento"] = local.Complemento.Valor;
                        linha["ComoChegarInternet"] = local.ComoChegarInternet.Valor;
                        linha["HorariosBilheteria"] = local.HorariosBilheteria.Valor;
                        linha["RetiradaBilheteria"] = local.RetiradaBilheteria.Valor;
                        linha["PaisID"] = local.PaisID.Valor;
                        linha["ImagemInternet"] = local.ImagemInternet.Valor;
                        linha["CodigoPraca"] = local.CodigoPraca.Valor;
                        linha["Latitude"] = local.Latitude.Valor;
                        linha["Longitude"] = local.Longitude.Valor;
                        linha["Alvara"] = local.Alvara.Valor;
                        linha["AVCB"] = local.AVCB.Valor;
                        linha["DataEmissaoAlvara"] = local.DataEmissaoAlvara.Valor;
                        linha["DataValidadeAlvara"] = local.DataValidadeAlvara.Valor;
                        linha["DataEmissaoAvcb"] = local.DataEmissaoAvcb.Valor;
                        linha["DataValidadeAvcb"] = local.DataValidadeAvcb.Valor;
                        linha["Lotacao"] = local.Lotacao.Valor;
                        linha["FonteImposto"] = local.FonteImposto.Valor;
                        linha["PorcentagemImposto"] = local.PorcentagemImposto.Valor;
                        linha["Ativo"] = local.Ativo.Valor;
                        tabela.Rows.Add(linha);
                    } while (this.Proximo());

                }

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Obtem uma tabela a ser jogada num relatorio
        /// </summary>
        /// <returns></returns>
        public override DataTable Relatorio()
        {

            try
            {

                DataTable tabela = new DataTable("RelatorioLocal");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("EmpresaID", typeof(int));
                    tabela.Columns.Add("ContratoID", typeof(int));
                    tabela.Columns.Add("Nome", typeof(string));
                    tabela.Columns.Add("Contato", typeof(string));
                    tabela.Columns.Add("Logradouro", typeof(string));
                    tabela.Columns.Add("Cidade", typeof(string));
                    tabela.Columns.Add("Estado", typeof(string));
                    tabela.Columns.Add("CEP", typeof(string));
                    tabela.Columns.Add("DDDTelefone", typeof(string));
                    tabela.Columns.Add("Telefone", typeof(string));
                    tabela.Columns.Add("Bairro", typeof(string));
                    tabela.Columns.Add("Numero", typeof(int));
                    tabela.Columns.Add("Estacionamento", typeof(bool));
                    tabela.Columns.Add("EstacionamentoObs", typeof(string));
                    tabela.Columns.Add("AcessoNecessidadeEspecial", typeof(bool));
                    tabela.Columns.Add("AcessoNecessidadeEspecialObs", typeof(string));
                    tabela.Columns.Add("ArCondicionado", typeof(bool));
                    tabela.Columns.Add("ComoChegar", typeof(string));
                    tabela.Columns.Add("Complemento", typeof(string));
                    tabela.Columns.Add("ComoChegarInternet", typeof(string));
                    tabela.Columns.Add("HorariosBilheteria", typeof(string));
                    tabela.Columns.Add("RetiradaBilheteria", typeof(bool));
                    tabela.Columns.Add("PaisID", typeof(int));
                    tabela.Columns.Add("ImagemInternet", typeof(string));
                    tabela.Columns.Add("CodigoPraca", typeof(string));
                    tabela.Columns.Add("Latitude", typeof(string));
                    tabela.Columns.Add("Longitude", typeof(string));
                    tabela.Columns.Add("Alvara", typeof(string));
                    tabela.Columns.Add("AVCB", typeof(string));
                    tabela.Columns.Add("DataEmissaoAlvara", typeof(DateTime));
                    tabela.Columns.Add("DataValidadeAlvara", typeof(DateTime));
                    tabela.Columns.Add("DataEmissaoAvcb", typeof(DateTime));
                    tabela.Columns.Add("DataValidadeAvcb", typeof(DateTime));
                    tabela.Columns.Add("Lotacao", typeof(int));
                    tabela.Columns.Add("FonteImposto", typeof(string));
                    tabela.Columns.Add("PorcentagemImposto", typeof(decimal));
                    tabela.Columns.Add("Ativo", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["EmpresaID"] = local.EmpresaID.Valor;
                        linha["ContratoID"] = local.ContratoID.Valor;
                        linha["Nome"] = local.Nome.Valor;
                        linha["Contato"] = local.Contato.Valor;
                        linha["Logradouro"] = local.Logradouro.Valor;
                        linha["Cidade"] = local.Cidade.Valor;
                        linha["Estado"] = local.Estado.Valor;
                        linha["CEP"] = local.CEP.Valor;
                        linha["DDDTelefone"] = local.DDDTelefone.Valor;
                        linha["Telefone"] = local.Telefone.Valor;
                        linha["Bairro"] = local.Bairro.Valor;
                        linha["Numero"] = local.Numero.Valor;
                        linha["Estacionamento"] = local.Estacionamento.Valor;
                        linha["EstacionamentoObs"] = local.EstacionamentoObs.Valor;
                        linha["AcessoNecessidadeEspecial"] = local.AcessoNecessidadeEspecial.Valor;
                        linha["AcessoNecessidadeEspecialObs"] = local.AcessoNecessidadeEspecialObs.Valor;
                        linha["ArCondicionado"] = local.ArCondicionado.Valor;
                        linha["ComoChegar"] = local.ComoChegar.Valor;
                        linha["Complemento"] = local.Complemento.Valor;
                        linha["ComoChegarInternet"] = local.ComoChegarInternet.Valor;
                        linha["HorariosBilheteria"] = local.HorariosBilheteria.Valor;
                        linha["RetiradaBilheteria"] = local.RetiradaBilheteria.Valor;
                        linha["PaisID"] = local.PaisID.Valor;
                        linha["ImagemInternet"] = local.ImagemInternet.Valor;
                        linha["CodigoPraca"] = local.CodigoPraca.Valor;
                        linha["Latitude"] = local.Latitude.Valor;
                        linha["Longitude"] = local.Longitude.Valor;
                        linha["Alvara"] = local.Alvara.Valor;
                        linha["AVCB"] = local.AVCB.Valor;
                        linha["DataEmissaoAlvara"] = local.DataEmissaoAlvara.Valor;
                        linha["DataValidadeAlvara"] = local.DataValidadeAlvara.Valor;
                        linha["DataEmissaoAvcb"] = local.DataEmissaoAvcb.Valor;
                        linha["DataValidadeAvcb"] = local.DataValidadeAvcb.Valor;
                        linha["Lotacao"] = local.Lotacao.Valor;
                        linha["FonteImposto"] = local.FonteImposto.Valor;
                        linha["PorcentagemImposto"] = local.PorcentagemImposto.Valor;
                        linha["Ativo"] = local.Ativo.Valor;
                        tabela.Rows.Add(linha);
                    } while (this.Proximo());

                }
                else
                { //erro: nao carregou a lista
                    tabela = null;
                }

                return tabela;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Retorna um IDataReader com ID e o Campo.
        /// </summary>
        /// <param name="campo">Informe o campo. Exemplo: Nome</param>
        /// <returns></returns>
        public override IDataReader ListaPropriedade(string campo)
        {

            try
            {
                string sql;
                switch (campo)
                {
                    case "EmpresaID":
                        sql = "SELECT ID, EmpresaID FROM tLocal WHERE " + FiltroSQL + " ORDER BY EmpresaID";
                        break;
                    case "ContratoID":
                        sql = "SELECT ID, ContratoID FROM tLocal WHERE " + FiltroSQL + " ORDER BY ContratoID";
                        break;
                    case "Nome":
                        sql = "SELECT ID, Nome FROM tLocal WHERE " + FiltroSQL + " ORDER BY Nome";
                        break;
                    case "Contato":
                        sql = "SELECT ID, Contato FROM tLocal WHERE " + FiltroSQL + " ORDER BY Contato";
                        break;
                    case "Logradouro":
                        sql = "SELECT ID, Logradouro FROM tLocal WHERE " + FiltroSQL + " ORDER BY Logradouro";
                        break;
                    case "Cidade":
                        sql = "SELECT ID, Cidade FROM tLocal WHERE " + FiltroSQL + " ORDER BY Cidade";
                        break;
                    case "Estado":
                        sql = "SELECT ID, Estado FROM tLocal WHERE " + FiltroSQL + " ORDER BY Estado";
                        break;
                    case "CEP":
                        sql = "SELECT ID, CEP FROM tLocal WHERE " + FiltroSQL + " ORDER BY CEP";
                        break;
                    case "DDDTelefone":
                        sql = "SELECT ID, DDDTelefone FROM tLocal WHERE " + FiltroSQL + " ORDER BY DDDTelefone";
                        break;
                    case "Telefone":
                        sql = "SELECT ID, Telefone FROM tLocal WHERE " + FiltroSQL + " ORDER BY Telefone";
                        break;
                    case "Bairro":
                        sql = "SELECT ID, Bairro FROM tLocal WHERE " + FiltroSQL + " ORDER BY Bairro";
                        break;
                    case "Numero":
                        sql = "SELECT ID, Numero FROM tLocal WHERE " + FiltroSQL + " ORDER BY Numero";
                        break;
                    case "Estacionamento":
                        sql = "SELECT ID, Estacionamento FROM tLocal WHERE " + FiltroSQL + " ORDER BY Estacionamento";
                        break;
                    case "EstacionamentoObs":
                        sql = "SELECT ID, EstacionamentoObs FROM tLocal WHERE " + FiltroSQL + " ORDER BY EstacionamentoObs";
                        break;
                    case "AcessoNecessidadeEspecial":
                        sql = "SELECT ID, AcessoNecessidadeEspecial FROM tLocal WHERE " + FiltroSQL + " ORDER BY AcessoNecessidadeEspecial";
                        break;
                    case "AcessoNecessidadeEspecialObs":
                        sql = "SELECT ID, AcessoNecessidadeEspecialObs FROM tLocal WHERE " + FiltroSQL + " ORDER BY AcessoNecessidadeEspecialObs";
                        break;
                    case "ArCondicionado":
                        sql = "SELECT ID, ArCondicionado FROM tLocal WHERE " + FiltroSQL + " ORDER BY ArCondicionado";
                        break;
                    case "ComoChegar":
                        sql = "SELECT ID, ComoChegar FROM tLocal WHERE " + FiltroSQL + " ORDER BY ComoChegar";
                        break;
                    case "Complemento":
                        sql = "SELECT ID, Complemento FROM tLocal WHERE " + FiltroSQL + " ORDER BY Complemento";
                        break;
                    case "ComoChegarInternet":
                        sql = "SELECT ID, ComoChegarInternet FROM tLocal WHERE " + FiltroSQL + " ORDER BY ComoChegarInternet";
                        break;
                    case "HorariosBilheteria":
                        sql = "SELECT ID, HorariosBilheteria FROM tLocal WHERE " + FiltroSQL + " ORDER BY HorariosBilheteria";
                        break;
                    case "RetiradaBilheteria":
                        sql = "SELECT ID, RetiradaBilheteria FROM tLocal WHERE " + FiltroSQL + " ORDER BY RetiradaBilheteria";
                        break;
                    case "PaisID":
                        sql = "SELECT ID, PaisID FROM tLocal WHERE " + FiltroSQL + " ORDER BY PaisID";
                        break;
                    case "ImagemInternet":
                        sql = "SELECT ID, ImagemInternet FROM tLocal WHERE " + FiltroSQL + " ORDER BY ImagemInternet";
                        break;
                    case "CodigoPraca":
                        sql = "SELECT ID, CodigoPraca FROM tLocal WHERE " + FiltroSQL + " ORDER BY CodigoPraca";
                        break;
                    case "Latitude":
                        sql = "SELECT ID, Latitude FROM tLocal WHERE " + FiltroSQL + " ORDER BY Latitude";
                        break;
                    case "Longitude":
                        sql = "SELECT ID, Longitude FROM tLocal WHERE " + FiltroSQL + " ORDER BY Longitude";
                        break;
                    case "Alvara":
                        sql = "SELECT ID, Alvara FROM tLocal WHERE " + FiltroSQL + " ORDER BY Alvara";
                        break;
                    case "AVCB":
                        sql = "SELECT ID, AVCB FROM tLocal WHERE " + FiltroSQL + " ORDER BY AVCB";
                        break;
                    case "DataEmissaoAlvara":
                        sql = "SELECT ID, DataEmissaoAlvara FROM tLocal WHERE " + FiltroSQL + " ORDER BY DataEmissaoAlvara";
                        break;
                    case "DataValidadeAlvara":
                        sql = "SELECT ID, DataValidadeAlvara FROM tLocal WHERE " + FiltroSQL + " ORDER BY DataValidadeAlvara";
                        break;
                    case "DataEmissaoAvcb":
                        sql = "SELECT ID, DataEmissaoAvcb FROM tLocal WHERE " + FiltroSQL + " ORDER BY DataEmissaoAvcb";
                        break;
                    case "DataValidadeAvcb":
                        sql = "SELECT ID, DataValidadeAvcb FROM tLocal WHERE " + FiltroSQL + " ORDER BY DataValidadeAvcb";
                        break;
                    case "Lotacao":
                        sql = "SELECT ID, Lotacao FROM tLocal WHERE " + FiltroSQL + " ORDER BY Lotacao";
                        break;
                    case "FonteImposto":
                        sql = "SELECT ID, FonteImposto FROM tLocal WHERE " + FiltroSQL + " ORDER BY FonteImposto";
                        break;
                    case "PorcentagemImposto":
                        sql = "SELECT ID, PorcentagemImposto FROM tLocal WHERE " + FiltroSQL + " ORDER BY PorcentagemImposto";
                        break;
                    case "Ativo":
                        sql = "SELECT ID, Ativo FROM tLocal WHERE " + FiltroSQL + " ORDER BY Ativo";
                        break;
                    default:
                        sql = null;
                        break;
                }

                IDataReader dataReader = bd.Consulta(sql);

                bd.Fechar();

                return dataReader;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Devolve um array dos IDs que compoem a lista
        /// </summary>
        /// <returns></returns>		
        public override int[] ToArray()
        {

            try
            {

                int[] a = (int[])lista.ToArray(typeof(int));

                return a;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Devolve uma string dos IDs que compoem a lista concatenada por virgula
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {

            try
            {

                StringBuilder idsBuffer = new StringBuilder();

                int n = lista.Count;
                for (int i = 0; i < n; i++)
                {
                    int id = (int)lista[i];
                    idsBuffer.Append(id + ",");
                }

                string ids = "";

                if (idsBuffer.Length > 0)
                {
                    ids = idsBuffer.ToString();
                    ids = ids.Substring(0, ids.Length - 1);
                }

                return ids;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    #endregion

    #region "LocalException"

    [Serializable]
    public class LocalException : Exception
    {

        public LocalException() : base() { }

        public LocalException(string msg) : base(msg) { }

        public LocalException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}