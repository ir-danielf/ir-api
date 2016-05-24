/**************************************************
* Arquivo: Contrato.cs
* Gerado: 16/03/2009
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace IRLib.Paralela
{

    public class Contrato : Contrato_B
    {

        public enum TipoDeCobrancaPapel
        {
            [System.ComponentModel.Description("Por utilização")]
            PorUtilizacao = 1,
            [System.ComponentModel.Description("Por envio")]
            PorEnvio = 2
        }

        public enum TipoDeRepasse
        {
            [System.ComponentModel.Description("Data da venda")]
            DataVenda = 1,
            [System.ComponentModel.Description("Data da apresentação")]
            DataApresentacao = 2
        }

        public enum TipoDeComissao
        {
            [System.ComponentModel.Description("Reter no repasse")]
            ReterNoRepasse = 1,
            [System.ComponentModel.Description("Emitir boleto ao término da apresentação")]
            EmitirBoletoTerminoApresentacao = 2
        }

        public enum TipoDePapelPagamento
        {
            [System.ComponentModel.Description("Reter no primeiro repasse")]
            ReterNoPrimeiroRepasse = 1,
            [System.ComponentModel.Description("Boleto mensal")]
            BoletoMensal = 2,
            [System.ComponentModel.Description("Boleto semanal")]
            BoletoSemanal = 3
        }

        public Contrato() { }

        #region Métodos de Manipulação do Contrato

        #region Inserir
        /// <summary>
        /// Inserir novo(a) Contrato
        /// </summary>
        /// <returns></returns>	
        internal bool Inserir(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cContrato");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tContrato(ID, EmpresaID, UsuarioID, Nome, Codigo, DataCriacao, Observacoes, TipoRepasse, TipoComissao, TipoPapelPagamento, PapelCobrancaUtilizacao, PapelComHolografia, MaximoParcelas) ");
                sql.Append("VALUES (@ID,@001,@002,'@003','@004','@005','@006',@007,@008,@009,'@010', '@011', '@012')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.EmpresaID.ValorBD);
                sql.Replace("@002", this.UsuarioID.ValorBD);
                sql.Replace("@003", this.Nome.ValorBD);
                sql.Replace("@004", this.Codigo.ValorBD);
                sql.Replace("@005", this.DataCriacao.ValorBD);
                sql.Replace("@006", this.Observacoes.ValorBD);
                sql.Replace("@007", this.TipoRepasse.ValorBD);
                sql.Replace("@008", this.TipoComissao.ValorBD);
                sql.Replace("@009", this.TipoPapelPagamento.ValorBD);
                sql.Replace("@010", this.PapelCobrancaUtilizacao.ValorBD);
                sql.Replace("@011", this.PapelComHolografia.ValorBD);
                sql.Replace("@012", this.MaximoParcelas.ValorBD);

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
        #endregion

        #region Atualizar
        /// <summary>
        /// Atualiza Contrato
        /// </summary>
        /// <returns></returns>	
        internal bool Atualizar(BD bd)
        {

            try
            {

                string sqlVersion = "SELECT MAX(Versao) FROM cContrato WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("U");
                InserirLog();

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tContrato SET Nome = '@003', Codigo = '@004', Observacoes = '@006', TipoRepasse = @007, TipoComissao = @008, TipoPapelPagamento = @009, PapelCobrancaUtilizacao = '@010', PapelComHolografia = '@011' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@003", this.Nome.ValorBD);
                sql.Replace("@004", this.Codigo.ValorBD);
                sql.Replace("@006", this.Observacoes.ValorBD);
                sql.Replace("@007", this.TipoRepasse.ValorBD);
                sql.Replace("@008", this.TipoComissao.ValorBD);
                sql.Replace("@009", this.TipoPapelPagamento.ValorBD);
                sql.Replace("@010", this.PapelCobrancaUtilizacao.ValorBD);
                sql.Replace("@011", this.PapelComHolografia.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region Excluir
        /// <summary>
        /// Exclui Contrato com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        internal bool Excluir(int id, BD bd)
        {

            try
            {

                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cContrato WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle("D");
                InserirLog();

                string sqlDelete = "DELETE FROM tContrato WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Controle e Log
        protected internal void InserirControle(string acao, BD bd)
        {

            try
            {

                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cContrato (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

        protected void InserirLog(BD bd)
        {

            try
            {

                StringBuilder sql = new StringBuilder();

                sql.Append("INSERT INTO xContrato (ID, Versao, EmpresaID, UsuarioID, Nome, Codigo, DataCriacao, Observacoes, TipoRepasse, TipoComissao, TipoPapelPagamento, PapelCobrancaUtilizacao) ");
                sql.Append("SELECT ID, @V, EmpresaID, UsuarioID, Nome, Codigo, DataCriacao, Observacoes, TipoRepasse, TipoComissao, TipoPapelPagamento, PapelCobrancaUtilizacao FROM tContrato WHERE ID = @I");
                sql.Replace("@I", this.Control.ID.ToString());
                sql.Replace("@V", this.Control.Versao.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #endregion

        public Contrato(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public override bool Excluir()
        {
            return Excluir(this.Control.ID);
        }

        public override bool Excluir(int id)
        {
            bool blnExcluir = true;
            BD bdGravar = null;

            ContratoPapel contratoPapel = null;
            ContratoPapelLista contratoPapelLista = null;

            try
            {
                bdGravar = new BD();
                bdGravar.IniciarTransacao();

                if (this.Control.UsuarioID == 0)
                    throw new ContratoException("É necessário informar o usuário que está executando essa operação.");

                #region Exclui os papeis

                contratoPapel = new ContratoPapel(this.Control.UsuarioID);
                contratoPapelLista = new ContratoPapelLista(this.Control.UsuarioID);

                contratoPapelLista.FiltroSQL = "ContratoID = " + id;
                contratoPapelLista.Carregar();
                if (contratoPapelLista.Primeiro())
                {
                    do
                    {
                        if (!contratoPapel.Excluir(contratoPapelLista.ContratoPapel.Control.ID, bdGravar))
                            throw new Exception("Não possível excluir o papel do contrato.");
                    } while (contratoPapelLista.Proximo());
                }

                #endregion

                #region Exclui o contrato

                this.Excluir(id, bdGravar);

                #endregion

                bdGravar.FinalizarTransacao();
            }
            catch (Exception ex)
            {
                if (bdGravar != null)
                {
                    bdGravar.DesfazerTransacao();
                }

                blnExcluir = false;

                throw ex;
            }

            finally
            {
                if (bdGravar != null)
                {
                    bdGravar.Fechar();
                    bdGravar = null;
                }

                contratoPapel = null;
                contratoPapelLista = null;
            }


            return blnExcluir;
        }
        /*
                    #region Caputura as contas
         
                    contrato.Contas = new List<IRLib.ClientObjects.EstruturaContratoConta>();
                    contratoContaLista = new ContratoContaLista(this.Control.UsuarioID);

                    contratoContaLista.FiltroSQL = "ContratoID = " + contratoID;
                    contratoContaLista.Carregar();
                    if (contratoContaLista.Primeiro())
                    {
                        do
                        {
                            contratoconta = new IRLib.ClientObjects.EstruturaContratoConta();
                            contratoconta.ID = contratoContaLista.ContratoConta.Control.ID;
                            contratoconta.Beneficiario = contratoContaLista.ContratoConta.Beneficiario.Valor;
                            contratoconta.Banco = contratoContaLista.ContratoConta.Banco.Valor;
                            contratoconta.Agencia = contratoContaLista.ContratoConta.Agencia.Valor;
                            contratoconta.Conta = contratoContaLista.ContratoConta.Conta.Valor;
                            contratoconta.CPFCNPJ = contratoContaLista.ContratoConta.CPFCNPJ.Valor;

                            contrato.Contas.Add(contratoconta);

                        } while (contratoContaLista.Proximo());
                    }

                    #endregion
         
            ClientObjects.EstruturaContratoConta contratoconta;
            ContratoContaLista contratoContaLista = null;
         
                #region Gravação das Contas
                contratoConta = new ContratoConta(usuarioidlogado);

                // Exclusão de contas
                foreach (int itemContratoContaID in estruturacontrato.ContratoContasExcluir)
                {
                    if (!contratoConta.Excluir(itemContratoContaID, bdGravar))
                        throw new ContratoContaException("Não possível excluir a conta do contrato.");
                }

                foreach (ClientObjects.EstruturaContratoConta itemContratoConta in estruturacontrato.Contas)
                {
                    if (itemContratoConta.Beneficiario == "")
                        throw new ContratoContaException("O beneficiário não pode ser vazio.");

                    if (itemContratoConta.Banco == "")
                        throw new ContratoContaException("O banco não pode ser vazio.");

                    if (itemContratoConta.Agencia == "")
                        throw new ContratoContaException("A agência não pode ser vazia.");

                    if (itemContratoConta.Conta == "")
                        throw new ContratoContaException("A conta não pode ser vazia.");

                    if (itemContratoConta.CPFCNPJ == "")
                        throw new ContratoContaException("O cpf / cnpj não pode ser vazio.");

                    if (!(Utilitario.IsCPF(itemContratoConta.CPFCNPJ) || Utilitario.IsCNPJ(itemContratoConta.CPFCNPJ)))
                        throw new ContratoContaException("O cpf / cnpj não é válido.");

                    contratoConta.ContratoID.Valor = estruturacontrato.ID;
                    contratoConta.Beneficiario.Valor = itemContratoConta.Beneficiario;
                    contratoConta.Banco.Valor = itemContratoConta.Banco;
                    contratoConta.Agencia.Valor = itemContratoConta.Agencia;
                    contratoConta.Conta.Valor = itemContratoConta.Conta;
                    contratoConta.CPFCNPJ.Valor = itemContratoConta.CPFCNPJ;

                    if (itemContratoConta.ID == 0)
                    {
                        if (!contratoConta.Inserir(bdGravar))
                            throw new ContratoException("Não foi possível inserir a conta do contrato.");
                    }
                    else
                    {
                        contratoConta.Control.ID = itemContratoConta.ID;
                        if (!contratoConta.Atualizar(bdGravar))
                            throw new ContratoContaException("Não possível atualizar a conta do contrato.");
                    }
                }



                #endregion         
         
        */

        public ClientObjects.EstruturaContrato CarregaContrato(int contratoID)
        {
            ClientObjects.EstruturaContrato contrato = new IRLib.Paralela.ClientObjects.EstruturaContrato();

            try
            {
                List<ClientObjects.EstruturaContrato> contratos = CarregaListaContratos(0, contratoID);
                if (contratos != null && contratos.Count == 1)
                {
                    contrato = contratos[0];
                }

            }
            catch
            {
                throw;
            }

            return contrato;
        }

        /// <summary>
        /// Captura contrados
        /// </summary>
        /// <param name="empresaID">ID da Empresa</param>
        /// <returns></returns>
        private List<ClientObjects.EstruturaContrato> CarregaListaContratos(int empresaID, int contratoID)
        {
            List<ClientObjects.EstruturaContrato> contratos = new List<IRLib.Paralela.ClientObjects.EstruturaContrato>();

            try
            {
                using (IDataReader oDataReader = bd.Consulta("" +
                    "SELECT " +
                    "   tContrato.ID, " +
                    "   tEmpresa.ID AS EmpresaID, " +
                    "   tEmpresa.Nome AS EmpresaNome, " +
                    "   tUsuario.ID AS UsuarioID, " +
                    "   tUsuario.Nome AS UsuarioNome, " +
                    "   tContrato.Nome, " +
                    "   tContrato.Codigo, " +
                    "   tContrato.DataCriacao, " +
                    "   tContrato.Observacoes, " +
                    "   tContrato.TipoRepasse, " +
                    "   tContrato.TipoComissao, " +
                    "   tContrato.TipoPapelPagamento, " +
                    "   tContrato.PapelCobrancaUtilizacao, " +
                    "   tContrato.PapelComHolografia " +
                    "FROM " +
                    "   tContrato (NOLOCK) " +
                    "INNER JOIN " +
                    "   tEmpresa (NOLOCK) " +
                    "ON " +
                    "   tEmpresa.ID = tContrato.EmpresaID " +
                    "INNER JOIN " +
                    "   tUsuario (NOLOCK) " +
                    "ON " +
                    "   tUsuario.ID = tContrato.UsuarioID " +
                    "WHERE " +
                        "1 = 1 " + 
                    ((empresaID != 0) ? "" + 
                    "AND " + 
                    "   (tContrato.EmpresaID = " + empresaID + ") " : "") +
                    ((contratoID != 0) ? "" +
                    "AND " +
                    "   (tContrato.ID = " + contratoID + ") " : "") +
                    "ORDER BY " +
                    "   tContrato.Nome"))
                {
                    ClientObjects.EstruturaContrato contrato;

                    while (oDataReader.Read())
                    {
                        contrato = new IRLib.Paralela.ClientObjects.EstruturaContrato();

                        contrato.ID = bd.LerInt("ID");
                        contrato.Nome = bd.LerString("Nome");
                        contrato.DataCriacao = bd.LerDateTime("DataCriacao");
                        contrato.Codigo = bd.LerString("Codigo");
                        contrato.Observacoes = bd.LerString("Observacoes");
                        contrato.TipoRepasse = (Contrato.TipoDeRepasse)bd.LerInt("TipoRepasse");
                        contrato.TipoComissao = (Contrato.TipoDeComissao)bd.LerInt("TipoComissao");
                        contrato.TipoPapelPagamento = (Contrato.TipoDePapelPagamento)bd.LerInt("TipoPapelPagamento");
                        contrato.PapelCobrancaUtilizacao = bd.LerBoolean("PapelCobrancaUtilizacao");
                        contrato.PapelComHolografia = bd.LerBoolean("PapelComHolografia");
                        contrato.EmpresaID = bd.LerInt("EmpresaID");
                        contrato.EmpresaNome = bd.LerString("EmpresaNome");
                        contrato.CriadorID = bd.LerInt("UsuarioID");
                        contrato.CriadorNome = bd.LerString("UsuarioNome");

                        contratos.Add(contrato);
                    }
                }

                bd.Fechar();
            }
            catch
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }

            return contratos;
        }

        /// <summary>
        /// Captura os contrados de uma empresa
        /// </summary>
        /// <param name="empresaID">ID da Empresa</param>
        /// <returns></returns>
        public List<ClientObjects.EstruturaContrato> CarregaListaContratos(int empresaID)
        {
            return CarregaListaContratos(empresaID, 0);
        }

        /// <summary>
        /// Captura os contrados de uma empresa
        /// </summary>
        /// <param name="empresaID">ID da Empresa</param>
        /// <returns></returns>
        public DataTable ContratosIDNome(int empresaID)
        {
            try
            {
                DataTable retorno = new DataTable();
                retorno.Columns.Add("ID", typeof(int));
                retorno.Columns.Add("Nome", typeof(string));
                string sql = "SELECT ID, Nome FROM tContrato (NOLOCK) WHERE EmpresaID = " + empresaID;

                bd.Consulta(sql);
                DataRow linha;
                while (bd.Consulta().Read())
                {
                    linha = retorno.NewRow();
                    linha["ID"] = bd.LerInt("ID");
                    linha["Nome"] = bd.LerString("Nome");
                    retorno.Rows.Add(linha);
                }
                bd.Fechar();
                return retorno;
            }
            catch
            {

                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// Gravação do Contrato
        /// </summary>
        /// <param name="estruturaContrato">Struct do Contrato</param>
        /// <param name="usuarioIDLogado">Usuário que está utilizando o sistema</param>
        /// <returns></returns>
        public override ClientObjects.EstruturaContrato GravarContrato(ClientObjects.EstruturaContrato estruturacontrato)
        {
            BD bdGravar = null;
            BD bdLeitura = null;
            ContratoPapel contratoPapel = null;
            ContratoFormaPagamento contratoFormaPagamento = null;

            try
            {
                bdLeitura = new BD();

                bdGravar = new BD();
                bdGravar.IniciarTransacao();

                #region Gravação do Contrato

                if (estruturacontrato.Nome.Trim() == "")
                    throw new ContratoException("O nome do contrato não pode ser vazio.");

                if (estruturacontrato.Codigo.Trim() == "")
                    throw new ContratoException("O código do contrato não pode ser vazio.");

                if (Convert.ToInt32(estruturacontrato.TipoComissao) == 0)
                    throw new ContratoException("O tipo de comissão não é válido.");

                if (Convert.ToInt32(estruturacontrato.TipoPapelPagamento) == 0)
                    throw new ContratoException("O tipo de pagamento não é válido.");

                if (Convert.ToInt32(estruturacontrato.TipoRepasse) == 0)
                    throw new ContratoException("O tipo de repasse não é válido.");
                
                this.Nome.Valor = estruturacontrato.Nome;
                this.Codigo.Valor = estruturacontrato.Codigo;
                this.Observacoes.Valor = estruturacontrato.Observacoes;
                this.TipoComissao.Valor = Convert.ToInt32(estruturacontrato.TipoComissao);
                this.TipoPapelPagamento.Valor = Convert.ToInt32(estruturacontrato.TipoPapelPagamento);
                this.TipoRepasse.Valor = Convert.ToInt32(estruturacontrato.TipoRepasse);
                this.PapelCobrancaUtilizacao.Valor = estruturacontrato.PapelCobrancaUtilizacao;
                this.PapelComHolografia.Valor = estruturacontrato.PapelComHolografia;
                this.MaximoParcelas.Valor = estruturacontrato.MaximoParcelas;

                if (estruturacontrato.ID == 0)
                {
                    this.EmpresaID.Valor = estruturacontrato.EmpresaID;
                    this.UsuarioID.Valor = estruturacontrato.CriadorID;
                    this.DataCriacao.Valor = DateTime.Now;

                    if (!this.Inserir(bdGravar))
                        throw new ContratoException("Não foi possível incluir o contrato.");
                }
                else
                {
                    this.Control.ID = estruturacontrato.ID;

                    if (!this.Atualizar(bdGravar))
                        throw new ContratoException("Não foi possível atualizar o contrato.");
                }

                // Captura o ID do Contrato
                estruturacontrato.ID = this.Control.ID;

                #endregion

                #region Gravação dos papeis

                foreach (ClientObjects.EstruturaContratoPapel itemContratoPapel in estruturacontrato.Papeis)
                {
                    contratoPapel = new ContratoPapel(this.Control.UsuarioID);
                    contratoPapel.ContratoID.Valor = estruturacontrato.ID;
                    contratoPapel.CanalTipoID.Valor = itemContratoPapel.CanalTipoID;
                    contratoPapel.CortesiaValor.Valor = itemContratoPapel.CortesiaValor;
                    contratoPapel.IngressoNormalValor.Valor = itemContratoPapel.IngressoNormalValor;
                    contratoPapel.PreImpressoValor.Valor = itemContratoPapel.PreImpressoValor;

                    if (itemContratoPapel.ID == 0)
                    {
                        if (!contratoPapel.Inserir(bdGravar))
                            throw new ContratoPapelException("Não foi possível gravar o papel do contrato.");
                    }
                    else
                    {
                        contratoPapel.Control.ID = itemContratoPapel.ID;
                        if (!contratoPapel.Atualizar(bdGravar))
                            throw new ContratoPapelException("Não foi possível gravar o papel do contrato.");
                    }
                }

                #endregion

                #region Gravação das Formas de Pagamento

                for (int contadorFormaPagamento = 0; contadorFormaPagamento <= estruturacontrato.FormaPagamento.Count - 1; contadorFormaPagamento++)
                {
                    using (IDataReader oDataReader = bdLeitura.Consulta("" +
                    "SELECT " +
                    " 	tFormaPagamento.ID AS FormaPagamentoID, " +
                    "   ISNULL(tContratoFormaPagamento.ID, 0) AS ContratoFormaPagamentoID " + 
                    "FROM tFormaPagamento (NOLOCK) " +
                    "LEFT JOIN tContratoFormaPagamento (NOLOCK) ON tContratoFormaPagamento.FormaPagamentoID = tFormaPagamento.ID AND tContratoFormaPagamento.ContratoID = " + this.Control.ID + " " + 
                    "WHERE tFormaPagamento.FormaPagamentoTipoID = " + estruturacontrato.FormaPagamento[contadorFormaPagamento].FormaPagamentoTipoID + ""))
                    {
                        while (oDataReader.Read())
                        {
                            contratoFormaPagamento = new ContratoFormaPagamento(this.Control.UsuarioID);
                            contratoFormaPagamento.ContratoID.Valor = this.Control.ID;
                            contratoFormaPagamento.FormaPagamentoID.Valor = bdLeitura.LerInt("FormaPagamentoID");
                            contratoFormaPagamento.Dias.Valor = estruturacontrato.FormaPagamento[contadorFormaPagamento].Dias;
                            contratoFormaPagamento.Taxa.Valor = estruturacontrato.FormaPagamento[contadorFormaPagamento].TaxaAdm;

                            if (bdLeitura.LerInt("ContratoFormaPagamentoID") == 0)
                            {
                                if (!contratoFormaPagamento.Inserir(bdGravar))
                                    throw new ContratoFormaPagamentoException("Não foi possível incluir a forma de pagamento.");
                            }
                            else
                            {
                                contratoFormaPagamento.Control.ID = bdLeitura.LerInt("ContratoFormaPagamentoID");
                                if (!contratoFormaPagamento.Atualizar(bdGravar))
                                    throw new ContratoFormaPagamentoException("Não foi possível alterar a forma de pagamento.");
                            }


                        }
                    }
                }

                #endregion

                bdGravar.FinalizarTransacao();
            }
            catch (Exception ex)
            {
                if (bdGravar != null)
                {
                    bdGravar.DesfazerTransacao();
                }
                throw ex;
            }

            finally
            {
                if (bdGravar != null)
                {
                    bdGravar.Fechar();
                    bdGravar = null;
                }

                if (bdLeitura != null)
                {
                    bdLeitura.Fechar();
                    bdLeitura = null;
                }

                contratoPapel = null;
            }
            return estruturacontrato;

        }


    }

    public class ContratoLista : ContratoLista_B
    {

        public ContratoLista() { }

        public ContratoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
