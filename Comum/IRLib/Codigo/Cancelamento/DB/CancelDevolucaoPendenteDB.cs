using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CTLib;
using System.Data;
using System.Runtime.Serialization;

namespace IRLib.CancelamentoIngresso
{
    #region CancelDevolucaoPendente_B
    public abstract class CancelDevolucaoPendente_B : BaseBD
    {
        public vendabilheteriaidvenda VendaBilheteriaIDVenda = new vendabilheteriaidvenda();
        public vendabilheteriaidcancel VendaBilheteriaIDCancel = new vendabilheteriaidcancel();
        public statuscancel StatusCancel = new statuscancel();
        public supervisorid SupervisorID = new supervisorid();
        public numerochamado NumeroChamado = new numerochamado();
        public caixaid CaixaID = new caixaid();
        public localid LocalID = new localid();
        public lojaid LojaID = new lojaid();
        public canalid CanalID = new canalid();
        public usuarioid UsuarioID = new usuarioid();
        public empresaid EmpresaID = new empresaid();
        public tipocancelamento TipoCancelamento = new tipocancelamento();
        public formadevolucao FormaDevolucao = new formadevolucao();
        public motivocancelamento MotivoCancelamento = new motivocancelamento();
        public submotivocancelamento SubMotivoCancelamento = new submotivocancelamento();
        public vlringressoestornado VlrIngressoEstornado = new vlringressoestornado();
        public vlrtxentregaestornado VlrTxEntregaEstornado = new vlrtxentregaestornado();
        public vlrtxconvenienciaestornado VlrTxConvenienciaEstornado = new vlrtxconvenienciaestornado();
        public vlrseguroestornado VlrSeguroEstornado = new vlrseguroestornado();

        public caixaiddevolucao CaixaIDDevolucao = new caixaiddevolucao();
        public localiddevolucao LocalIDDevolucao = new localiddevolucao();
        public lojaiddevolucao LojaIDDevolucao = new lojaiddevolucao();
        public canaliddevolucao CanalIDDevolucao = new canaliddevolucao();
        public empresaiddevolucao EmpresaIDDevolucao = new empresaiddevolucao();
        public usuarioiddevolucao UsuarioIDDevolucao = new usuarioiddevolucao();

        public CancelDevolucaoPendente_B() { }

        // passar o Usuario logado no sistema
        public CancelDevolucaoPendente_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos da pendencia
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {
            try
            {
                string sql = "SELECT * FROM tCancelDevolucaoPendente WHERE ID = " + id;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = id;
                    this.VendaBilheteriaIDVenda.ValorBD = bd.LerInt("VendaBilheteriaIDVenda").ToString();
                    this.VendaBilheteriaIDCancel.ValorBD = bd.LerInt("VendaBilheteriaIDCancel").ToString();
                    this.StatusCancel.ValorBD = bd.LerString("StatusCancel");
                    this.SupervisorID.ValorBD = bd.LerInt("SupervisorID").ToString();
                    this.NumeroChamado.ValorBD = bd.LerString("NumeroChamado");
                    this.CaixaID.ValorBD = bd.LerInt("CaixaID").ToString();
                    this.LocalID.ValorBD = bd.LerInt("LocalID").ToString();
                    this.LojaID.ValorBD = bd.LerInt("LojaID").ToString();
                    this.CanalID.ValorBD = bd.LerInt("CanalID").ToString();
                    this.UsuarioID.ValorBD = bd.LerInt("UsuarioID").ToString();
                    this.EmpresaID.ValorBD = bd.LerInt("EmpresaID").ToString();
                    this.TipoCancelamento.ValorBD = bd.LerInt("TipoCancelamento").ToString();
                    this.FormaDevolucao.ValorBD = bd.LerInt("FormaDevolucao").ToString();
                    this.MotivoCancelamento.ValorBD = bd.LerInt("MotivoCancelamento").ToString();
                    this.SubMotivoCancelamento.ValorBD = bd.LerInt("SubMotivoCancelamento").ToString();
                    this.VlrIngressoEstornado.ValorBD = bd.LerDecimal("VlrIngressoEstornado").ToString();
                    this.VlrTxEntregaEstornado.ValorBD = bd.LerDecimal("VlrTxEntregaEstornado").ToString();
                    this.VlrTxConvenienciaEstornado.ValorBD = bd.LerDecimal("VlrTxConvenienciaEstornado").ToString();
                    this.VlrSeguroEstornado.ValorBD = bd.LerDecimal("VlrSeguroEstornado").ToString();

                    this.CaixaIDDevolucao.ValorBD = bd.LerInt("CaixaIDDevolucao").ToString();
                    this.LocalIDDevolucao.ValorBD = bd.LerInt("LocalIDDevolucao").ToString();
                    this.LojaIDDevolucao.ValorBD = bd.LerInt("LojaIDDevolucao").ToString();
                    this.CanalIDDevolucao.ValorBD = bd.LerInt("CanalIDDevolucao").ToString();
                    this.EmpresaIDDevolucao.ValorBD = bd.LerInt("EmpresaIDDevolucao").ToString();
                    this.UsuarioIDDevolucao.ValorBD = bd.LerInt("UsuarioIDDevolucao").ToString();
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
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// Inserir novo(a) Pendencia
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO tCancelDevolucaoPendente(VendaBilheteriaIDVenda, VendaBilheteriaIDCancel, StatusCancel, SupervisorID, NumeroChamado, CaixaID, LocalID, LojaID, CanalID, UsuarioID, EmpresaID, TipoCancelamento, FormaDevolucao, MotivoCancelamento, SubMotivoCancelamento, VlrIngressoEstornado, VlrTxEntregaEstornado, VlrTxConvenienciaEstornado, VlrSeguroEstornado, DataInsert) ");
                sql.Append("VALUES (@001, @002, '@003', @004, '@005', @006, @007, @008, @009, @010, @011, @012, @013, @014, @015, @016, @017, @018, @019, getdate()); SELECT SCOPE_IDENTITY();");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.VendaBilheteriaIDVenda.ValorBD);
                sql.Replace("@002", this.VendaBilheteriaIDCancel.ValorBD);
                sql.Replace("@003", this.StatusCancel.ValorBD);
                sql.Replace("@004", this.SupervisorID.ValorBD);
                sql.Replace("@005", this.NumeroChamado.ValorBD);
                sql.Replace("@006", this.CaixaID.ValorBD);
                sql.Replace("@007", this.LocalID.ValorBD);
                sql.Replace("@008", this.LojaID.ValorBD);
                sql.Replace("@009", this.CanalID.ValorBD);
                sql.Replace("@010", this.UsuarioID.ValorBD);
                sql.Replace("@011", this.EmpresaID.ValorBD);
                sql.Replace("@012", this.TipoCancelamento.ValorBD);
                sql.Replace("@013", this.FormaDevolucao.ValorBD);
                sql.Replace("@014", this.MotivoCancelamento.ValorBD);
                sql.Replace("@015", this.SubMotivoCancelamento.ValorBD);
                sql.Replace("@016", this.VlrIngressoEstornado.ValorBD);
                sql.Replace("@017", this.VlrTxEntregaEstornado.ValorBD);
                sql.Replace("@018", this.VlrTxConvenienciaEstornado.ValorBD);
                sql.Replace("@019", this.VlrSeguroEstornado.ValorBD);

                this.Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));
                bd.Fechar();

                return this.Control.ID > 0;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }


        }

        /// <summary>
        /// Inserir novo(a) Pendencia
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO tCancelDevolucaoPendente(VendaBilheteriaIDVenda, VendaBilheteriaIDCancel, StatusCancel, SupervisorID, NumeroChamado, CaixaID, LocalID, LojaID, CanalID, UsuarioID, EmpresaID, TipoCancelamento, FormaDevolucao, MotivoCancelamento, SubMotivoCancelamento, VlrIngressoEstornado, VlrTxEntregaEstornado, VlrTxConvenienciaEstornado, VlrSeguroEstornado, DataInsert) ");
            sql.Append("VALUES (@001, @002, '@003', @004, '@005', @006, @007, @008, @009, @010, @011, @012, @013, @014, @015, @016, @017, @018, @019, getdate()); SELECT SCOPE_IDENTITY();");

            sql.Replace("@ID", this.Control.ID.ToString());
            sql.Replace("@001", this.VendaBilheteriaIDVenda.ValorBD);
            sql.Replace("@002", this.VendaBilheteriaIDCancel.ValorBD);
            sql.Replace("@003", this.StatusCancel.ValorBD);
            sql.Replace("@004", this.SupervisorID.ValorBD);
            sql.Replace("@005", this.NumeroChamado.ValorBD);
            sql.Replace("@006", this.CaixaID.ValorBD);
            sql.Replace("@007", this.LocalID.ValorBD);
            sql.Replace("@008", this.LojaID.ValorBD);
            sql.Replace("@009", this.CanalID.ValorBD);
            sql.Replace("@010", this.UsuarioID.ValorBD);
            sql.Replace("@011", this.EmpresaID.ValorBD);
            sql.Replace("@012", this.TipoCancelamento.ValorBD);
            sql.Replace("@013", this.FormaDevolucao.ValorBD);
            sql.Replace("@014", this.MotivoCancelamento.ValorBD);
            sql.Replace("@015", this.SubMotivoCancelamento.ValorBD);
            sql.Replace("@016", this.VlrIngressoEstornado.ValorBD);
            sql.Replace("@017", this.VlrTxEntregaEstornado.ValorBD);
            sql.Replace("@018", this.VlrTxConvenienciaEstornado.ValorBD);
            sql.Replace("@019", this.VlrSeguroEstornado.ValorBD);


            this.Control.ID = Convert.ToInt32(bd.ConsultaValor(sql.ToString()));

            return this.Control.ID > 0;


        }


        /// <summary>
        /// Atualiza Pendencia
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tCancelDevolucaoPendente SET VendaBilheteriaIDVenda = '@001', VendaBilheteriaIDCancel = '@002', StatusCancel = @003, SupervisorID = @004, NumeroChamado  = '@005', CaixaID = @006, LocalID = @007, LojaID = @008, CanalID = @009, UsuarioID = @010, EmpresaID = @011,  TipoCancelamento = @012, FormaDevolucao = @013, MotivoCancelamento = @014, SubMotivoCancelamento = @015, VlrIngressoEstornado = @016, VlrTxEntregaEstornado = @017, VlrTxConvenienciaEstornado = @018, VlrSeguroEstornado = @019 ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.VendaBilheteriaIDVenda.ValorBD);
                sql.Replace("@002", this.VendaBilheteriaIDCancel.ValorBD);
                sql.Replace("@003", this.StatusCancel.ValorBD);
                sql.Replace("@004", this.SupervisorID.ValorBD);
                sql.Replace("@005", this.NumeroChamado.ValorBD);
                sql.Replace("@006", this.CaixaID.ValorBD);
                sql.Replace("@007", this.LocalID.ValorBD);
                sql.Replace("@008", this.LojaID.ValorBD);
                sql.Replace("@009", this.CanalID.ValorBD);
                sql.Replace("@010", this.UsuarioID.ValorBD);
                sql.Replace("@011", this.EmpresaID.ValorBD);
                sql.Replace("@012", this.TipoCancelamento.ValorBD);
                sql.Replace("@013", this.FormaDevolucao.ValorBD);
                sql.Replace("@013", this.MotivoCancelamento.ValorBD);
                sql.Replace("@014", this.SubMotivoCancelamento.ValorBD);
                sql.Replace("@016", this.VlrIngressoEstornado.ValorBD);
                sql.Replace("@017", this.VlrTxEntregaEstornado.ValorBD);
                sql.Replace("@018", this.VlrTxConvenienciaEstornado.ValorBD);
                sql.Replace("@019", this.VlrSeguroEstornado.ValorBD);


                int x = bd.Executar(sql.ToString());
                bd.Fechar();

                bool result = Convert.ToBoolean(x);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// Atualiza Pendencia
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE tCancelDevolucaoPendente SET VendaBilheteriaIDVenda = '@001', VendaBilheteriaIDCancel = '@002', StatusCancel = @003, SupervisorID = @004, NumeroChamado  = '@005', CaixaID = @006, LocalID = @007, LojaID = @008, CanalID = @009, UsuarioID = @010, EmpresaID = @011,  TipoCancelamento = @012, FormaDevolucao = @013, MotivoCancelamento = @014, SubMotivoCancelamento = @015, VlrIngressoEstornado = @016, VlrTxEntregaEstornado = @017, VlrTxConvenienciaEstornado = @018, VlrSeguroEstornado = @019 ");
            sql.Append("WHERE ID = @ID");
            sql.Replace("@ID", this.Control.ID.ToString());
            sql.Replace("@001", this.VendaBilheteriaIDVenda.ValorBD);
            sql.Replace("@002", this.VendaBilheteriaIDCancel.ValorBD);
            sql.Replace("@003", this.StatusCancel.ValorBD);
            sql.Replace("@004", this.SupervisorID.ValorBD);
            sql.Replace("@005", this.NumeroChamado.ValorBD);
            sql.Replace("@006", this.CaixaID.ValorBD);
            sql.Replace("@007", this.LocalID.ValorBD);
            sql.Replace("@008", this.LojaID.ValorBD);
            sql.Replace("@009", this.CanalID.ValorBD);
            sql.Replace("@010", this.UsuarioID.ValorBD);
            sql.Replace("@011", this.EmpresaID.ValorBD);
            sql.Replace("@012", this.TipoCancelamento.ValorBD);
            sql.Replace("@013", this.FormaDevolucao.ValorBD);
            sql.Replace("@013", this.MotivoCancelamento.ValorBD);
            sql.Replace("@014", this.SubMotivoCancelamento.ValorBD);
            sql.Replace("@016", this.VlrIngressoEstornado.ValorBD);
            sql.Replace("@017", this.VlrTxEntregaEstornado.ValorBD);
            sql.Replace("@018", this.VlrTxConvenienciaEstornado.ValorBD);
            sql.Replace("@019", this.VlrSeguroEstornado.ValorBD);

            int x = bd.Executar(sql.ToString());

            bool result = Convert.ToBoolean(x);

            return result;


        }


        /// <summary>
        /// Exclui Pendencia com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {

            try
            {

                string sqlDelete = "DELETE FROM tArquivo WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);
                bd.Fechar();

                bool result = Convert.ToBoolean(x);
                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// Exclui Pendencia com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {


            string sqlDelete = "DELETE FROM tArquivo WHERE ID=" + id;

            int x = bd.Executar(sqlDelete);

            bool result = Convert.ToBoolean(x);
            return result;

        }

        /// <summary>
        /// Exclui Pendencia
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

            this.VendaBilheteriaIDVenda.Limpar();
            this.VendaBilheteriaIDCancel.Limpar();
            this.StatusCancel.Limpar();
            this.SupervisorID.Limpar();
            this.NumeroChamado.Limpar();
            this.CaixaID.Limpar();
            this.LojaID.Limpar();
            this.CanalID.Limpar();
            this.UsuarioID.Limpar();
            this.EmpresaID.Limpar();
            this.TipoCancelamento.Limpar();
            this.FormaDevolucao.Limpar();
            this.MotivoCancelamento.Limpar();
            this.SubMotivoCancelamento.Limpar();
            this.VlrIngressoEstornado.Limpar();
            this.VlrTxEntregaEstornado.Limpar();
            this.VlrTxConvenienciaEstornado.Limpar();
            this.VlrSeguroEstornado.Limpar();
            
            this.CaixaIDDevolucao.Limpar();
            this.LocalIDDevolucao.Limpar();
            this.LojaIDDevolucao.Limpar();
            this.CanalIDDevolucao.Limpar();
            this.EmpresaIDDevolucao.Limpar();
            this.UsuarioIDDevolucao.Limpar();

            this.Control.ID = 0;
            this.Control.Versao = 0;
        }

        public override void Desfazer()
        {
            this.VendaBilheteriaIDVenda.Desfazer();
            this.VendaBilheteriaIDCancel.Desfazer();
            this.StatusCancel.Desfazer();
            this.SupervisorID.Desfazer();
            this.NumeroChamado.Desfazer();
            this.CaixaID.Desfazer();
            this.LojaID.Desfazer();
            this.CanalID.Desfazer();
            this.UsuarioID.Desfazer();
            this.EmpresaID.Desfazer();
            this.TipoCancelamento.Desfazer();
            this.FormaDevolucao.Desfazer();
            this.MotivoCancelamento.Desfazer();
            this.SubMotivoCancelamento.Desfazer();
            this.VlrIngressoEstornado.Desfazer();
            this.VlrTxEntregaEstornado.Desfazer();
            this.VlrTxConvenienciaEstornado.Desfazer();
            this.VlrSeguroEstornado.Desfazer();

            this.CaixaIDDevolucao.Desfazer();
            this.LocalIDDevolucao.Desfazer();
            this.LojaIDDevolucao.Desfazer();
            this.CanalIDDevolucao.Desfazer();
            this.EmpresaIDDevolucao.Desfazer();
            this.UsuarioIDDevolucao.Desfazer();

        }



        public class vendabilheteriaidvenda : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "VendaBilheteriaIDVenda";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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
        public class vendabilheteriaidcancel : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "VendaBilheteriaIDCancel";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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
        public class statuscancel : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "StatusCancel";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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
        public class supervisorid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "SupervisorID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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
        public class numerochamado : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "NumeroChamado";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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
        public class caixaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "CaixaID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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
        public class localid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "LocalID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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
        public class lojaid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "LojaID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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
        public class canalid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "CanalID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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
        public class usuarioid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "UsuarioID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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
                    return 1;
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
        public class tipocancelamento : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "TipoCancelamento";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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
        public class formadevolucao : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "FormaDevolucao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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
        public class motivocancelamento : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "MotivoCancelamento";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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
        public class submotivocancelamento : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "SubMotivoCancelamento";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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

        public class vlringressoestornado : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "VlrIngressoEstornado";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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
        public class vlrtxentregaestornado : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "VlrTxEntregaEstornado";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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
        public class vlrtxconvenienciaestornado : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "VlrTxConvenienciaEstornado";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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
        public class vlrseguroestornado : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "VlrSeguroEstornado";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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

        public class caixaiddevolucao : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "CaixaIDDevolucao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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
        public class localiddevolucao : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "LocalIDDevolucao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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
        public class lojaiddevolucao : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "LojaIDDevolucao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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
        public class canaliddevolucao : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "CanalIDDevolucao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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
        public class empresaiddevolucao : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "EmpresaIDDevolucao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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
        public class usuarioiddevolucao : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "UsuarioIDDevolucao";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 1;
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
    }

    #endregion

    #region CancelDevolucaoPendenteLista_B
    public abstract class CancelDevolucaoPendenteLista_B : BaseLista
    {

        protected CancelDevolucaoPendente cancelDevolucaoPendente;

        // passar o Usuario logado no sistema
        public CancelDevolucaoPendenteLista_B()
        {
            cancelDevolucaoPendente = new CancelDevolucaoPendente();
        }

        public CancelDevolucaoPendente CancelDevolucaoPendente
        {
            get { return cancelDevolucaoPendente; }
        }

        /// <summary>
        /// Retorna um IBaseBD de Pendencia especifico(a)
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
                    cancelDevolucaoPendente.Ler(id);
                    return cancelDevolucaoPendente;
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
                    sql = "SELECT ID FROM tCancelDevolucaoPendente";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tCancelDevolucaoPendente";

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
                    sql = "SELECT ID FROM tCancelDevolucaoPendente";
                else
                    sql = "SELECT top " + tamanhoMax + " ID FROM tCancelDevolucaoPendente";

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
        /// Preenche pendencia corrente da lista
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        protected override void Ler(int id)
        {

            try
            {

                cancelDevolucaoPendente.Ler(id);

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

                bool ok = cancelDevolucaoPendente.Excluir();
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

                    try
                    {
                        string ids = ToString();

                        string sqlDelete = "DELETE FROM tCancelDevolucaoPendente WHERE ID in (" + ids + ")";

                        int x = bd.Executar(sqlDelete);
                        bd.Fechar();

                        ok = Convert.ToBoolean(x);

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

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
        /// Inseri novo(a) Pendencia na lista
        /// </summary>
        /// <returns></returns>		
        public override bool Inserir()
        {

            try
            {

                bool ok = cancelDevolucaoPendente.Inserir();
                if (ok)
                {
                    lista.Add(cancelDevolucaoPendente.Control.ID);
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
        /// Obtem uma tabela de todos os campos de Pendencias carregados na lista
        /// </summary>
        /// <returns></returns>
        public override DataTable Tabela()
        {

            try
            {

                DataTable tabela = new DataTable("CancelDevolucaoPendente");

                tabela.Columns.Add("ID", typeof(int));
                tabela.Columns.Add("VendaBilheteriaIDVenda", typeof(int));
                tabela.Columns.Add("VendaBilheteriaIDCancel", typeof(int));
                tabela.Columns.Add("StatusCancel", typeof(string));

                if (this.Primeiro())
                {

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["ID"] = cancelDevolucaoPendente.Control.ID;
                        linha["VendaBilheteriaIDVenda"] = cancelDevolucaoPendente.VendaBilheteriaIDVenda.Valor;
                        linha["VendaBilheteriaIDCancel"] = cancelDevolucaoPendente.VendaBilheteriaIDCancel.Valor;
                        linha["StatusCancel"] = cancelDevolucaoPendente.StatusCancel.Valor;
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

                DataTable tabela = new DataTable("RelatorioCancelDevolucaoPendente");

                if (this.Primeiro())
                {

                    tabela.Columns.Add("VendaBilheteriaIDVenda", typeof(int));
                    tabela.Columns.Add("VendaBilheteriaIDCancel", typeof(int));
                    tabela.Columns.Add("StatusCancel", typeof(string));

                    do
                    {
                        DataRow linha = tabela.NewRow();
                        linha["VendaBilheteriaIDVenda"] = cancelDevolucaoPendente.VendaBilheteriaIDVenda.Valor;
                        linha["VendaBilheteriaIDCancel"] = cancelDevolucaoPendente.VendaBilheteriaIDCancel.Valor;
                        linha["StatusCancel"] = cancelDevolucaoPendente.StatusCancel.Valor;
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
                    case "VendaBilheteriaIDVenda":
                        sql = "SELECT ID, VendaBilheteriaIDVenda FROM tCancelDevolucaoPendente WHERE " + FiltroSQL + " ORDER BY VendaBilheteriaIDVenda";
                        break;
                    case "VendaBilheteriaIDCancel":
                        sql = "SELECT ID, VendaBilheteriaIDCancel FROM tCancelDevolucaoPendente WHERE " + FiltroSQL + " ORDER BY VendaBilheteriaIDCancel";
                        break;
                    case "StatusCancel":
                        sql = "SELECT ID, StatusCancel FROM tCancelDevolucaoPendente WHERE " + FiltroSQL + " ORDER BY StatusCancel";
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

    #region "CancelDevolucaoPendenteException"

    [Serializable]
    public class CancelDevolucaoPendenteException : Exception
    {

        public CancelDevolucaoPendenteException() : base() { }

        public CancelDevolucaoPendenteException(string msg) : base(msg) { }

        public CancelDevolucaoPendenteException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

    #endregion

}
