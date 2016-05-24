using IRCore.BusinessObject.Enumerator;
using IRCore.BusinessObject.Estrutura;
using IRCore.BusinessObject.Models;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using IRCore.DataAccess.Model.Enumerator;
using IRCore.Util;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IRCore.BusinessObject
{
    public class NewsletterBO : MasterBO<NewsletterADO>
    {
        public NewsletterBO(MasterADOBase ado = null) : base(ado) { }

        public RetornoModel Subscribe(NewsAssinante obj)
        {
            string pattern = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+(?:[A-Z]{2}|com|org|net|edu|gov|mil|biz|info|mobi|name|aero|asia|jobs|museum)\b";
            Match match = Regex.Match(obj.Email.Trim(), pattern, RegexOptions.IgnoreCase);

            NewsAssinante nao = ado.Consultar(obj.Email, obj.EventoID);
            if (nao != null)
            {
                if (nao.StatusAsEnum != enumNewsAssinantes.ativo)
                {
                    nao.StatusAsEnum = enumNewsAssinantes.ativo;
                    ado.Salvar(nao);
                    return new RetornoModel() { Sucesso = true, Mensagem = "Seu email foi registrado com sucesso" };
                }
                else
                {
                    return new RetornoModel() { Sucesso = true, Mensagem = "Seu email já esta registrado em nosso Newsletter" };
                }
            }
            if (match.Success)
            {
                obj.DataInscricao = DateTime.Now;
                obj.StatusAsEnum = enumNewsAssinantes.ativo;
                ado.Salvar(obj);
                return new RetornoModel() { Sucesso = true, Mensagem = "Seu email foi registrado com sucesso" };
            }
            else
            {
                return new RetornoModel() { Sucesso = false, Mensagem = "Erro ao registrar seu email, tente novamente" };
            }
        }

        public RetornoModel Unsubscribe(string email, int? eventoID)
        {

            NewsAssinante na = ado.Consultar(email, eventoID);
            na.StatusAsEnum = enumNewsAssinantes.inativo;
            try
            {
                return new RetornoModel() { Sucesso = ado.Salvar(na), Mensagem = "Email desinscrito com sucesso" };
            }catch{
                return new RetornoModel() { Sucesso = false, Mensagem = "Email não registrado" };
            }
        }

        /// <summary>
        /// Lista Paginada
        /// </summary>
        /// <param name="busca"></param>
        /// <returns></returns>
        public List<NewsAssinante> Listar(string busca = null, int? eventoId = null)
        {
            return ado.Listar(busca, eventoId);
        }


        /// <summary>
        /// Lista Paginada
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="busca"></param>
        /// <returns></returns>
        public IPagedList<NewsAssinante> Listar(int pageNumber, int pageSize, string busca = null, int? eventoId = null)
        {
            return ado.Listar(pageNumber, pageSize, busca, eventoId);
        }

        public byte[] GerarCSV(string busca = null)
        {
            var lista = this.Listar(busca);
            var csv = new StringBuilder();
            string newLine = "Nome;Email" + Environment.NewLine;
            csv.Append(newLine);
            foreach (var news in lista)
            {
                newLine = string.Format("{0}; {1}{2}", news.Nome, news.Email, Environment.NewLine);
                csv.Append(newLine);
            }
            return Encoding.ASCII.GetBytes(csv.ToString()); ;
        }

        /// <summary>
        /// Salva na base de dados
        /// </summary>
        /// <param name="obj"></param>
        public void Salvar(NewsAssinante obj)
        {
            ado.Salvar(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public NewsAssinante Consultar(int id)
        {
            return ado.Consultar(id);
        }
    }
}