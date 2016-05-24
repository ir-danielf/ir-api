/**************************************************
* Arquivo: MapaEsquematico.cs
* Gerado: 27/05/2010
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
namespace IRLib
{

    public class MapaEsquematico : MapaEsquematico_B
    {
        public enum EnumEstadoMapaEsquematico
        { Novo, Editando, Visualizando }
        public enum EnumAcoesMapaEsquematico
        {
            Manter, Novo, Editar, Cancelar, Remover, Salvar, Recarregar,
            AssociarImg, AssociarSetor, AssociarEventoAP, AbrirNovoLocal,
            LimparAreas
        }
        public enum EnumMapaEsquematicoInnerAcao
        {
            Novo, Remover, Editar, Manter, Removido
        }
        public enum EnumAcaoArea
        {
            Linha,
            Area,
        }

        public MapaEsquematico() { }

        public MapaEsquematico(int usuarioIdLogado) : base(usuarioIdLogado) { }

        public List<EstruturaMapaEsquematico> CarregarMapas(int localID)
        {
            try
            {
                List<EstruturaMapaEsquematico> listaRetorno = new List<EstruturaMapaEsquematico>();

                EstruturaMapaEsquematico mapa = new EstruturaMapaEsquematico();
                string sql = @"SELECT me.ID AS MapaID, me.Nome AS MapaNome,
                                           mes.ID as MapaSetorID, mes.Coordenadas, s.ID AS SetorID, s.Nome as SetorNome
                                           FROM tMapaEsquematico me (NOLOCK) 
                                           INNER JOIN tMapaEsquematicoSetor mes (NOLOCK) ON me.ID = mes.MapaID 
                                           INNER JOIN tSetor s (NOLOCK) ON mes.SetorID = s.ID 
                                           WHERE me.LocalID = " + localID +
                                           "ORDER BY me.ID ASC, mes.ID DESC ";

                bd.Consulta(sql);

                int MapaID = 0;
                while (bd.Consulta().Read())
                {
                    MapaID = bd.LerInt("MapaID");
                    if (listaRetorno.Where(c => c.ID == MapaID).Count() == 0)
                    {
                        mapa = new EstruturaMapaEsquematico
                                   {
                                       ID = MapaID,
                                       LocalID = localID,
                                       Nome = bd.LerString("MapaNome"),
                                       InnerAcao = EnumMapaEsquematicoInnerAcao.Manter,
                                       MapasSetor = new List<EstruturaMapaEsquematicoSetor>()
                                   };
                        listaRetorno.Add(mapa);
                    }
                    if (bd.LerInt("MapaSetorID") > 0)
                    {
                        mapa.MapasSetor.Add(new EstruturaMapaEsquematicoSetor()
                        {
                            ID = bd.LerInt("MapaSetorID"),
                            //Coordenadas = bd.LerString("Coordenadas"),
                            MapaID = MapaID,
                            SetorID = bd.LerInt("SetorID"),
                            SetorNome = bd.LerString("SetorNome"),
                            Pontos = this.RetornarPontos(bd.LerString("Coordenadas")),
                            InnerAcao = EnumMapaEsquematicoInnerAcao.Manter,
                            Fechado = true,
                        });
                    }
                }

                listaRetorno.OrderBy(c => c.Nome).ToList();

                listaRetorno.Insert(0, new EstruturaMapaEsquematico()
                {
                    ID = 0,
                    Nome = "Selecione...",
                });

                return listaRetorno;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public List<Point> RetornarPontos(string coords)
        {
            List<Point> ptos = new List<Point>();
            foreach (string coord in coords.Split(';'))
                //foreach (string pt in coord.Split(','))
                ptos.Add(new Point(Convert.ToInt32(coord.Split(',')[0]), Convert.ToInt32(coord.Split(',')[1])));

            return ptos;
        }

        public EstruturaMapaEsquematico InserirNovoMapa(EstruturaMapaEsquematico item)
        {
            try
            {
                bd.IniciarTransacao();

                this.LocalID.Valor = item.LocalID;
                this.Nome.Valor = item.Nome;
                this.Inserir(bd);

                var mes = new MapaEsquematicoSetor(this.Control.UsuarioID);
                foreach (var itemSetor in item.MapasSetor)
                {
                    if (itemSetor.InnerAcao == EnumMapaEsquematicoInnerAcao.Remover ||
                        itemSetor.InnerAcao == EnumMapaEsquematicoInnerAcao.Removido || !itemSetor.Fechado)
                        continue;
                    mes.Limpar();
                    mes.MapaID.Valor = this.Control.ID;
                    mes.SetorID.Valor = itemSetor.SetorID;
                    mes.Coordenadas.Valor = itemSetor.PontosToString();
                    mes.Inserir(bd);

                    itemSetor.InnerAcao = EnumMapaEsquematicoInnerAcao.Manter;
                    itemSetor.MapaID = this.Control.ID;
                    item.ID = mes.Control.ID;
                }

                item.ID = this.Control.ID;
               
                item.InnerAcao = EnumMapaEsquematicoInnerAcao.Manter;
                bd.FinalizarTransacao();

                return item;
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

        public void RemoverMapa(EstruturaMapaEsquematico mapa)
        {
            try
            {
                bd.IniciarTransacao();
                var mes = new MapaEsquematicoSetor(this.Control.UsuarioID);

                bd.Executar(
                    @"UPDATE tEvento SET tEvento.MapaEsquematicoID = 0 WHERE MapaEsquematicoID = " + mapa.ID);

                bd.Executar(
                    @"UPDATE tApresentacao SET tApresentacao.MapaEsquematicoID = 0 WHERE MapaEsquematicoID = " + mapa.ID);


                foreach (EstruturaMapaEsquematicoSetor itemSetor in mapa.MapasSetor)
                    mes.Excluir(bd, itemSetor.ID);

                this.Excluir(bd, mapa.ID);

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

        public EstruturaMapaEsquematico AlterarMapa(EstruturaMapaEsquematico item)
        {
            try
            {
                bd.IniciarTransacao();
                
                this.Control.ID = item.ID;
                this.Nome.Valor = item.Nome;
                this.LocalID.Valor = item.LocalID;

                MapaEsquematicoSetor mes = new MapaEsquematicoSetor(this.Control.UsuarioID);
                foreach (EstruturaMapaEsquematicoSetor itemSetor in item.MapasSetor)
                {
                    mes.Limpar();
                    mes.MapaID.Valor = item.ID;
                    mes.SetorID.Valor = itemSetor.SetorID;
                    mes.Coordenadas.Valor = itemSetor.PontosToString();

                    if (itemSetor.SetorID == 0)
                        continue;

                    switch (itemSetor.InnerAcao)
                    {
                        case EnumMapaEsquematicoInnerAcao.Editar:
                            mes.Control.ID = itemSetor.ID;
                            mes.Atualizar(bd);
                            itemSetor.InnerAcao = EnumMapaEsquematicoInnerAcao.Manter;
                            break;
                        case EnumMapaEsquematicoInnerAcao.Novo:
                            mes.Inserir(bd);
                            itemSetor.ID = mes.Control.ID;
                            itemSetor.InnerAcao = EnumMapaEsquematicoInnerAcao.Manter;
                            break;
                        //case EnumMapaEsquematicoInnerAcao.Remover:
                        //    mes.Excluir(bd, itemSetor.ID);
                        //    itemSetor.InnerAcao = EnumMapaEsquematicoInnerAcao.Removido;
                        //    break;
                        //default:
                        //    //Ignora!
                        //    break;
                    }
                }

                for (int i = 0; i < item.IdsRemover.Count; i++)
                    if(item.IdsRemover[i] > 0) 
                        mes.Excluir(bd, item.IdsRemover[i]);
                
                item.IdsRemover.Clear();
                item.MapasSetor.RemoveAll(c => c.SetorID == 0);
                bd.FinalizarTransacao();

                return item;
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

        /// <summary>
        /// Atualiza MapaEsquematico
        /// </summary>
        /// <returns></returns>	
        public bool Atualizar(BD bd)
        {

            try
            {
                string sqlVersion = "SELECT MAX(Versao) FROM cMapaEsquematico WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlVersion);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle(bd, "U");
                InserirLog(bd);

                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE tMapaEsquematico SET LocalID = @001, Nome = '@002' ");
                sql.Append("WHERE ID = @ID");
                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.LocalID.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);

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
        /// Inserir novo(a) MapaEsquematico
        /// </summary>
        /// <returns></returns>	
        public bool Inserir(BD bd)
        {

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT MAX(ID) AS ID FROM cMapaEsquematico");
                object obj = bd.ConsultaValor(sql);
                int id = (obj != null) ? Convert.ToInt32(obj) : 0;

                this.Control.ID = ++id;
                this.Control.Versao = 0;

                sql = new StringBuilder();
                sql.Append("INSERT INTO tMapaEsquematico(ID, LocalID, Nome) ");
                sql.Append("VALUES (@ID,@001,'@002')");

                sql.Replace("@ID", this.Control.ID.ToString());
                sql.Replace("@001", this.LocalID.ValorBD);
                sql.Replace("@002", this.Nome.ValorBD);

                int x = bd.Executar(sql.ToString());

                bool result = (x == 1);

                if (result)
                    InserirControle(bd, "I");

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Exclui MapaEsquematico com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public bool Excluir(BD bd, int id)
        {

            try
            {
                this.Control.ID = id;

                string sqlSelect = "SELECT MAX(Versao) FROM cMapaEsquematico WHERE ID=" + this.Control.ID;
                object obj = bd.ConsultaValor(sqlSelect);
                int versao = (obj != null) ? Convert.ToInt32(obj) : 0;
                this.Control.Versao = versao;

                InserirControle(bd, "D");
                InserirLog(bd);

                string sqlDelete = "DELETE FROM tMapaEsquematico WHERE ID=" + id;

                int x = bd.Executar(sqlDelete);

                bool result = (x == 1);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void InserirControle(BD bd, string acao)
        {

            try
            {

                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("INSERT INTO cMapaEsquematico (ID, Versao, Acao, TimeStamp, UsuarioID) ");
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

                sql.Append("INSERT INTO xMapaEsquematico (ID, Versao, LocalID, Nome) ");
                sql.Append("SELECT ID, @V, LocalID, Nome FROM tMapaEsquematico WHERE ID = @I");
                sql.Replace("@I", this.Control.ID.ToString());
                sql.Replace("@V", this.Control.Versao.ToString());

                bd.Executar(sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }

    public class MapaEsquematicoLista : MapaEsquematicoLista_B
    {

        public MapaEsquematicoLista() { }

        public MapaEsquematicoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
