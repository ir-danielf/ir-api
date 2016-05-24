/**************************************************
* Arquivo: AssinaturaAno.cs
* Gerado: 09/09/2011
* Autor: Celeritas Ltda
***************************************************/

using CTLib;
using IRLib.Paralela.ClientObjects;
using System;
using System.Collections.Generic;

namespace IRLib.Paralela
{

    public class AssinaturaAno : AssinaturaAno_B
    {

        public AssinaturaAno() { }

        public AssinaturaAno(int usuarioIDLogado) : base(usuarioIDLogado) { }

        public void Inserir(BD bd, EstruturaAssinaturaAno eAA, int AssinaturaID)
        {
            this.Ano.Valor = eAA.Ano.ToString();
            this.Informacoes.Valor = eAA.AnoInfo;
            this.AssinaturaID.Valor = AssinaturaID;

            this.Inserir(bd);

        }

        public void Atualizar(BD bd, EstruturaAssinaturaAno eAA, int AssinaturaID)
        {
            this.Control.ID = eAA.ID;
            this.Ano.Valor = eAA.Ano.ToString();
            this.Informacoes.Valor = eAA.AnoInfo;
            this.AssinaturaID.Valor = AssinaturaID;

            this.Atualizar(bd);
        }


        public List<EstruturaIDNome> BuscarTemporadas(int assinaturaTipoID)
        {
            try
            {
                string sql
                    = @"
                        SELECT 
                            DISTINCT 
                                an.Ano
                            FROM tAssinatura a (NOLOCK)
                            INNER JOIN tAssinaturaAno an (NOLOCK) ON an.AssinaturaID = a.ID
                            WHERE a.AssinaturaTipoID = " + assinaturaTipoID;

                if (!bd.Consulta(sql).Read())
                    throw new Exception("Não existem temporadas para este tipo de assinautra.");


                List<EstruturaIDNome> lista = new List<EstruturaIDNome>();
                do
                {
                    lista.Add(new EstruturaIDNome()
                    {
                        ID = bd.LerInt("Ano"),
                        Nome = bd.LerInt("Ano").ToString()
                    });
                } while (bd.Consulta().Read());

                lista.Insert(0, new EstruturaIDNome() { ID = 0, Nome = "Todas" });

                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }
    }

    public class AssinaturaAnoLista : AssinaturaAnoLista_B
    {

        public AssinaturaAnoLista() { }

        public AssinaturaAnoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }

    }

}
