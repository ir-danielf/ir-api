namespace IRCore.DataAccess.Model
{
    public class InfosObrigatoriasIngresso
    {
        public string alvara { get; set; }
        public string AVCB { get; set; }
        public string fonteImposto { get; set; }
        public string porcentagemImposto { get; set; }
        public string dataEmissaoAlvara { get; set; }
        public string dataValidadeAlvara { get; set; }
        public string dataEmissaoAvcb { get; set; }
        public string dataValidadeAvcb { get; set; }
        public string lotacao { get; set; }
        public string produtorRazaoSocial { get; set; }
        public string produtorCpfCnpj { get; set; }
        public int? localId { get; set; }
        public string atencao { get; set; }
    }
}