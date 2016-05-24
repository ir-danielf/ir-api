using CTLib;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;

namespace IRLib
{

    /// <summary>
    /// Gerenciador do ImagemAtualizador
    /// </summary>
    [ObjectType(ObjectType.RemotingType.SingleCall)]
    public class ImagemAtualizador : MarshalByRefObject
    {

        public const int TAM_MAX_IMAGEM = 70; //em Kbytes (* 1024) = Bytes
        public const int TAM_MAX_IMAGEM_CANHOTO = 15; //em Kbytes (* 1024) = Bytes

        public ImagemAtualizador() { }

        /// <summary>
        /// Atualiza a cor dos preços com o mesmo nome no evento e retorna os PrecoIDs
        /// </summary>
        /// <returns></returns>
        public int[] AtualizarCor(int apresentacaoSetorID, string nome, int corID)
        {

            BD bd = new BD();

            try
            {

                string sqlApresentacaoID = "SELECT ApresentacaoID FROM tApresentacaoSetor WHERE ID=" + apresentacaoSetorID;
                object ret = bd.ConsultaValor(sqlApresentacaoID);
                int apresentacaoID = (ret != null) ? Convert.ToInt32(ret) : 0;

                string sqlEventoID = "SELECT EventoID FROM tApresentacao WHERE ID=" + apresentacaoID;
                ret = bd.ConsultaValor(sqlEventoID);
                int eventoID = (ret != null) ? Convert.ToInt32(ret) : 0;

                string sql = "SELECT tPreco.ID AS PrecoID " +
                    "FROM tPreco (NOLOCK) " +
                    "INNER JOIN tApresentacaoSetor ON tApresentacaoSetor.ID = tPreco.ApresentacaoSetorID " +
                    "INNER JOIN tApresentacao ON tApresentacao.ID = tApresentacaoSetor.ApresentacaoID " +
                    "WHERE tApresentacao.EventoID=" + eventoID + " AND tPreco.Nome='" + nome + "'";
                bd.Consulta(sql);

                ArrayList precosIDs = new ArrayList();

                while (bd.Consulta().Read())
                {
                    precosIDs.Add(bd.LerInt("PrecoID"));
                }
                bd.Consulta().Close();

                precosIDs.TrimToSize();

                string sqlUpdate = "UPDATE tPreco SET CorID=" + corID + " " +
                    "WHERE ID in (" + Utilitario.ArrayToString(precosIDs) + ")";
                bd.Executar(sqlUpdate);

                return (int[])precosIDs.ToArray(typeof(int));

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
        /// Carrega as apresentacoes/setores
        /// </summary>
        /// <returns></returns>


        public DataSet Carregar()
        {
            BD bd = new BD();

            try
            {
                DataSet buffer = new DataSet("Buffer");

                DataTable eventos = new DataTable("Evento");
                eventos.Columns.Add("ID", typeof(int));
                eventos.Columns.Add("Nome", typeof(string));
                eventos.Columns.Add("VersaoImagemIngresso", typeof(int));
                eventos.Columns.Add("VersaoImagemVale", typeof(int));
                eventos.Columns.Add("VersaoImagemVale2", typeof(int));
                eventos.Columns.Add("VersaoImagemVale3", typeof(int));

                DataTable apresentacoes = new DataTable("Apresentacao");
                apresentacoes.Columns.Add("ID", typeof(int));
                apresentacoes.Columns.Add("Horario", typeof(string));
                apresentacoes.Columns.Add("Evento", typeof(string));
                apresentacoes.Columns.Add("EventoID", typeof(int));
                apresentacoes.Columns.Add("VersaoImagemIngresso", typeof(int));
                apresentacoes.Columns.Add("VersaoImagemVale", typeof(int));
                apresentacoes.Columns.Add("VersaoImagemVale2", typeof(int));
                apresentacoes.Columns.Add("VersaoImagemVale3", typeof(int));

                DataTable apresentacaoSetores = new DataTable("ApresentacaoSetor");
                apresentacaoSetores.Columns.Add("ID", typeof(int));
                apresentacaoSetores.Columns.Add("Evento", typeof(string));
                apresentacaoSetores.Columns.Add("Horario", typeof(string));
                apresentacaoSetores.Columns.Add("VersaoImagemIngresso", typeof(int));
                apresentacaoSetores.Columns.Add("VersaoImagemVale", typeof(int));
                apresentacaoSetores.Columns.Add("VersaoImagemVale2", typeof(int));
                apresentacaoSetores.Columns.Add("VersaoImagemVale3", typeof(int));

                DataTable localImagens = new DataTable("LocalImagemMapa");
                localImagens.Columns.Add("ID", typeof(int));
                localImagens.Columns.Add("Nome", typeof(string));
                localImagens.Columns.Add("VersaoImagem", typeof(int));

                DataTable valeIngressoTipo = new DataTable("ValeIngressoTipo");
                valeIngressoTipo.Columns.Add("ID");
                valeIngressoTipo.Columns.Add("Nome");
                valeIngressoTipo.Columns.Add("VersaoImagem");
                valeIngressoTipo.Columns.Add("VersaoImagemInternet");

                DataTable setor = new DataTable("Setor");
                setor.Columns.Add("ID", typeof(int));
                setor.Columns.Add("Nome", typeof(string));
                setor.Columns.Add("VersaoBackground", typeof(int));

                string hoje = System.DateTime.Today.ToString("yyyyMMdd") + "000000";

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("SELECT DISTINCT tEvento.ID , tEvento.Nome, ");
                sb.Append("tEvento.VersaoImagemIngresso, ");
                sb.Append("tEvento.VersaoImagemVale, ");
                sb.Append("tEvento.VersaoImagemVale2, ");
                sb.Append("tEvento.VersaoImagemVale3 ");
                sb.Append("FROM tEvento (NOLOCK) ");
                sb.Append("LEFT join tApresentacao (NOLOCK) on tApresentacao.EventoID = tEvento.ID   ");
                sb.Append("LEFT join tApresentacaoSetor (NOLOCK) on tApresentacaoSetor.apresentacaoid = tApresentacao.id ");
                sb.Append("LEFT JOIN tLocalImagemMapa (NOLOCK) ON tEvento.LocalID = tLocalImagemMapa.LocalID ");
                sb.Append("WHERE tEvento.ID=tApresentacao.EventoID ");
                sb.Append("AND tApresentacaoSetor.ApresentacaoID=tApresentacao.ID AND ");
                sb.Append("(tApresentacao.DisponivelVenda='T' OR tApresentacao.DisponivelAjuste='T') ");
                sb.Append("AND tApresentacao.Horario >= '" + hoje + "' ");
                SqlCommand cmd = new SqlCommand(sb.ToString(), (SqlConnection)bd.Cnn);
                SqlDataAdapter Adapter = new SqlDataAdapter(cmd);
                Adapter.Fill(eventos);
                Adapter.Dispose();
                cmd.Dispose();

                sb = new System.Text.StringBuilder();
                sb.Append("SELECT DISTINCT tApresentacao.ID, tApresentacao.Horario, ");
                sb.Append("tEvento.ID AS EventoID, tEvento.Nome AS Evento, ");
                sb.Append("tApresentacao.VersaoImagemIngresso, ");
                sb.Append("tApresentacao.VersaoImagemVale, ");
                sb.Append("tApresentacao.VersaoImagemVale2, ");
                sb.Append("tApresentacao.VersaoImagemVale3 ");
                sb.Append("FROM tApresentacao (NOLOCK) ");
                sb.Append("LEFT JOIN tEvento (NOLOCK) ON tEvento.ID = tApresentacao.EventoID ");
                sb.Append("LEFT join tApresentacaoSetor (NOLOCK) on tApresentacaoSetor.apresentacaoid = tApresentacao.id ");
                sb.Append("LEFT JOIN tLocalImagemMapa (NOLOCK) ON tEvento.LocalID = tLocalImagemMapa.LocalID ");
                sb.Append("WHERE tEvento.ID=tApresentacao.EventoID ");
                sb.Append("AND tApresentacaoSetor.ApresentacaoID=tApresentacao.ID AND ");
                sb.Append("(tApresentacao.DisponivelVenda='T' OR tApresentacao.DisponivelAjuste='T') ");
                sb.Append("AND tApresentacao.Horario >= '" + hoje + "' ");
                cmd = new SqlCommand(sb.ToString(), (SqlConnection)bd.Cnn);
                Adapter = new SqlDataAdapter(cmd);
                Adapter.Fill(apresentacoes);
                Adapter.Dispose();
                cmd.Dispose();

                sb = new System.Text.StringBuilder();
                sb.Append("SELECT DISTINCT tLocalImagemMapa.ID, tLocalImagemMapa.Nome, ");
                sb.Append("tLocalImagemMapa.VersaoImagem ");
                sb.Append("FROM tApresentacao (NOLOCK) ");
                sb.Append("LEFT JOIN tEvento (NOLOCK) ON tEvento.ID = tApresentacao.EventoID ");
                sb.Append("LEFT join tApresentacaoSetor (NOLOCK) on tApresentacaoSetor.apresentacaoid = tApresentacao.id ");
                sb.Append("LEFT JOIN tLocalImagemMapa (NOLOCK) ON tEvento.LocalID = tLocalImagemMapa.LocalID ");
                sb.Append("WHERE tEvento.ID=tApresentacao.EventoID ");
                sb.Append("AND tApresentacaoSetor.ApresentacaoID=tApresentacao.ID AND ");
                sb.Append("(tApresentacao.DisponivelVenda='T' OR tApresentacao.DisponivelAjuste='T') ");
                sb.Append("AND tApresentacao.Horario >= '" + hoje + "' ");
                sb.Append("AND tLocalImagemMapa.ID IS NOT NULL ");
                cmd = new SqlCommand(sb.ToString(), (SqlConnection)bd.Cnn);
                Adapter = new SqlDataAdapter(cmd);
                Adapter.Fill(localImagens);
                Adapter.Dispose();
                cmd.Dispose();

                sb = new System.Text.StringBuilder();
                sb.Append("SELECT DISTINCT tApresentacaoSetor.ID, ");
                sb.Append("tApresentacaoSetor.VersaoImagemIngresso, ");
                sb.Append("tEvento.Nome as Evento, tApresentacao.Horario, ");
                sb.Append("tApresentacaoSetor.VersaoImagemVale, ");
                sb.Append("tApresentacaoSetor.VersaoImagemVale2, ");
                sb.Append("tApresentacaoSetor.VersaoImagemVale3 ");
                sb.Append("FROM tApresentacao (NOLOCK) ");
                sb.Append("LEFT JOIN tEvento (NOLOCK) ON tEvento.ID = tApresentacao.EventoID ");
                sb.Append("LEFT join tApresentacaoSetor (NOLOCK) on tApresentacaoSetor.apresentacaoid = tApresentacao.id ");
                sb.Append("LEFT JOIN tLocalImagemMapa (NOLOCK) ON tEvento.LocalID = tLocalImagemMapa.LocalID ");
                sb.Append("WHERE tEvento.ID=tApresentacao.EventoID ");
                sb.Append("AND tApresentacaoSetor.ApresentacaoID=tApresentacao.ID AND ");
                sb.Append("(tApresentacao.DisponivelVenda='T' OR tApresentacao.DisponivelAjuste='T') ");
                sb.Append("AND tApresentacao.Horario >= '" + hoje + "' ");
                cmd = new SqlCommand(sb.ToString(), (SqlConnection)bd.Cnn);
                Adapter = new SqlDataAdapter(cmd);
                Adapter.Fill(apresentacaoSetores);
                Adapter.Dispose();
                cmd.Dispose();

                sb = new System.Text.StringBuilder();
                sb.Append("SELECT DISTINCT vit.ID, vit.Nome, IsNull(vit.VersaoImagem,0 ) AS VersaoImagem, IsNull(vit.VersaoImagemInternet, 0) VersaoImagemInternet ");
                sb.Append("FROM tValeIngressoTipo vit (NOLOCK) WHERE vit.ValidadeData = '' OR vit.ValidadeData > '" + hoje + "'");
                cmd = new SqlCommand(sb.ToString(), (SqlConnection)bd.Cnn);
                Adapter = new SqlDataAdapter(cmd);
                Adapter.Fill(apresentacaoSetores);
                Adapter.Dispose();
                cmd.Dispose();

                sb = new System.Text.StringBuilder();
                sb.Append("SELECT DISTINCT tSetor.ID, tSetor.Nome, IsNull(tSetor.VersaoBackground, 0) AS VersaoBackground ");
                sb.Append("FROM tSetor (NOLOCK) ");
                sb.Append("INNER JOIN tApresentacaoSetor (NOLOCK) ON tSetor.ID = tApresentacaoSetor.SetorID ");
                sb.Append("INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tApresentacaoSetor.ApresentacaoID ");
                sb.Append("WHERE tApresentacao.DisponivelVenda = 'T' AND tApresentacao.Horario > '" + hoje + "'");
                cmd = new SqlCommand(sb.ToString(), (SqlConnection)bd.Cnn);
                Adapter = new SqlDataAdapter(cmd);
                Adapter.Fill(setor);
                Adapter.Dispose();
                cmd.Dispose();


                buffer.Tables.Add(eventos);
                buffer.Tables.Add(apresentacoes);
                buffer.Tables.Add(apresentacaoSetores);
                buffer.Tables.Add(localImagens);
                buffer.Tables.Add(valeIngressoTipo);
                buffer.Tables.Add(setor);

                return buffer;

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

        public DataSet CarregarCanal(int canalID)
        {
            BD bd = new BD();

            try
            {
                DataSet buffer = new DataSet("Buffer");

                DataTable eventos = new DataTable("Evento");
                eventos.Columns.Add("ID", typeof(int));
                eventos.Columns.Add("Nome", typeof(string));
                eventos.Columns.Add("VersaoImagemIngresso", typeof(int));
                eventos.Columns.Add("VersaoImagemVale", typeof(int));
                eventos.Columns.Add("VersaoImagemVale2", typeof(int));
                eventos.Columns.Add("VersaoImagemVale3", typeof(int));

                DataTable apresentacoes = new DataTable("Apresentacao");
                apresentacoes.Columns.Add("ID", typeof(int));
                apresentacoes.Columns.Add("Horario", typeof(string));
                apresentacoes.Columns.Add("Evento", typeof(string));
                apresentacoes.Columns.Add("EventoID", typeof(int));
                apresentacoes.Columns.Add("VersaoImagemIngresso", typeof(int));
                apresentacoes.Columns.Add("VersaoImagemVale", typeof(int));
                apresentacoes.Columns.Add("VersaoImagemVale2", typeof(int));
                apresentacoes.Columns.Add("VersaoImagemVale3", typeof(int));

                DataTable apresentacaoSetores = new DataTable("ApresentacaoSetor");
                apresentacaoSetores.Columns.Add("ID", typeof(int));
                apresentacaoSetores.Columns.Add("Evento", typeof(string));
                apresentacaoSetores.Columns.Add("Horario", typeof(string));
                apresentacaoSetores.Columns.Add("VersaoImagemIngresso", typeof(int));
                apresentacaoSetores.Columns.Add("VersaoImagemVale", typeof(int));
                apresentacaoSetores.Columns.Add("VersaoImagemVale2", typeof(int));
                apresentacaoSetores.Columns.Add("VersaoImagemVale3", typeof(int));

                DataTable localImagens = new DataTable("LocalImagemMapa");
                localImagens.Columns.Add("ID", typeof(int));
                localImagens.Columns.Add("Nome", typeof(string));
                localImagens.Columns.Add("VersaoImagem", typeof(int));

                DataTable valeIngressoTipo = new DataTable("ValeIngressoTipo");
                valeIngressoTipo.Columns.Add("ID");
                valeIngressoTipo.Columns.Add("Nome");
                valeIngressoTipo.Columns.Add("VersaoImagem");
                valeIngressoTipo.Columns.Add("VersaoImagemInternet");

                DataTable setor = new DataTable("Setor");
                setor.Columns.Add("ID", typeof(int));
                setor.Columns.Add("Nome", typeof(string));
                setor.Columns.Add("VersaoBackground", typeof(int));

                string hoje = System.DateTime.Today.ToString("yyyyMMdd") + "000000";

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("SELECT DISTINCT tEvento.ID , tEvento.Nome, ");
                sb.Append("tEvento.VersaoImagemIngresso, ");
                sb.Append("tEvento.VersaoImagemVale, ");
                sb.Append("tEvento.VersaoImagemVale2, ");
                sb.Append("tEvento.VersaoImagemVale3 ");
                sb.Append("FROM tApresentacao (NOLOCK) ");
                sb.Append("LEFT JOIN tEvento (NOLOCK) ON tEvento.ID = tApresentacao.EventoID ");
                sb.Append("LEFT JOIN tCanalEvento (NOLOCK) ON tCanalEvento.EventoID = tEvento.ID ");
                sb.Append("LEFT join tApresentacaoSetor (NOLOCK) on tApresentacaoSetor.apresentacaoid = tApresentacao.id ");
                sb.Append("LEFT JOIN tLocalImagemMapa (NOLOCK) ON tEvento.LocalID = tLocalImagemMapa.LocalID ");
                sb.Append("WHERE tEvento.ID=tApresentacao.EventoID AND tApresentacaoSetor.ApresentacaoID=tApresentacao.ID ");
                sb.Append("AND tCanalEvento.CanalID=" + canalID + " AND tEvento.ID=tCanalEvento.EventoID ");
                sb.Append("AND (tApresentacao.DisponivelVenda='T' OR tApresentacao.DisponivelAjuste='T') ");
                sb.Append("AND tApresentacao.Horario >= '" + hoje + "' ");
                sb.Append("ORDER BY tEvento.Nome");
                SqlCommand cmd = new SqlCommand(sb.ToString(), (SqlConnection)bd.Cnn);
                SqlDataAdapter Adapter = new SqlDataAdapter(cmd);
                Adapter.Fill(eventos);
                Adapter.Dispose();
                cmd.Dispose();

                sb = new System.Text.StringBuilder();
                sb.Append("SELECT DISTINCT tApresentacao.ID, tApresentacao.Horario, ");
                sb.Append("tEvento.ID AS EventoID, tEvento.Nome AS Evento, ");
                sb.Append("tApresentacao.VersaoImagemIngresso, ");
                sb.Append("tApresentacao.VersaoImagemVale, ");
                sb.Append("tApresentacao.VersaoImagemVale2, ");
                sb.Append("tApresentacao.VersaoImagemVale3 ");
                sb.Append("FROM tApresentacao (NOLOCK) ");
                sb.Append("LEFT JOIN tEvento (NOLOCK) ON tEvento.ID = tApresentacao.EventoID ");
                sb.Append("LEFT JOIN tCanalEvento (NOLOCK) ON tCanalEvento.EventoID = tEvento.ID ");
                sb.Append("LEFT join tApresentacaoSetor (NOLOCK) on tApresentacaoSetor.apresentacaoid = tApresentacao.id ");
                sb.Append("LEFT JOIN tLocalImagemMapa (NOLOCK) ON tEvento.LocalID = tLocalImagemMapa.LocalID ");
                sb.Append("WHERE tEvento.ID=tApresentacao.EventoID AND tApresentacaoSetor.ApresentacaoID=tApresentacao.ID ");
                sb.Append("AND tCanalEvento.CanalID=" + canalID + " AND tEvento.ID=tCanalEvento.EventoID ");
                sb.Append("AND (tApresentacao.DisponivelVenda='T' OR tApresentacao.DisponivelAjuste='T') ");
                sb.Append("AND tApresentacao.Horario >= '" + hoje + "' ");
                sb.Append("ORDER BY tEvento.Nome,tApresentacao.Horario");
                cmd = new SqlCommand(sb.ToString(), (SqlConnection)bd.Cnn);
                Adapter = new SqlDataAdapter(cmd);
                Adapter.Fill(apresentacoes);
                Adapter.Dispose();
                cmd.Dispose();

                sb = new System.Text.StringBuilder();
                sb.Append("SELECT DISTINCT tLocalImagemMapa.ID, tLocalImagemMapa.Nome, ");
                sb.Append("tLocalImagemMapa.VersaoImagem ");
                sb.Append("FROM tApresentacao (NOLOCK) ");
                sb.Append("LEFT JOIN tEvento (NOLOCK) ON tEvento.ID = tApresentacao.EventoID ");
                sb.Append("LEFT JOIN tCanalEvento (NOLOCK) ON tCanalEvento.EventoID = tEvento.ID ");
                sb.Append("LEFT join tApresentacaoSetor (NOLOCK) on tApresentacaoSetor.apresentacaoid = tApresentacao.id ");
                sb.Append("LEFT JOIN tLocalImagemMapa (NOLOCK) ON tEvento.LocalID = tLocalImagemMapa.LocalID ");
                sb.Append("WHERE tEvento.ID=tApresentacao.EventoID AND tApresentacaoSetor.ApresentacaoID=tApresentacao.ID ");
                sb.Append("AND tCanalEvento.CanalID=" + canalID + " AND tEvento.ID=tCanalEvento.EventoID ");
                sb.Append("AND (tApresentacao.DisponivelVenda='T' OR tApresentacao.DisponivelAjuste='T') ");
                sb.Append("AND tApresentacao.Horario >= '" + hoje + "' ");
                cmd = new SqlCommand(sb.ToString(), (SqlConnection)bd.Cnn);
                Adapter = new SqlDataAdapter(cmd);
                Adapter.Fill(localImagens);
                Adapter.Dispose();
                cmd.Dispose();

                sb = new System.Text.StringBuilder();
                sb.Append("SELECT DISTINCT tApresentacaoSetor.ID, ");
                sb.Append("tApresentacaoSetor.VersaoImagemIngresso, ");
                sb.Append("tEvento.Nome as Evento, tApresentacao.Horario, ");
                sb.Append("tApresentacaoSetor.VersaoImagemVale, ");
                sb.Append("tApresentacaoSetor.VersaoImagemVale2, ");
                sb.Append("tApresentacaoSetor.VersaoImagemVale3 ");
                sb.Append("FROM tApresentacao (NOLOCK) ");
                sb.Append("LEFT JOIN tEvento (NOLOCK) ON tEvento.ID = tApresentacao.EventoID ");
                sb.Append("LEFT JOIN tCanalEvento (NOLOCK) ON tCanalEvento.EventoID = tEvento.ID ");
                sb.Append("LEFT join tApresentacaoSetor (NOLOCK) on tApresentacaoSetor.apresentacaoid = tApresentacao.id ");
                sb.Append("LEFT JOIN tLocalImagemMapa (NOLOCK) ON tEvento.LocalID = tLocalImagemMapa.LocalID ");
                sb.Append("WHERE tEvento.ID=tApresentacao.EventoID AND tApresentacaoSetor.ApresentacaoID=tApresentacao.ID ");
                sb.Append("AND tCanalEvento.CanalID=" + canalID + " AND tEvento.ID=tCanalEvento.EventoID ");
                sb.Append("AND (tApresentacao.DisponivelVenda='T' OR tApresentacao.DisponivelAjuste='T') ");
                sb.Append("AND tApresentacao.Horario >= '" + hoje + "' ");
                sb.Append("ORDER BY tEvento.Nome,tApresentacao.Horario");
                cmd = new SqlCommand(sb.ToString(), (SqlConnection)bd.Cnn);
                Adapter = new SqlDataAdapter(cmd);
                Adapter.Fill(apresentacaoSetores);
                Adapter.Dispose();
                cmd.Dispose();

                sb = new System.Text.StringBuilder();
                sb.Append("SELECT DISTINCT vit.ID, vit.Nome, IsNull(vit.VersaoImagem,0 ) AS VersaoImagem, IsNull(vit.VersaoImagemInternet, 0) VersaoImagemInternet ");
                sb.Append("FROM tValeIngressoTipo vit (NOLOCK) ");
                sb.Append("INNER JOIN tCanalValeIngresso (NOLOCK) ON tCanalValeIngresso.ValeIngressoTipoID = vit.ID ");
                sb.Append("WHERE tCanalValeIngresso.CanalID = " + canalID + " AND (vit.ValidadeData = '' OR vit.ValidadeData > '" + hoje + "') ");
                sb.Append("ORDER BY vit.Nome");
                cmd = new SqlCommand(sb.ToString(), (SqlConnection)bd.Cnn);
                Adapter = new SqlDataAdapter(cmd);
                Adapter.Fill(valeIngressoTipo);
                Adapter.Dispose();
                cmd.Dispose();

                sb = new System.Text.StringBuilder();
                sb.Append("SELECT DISTINCT tSetor.ID, tSetor.Nome, IsNull(tSetor.VersaoBackground, 0) AS VersaoBackground ");
                sb.Append("FROM tSetor (NOLOCK) ");
                sb.Append("INNER JOIN tApresentacaoSetor (NOLOCK) ON tApresentacaoSetor.SetorID = tSetor.ID ");
                sb.Append("INNER JOIN tApresentacao (NOLOCK) ON tApresentacaoSetor.ApresentacaoID = tApresentacao.ID ");
                sb.Append("INNER JOIN tEvento (NOLOCK) ON tApresentacao.EventoID = tEvento.ID ");
                sb.Append("INNER JOIN tCanalEvento (NOLOCK) ON tEvento.ID = tCanalEvento.EventoID ");
                sb.Append("WHERE tCanalEvento.CanalID = " + canalID + " AND tApresentacao.DisponivelVenda='T' ");
                sb.Append("AND tApresentacao.Horario > '" + hoje + "' ");
                sb.Append("ORDER BY tSetor.ID");
                cmd = new SqlCommand(sb.ToString(), (SqlConnection)bd.Cnn);
                Adapter = new SqlDataAdapter(cmd);
                Adapter.Fill(setor);
                Adapter.Dispose();
                cmd.Dispose();


                buffer.Tables.Add(eventos);
                buffer.Tables.Add(apresentacoes);
                buffer.Tables.Add(apresentacaoSetores);
                buffer.Tables.Add(localImagens);
                buffer.Tables.Add(valeIngressoTipo);
                buffer.Tables.Add(setor);
                return buffer;

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

        public DataSet CarregarLocal(int localID)
        {
            BD bd = new BD();

            try
            {

                DataSet buffer = new DataSet("Buffer");

                DataTable eventos = new DataTable("Evento");
                eventos.Columns.Add("ID", typeof(int));
                eventos.Columns.Add("Nome", typeof(string));
                eventos.Columns.Add("VersaoImagemIngresso", typeof(int));
                eventos.Columns.Add("VersaoImagemVale", typeof(int));
                eventos.Columns.Add("VersaoImagemVale2", typeof(int));
                eventos.Columns.Add("VersaoImagemVale3", typeof(int));

                DataTable apresentacoes = new DataTable("Apresentacao");
                apresentacoes.Columns.Add("ID", typeof(int));
                apresentacoes.Columns.Add("Horario", typeof(string));
                apresentacoes.Columns.Add("Evento", typeof(string));
                apresentacoes.Columns.Add("EventoID", typeof(int));
                apresentacoes.Columns.Add("VersaoImagemIngresso", typeof(int));
                apresentacoes.Columns.Add("VersaoImagemVale", typeof(int));
                apresentacoes.Columns.Add("VersaoImagemVale2", typeof(int));
                apresentacoes.Columns.Add("VersaoImagemVale3", typeof(int));

                DataTable apresentacaoSetores = new DataTable("ApresentacaoSetor");
                apresentacaoSetores.Columns.Add("ID", typeof(int));
                apresentacaoSetores.Columns.Add("Evento", typeof(string));
                apresentacaoSetores.Columns.Add("Horario", typeof(string));
                apresentacaoSetores.Columns.Add("VersaoImagemIngresso", typeof(int));
                apresentacaoSetores.Columns.Add("VersaoImagemVale", typeof(int));
                apresentacaoSetores.Columns.Add("VersaoImagemVale2", typeof(int));
                apresentacaoSetores.Columns.Add("VersaoImagemVale3", typeof(int));

                DataTable localImagens = new DataTable("LocalImagemMapa");
                localImagens.Columns.Add("ID", typeof(int));
                localImagens.Columns.Add("Nome", typeof(string));
                localImagens.Columns.Add("VersaoImagem", typeof(int));

                DataTable valeIngressoTipo = new DataTable("ValeIngressoTipo");
                valeIngressoTipo.Columns.Add("ID");
                valeIngressoTipo.Columns.Add("Nome");
                valeIngressoTipo.Columns.Add("VersaoImagem");
                valeIngressoTipo.Columns.Add("VersaoImagemInternet");

                DataTable setor = new DataTable("Setor");
                setor.Columns.Add("ID", typeof(int));
                setor.Columns.Add("Nome", typeof(string));
                setor.Columns.Add("VersaoBackground", typeof(int));

                string hoje = System.DateTime.Today.ToString("yyyyMMdd") + "000000";

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("SELECT DISTINCT tEvento.ID , tEvento.Nome, ");
                sb.Append("tEvento.VersaoImagemIngresso, ");
                sb.Append("tEvento.VersaoImagemVale, ");
                sb.Append("tEvento.VersaoImagemVale2, ");
                sb.Append("tEvento.VersaoImagemVale3 ");
                sb.Append("FROM tApresentacao ");
                sb.Append("LEFT join tApresentacaoSetor (NOLOCK) on tApresentacaoSetor.apresentacaoid = tApresentacao.id ");
                sb.Append("LEFT JOIN tEvento (NOLOCK) ON tEvento.ID = tApresentacao.EventoID ");
                sb.Append("WHERE tEvento.ID=tApresentacao.EventoID AND tApresentacaoSetor.ApresentacaoID=tApresentacao.ID ");
                sb.Append("AND tEvento.LocalID=" + localID + " ");
                sb.Append("AND (tApresentacao.DisponivelVenda='T' OR tApresentacao.DisponivelAjuste='T') ");
                sb.Append("AND tApresentacao.Horario >= '" + hoje + "' ");
                sb.Append("ORDER BY tEvento.Nome");
                SqlCommand cmd = new SqlCommand(sb.ToString(), (SqlConnection)bd.Cnn);
                SqlDataAdapter Adapter = new SqlDataAdapter(cmd);
                Adapter.Fill(eventos);
                Adapter.Dispose();
                cmd.Dispose();

                sb = new System.Text.StringBuilder();
                sb.Append("SELECT DISTINCT tApresentacao.ID, tApresentacao.Horario, ");
                sb.Append("tEvento.ID AS EventoID, tEvento.Nome AS Evento, ");
                sb.Append("tApresentacao.VersaoImagemIngresso, ");
                sb.Append("tApresentacao.VersaoImagemVale, ");
                sb.Append("tApresentacao.VersaoImagemVale2, ");
                sb.Append("tApresentacao.VersaoImagemVale3 ");
                sb.Append("FROM tApresentacao ");
                sb.Append("LEFT join tApresentacaoSetor (NOLOCK) on tApresentacaoSetor.apresentacaoid = tApresentacao.id ");
                sb.Append("LEFT JOIN tEvento (NOLOCK) ON tEvento.ID = tApresentacao.EventoID ");
                sb.Append("WHERE tEvento.ID=tApresentacao.EventoID AND tApresentacaoSetor.ApresentacaoID=tApresentacao.ID ");
                sb.Append("AND tEvento.LocalID=" + localID + " ");
                sb.Append("AND (tApresentacao.DisponivelVenda='T' OR tApresentacao.DisponivelAjuste='T') ");
                sb.Append("AND tApresentacao.Horario >= '" + hoje + "' ");
                sb.Append("ORDER BY tEvento.Nome,tApresentacao.Horario");
                cmd = new SqlCommand(sb.ToString(), (SqlConnection)bd.Cnn);
                Adapter = new SqlDataAdapter(cmd);
                Adapter.Fill(apresentacoes);
                Adapter.Dispose();
                cmd.Dispose();

                cmd = new SqlCommand("SELECT ID, Nome, VersaoImagem FROM tLocalImagemMapa(NOLOCK) WHERE LocalID = " + localID, (SqlConnection)bd.Cnn);
                Adapter = new SqlDataAdapter(cmd);
                Adapter.Fill(localImagens);

                sb = new System.Text.StringBuilder();
                sb.Append("SELECT DISTINCT tApresentacaoSetor.ID, ");
                sb.Append("tApresentacaoSetor.VersaoImagemIngresso, ");
                sb.Append("tEvento.Nome as Evento, tApresentacao.Horario, ");
                sb.Append("tApresentacaoSetor.VersaoImagemVale, ");
                sb.Append("tApresentacaoSetor.VersaoImagemVale2, ");
                sb.Append("tApresentacaoSetor.VersaoImagemVale3 ");
                sb.Append("FROM tApresentacao ");
                sb.Append("LEFT join tApresentacaoSetor (NOLOCK) on tApresentacaoSetor.apresentacaoid = tApresentacao.id ");
                sb.Append("LEFT JOIN tEvento (NOLOCK) ON tEvento.ID = tApresentacao.EventoID ");
                sb.Append("WHERE tEvento.ID=tApresentacao.EventoID AND tApresentacaoSetor.ApresentacaoID=tApresentacao.ID ");
                sb.Append("AND tEvento.LocalID=" + localID + " ");
                sb.Append("AND (tApresentacao.DisponivelVenda='T' OR tApresentacao.DisponivelAjuste='T') ");
                sb.Append("AND tApresentacao.Horario >= '" + hoje + "' ");
                sb.Append("ORDER BY tEvento.Nome,tApresentacao.Horario");
                cmd = new SqlCommand(sb.ToString(), (SqlConnection)bd.Cnn);
                Adapter = new SqlDataAdapter(cmd);
                Adapter.Fill(apresentacaoSetores);
                Adapter.Dispose();
                cmd.Dispose();

                sb = new System.Text.StringBuilder();
                sb.Append("SELECT DISTINCT vit.ID, vit.Nome, IsNull(vit.VersaoImagem,0 ) AS VersaoImagem, IsNull(vit.VersaoImagemInternet, 0) VersaoImagemInternet ");
                sb.Append("FROM tValeIngressoTipo vit (NOLOCK) WHERE vit.ValidadeData = '' OR vit.ValidadeData > '" + hoje + "'");
                cmd = new SqlCommand(sb.ToString(), (SqlConnection)bd.Cnn);
                Adapter = new SqlDataAdapter(cmd);
                Adapter.Fill(valeIngressoTipo);
                Adapter.Dispose();
                cmd.Dispose();

                sb = new System.Text.StringBuilder();
                sb.Append("SELECT DISTINCT tSetor.ID, tSetor.Nome, IsNull(tSetor.VersaoBackground, 0) AS VersaoBackground ");
                sb.Append("FROM tSetor (NOLOCK) ");
                sb.Append("INNER JOIN tApresentacaoSetor (NOLOCK) ON tSetor.ID = tApresentacaoSetor.SetorID ");
                sb.Append("INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tApresentacaoSetor.ApresentacaoID ");
                sb.Append("WHERE tSetor.LocalID = " + localID + " AND tApresentacao.DisponivelVenda = 'T' AND tApresentacao.Horario > '" + hoje + "'");
                cmd = new SqlCommand(sb.ToString(), (SqlConnection)bd.Cnn);
                Adapter = new SqlDataAdapter(cmd);
                Adapter.Fill(setor);
                Adapter.Dispose();
                cmd.Dispose();

                buffer.Tables.Add(eventos);
                buffer.Tables.Add(apresentacoes);
                buffer.Tables.Add(apresentacaoSetores);
                buffer.Tables.Add(localImagens);
                buffer.Tables.Add(valeIngressoTipo);
                buffer.Tables.Add(setor);

                return buffer;

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

        public DataSet CarregarImagensMapa(int localImagemID)
        {
            BD bd = new BD();

            try
            {
                DataSet buffer = new DataSet("Buffer");

                DataTable localImagens = new DataTable("LocalImagemMapa");
                localImagens.Columns.Add("ID", typeof(int));
                localImagens.Columns.Add("Nome", typeof(string));
                localImagens.Columns.Add("VersaoImagem", typeof(int));

                string hoje = DateTime.Today.ToString("yyyyMMdd") + "000000";

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("SELECT ID, Nome, VersaoImagem FROM tLocalImagemMapa WHERE ID = " + localImagemID);

                SqlCommand cmd = new SqlCommand(sb.ToString(), (SqlConnection)bd.Cnn);
                SqlDataAdapter Adapter = new SqlDataAdapter(cmd);
                Adapter = new SqlDataAdapter(cmd);
                Adapter.Fill(localImagens);
                Adapter.Dispose();
                cmd.Dispose();

                buffer.Tables.Add(localImagens);

                return buffer;
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

        public DataSet CarregarImagensSetor(int setorID)
        {
            BD bd = new BD();

            try
            {
                DataSet buffer = new DataSet("Buffer");

                DataTable setor = new DataTable("Setor");
                setor.Columns.Add("ID", typeof(int));
                setor.Columns.Add("Nome", typeof(string));
                setor.Columns.Add("VersaoBackground", typeof(int));

                string hoje = DateTime.Today.ToString("yyyyMMdd") + "000000";

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("SELECT DISTINCT tSetor.ID, tSetor.Nome, IsNull(tSetor.VersaoBackground, 0) AS VersaoBackground ");
                sb.Append("FROM tSetor (NOLOCK) ");
                sb.Append("INNER JOIN tApresentacaoSetor (NOLOCK) ON tSetor.ID = tApresentacaoSetor.SetorID ");
                sb.Append("INNER JOIN tApresentacao (NOLOCK) ON tApresentacao.ID = tApresentacaoSetor.ApresentacaoID ");
                sb.Append("WHERE tSetor.ID = " + setorID + " AND tApresentacao.DisponivelVenda = 'T' AND tApresentacao.Horario > '" + hoje + "'");

                SqlCommand cmd = new SqlCommand(sb.ToString(), (SqlConnection)bd.Cnn);
                SqlDataAdapter Adapter = new SqlDataAdapter(cmd);
                Adapter = new SqlDataAdapter(cmd);
                Adapter.Fill(setor);
                Adapter.Dispose();
                cmd.Dispose();

                buffer.Tables.Add(setor);

                return buffer;
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

        public DataSet CarregarImagensValeIngresso(int ValeIngressoTipoID)
        {
            BD bd = new BD();

            try
            {
                DataSet buffer = new DataSet("Buffer");

                DataTable valeIngressoTipo = new DataTable("ValeIngressoTipo");
                valeIngressoTipo.Columns.Add("ID");
                valeIngressoTipo.Columns.Add("Nome");
                valeIngressoTipo.Columns.Add("VersaoImagem");
                valeIngressoTipo.Columns.Add("VersaoImagemInternet");

                string hoje = DateTime.Today.ToString("yyyyMMdd") + "000000";

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("SELECT DISTINCT vit.ID, vit.Nome, IsNull(vit.VersaoImagem,0 ) AS VersaoImagem, IsNull(vit.VersaoImagemInternet, 0) VersaoImagemInternet ");
                sb.Append("FROM tValeIngressoTipo vit (NOLOCK) WHERE (vit.ValidadeData = '' OR vit.ValidadeData > '" + hoje + "') AND vit.ID = " + ValeIngressoTipoID);

                SqlCommand cmd = new SqlCommand(sb.ToString(), (SqlConnection)bd.Cnn);
                SqlDataAdapter Adapter = new SqlDataAdapter(cmd);
                Adapter.Fill(valeIngressoTipo);
                Adapter.Dispose();
                cmd.Dispose();

                buffer.Tables.Add(valeIngressoTipo);

                return buffer;
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

        public DataSet CarregarImagensIngresso(int eventoID)
        {
            BD bd = new BD();

            try
            {
                DataSet buffer = new DataSet("Buffer");

                DataTable eventos = new DataTable("Evento");
                eventos.Columns.Add("ID", typeof(int));
                eventos.Columns.Add("Nome", typeof(string));
                eventos.Columns.Add("VersaoImagemIngresso", typeof(int));
                eventos.Columns.Add("VersaoImagemVale", typeof(int));
                eventos.Columns.Add("VersaoImagemVale2", typeof(int));
                eventos.Columns.Add("VersaoImagemVale3", typeof(int));

                DataTable apresentacoes = new DataTable("Apresentacao");
                apresentacoes.Columns.Add("ID", typeof(int));
                apresentacoes.Columns.Add("Horario", typeof(string));
                apresentacoes.Columns.Add("Evento", typeof(string));
                apresentacoes.Columns.Add("EventoID", typeof(int));
                apresentacoes.Columns.Add("VersaoImagemIngresso", typeof(int));
                apresentacoes.Columns.Add("VersaoImagemVale", typeof(int));
                apresentacoes.Columns.Add("VersaoImagemVale2", typeof(int));
                apresentacoes.Columns.Add("VersaoImagemVale3", typeof(int));

                DataTable apresentacaoSetores = new DataTable("ApresentacaoSetor");
                apresentacaoSetores.Columns.Add("ID", typeof(int));
                apresentacaoSetores.Columns.Add("Evento", typeof(string));
                apresentacaoSetores.Columns.Add("Horario", typeof(string));
                apresentacaoSetores.Columns.Add("VersaoImagemIngresso", typeof(int));
                apresentacaoSetores.Columns.Add("VersaoImagemVale", typeof(int));
                apresentacaoSetores.Columns.Add("VersaoImagemVale2", typeof(int));
                apresentacaoSetores.Columns.Add("VersaoImagemVale3", typeof(int));

                string hoje = DateTime.Today.ToString("yyyyMMdd") + "000000";

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("SELECT DISTINCT tEvento.ID , tEvento.Nome, ");
                sb.Append("tEvento.VersaoImagemIngresso, ");
                sb.Append("tEvento.VersaoImagemVale, ");
                sb.Append("tEvento.VersaoImagemVale2, ");
                sb.Append("tEvento.VersaoImagemVale3 ");
                sb.Append("FROM tApresentacao ");
                sb.Append("LEFT join tApresentacaoSetor (NOLOCK) on tApresentacaoSetor.apresentacaoid = tApresentacao.id ");
                sb.Append("LEFT JOIN tEvento (NOLOCK) ON tEvento.ID = tApresentacao.EventoID ");
                sb.Append("WHERE tEvento.ID=tApresentacao.EventoID AND tApresentacaoSetor.ApresentacaoID=tApresentacao.ID ");
                sb.Append("AND tEvento.ID = " + eventoID + " ");
                sb.Append("AND (tApresentacao.DisponivelVenda='T' OR tApresentacao.DisponivelAjuste='T') ");
                sb.Append("AND tApresentacao.Horario >= '" + hoje + "' ");

                SqlCommand cmd = new SqlCommand(sb.ToString(), (SqlConnection)bd.Cnn);
                SqlDataAdapter Adapter = new SqlDataAdapter(cmd);
                Adapter.Fill(eventos);
                Adapter.Dispose();
                cmd.Dispose();

                sb = new System.Text.StringBuilder();
                sb.Append("SELECT DISTINCT tApresentacao.ID, tApresentacao.Horario, ");
                sb.Append("tEvento.ID AS EventoID, tEvento.Nome AS Evento, ");
                sb.Append("tApresentacao.VersaoImagemIngresso, ");
                sb.Append("tApresentacao.VersaoImagemVale, ");
                sb.Append("tApresentacao.VersaoImagemVale2, ");
                sb.Append("tApresentacao.VersaoImagemVale3 ");
                sb.Append("FROM tApresentacao ");
                sb.Append("LEFT join tApresentacaoSetor (NOLOCK) on tApresentacaoSetor.apresentacaoid = tApresentacao.id ");
                sb.Append("LEFT JOIN tEvento (NOLOCK) ON tEvento.ID = tApresentacao.EventoID ");
                sb.Append("WHERE tEvento.ID=tApresentacao.EventoID AND tApresentacaoSetor.ApresentacaoID=tApresentacao.ID ");
                sb.Append("AND tEvento.ID = " + eventoID + " ");
                sb.Append("AND (tApresentacao.DisponivelVenda='T' OR tApresentacao.DisponivelAjuste='T') ");
                sb.Append("AND tApresentacao.Horario >= '" + hoje + "' ");

                cmd = new SqlCommand(sb.ToString(), (SqlConnection)bd.Cnn);
                Adapter = new SqlDataAdapter(cmd);
                Adapter.Fill(apresentacoes);
                Adapter.Dispose();
                cmd.Dispose();

                sb = new System.Text.StringBuilder();
                sb.Append("SELECT DISTINCT tApresentacaoSetor.ID, ");
                sb.Append("tApresentacaoSetor.VersaoImagemIngresso, ");
                sb.Append("tEvento.Nome as Evento, tApresentacao.Horario, ");
                sb.Append("tApresentacaoSetor.VersaoImagemVale, ");
                sb.Append("tApresentacaoSetor.VersaoImagemVale2, ");
                sb.Append("tApresentacaoSetor.VersaoImagemVale3 ");
                sb.Append("FROM tApresentacao ");
                sb.Append("LEFT join tApresentacaoSetor (NOLOCK) on tApresentacaoSetor.apresentacaoid = tApresentacao.id ");
                sb.Append("LEFT JOIN tEvento (NOLOCK) ON tEvento.ID = tApresentacao.EventoID ");
                sb.Append("WHERE tEvento.ID=tApresentacao.EventoID AND tApresentacaoSetor.ApresentacaoID=tApresentacao.ID ");
                sb.Append("AND tEvento.ID = " + eventoID + " ");
                sb.Append("AND (tApresentacao.DisponivelVenda='T' OR tApresentacao.DisponivelAjuste='T') ");
                sb.Append("AND tApresentacao.Horario >= '" + hoje + "' ");
                sb.Append("ORDER BY tApresentacao.Horario");

                cmd = new SqlCommand(sb.ToString(), (SqlConnection)bd.Cnn);
                Adapter = new SqlDataAdapter(cmd);
                Adapter.Fill(apresentacaoSetores);
                Adapter.Dispose();
                cmd.Dispose();

                buffer.Tables.Add(eventos);
                buffer.Tables.Add(apresentacoes);
                buffer.Tables.Add(apresentacaoSetores);

                return buffer;
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
    }

    [Serializable]
    public class ImagemAtualizadorException : Exception
    {

        public ImagemAtualizadorException() : base() { }

        public ImagemAtualizadorException(string msg) : base(msg) { }

        public ImagemAtualizadorException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

}
