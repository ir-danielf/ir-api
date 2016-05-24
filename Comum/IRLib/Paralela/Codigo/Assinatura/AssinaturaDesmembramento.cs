/**************************************************
* Arquivo: AssinaturaDesmembramento.cs
* Gerado: 04/01/2012
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.Paralela.ClientObjects;
using IRLib.Paralela.ClientObjects.Assinaturas;
using System;
using System.Text;

namespace IRLib.Paralela
{

    public class AssinaturaDesmembramento : AssinaturaDesmembramento_B
    {
        int UsuarioLogado { get; set; }
        public AssinaturaDesmembramento() { }

        public AssinaturaDesmembramento(int usuarioID)
        {
            UsuarioLogado = usuarioID;
        }

        public void Desmembrar(int clienteID, EstruturaAssinaturaBloqueio assinatura, string motivo)
        {
            try
            {
                bd.IniciarTransacao();
                this.LerPelaAssinatura(assinatura.AssinaturaClienteID);

                new AssinaturaBancoIngresso().AlterarCliente(bd, assinatura.AssinaturaClienteID, assinatura.ClienteID, this.ClienteID.Valor, clienteID);

                this.RegistrarDesmembramento(bd, clienteID, assinatura, motivo);
                this.TornarDesmembrado(bd, assinatura.AssinaturaClienteID, clienteID, true);
                bd.FinalizarTransacao();
            }
            catch (Exception)
            {
                bd.DesfazerTransacao();
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public void LerPelaAssinatura(int assinaturaClienteID)
        {

            try
            {
                BD bd = new BD();
                string sql = "SELECT * FROM tAssinaturaDesmembramento WHERE AssinaturaClienteID = " + assinaturaClienteID;
                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    this.Control.ID = bd.LerInt("ID");
                    this.ClienteID.ValorBD = bd.LerInt("ClienteID").ToString();
                    this.AntigoClienteID.ValorBD = bd.LerInt("AntigoClienteID").ToString();
                    this.AssinaturaClienteID.ValorBD = bd.LerInt("AssinaturaClienteID").ToString();
                    this.TimeStamp.ValorBD = bd.LerString("TimeStamp");
                    this.UsuarioID.ValorBD = bd.LerInt("UsuarioID").ToString();
                    this.Motivo.ValorBD = bd.LerString("Motivo");
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

        private void RegistrarDesmembramento(BD bd, int clienteID, EstruturaAssinaturaBloqueio assinatura, string motivo)
        {

            if (this.Control.ID == 0)
                this.AntigoClienteID.Valor = assinatura.ClienteID;

            this.ClienteID.Valor = clienteID;
            this.AssinaturaClienteID.Valor = assinatura.AssinaturaClienteID;
            this.TimeStamp.Valor = DateTime.Now;
            this.Motivo.Valor = motivo;
            this.UsuarioID.Valor = UsuarioLogado;

            if (this.Control.ID == 0)
                this.Inserir(bd);
            else
                this.Atualizar(bd);

        }

        private void TornarDesmembrado(BD bd, int assinaturaClienteID, int clienteID, bool desmembrada)
        {
            BD bdAux = new BD();

            try
            {

                if (clienteID <= 0)
                    throw new Exception("Não foi possível desmembrar, Cliente não encontrado");
                
                var desmembra = desmembrada ? 'T' : 'F';
                
                StringBuilder stbDesmembramento = new StringBuilder();
                stbDesmembramento.AppendFormat("UPDATE tIngresso SET ClienteID = {0} ", clienteID);
                stbDesmembramento.AppendFormat("WHERE AssinaturaClienteID = {0}", assinaturaClienteID);

                if (bd.Executar(stbDesmembramento.ToString()) == 0)
                    throw new Exception("Não foi possível efetuar a ação de desmembramento de um dos ingressos.");

                stbDesmembramento = new StringBuilder();
                stbDesmembramento.AppendFormat("UPDATE tAssinaturaCliente SET ClienteID = {0},Desmembrada = '" + desmembra + "' ", clienteID);
                stbDesmembramento.AppendFormat("WHERE ID = {0}", assinaturaClienteID);

                if (bd.Executar(stbDesmembramento.ToString()) == 0)
                    throw new Exception("Não foi possível efetuar a ação de desmembramento de um dos ingressos.");


            }
            finally
            {
                bdAux.Fechar();
            }
        }

        public EstruturaAssinaturaDesmembramento CarregarEstrutura(int assinaturaClienteID)
        {
            try
            {
                EstruturaAssinaturaDesmembramento retorno = new EstruturaAssinaturaDesmembramento();
                string sql = @"SELECT ad.Motivo,l.Codigo as Lugar, s.Nome as Setor,a.Nome as Assinatura,
                            aa.Ano as Temporada,u.Nome as Usuario, c.Nome as ClienteAtual, cd.Nome as ClienteAntigo,
                            ac.Desmembrada
                            FROM tAssinaturaCliente ac (NOLOCK) 
                            INNER JOIN tLugar l (NOLOCK) ON l.ID = ac.LugarID
                            INNER JOIN tSetor s (NOLOCK) ON s.ID = ac.SetorID
                            INNER JOIN tAssinatura a (NOLOCK) ON a.ID = ac.AssinaturaID
                            INNER JOIN tAssinaturaAno aa (NOLOCK) ON aa.ID = ac.AssinaturaAnoID
                            INNER JOIN tCliente c (NOLOCK) ON c.ID = ac.ClienteID

                            LEFT JOIN tAssinaturaDesmembramento ad (NOLOCK) ON ac.ID = ad.AssinaturaClienteID
                            LEFT JOIN tUsuario u (NOLOCK) ON u.ID = ad.UsuarioID
                            LEFT JOIN tCliente cd (NOLOCK) ON cd.ID = ad.AntigoClienteID

                            WHERE ac.ID = " + assinaturaClienteID;

                bd.Consulta(sql);

                if (bd.Consulta().Read())
                {
                    bool desmembrada = bd.LerBoolean("Desmembrada");
                    retorno.Assinatura = bd.LerString("Assinatura");
                    retorno.Temporada = bd.LerString("Temporada");
                    retorno.Setor = bd.LerString("Setor");
                    retorno.Lugar = bd.LerString("Lugar");
                    retorno.ClienteResponsavel = desmembrada ? bd.LerString("ClienteAntigo") : bd.LerString("ClienteAtual");
                    retorno.ClienteNovo = desmembrada ? bd.LerString("ClienteAtual") : "";
                    retorno.Motivo = bd.LerString("Motivo");
                    retorno.Usuario = bd.LerString("Usuario");
                    retorno.Notificacao = desmembrada ? 1 : 2;
                }

                bd.Fechar();

                return retorno;

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

        public void RemoverDesmembramento(EstruturaAssinaturaBloqueio assinatura)
        {
            try
            {
                bd.IniciarTransacao();
                this.LerPelaAssinatura(assinatura.AssinaturaClienteID);

                new AssinaturaBancoIngresso().AlterarCliente(bd, assinatura.AssinaturaClienteID, assinatura.ClienteID, this.AntigoClienteID.Valor, 0);

                this.TornarDesmembrado(bd, assinatura.AssinaturaClienteID, this.AntigoClienteID.Valor, false);
                this.Excluir(bd, this.Control.ID);
                bd.FinalizarTransacao();
            }
            catch (Exception)
            {
                bd.DesfazerTransacao();
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }


    }

    public class AssinaturaDesmembramentoLista : AssinaturaDesmembramentoLista_B
    {
        public AssinaturaDesmembramentoLista() { }
    }

}
