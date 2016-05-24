using CTLib;
using System;
using System.Runtime.Serialization;

namespace IRLib {

	/// <summary>
	/// ApresentacaoDisponibilizarGerenciador.
	/// </summary>
[ObjectType(ObjectType.RemotingType.SingleCall)]
	public class ApresentacaoDisponibilizarGerenciador :MarshalByRefObject
    {

		/// <summary>
		/// Retorna as flags da apresentacao
		/// </summary>
		/// <returns></returns>
		public bool[] Info(int apresentacaoID){
                BD bd = new BD();

                try
                {
                    bool[] retorno = new bool[3];

                    string sql = "SELECT DisponivelVenda,DisponivelAjuste,DisponivelRelatorio " +
                        "FROM tApresentacao " +
                        "WHERE ID=" + apresentacaoID;

                    bd.Consulta(sql);

                    if (bd.Consulta().Read())
                    {
                        retorno[0] = bd.LerBoolean("DisponivelRelatorio");
                        retorno[1] = bd.LerBoolean("DisponivelVenda");
                        retorno[2] = bd.LerBoolean("DisponivelAjuste");
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

		/// <summary>
		/// Executa operação manual
		/// </summary>
		public void Manual(int localID, int eventoIDNaoPassou, int apresentacaoIDNaoPassou, bool ajustarNaoPassaram, bool vendaNaoPassaram,  bool relatoriosNaoPassaram, int eventoIDJaPassou, int apresentacaoIDJaPassou, bool ajustarJaPassaram, bool vendaJaPassaram, bool relatoriosJaPassaram){
BD bd = new BD();
			try{
                
				bd.IniciarTransacao();

				string filtroJaPassou = " AND tApresentacao.Horario < '"+System.DateTime.Today.ToString("yyyyMMdd") + "000000"+"'";
				string filtroNaoPassou = " AND tApresentacao.Horario >= '"+System.DateTime.Today.ToString("yyyyMMdd") + "000000"+"'";

				string filtro = (apresentacaoIDNaoPassou==-1) ? "tEvento.ID="+eventoIDNaoPassou+" " : "tApresentacao.ID="+apresentacaoIDNaoPassou+" ";
				filtro = (eventoIDNaoPassou==-1) ? "tEvento.LocalID="+localID+" " : filtro;

				string innerJoin = (apresentacaoIDNaoPassou==-1) ? "INNER JOIN tEvento ON tApresentacao.EventoID=tEvento.ID " : "";

				string ajustar = (ajustarNaoPassaram) ? "T" : "F";
				string vender = (vendaNaoPassaram) ? "T" : "F";
				string relatorio = (relatoriosNaoPassaram) ? "T" : "F";

				string sql = "UPDATE tApresentacao "+
					"SET DisponivelVenda='"+vender+"', DisponivelAjuste='"+ajustar+"', DisponivelRelatorio='"+relatorio+"' "+
					"FROM tApresentacao "+
					innerJoin+
					"WHERE "+filtro+filtroNaoPassou;

				bd.Executar(sql);

				filtro = (apresentacaoIDJaPassou==-1) ? "tEvento.ID="+eventoIDJaPassou+" " : "tApresentacao.ID="+apresentacaoIDJaPassou+" ";
				filtro = (eventoIDJaPassou==-1) ? "tEvento.LocalID="+localID+" " : filtro;

				innerJoin = (apresentacaoIDJaPassou==-1) ? "INNER JOIN tEvento ON tApresentacao.EventoID=tEvento.ID " : "";

				ajustar = (ajustarJaPassaram) ? "T" : "F";
				vender = (vendaJaPassaram) ? "T" : "F";
				relatorio = (relatoriosJaPassaram) ? "T" : "F";

				sql = "UPDATE tApresentacao "+
					"SET DisponivelVenda='"+vender+"', DisponivelAjuste='"+ajustar+"', DisponivelRelatorio='"+relatorio+"' "+
					"FROM tApresentacao "+
					innerJoin+
					"WHERE "+filtro+filtroJaPassou;

				bd.Executar(sql);

				bd.FinalizarTransacao();

			}catch(Exception ex){
				bd.DesfazerTransacao();
				throw ex;
			}finally{
				bd.Fechar();
			}

		}

		/// <summary>
		/// Executa operação automatica
		/// </summary>
		public void Automatico(int localID, int eventoID, bool tirarDoArJaPassaram, bool tirarDoRelatorioJaPassaram, bool colocarNoArNaoPassaram){
            BD bd = new BD();
			try{
                
				bd.IniciarTransacao();

				string filtro = (eventoID!=0) ? "tEvento.ID="+eventoID+" " : "tEvento.LocalID="+localID+" ";

				string filtroJaPassou = " AND tApresentacao.Horario < '"+System.DateTime.Today.ToString("yyyyMMdd") + "000000"+"'";
				string filtroNaoPassou = " AND tApresentacao.Horario >= '"+System.DateTime.Today.ToString("yyyyMMdd") + "000000"+"'";

				if (tirarDoArJaPassaram){

					string sql = "UPDATE tApresentacao "+
						"SET DisponivelVenda='F', DisponivelAjuste='F' "+
						"FROM tApresentacao "+
						"INNER JOIN tEvento ON tApresentacao.EventoID=tEvento.ID "+
						"WHERE "+filtro+filtroJaPassou;

					bd.Executar(sql);
				}

				if (tirarDoRelatorioJaPassaram){

					string sql = "UPDATE tApresentacao "+
						"SET DisponivelRelatorio='F' "+
						"FROM tApresentacao "+
						"INNER JOIN tEvento ON tApresentacao.EventoID=tEvento.ID "+
						"WHERE "+filtro+filtroJaPassou;

					bd.Executar(sql);
				}

				if (colocarNoArNaoPassaram){

					string sql = "UPDATE tApresentacao "+
						"SET DisponivelVenda='T', DisponivelAjuste='T', DisponivelRelatorio='T' "+
						"FROM tApresentacao "+
						"INNER JOIN tEvento ON tApresentacao.EventoID=tEvento.ID "+
						"WHERE "+filtro+filtroNaoPassou;

					bd.Executar(sql);

				}

				bd.FinalizarTransacao();

			}catch(Exception ex){
				bd.DesfazerTransacao();
				throw ex;
			}finally{
				bd.Fechar();
			}

		}


	}

	[Serializable]
	public class ApresentacaoDisponibilizarGerenciadorException : Exception {

		public ApresentacaoDisponibilizarGerenciadorException() : base (){}

		public ApresentacaoDisponibilizarGerenciadorException(string msg) : base (msg){}

		public ApresentacaoDisponibilizarGerenciadorException(SerializationInfo info, StreamingContext context) : base(info, context) {}

		public override void GetObjectData(SerializationInfo info, StreamingContext context){
			base.GetObjectData(info, context);
		}

	}

}
