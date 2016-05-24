/**************************************************
* Arquivo: AssinaturaItem.cs
* Gerado: 09/09/2011
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IRLib
{

    public class AssinaturaItem : AssinaturaItem_B
    {

        public AssinaturaItem() { }

        public AssinaturaItem(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public void ExcluirItensAssinatura(int AssinaturaAnoID)
        {
            ExcluirItensAssinatura(bd, AssinaturaAnoID);
        }

        public void ExcluirItensAssinatura(BD bd, int AssinaturaAnoID)
        {
            try
            {
                List<int> lstExcluir = new List<int>();
                lstExcluir = this.BuscarItensExclusao(AssinaturaAnoID);
                foreach (int itemExclusao in lstExcluir)
                {
                    this.Excluir(bd, itemExclusao);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        private List<int> BuscarItensExclusao(int AssinaturaAnoID)
        {
            try
            {
                List<int> lista = new List<int>();
                string sql = @"SELECT DISTINCT ID from tAssinaturaItem WHERE AssinaturaAnoID = " + AssinaturaAnoID;

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    lista.Add(bd.LerInt("ID"));
                }
                return lista;
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
        public List<SetorInfo> BuscaSetoresEsquematico(int assinaturaID, int ano, IRLib.SingleTonObjects singleTon, ref int mapaEsquematicoID)
        {
            return BuscaSetoresEsquematico(assinaturaID, ano, singleTon, ref mapaEsquematicoID, 0, false);
        }

        public List<SetorInfo> BuscaSetoresEsquematico(int assinaturaID, int ano, IRLib.SingleTonObjects singleTon, ref int mapaEsquematicoID, int apresentacaoID, bool bancoIngresso)
        {
            try
            {
                string sql =
                    @"SELECT
                        DISTINCT ApresentacaoID, SetorID
                    FROM tAssinatura a (NOLOCK) 
                    INNER JOIN tAssinaturaAno an (NOLOCK) ON an.AssinaturaID = a.ID
                    INNER JOIN tAssinaturaItem ai (NOLOCK) ON ai.AssinaturaAnoID = an.ID
                    WHERE a.ID = " + assinaturaID + " AND an.Ano = '" + ano + "' AND a.Ativo = 'T' " + (apresentacaoID > 0 ? " AND ai.ApresentacaoID = " + apresentacaoID : string.Empty);

                bd.Consulta(sql);

                if (!bd.Consulta().Read())
                    throw new Exception("Não foi possível encontrar as informações da assinatura selecionada.");

                List<int> setoresID = new List<int>();

                if (apresentacaoID == 0)
                    apresentacaoID = bd.LerInt("ApresentacaoID");

                do
                {
                    if (!setoresID.Contains(bd.LerInt("SetorID")))
                        setoresID.Add(bd.LerInt("SetorID"));

                } while (bd.Consulta().Read());


                var mapaEsquematicoInfo = singleTon.bufferMapaEsquematico.BuscarInformacaoEstrutura(apresentacaoID);

                //var mapaEsquematicoInfo = new IRLib.BufferMapaEsquematico().BuscarInformacaoEstrutura(apresentacaoID);

                mapaEsquematicoID = mapaEsquematicoInfo.ID;

                return
                    (from me in mapaEsquematicoInfo.Setores
                     join s in setoresID on me.ID equals s
                     where me.Coordenadas.Count > 0
                     select me).ToList();
            }
            finally
            {
                bd.Fechar();
            }
        }


        public List<EstruturaIDNome> BuscarSetores(int assinaturaTipoID, int assinaturaID, int ano)
        {
            try
            {
                string filtroAssinatura = assinaturaID > 0 ? " AND a.ID = " + assinaturaID : string.Empty;
                string filtroAno = ano > 0 ? " AND an.Ano = '" + ano + "'" : string.Empty;

                string sql =
                    string.Format(@"
                        SELECT 
                            DISTINCT s.ID, s.Nome
                            FROM tAssinatura a (NOLOCK)
                            INNER JOIN tAssinaturaAno an (NOLOCK) ON an.AssinaturaID = a.ID
                            INNER JOIN tAssinaturaItem ai (NOLOCK) ON ai.AssinaturaAnoID = an.ID
                            INNER JOIN tSetor s (NOLOCK) ON s.ID = ai.SetorID
                        WHERE a.AssinaturaTipoID = {0} {1} {2}
                    ", assinaturaTipoID, filtroAssinatura, filtroAno);

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Não existem setores associados a esta temporada, tipo e série.");

                List<EstruturaIDNome> lista = new List<EstruturaIDNome>();

                do
                {
                    lista.Add(new EstruturaIDNome()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                    });
                } while (bd.Consulta().Read());

                lista.Insert(0, new EstruturaIDNome() { ID = 0, Nome = "Todos" });
                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }
    }

    public class AssinaturaItemLista : AssinaturaItemLista_B
    {

        public AssinaturaItemLista() { }

        public AssinaturaItemLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
