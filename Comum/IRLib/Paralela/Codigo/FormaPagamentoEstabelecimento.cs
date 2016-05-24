using System;
using System.Data;
using CTLib;
using System.Linq;
using System.Collections.Generic;

namespace IRLib.Paralela
{

    public class FormaPagamentoEstabelecimento : FormaPagamentoEstabelecimento_B
    {
        public FormaPagamentoEstabelecimento() { }

        public FormaPagamentoEstabelecimento(int usuarioIDLogado) : base(usuarioIDLogado) { }
    }
    public class FormaPagamentoEstabelecimentoLista : FormaPagamentoEstabelecimentoLista_B
    {
        public FormaPagamentoEstabelecimentoLista() { }
        public FormaPagamentoEstabelecimentoLista(int usuarioIDLogado) : base(usuarioIDLogado) { }
    }
}