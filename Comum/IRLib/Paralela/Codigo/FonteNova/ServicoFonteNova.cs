using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;

namespace IRLib.Paralela
{
    public class ServicoFonteNova : IRLib.FonteNovaService.WSBase
    {
        private string AChave { get; set; }
        private string Login = ConfigGerador.Config.Fontenova.Username.Valor;
        private string Senha = ConfigGerador.Config.Fontenova.Password.Valor;
        private List<Tipo> ListaTipos { get; set; }

        private string stringListaTipos { get; set; }

        public ServicoFonteNova()
        {
            this.EfetuarLogin();
        }

        public bool EfetuarLogin()
        {
            try
            {
                this.AChave = this.efetuarLogin(this.Login, this.Senha);

                if (!string.IsNullOrEmpty(this.AChave))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool EfetuarLogout()
        {
            try
            {
                if (this.efetuarLogout(AChave))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ListarTipos()
        {
            try
            {
                string listatipos = this.listarTipos(this.AChave);

                if (string.Compare(listatipos, stringListaTipos) != 0)
                {
                    stringListaTipos = listatipos;

                    this.ListaTipos = JsonConvert.DeserializeObject<List<Tipo>>(listatipos);

                    if (ListaTipos.Count > 0)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                return
                    true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ListarTipos(string nomeEvento)
        {
            try
            {
                string listatipos = this.listarTipos(this.AChave);

                if (string.Compare(listatipos, stringListaTipos) != 0)
                {
                    stringListaTipos = listatipos;

                    this.ListaTipos = JsonConvert.DeserializeObject<List<Tipo>>(listatipos);

                    if (ListaTipos.Count > 0)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                return
                    true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Bloquear(string aCodigo, string aIdEvento, string aMsgDisplay1Linha1, string aMsgDisplay1Linha2, string aMsgDisplay2Linha1, string aMsgDisplay2Linha2)
        {
            try
            {
                if (this.bloquear(this.AChave, aIdEvento, aCodigo, aMsgDisplay1Linha1, aMsgDisplay1Linha2, aMsgDisplay2Linha1, aMsgDisplay2Linha2))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Inserir(string ACodigoAcessoTipo, string ACodigoAcesso, Extra AExtra)
        {
            try
            {
                string extra = string.Empty;

                if (AExtra != null)
                    extra = JsonConvert.SerializeObject(AExtra);

                return this.inserir(this.AChave, ACodigoAcessoTipo, ACodigoAcesso, extra);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Liberar(string aCodigo, string aIdEvento)
        {
            try
            {
                if (this.liberar(this.AChave, aIdEvento, aCodigo))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool VerificarCodigoAcessou(string aCodigo)
        {
            try
            {
                string acesso = this.verificarCodigoAcessou(this.AChave, aCodigo);

                List<Acessou> acessou = JsonConvert.DeserializeObject<List<Acessou>>(acesso);

                if (acessou.FirstOrDefault().acessou)
                    return false;
                else
                    return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    [DataContract()]
    public class Tipo
    {
        [DataMember]
        public string idacesso_tipo { get; set; }
        [DataMember]
        public string dscacesso_tipo { get; set; }
        [DataMember]
        public object regras_acesso { get; set; }
        [DataMember]
        public object limite_ocupacao { get; set; }
        [DataMember]
        public string idevento { get; set; }
        [DataMember]
        public string dscevento { get; set; }
        [DataMember]
        public string permissao_evento { get; set; }
        [DataMember]
        public string qtde_acessos { get; set; }
        [DataMember]
        public string regras_acesso_lookup { get; set; }
    }

    [DataContract()]
    public class Extra
    {
        [DataMember]
        public string bloco { get; set; }
        [DataMember]
        public string fila { get; set; }
        [DataMember]
        public string assento { get; set; }
        [DataMember]
        public string matricula { get; set; }
        [DataMember]
        public string nmpessoa { get; set; }
        [DataMember]
        public string localizador { get; set; }
        [DataMember]
        public string valor { get; set; }
    }

    [DataContract()]
    public class Acessou
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string dsc { get; set; }
        [DataMember]
        public string cod { get; set; }
        [DataMember]
        public bool acessou { get; set; }
        [DataMember]
        public string dthr { get; set; }
        [DataMember]
        public string portoes { get; set; }
    }
}
