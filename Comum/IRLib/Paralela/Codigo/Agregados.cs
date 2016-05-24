/**************************************************
* Arquivo: Agregados.cs
* Gerado: 11/10/2011
* Autor: Celeritas Ltda
***************************************************/

using System;
using System.Collections.Generic;

namespace IRLib.Paralela
{

    public class Agregados : Agregados_B
    {

        public Agregados() { }

        public Agregados(int usuarioIDLogado) : base(usuarioIDLogado) { }


        public List<IRLib.Paralela.Assinaturas.Models.Agregados> ListarTodos(int ClienteID)
        {
            try
            {
                SituacaoProfissional situacaoProfissional = new SituacaoProfissional();
                List<IRLib.Paralela.Assinaturas.Models.Agregados> lista = new List<Assinaturas.Models.Agregados>();

                string Sql = @"                    select agr.nome, agr.email, agr.ID, agr.Profissao, agr.DataNascimento,agr.grauParentesco, agr.SituacaoProfissionalID
					 from tagregados agr(nolock) 
                    where agr.clienteid = " + ClienteID;

                bd.Consulta(Sql);

                while (bd.Consulta().Read())
                {
                    situacaoProfissional.Ler(bd.LerInt("SituacaoProfissionalID"));

                    lista.Add(new IRLib.Paralela.Assinaturas.Models.Agregados()
                    {
                        ID = bd.LerInt("ID"),
                        ClienteID = ClienteID,
                        Email = bd.LerString("Email"),
                        Nome = bd.LerString("Nome"),
                        Profissao = bd.LerString("Profissao"),
                        DataNascimento = bd.LerStringFormatoData("DataNascimento"),
                        grauParentescoID = bd.LerInt("GrauParentesco"),
                        SituacaoProfissional = situacaoProfissional.Situacao.Valor
                    });
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

        public bool Salvar(IRLib.Paralela.Assinaturas.Models.Agregados agregado)
        {
            try
            {
                this.ClienteID.Valor = agregado.ClienteID;
                this.Nome.Valor = agregado.Nome;
                this.DataNascimento.Valor = Convert.ToDateTime(agregado.DataNascimento);
                this.GrauParentesco.Valor = agregado.grauParentescoID;
                this.Profissao.Valor = agregado.Profissao;
                this.Email.Valor = agregado.Email;

                SituacaoProfissional situacaoProfissional = new SituacaoProfissional();

                int situacao = situacaoProfissional.BuscarIDPeloNome(agregado.SituacaoProfissional);

                if (situacao > 0)
                    this.SituacaoProfissionalID.Valor = situacao;

                return this.Inserir();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Alterar(IRLib.Paralela.Assinaturas.Models.Agregados agregado)
        {
            try
            {
                this.Ler(agregado.ID);

                this.ClienteID.Valor = agregado.ClienteID;
                this.Nome.Valor = agregado.Nome;
                this.DataNascimento.Valor = Convert.ToDateTime(agregado.DataNascimento);
                this.GrauParentesco.Valor = agregado.grauParentescoID;
                this.Profissao.Valor = agregado.Profissao;

                SituacaoProfissional situacaoProfissional = new SituacaoProfissional();

                int situacao = situacaoProfissional.BuscarIDPeloNome(agregado.SituacaoProfissional);

                if (situacao > 0)
                    this.SituacaoProfissionalID.Valor = situacao;

                return this.Atualizar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public IRLib.Paralela.Assinaturas.Models.Agregados VerificaAgregado(int AgregadoID, int ClienteID)
        {
            try
            {
                SituacaoProfissional situacaoProfissional = new SituacaoProfissional();
                IRLib.Paralela.Assinaturas.Models.Agregados agregado = new Assinaturas.Models.Agregados();

                string Sql = @"SELECT * FROM tAgregados WHERE ClienteID = " + ClienteID + "AND ID = " + AgregadoID;

                bd.Consulta(Sql);

                if (bd.Consulta().Read())
                {
                    situacaoProfissional.Ler(bd.LerInt("SituacaoProfissionalID"));
                    agregado.ID = bd.LerInt("ID");
                    agregado.ClienteID = ClienteID;
                    agregado.Nome = bd.LerString("Nome");
                    agregado.Profissao = bd.LerString("Profissao");
                    agregado.DataNascimento = bd.LerStringFormatoData("DataNascimento");
                    agregado.grauParentescoID = bd.LerInt("GrauParentesco");
                    agregado.SituacaoProfissional = situacaoProfissional.Situacao.Valor;

                }

                return agregado;
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

    public class AgregadosLista : AgregadosLista_B
    {

        public AgregadosLista() { }

        public AgregadosLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
