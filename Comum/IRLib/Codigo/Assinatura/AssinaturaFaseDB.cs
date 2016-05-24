using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib
{
    public partial class AssinaturaFase_B : BaseBD
    {
        public override void Ler(int id)
        { }

        public override bool Excluir(int id)
        { return false; }

        public override bool Atualizar()
        { return false; }

        public override void Desfazer()
        { }
        public override bool Inserir()
        { return false; }
        public override void Limpar()
        { }

        public List<EstruturaIDNome> CarregarFases()
        {
            try
            {
                List<EstruturaIDNome> lista = new List<EstruturaIDNome>();
                lista.Add(new EstruturaIDNome() { ID = 0, Nome = "Selecione a fase..." });

                var sql = "SELECT ID, Nome FROM tAssinaturaFase ";

                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    lista.Add(new EstruturaIDNome()
                    {
                        ID = bd.LerInt("ID"),
                        Nome = bd.LerString("Nome"),
                    });
                }
                return lista;
            }
            finally
            {
                bd.Fechar();
            }
        }
    }
}